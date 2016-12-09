using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class VideoController : MonoBehaviour
{
    string url = "";
    public RawImage _player;
    public AudioSource _sound;
    Message loadingBox;
    int progress;
    AzureManager azureManager;
    WWW www;


    void Start()
    {
        loadingBox = Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("data_loader_getting_data"), Message.Type.Info, false, true);
        azureManager = new AzureManager();
        azureManager.ProgressChanged += Progress;
        StartCoroutine(DataManager.GetVideoIdByPath());
        
        StartCoroutine(azureManager.GetBlob(Global.Instance.videoPath));

        url = @Application.persistentDataPath + "/video.mp4";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        url = "file:///" + Application.persistentDataPath + "/video.ogv";
#endif

    }

    void Progress(object sender, AzureManager.ProgressEventArgs e)
    {
        progress = int.Parse((e.Progress * 100).ToString("F0"));
        loadingBox.Text = Text.Instance.GetString("sceneloader_downloading") + " " + progress + "%";
        if (e.Progress == 2)
        {

            StartCoroutine(LoadVideo());
        }
    }

    void OnDestroy()
    {

        DeleteLocalVideo();
    }

    void DeleteLocalVideo()
    {
        string videoPath = @Application.persistentDataPath + "/video";
#if UNITY_IPHONE

        if (File.Exists("/private" + videoPath + ".mp4"))
        {
            File.Delete("/private" + videoPath + ".mp4");
        }
#endif
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (File.Exists(videoPath + ".ogv"))
        {
            File.Delete(videoPath + ".ogv");
        }
#endif
#if UNITY_ANDROID
        if (File.Exists(videoPath + ".mp4"))
        {
            File.Delete(videoPath + ".mp4");
        }
#endif
    }

    IEnumerator LoadVideo()
    {

        www = new WWW(url);

        if (www.error != null)
        {
            Debug.Log("Error: Can't load video");
            Debug.Log(Application.persistentDataPath);
            Debug.Log(www.error);
            yield break;
        }
        else
        {
            System.Threading.Thread.Sleep(2000);
            StartCoroutine(DataManager.GetVideoCount());
            AddVideoUserView();
            loadingBox.Destroy();

            #if UNITY_IOS || UNITY_ANDROID
            StartCoroutine(PlayVideoOnHandheld());
            #endif

            #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            PlayVideoOnMovieTexture();
#endif
        }

    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    void PlayVideoOnMovieTexture()
    {
        MovieTexture video = www.movie;

        _player.texture = video;
        _sound.clip = video.audioClip;
        video.Play();
        _sound.Play();
    }
#endif


    IEnumerator PlayVideoOnHandheld()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Color bgColor = Color.black;
        FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
        FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFill;

        Handheld.PlayFullScreenMovie(url, bgColor, controlMode, scalingMode);

        yield return new WaitForSeconds(1f); //wait for Handheld to lock Screen.orientation

        Screen.orientation = ScreenOrientation.AutoRotation;
        //while (Screen.currentResolution.height < Screen.currentResolution.width)
        //{
        //    yield return null;
        //}

        SceneLoader.Instance.CurrentScene = 0;
    }

    void AddVideoUserView()
    {
        var userId = Global.Instance.UserId;
        var videoId = Global.Instance.qrVideoId;
        var stamp = DateTime.Now;

        var qrVideoUserView = new QrVideoUserView(videoId, userId, stamp);
        StartCoroutine(DataManager.UploadQrVideoUserView(qrVideoUserView));
        UpdateVideoCount();
    }

    void UpdateVideoCount()
    {
        
        Guid id = Global.Instance.qrVideoId;
        int count = Global.Instance.getVideoUserViewCount.Count;
        StartCoroutine(DataManager.UpdateVideoCount(id, count));
    }
}
