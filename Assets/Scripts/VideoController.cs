using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    //string url = "https://welfaredenmark.blob.core.windows.net/vfo-recordings-staging/ogv";
    string url;

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
        url = Global.Instance.videoUrl;
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
