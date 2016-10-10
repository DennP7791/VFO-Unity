
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temporary help class til we create something with more learning, images etc.
public class Video : MonoBehaviour {
	
	private static Video _instance; //singleton

    public static Video Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Video)GameObject.FindObjectOfType(typeof(Video));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Video";
                    _instance = (Video)container.AddComponent(typeof(Video));
                }
            }

            return _instance;
        }	
    }
	
    public string videoFile;
    public Texture2D blank;

    private Rect videoWindow;
    private Rect blankWindow;

#if UNITY_ANDROID || UNITY_IOS
#else
	private UnityEngine.MovieTexture movie;
#endif

	private bool showMovie = false;
    private bool hasShownMovie = false;
    private bool showBackground = false;

#if UNITY_ANDROID || UNITY_IOS

    private IEnumerator StreamVideoMobile(string aud)
    {
        Debug.Log("Streaming mobile video from local path..");

        Handheld.PlayFullScreenMovie(aud, Color.white, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Debug.Log("Streaming mobile video done.");
    }

#else

    private IEnumerator StreamVideo(string aud)
    {
        //string path = "http://vlab.dk/diverse/vfo/video/10_hurt_back.ogg"; //MC 26-07-2016 - Deprecated - Looks like video files are stored online to limit application size

        //string path = Application.dataPath + "/video/" + aud; //MC 26-07-2016 - Deprecated - Looks like video files are stored online to limit application size

        string path = "http://vfoclient.welfaredenmark.com/" + "/video/" + aud; //MC 26-07-2016 - Added to point to correct online location of video files - NB: Only Danish located here

        WWW www = new WWW(path);
        
        movie = www.movie;

        yield return www;
    }

#endif
	
	public void PlayMovie(int t)
	{
        blankWindow = new Rect(0, 35, Screen.width, Screen.height);
        
        int _w = Screen.width;
        int _h = (int)((float)Screen.width * 0.64f);
        int _c = (int)(((float)Screen.height / 2.0f) - ((float)_h / 2.0f));
        videoWindow = new Rect(0, _c, _w, _h); 

#if UNITY_ANDROID || UNITY_IOS

        if (!string.IsNullOrEmpty(videoFile)) //MC Added 04-08-2016 - Change video file ext. to mp4 format for mobiles.
        {
            videoFile = videoFile.Split('.')[0] + ".mp4";
        }

        Debug.Log("Trying to play video: " + videoFile);

        StopCoroutine("StreamVideoMobile");
        StartCoroutine(StreamVideoMobile(videoFile));   
#else
        StopCoroutine("StreamVideo");
        StartCoroutine("StreamVideo", videoFile);
#endif

        showBackground = true;
	}

    // Use this for initialization
    void Awake()
    {

    }

    void OnDestroy()
    {
        _instance = null;
    }

    void OnGUI()
    {

#if UNITY_ANDROID || UNITY_IOS
#else
        if (showBackground)
            GUI.DrawTexture(blankWindow, blank);

        if(showMovie)
            GUI.DrawTexture(videoWindow, movie);
#endif

    }
	
#if UNITY_ANDROID || UNITY_IOS
#else

	// Update is called once per frame
    void Update()
    {

        if (movie && showMovie && !hasShownMovie)
        {
            if (!movie.isPlaying)
            {
                showMovie = false;
                showBackground = false;
                hasShownMovie = true;
                movie.Stop();
            }
        }
        else if(movie && !showMovie)
        {
            if (movie.isReadyToPlay && !movie.isPlaying && !hasShownMovie)
            {
                movie.loop = false;
                movie.Play();
                showMovie = true;
            }
        }
    }

#endif
}
