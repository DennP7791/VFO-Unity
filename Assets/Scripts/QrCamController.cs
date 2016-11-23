using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QrCamController : MonoBehaviour
{
    private WebCamTexture _camTexture;
    private Thread _qrThread;
    private Color32[] _c;
    private int _w, _h;
    private List<QrVideo> videoList;
    BarcodeReader barcodeReader;

    public RawImage RawImage;
    public UnityEngine.UI.Text StatusText;

    private bool _isQuit;
    private bool _qrFound;
    private ZXing.Result result;

    EncryptKey encrypt_key;
    DataManager data_manager;
    public UnityEngine.UI.Text errorMessage;
    public InputField passwordInput;
    public GameObject passwordCanvas;
    public Button SubmibButton;
    private bool isSecureVideo = false;

    void Start()
    {
        Initialize();

        OnEnable();
        RawImage.texture = _camTexture;
        RawImage.material.mainTexture = _camTexture;
        RawImage.SetNativeSize();

        StatusText.text = "Searching For QR";

        _qrThread = new Thread(DecodeQr);
        _qrThread.Start();
    }

    void Initialize()
    {
        encrypt_key = new EncryptKey();
        data_manager = new DataManager();
        SubmibButton.onClick.AddListener(SumbitPassword);

        barcodeReader = new BarcodeReader { AutoRotate = false, TryHarder = false };
        videoList = Global.Instance.qrVideos;
        _camTexture = new WebCamTexture();
    }

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

    void AdjustCamera()
    {
        if (_camTexture.width < 100)
        {
            Debug.Log("Still waiting another frame for correct info...");
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

    void OnEnable()
    {
        if (_camTexture != null)
        {
            _camTexture.Play();
            _w = _camTexture.width;
            _h = _camTexture.height;
        }
    }

    void OnDisable()
    {
        if (_camTexture != null)
        {
            _camTexture.Pause();
        }
    }

    void OnDestroy()
    {
        _qrThread.Abort();
        _camTexture.Stop();
        RawImage.material.mainTexture = null;
    }

    void OnApplicationQuit()
    {
        _isQuit = true;
    }

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

    //
    //------------------Encryption-----------------------
    //
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
            Debug.Log("Error: " + e);
        }
    }

    public void onSuccess(string result)
    {
        _qrFound = false;
        Global.Instance.videoPath = result;
        SceneLoader.Instance.CurrentScene = 1002;
    }

    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }

    public void onFail()
    {

    }
}