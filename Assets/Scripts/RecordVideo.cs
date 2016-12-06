using System;
using UnityEngine;
using System.Collections;
using System.IO;
using CameraShot;
using UnityEngine.SceneManagement;

public class RecordVideo : MonoBehaviour {

    //On Awake sets up the camera listener, 
    //so that we can update the video path in Global, 
    //upon saving the video.
    void Awake()
    {
        CameraShotEventListener.onVideoSaved += OnVideoSaved;
#if UNITY_ANDROID
        AndroidCameraShot.LaunchCameraForVideoCapture();
#endif
#if UNITY_IPHONE
        IOSCameraShot.LaunchCameraForVideoCapture(0);
#endif
    }

    //Removes the event when the user exits the Scene.
    void OnDisable()
    {
        CameraShotEventListener.onVideoSaved -= OnVideoSaved;
    }

    //If the user clicks save, 
    //the global video path is updated to the video filepath, 
    //and the scene is changed to video details.
    void OnVideoSaved(string path)
    {
        Global.Instance.videoPath = path;
        SceneLoader.Instance.CurrentScene = 1004;
    }
}
