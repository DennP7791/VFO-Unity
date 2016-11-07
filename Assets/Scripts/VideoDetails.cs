using System;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Text = UnityEngine.UI.Text;

public class VideoDetails : MonoBehaviour
{
    public InputField Name;
    public InputField Description;
    public Dropdown LocalVideos;
    public Dropdown Categories;
    public Button SaveButton;
    public Button UploadButton;
    public Button DeleteButton;

    private QrVideo _selectedVideo;
    private List<string> _categoryList = new List<string>();
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _linkMenuScene = 0;

	void Start ()
	{
	    LocalVideos.enabled = false;

        _previousScene = SceneLoader.Instance.PreviousScene;
        GetCategories();

	    if (_previousScene == _linkMenuScene)
	    {
            GetLocalVideos();
	    }


        AddListeners();
    }

    void GetCategories()
    {
        foreach (var cat in Global.Instance.videoCategories)
        {
            Categories.options.Add(new Dropdown.OptionData() {text=cat.Name});
        }
        Categories.value = 1;
        Categories.value = 0;
    }

    void GetLocalVideos()
    {
        LocalVideos.enabled = true;

        foreach (var lv in Global.Instance.localVideos)
        {
            LocalVideos.options.Add(new Dropdown.OptionData() {text = lv.Name});
        }
        LocalVideos.value = 1;
        LocalVideos.value = 0;

        SelectVideo();
    }

    void AddListeners()
    {
        //DeleteButton.onClick.AddListener(GoToPreviousScene);
        SaveButton.onClick.AddListener(SaveVideoDetails);
        UploadButton.onClick.AddListener(UploadVideo);
        DeleteButton.onClick.AddListener(DeleteVideo);
        LocalVideos.onValueChanged.AddListener(
            delegate
            {
                SelectVideo();
            });
    }

    void GoToPreviousScene()
    {
        SceneLoader.Instance.CurrentScene = _previousScene;
    }

    void DeleteVideo()
    {
    #if UNITY_IPHONE
        if (File.Exists("/private" + _selectedVideo.Path))
        {
            File.Delete("/private" + _selectedVideo.Path);
            if (!File.Exists(_selectedVideo.Path))
            {
                StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                RemoveVideoFromList();
            }
        }
    #else
        if (File.Exists(_selectedVideo.Path))
        {
            File.Delete(_selectedVideo.Path);
            if (!File.Exists(_selectedVideo.Path))
            {
                StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                RemoveVideoFromList();
            }
        }
        else
        {
            Debug.Log("File not found");
        }
    #endif
    }

    void SaveVideoDetails()
    {
        QrVideo video;
        Debug.Log(_previousScene);
        Debug.Log(Global.Instance.userGroup.GroupName);

        if (_previousScene == _recordVideoScene)
        {
            video = new QrVideo(Name.text, Description.text, Global.Instance.videoPath, 0, Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
            StartCoroutine(DataManager.UploadQrVideo(video));
        }

        if (_previousScene == _linkMenuScene)
        {
            _selectedVideo.Name = Name.text;
            _selectedVideo.Description = Description.text;
            _selectedVideo.VideoCategoryId = Categories.value + 1;

            StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
        }
    }

    void UploadVideo()
    {
        string blockBlobReference = Name.text.Replace(" ","") + "_" + _selectedVideo.Id;
        StartCoroutine(AzureManager.PutBlob(_selectedVideo.Path, blockBlobReference));
        QrVideo video = new QrVideo(_selectedVideo.Id, Name.text, Description.text, blockBlobReference, 0, Global.Instance.userGroup.Id, Global.Instance.UserId, DateTime.Now, Categories.value + 1);
        StartCoroutine(DataManager.UpdateQrVideo(video));
        RemoveVideoFromList();
    }

    void SelectVideo()
    {
        _selectedVideo = Global.Instance.localVideos[LocalVideos.value];

        Name.text = _selectedVideo.Name;
        Description.text = _selectedVideo.Description;
        Categories.value = _selectedVideo.VideoCategoryId - 1;
    }

    void RemoveVideoFromList()
    {
        Global.Instance.localVideos.Remove(_selectedVideo);
        LocalVideos.ClearOptions();
        GetLocalVideos();
    }

    void DeleteFile()
    {
#if UNITY_IPHONE
            if (File.Exists("/private" + _selectedVideo.Path))
        {
            File.Delete("/private" + _selectedVideo.Path);
            Debug.Log("File deleted");
        }
#else
        Debug.Log("Selected video path: " + _selectedVideo.Path);
        if (File.Exists(_selectedVideo.Path))
        {
            File.Delete(_selectedVideo.Path);
            Debug.Log("File deleted");
        }
        else
        {
            Debug.Log("File not found");
        }
#endif
    }

}
