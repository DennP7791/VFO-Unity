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

    /// <summary>
    /// Setups up the loading screen while the application downloads the video,
    /// Sets up the url, depending on the platform.
    /// </summary>
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

        Debug.Log("Start [HANS]: " + url);
    }
    /// <summary>
    /// Updates the progress throughout the download, and if the file has been downloaded and saved to the device, it will play the video.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Progress(object sender, AzureManager.ProgressEventArgs e)
    {
        progress = int.Parse((e.Progress * 100).ToString("F0"));
        loadingBox.Text = Text.Instance.GetString("sceneloader_downloading") + " " + progress + "%";
        if (e.Progress == 2)
        {

            StartCoroutine(LoadVideo());
        }
    }
    /// <summary>
    /// Upon exiting the scene this method is called. 
    /// </summary>
    void OnDestroy()
    {

        DeleteLocalVideo();
    }

    /// <summary>
    /// Deletes the downloaded file, depending on the platform.
    /// </summary>
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

    /// <summary>
    /// Loads the video,
    /// Once the video is loaded, 
    /// Choose a method, depending on platform.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// On Windows, sets up the video texture, to display the chosen video.
    /// </summary>
    void PlayVideoOnMovieTexture()
    {
        try
        {
            MovieTexture video = www.movie;
            _player.texture = video;
            _sound.clip = video.audioClip;
            video.Play();
            _sound.Play();
        }
        catch (Exception ex)
        {
            Debug.Log("PlayVideoOnMovieTexture [HANS]: " + ex.Message);
        }
    }
#endif

    /// <summary>
    /// On Android and IOS, opens the device specific videoplayer.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayVideoOnHandheld()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Color bgColor = Color.black;
        FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
        FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFill;

        //Handheld.PlayFullScreenMovie(url, bgColor, controlMode, scalingMode);

#if UNITY_IOS
        string tempUrl = "file://" + url;
        Debug.Log("PlayVideoOnHandhled [HANS]: " + tempUrl);
        Handheld.PlayFullScreenMovie(tempUrl, bgColor, controlMode, scalingMode);
#else
        Handheld.PlayFullScreenMovie(url, bgColor, controlMode, scalingMode);
#endif

        yield return new WaitForSeconds(1f); //wait for Handheld to lock Screen.orientation

        Screen.orientation = ScreenOrientation.AutoRotation;
        //while (Screen.currentResolution.height < Screen.currentResolution.width)
        //{
        //    yield return null;
        //}

        SceneLoader.Instance.CurrentScene = 0;
    }

    /// <summary>
    /// Sets up and starts UpdateVideoUserView.
    /// </summary>
    void AddVideoUserView()
    {

        var userId = Global.Instance.UserId;
        var videoId = Global.Instance.qrVideoId;
        var stamp = DateTime.Now;
        var qrVideoUserView = new QrVideoUserView(videoId, userId, stamp);

        StartCoroutine(UpdateVideoUserView(qrVideoUserView));
    }
    /// <summary>
    /// Saves the VideoUserView,
    /// Gets the VideoUserView counts,
    /// Updates the apropriate video, with the new count.
    /// </summary>
    /// <param name="qrVideoUserView"></param>
    /// <returns></returns>
    private IEnumerator UpdateVideoUserView(QrVideoUserView qrVideoUserView)
    {
        yield return StartCoroutine(DataManager.UploadQrVideoUserView(qrVideoUserView));
        yield return StartCoroutine(DataManager.GetVideoCount());
        Guid id = Global.Instance.qrVideoId;
        int count = Global.Instance.getVideoUserViewCount.Count;
        yield return StartCoroutine(DataManager.UpdateVideoCount(id, count));
    }
}
