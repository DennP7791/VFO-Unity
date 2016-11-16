using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    //string url = "https://welfaredenmark.blob.core.windows.net/vfo-recordings-staging/ogv";
    string url = "";

    public RawImage _player;
    public AudioSource _sound;
    MovieTexture video;
    Message loadingBox;
    int progress;
    WWW www;

    void Start ()
    {
        loadingBox = Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("data_loader_getting_data"), Message.Type.Info, false, true);
        //_player = GetComponent<RawImage>();
        //_sound = GetComponent<AudioSource>();
        AzureManager.GetBlob(Global.Instance.videoPath);
        url = @Application.persistentDataPath + "/video.mp4";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        url = "file:///" + Application.persistentDataPath + "/video.ogv";
#endif

        StartCoroutine(LoadVideo());
    }

    IEnumerator LoadVideo()
    {
        www = new WWW(url);

        while (!www.isDone)
        {
            progress = int.Parse((www.progress * 100).ToString("F0"));
            yield return null;
        }

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
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR
            PlayVideoOnMovieTexture();
            #endif
        }

    }
	
	// Update is called once per frame
	void Update () {
        loadingBox.Text = Text.Instance.GetString("sceneloader_downloading") + " " + progress + "%";
        if (Input.GetKeyDown(KeyCode.Space) && video.isPlaying)
        {
            video.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !video.isPlaying)
        {
            video.Play();
        }
    }

    void PlayVideoOnMovieTexture()
    {
        video = www.movie;
        _player.texture = video;
        _sound.clip = video.audioClip;
        video.Play();
        _sound.Play();
    }


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
