using UnityEngine;
using System.Collections;
using CameraShot;
using UnityEngine.SceneManagement;

public class RecordVideo : MonoBehaviour {

    void Awake()
    {
        CameraShotEventListener.onVideoSaved += OnVideoSaved;
        #if UNITY_ANDROID
        AndroidCameraShot.LaunchCameraForVideoCapture();
        #endif
        //SceneManager.LoadScene("video_details");
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2f);
        SceneLoader.Instance.CurrentScene = 1004;
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
