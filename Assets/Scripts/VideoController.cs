using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class VideoController : MonoBehaviour
{
    string url = "";

    public RawImage _player;
    public AudioSource _sound;
    //MovieTexture video;
    Message loadingBox;
    int progress;
    AzureManager azureManager;
    WWW www;

    void Start ()
    {
        loadingBox = Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("data_loader_getting_data"), Message.Type.Info, false, true);
        azureManager = new AzureManager();
        azureManager.ProgressChanged += Progress;
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
        if(e.Progress == 2)
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
            loadingBox.Destroy();
#if UNITY_IOS || UNITY_ANDROID
            StartCoroutine(PlayVideoOnHandheld());
#endif
            /*
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            PlayVideoOnMovieTexture();
            #endif
            */
        }

    }
    /*
    void PlayVideoOnMovieTexture()
    {

        video = www.movie;
        _player.texture = video;
        _sound.clip = video.audioClip;
        video.Play();
        _sound.Play();
    }*/


    IEnumerator PlayVideoOnHandheld()
    {
        Color bgColor = Color.black;
        FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
        FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFill;

        Handheld.PlayFullScreenMovie(url, bgColor, controlMode, scalingMode);
        yield return new WaitForSeconds(1f); //wait for Handheld to lock Screen.orientation

        Screen.orientation = ScreenOrientation.AutoRotation;
        while (Screen.currentResolution.height < Screen.currentResolution.width)
        {
            yield return null;
        }
    }
}
