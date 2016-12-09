using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QrCamController : MonoBehaviour
{
    private WebCamTexture _camTexture; //Used to render the video input.
    private Thread _qrThread;
    private Color32[] _c;
    private int _w, _h;
    private List<QrVideo> videoList;
    BarcodeReader barcodeReader; //ZXing Barcodereader, to decode QR - Codes.

    public RawImage RawImage;
    public UnityEngine.UI.Text StatusText;

    private bool _isQuit;
    private bool _qrFound;
    private ZXing.Result result; //ZXing result, used to create a string from the decoded QR - Code.

    EncryptKey encrypt_key; 
    DataManager data_manager;
    public UnityEngine.UI.Text errorMessage;
    public InputField passwordInput;
    public GameObject passwordCanvas;
    public Button SubmibButton;
    private bool isSecureVideo = false;
    /// <summary>
    /// Used to start the intialization of the class.
    /// </summary>
    void Start()
    {
        Initialize();

        OnEnable();
        RawImage.texture = _camTexture;
        RawImage.material.mainTexture = _camTexture; 
        RawImage.SetNativeSize();

        StatusText.text = "Searching For QR";


        _qrThread = new Thread(DecodeQr); //Used to run the decode async.
        _qrThread.Start();
    }
    /// <summary>
    /// Initializes the variables.
    /// </summary>
    void Initialize()
    {
        encrypt_key = new EncryptKey();
        data_manager = new DataManager();
        SubmibButton.onClick.AddListener(SumbitPassword);

        barcodeReader = new BarcodeReader { AutoRotate = false, TryHarder = false };
        videoList = Global.Instance.qrVideos;
        _camTexture = new WebCamTexture();
    }
    /// <summary>
    ///Update is called every frame, used to check if a QR - Code has been found. 
    ///If a QR - Code, has not been found, adjusts the camera.
    /// </summary>
    void Update()
    {
        if (_c == null)
        {
            _c = _camTexture.GetPixels32();
        }
        if (_qrFound)
        {
            StatusText.text = "QR Video Found";
            if (isSecure())
            {
                passwordCanvas.SetActive(true);
            }
            else
            {
                passwordCanvas.SetActive(false);
                LoadVideo(result.ToString());
            }
        }
        AdjustCamera();
    }
    /// <summary>
    /// Adjusts the camera, and makes sure that the camera orientation is correct.
    /// </summary>
    void AdjustCamera()
    {
        if (_camTexture.width < 100)
        {
            return;
        }

        var cwNeeded = _camTexture.videoRotationAngle;
        var ccwNeeded = -cwNeeded;

        if (_camTexture.videoVerticallyMirrored) ccwNeeded += 180;

        RawImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, ccwNeeded);

        var videoRatio = _camTexture.width / _camTexture.height;

        RawImage.GetComponent<AspectRatioFitter>().aspectRatio = videoRatio;

        if (_camTexture.videoVerticallyMirrored)
        {
            RawImage.uvRect = new Rect(1, 0, -1, 1); // means flip on vertical axis
        }
        else
        {
            RawImage.uvRect = new Rect(0, 0, 1, 1);  // means no flip
        }
    }
    /// <summary>
    /// Called when the class is enabled, calls play on camtexture and sets the width and height of it, if it has been instantiated.
    /// </summary>
    void OnEnable()
    {
        if (_camTexture != null)
        {
            _camTexture.Play();
            _w = _camTexture.width;
            _h = _camTexture.height;
        }
    }
    /// <summary>
    /// Pauses the Camtexture on disable.
    /// </summary>
    void OnDisable()
    {
        if (_camTexture != null)
        {
            _camTexture.Pause();
        }
    }
    /// <summary>
    /// Aborts everything, and sets the Maintexture of the RawImage equal to null, 
    /// this is to avoid a bug with RawImage.material.maintexture, 
    /// causing it to set the image of every game object to an image of the recorded camtexture,
    /// throughout the entire project. 
    /// </summary>
      void OnDestroy()
    {
        _qrThread.Abort();
        _camTexture.Stop();
        RawImage.material.mainTexture = null;
    }
    /// <summary>
    /// Tells the qr scanner, to begin quitting, by setting the boolean true.
    /// </summary>
    void OnApplicationQuit()
    {
        _isQuit = true;
    }
    /// <summary>
    /// If a QR-Code has not been found, it will try to decode the result from the BarcodeReader. 
    /// If the result is a QR Code, it sets qr found true.
    /// </summary>
    void DecodeQr()
    {
        while (!_qrFound)
        {
            if (_isQuit)
                break;

            try
            {
                result = barcodeReader.Decode(_c, _w, _h);
                if (result != null)
                {
                    _qrFound = true;
                }

                Thread.Sleep(200);
                _c = null;
            }
            catch
            {
            }
        }
    }
    /// <summary>
    ///Goes through the videoList to check if any video has a path equal to the result,
    ///if a video a is found it is added to Global.VideoPath and the scene is changed to the video player.
    /// </summary>
    /// <param name="result"></param>
    void LoadVideo(string result)
    {
        foreach (var vid in videoList)
        {
            if (vid.Path.Equals(result))
            {
                _qrFound = false;
                Global.Instance.videoPath = result;
                SceneLoader.Instance.CurrentScene = 1002;
            }
        }
    }
    
    #region   //Encryption
    /// <summary>
    /// Checks if the scanned Path, matches a Secure video the user has access too.
    /// </summary>
    /// <returns></returns>
    bool isSecure()
    {
        foreach (var vid in videoList)
        {
            if (vid.VideoCategoryId == 1 && vid.VideoCategoryId == 2)
            {
                isSecureVideo = false;
            }
            else
            {
                isSecureVideo = true;
            }
        }
        return isSecureVideo;
    }
    /// <summary>
    /// Checks if the entered password is equal to the saved password.
    /// </summary>
    private void SumbitPassword()
    {
        try
        {
            if (passwordInput.text.Length == 0)
            {
                errorMessage.text = "Password Is Empty!";
            }
            else
            {
                string savedPassword = Global.Instance.userGroupVideoCredintial.Password.ToString();
                byte[] salt = GetBytes(Global.Instance.userGroupVideoCredintial.Salt.ToString());

                string input_Password = encrypt_key.Hash(passwordInput.text, salt);
                string db_Password = savedPassword;

                if (db_Password == input_Password)
                {
                    StartCoroutine(DataManager.validateSeureQrVideo(result.ToString()));
                }
                else
                {
                    errorMessage.text = "Wrong password!";
                }
            }
        }
        catch (Exception e)
        {
            errorMessage.text = "Something went wrong, Contact support!";
        }
    }
    /// <summary>
    /// If the password matches, sets the video path and loads the video player scene.
    /// </summary>
    /// <param name="result"></param>
    public void onSuccess(string result)
    {
        _qrFound = false;
        Global.Instance.videoPath = result;
        SceneLoader.Instance.CurrentScene = 1002;
    }

    /// <summary>
    /// Converts string to byte[]
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
    /// <summary>
    /// Converts byte[] to string.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public void onFail()
    {

    }
    #endregion
}