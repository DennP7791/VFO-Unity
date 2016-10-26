using UnityEngine;
using System.Collections;
using CameraShot;

public class RecordVideo : MonoBehaviour {

    void Awake()
    {
        CameraShotEventListener.onVideoSaved += OnVideoSaved;
        #if UNITY_ANDROID
        AndroidCameraShot.LaunchCameraForVideoCapture();
        #endif
    }

    void OnDisable()
    {
        CameraShotEventListener.onVideoSaved -= OnVideoSaved;
    }

    void OnVideoSaved(string path)
    {
        Global.Instance.videoPath = path;
        SceneLoader.Instance.CurrentScene = 1004;
    }
}
