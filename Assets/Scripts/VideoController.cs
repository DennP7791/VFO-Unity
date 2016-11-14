using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    //string url = "https://welfaredenmark.blob.core.windows.net/vfo-recordings-staging/ogv";
    string url = "";
    //string url = "http://localhost:59477/Service/DownloadVideoStream?videoName=ogv"; test - forsøgte at kalde www.movie direkte på http get, i håb om at kunne hive videos ud af den stream jeg sender

    public RawImage _player;
    public AudioSource _sound;
    MovieTexture video;
    Message loadingBox;
    int progress;

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
        WWW www = new WWW(url);

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
            video = www.movie;
            _player.texture = video;
            _sound.clip = video.audioClip;
            loadingBox.Destroy();
            video.Play();
            _sound.Play();
        }
        
    }

    void OnDestroy()
    {
        video.Stop();
        _sound.Stop();
    }

    void OnDisable()
    {
        video.Pause();
        _sound.Pause();
    }

    void ApplicationQuit()
    {
        video.Stop();
        _sound.Stop();
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
}
