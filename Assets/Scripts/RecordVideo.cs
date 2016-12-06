using System;
using UnityEngine;
using System.Collections;
using System.IO;
using CameraShot;
using UnityEngine.SceneManagement;

public class RecordVideo : MonoBehaviour {

    void Awake()
    {
        CameraShotEventListener.onVideoSaved += OnVideoSaved;
#if UNITY_ANDROID
        AndroidCameraShot.LaunchCameraForVideoCapture();
#endif

        //SceneLoader.Instance.PreviousScene = 1003;
        //Global.Instance.videoPath = "C:\\Users\\Dennis\\Desktop\\downloads\\sample.mp4";
        //SceneManager.LoadScene("video_details");
    }

    void OnDisable()
    {
        CameraShotEventListener.onVideoSaved -= OnVideoSaved;
    }

    void OnVideoSaved(string path)
    {
        //TODO: Move video to specific path?
        Global.Instance.videoPath = path;
        SceneLoader.Instance.CurrentScene = 1004;
    }
}
