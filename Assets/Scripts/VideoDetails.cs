﻿using System;
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

    private Message _confirmUploadMessages;
    private QrVideo _selectedVideo;
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
        SaveButton.onClick.AddListener(SaveVideoDetails);
        UploadButton.onClick.AddListener(UploadVideo);
        DeleteButton.onClick.AddListener(DeleteVideo);
        LocalVideos.onValueChanged.AddListener(
            delegate
            {
                SelectVideo();
            });
    }

    void DeleteVideo()
    {
        if (DeleteVideoFile())
        {
            StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
            RemoveVideoFromList();
        }
    }

    bool DeleteVideoFile()
    {
    #if UNITY_IPHONE
        if (File.Exists("/private" + _selectedVideo.Path))
        {
            File.Delete("/private" + _selectedVideo.Path);
            if (!File.Exists(_selectedVideo.Path))
            {
                return true;
            }
        }
    #else
        if (File.Exists(_selectedVideo.Path))
        {
            File.Delete(_selectedVideo.Path);
            if (!File.Exists(_selectedVideo.Path))
            {
                return true; 
            }
        }
    #endif
        else
        {
            Debug.Log("File not found");
        }
        return false;
    }

    void SaveVideoDetails()
    {
        Debug.Log(_previousScene);
        Debug.Log(Global.Instance.userGroup.GroupName);

        if (_previousScene == _recordVideoScene)
        {
            _selectedVideo = new QrVideo(Name.text, Description.text, Global.Instance.videoPath, 0, Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
            StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
        }

        if (_previousScene == _linkMenuScene)
        {
            UpdateSelectedVideoFromInputFields();
            StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
        }
    }

    void UploadVideo()
    {
        UpdateSelectedVideoFromInputFields();
        string blockBlobReference = _selectedVideo.Name.Replace(" ", "") + "_" + _selectedVideo.Id;
        AzureManager.PutBlob(_selectedVideo.Path, blockBlobReference);
        //_confirmUploadMessages = Util.CancellableMessageBox(new Rect(0, 0, 300, 200),
        //    "Du er ved at uploade din video " + _selectedVideo.Name + ". Denne video er af typen \"" +
        //    Global.Instance.videoCategories[LocalVideos.value] + "\". Er du sikker på at du vil fortsætte med at uploade videoen?", true, Message.Type.Info, delegate(Message message, bool value) {  });
        MakeVideoLive(blockBlobReference);
        RemoveVideoFromList();
        DeleteVideoFile();
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

    void UpdateSelectedVideoFromInputFields()
    {
        _selectedVideo.Name = Name.text;
        _selectedVideo.Description = Description.text;
        _selectedVideo.VideoCategoryId = Categories.value + 1;
    }

    void MakeVideoLive(string blockBlobReference)
    {
        _selectedVideo.Path = blockBlobReference;
        _selectedVideo.ReleaseDate = DateTime.Now;

        if (_previousScene == _linkMenuScene)
        {
            StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
        }
        else if (_previousScene == _recordVideoScene)
        {
            StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
        }
    }


}
