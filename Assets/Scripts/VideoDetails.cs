using System;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using Text = UnityEngine.UI.Text;
using System.Security.Cryptography;

public class VideoDetails : MonoBehaviour
{
    public InputField Name;
    public InputField Description;
    public Dropdown LocalVideos;
    public Dropdown Categories;
    public Button SaveButton;
    public Button UploadButton;
    public Button DeleteButton;
    public UnityEngine.UI.Text ErrorMessage;

    AzureManager am = new AzureManager();
    EncryptVideo ev = new EncryptVideo();
    private string _progress = "0";
    private string _localPath = "";
    private Message _confirmUploadMessage;
    private Message _uploadProgressMessage;
    private bool isEncrypted = true;

    private QrVideo _selectedVideo;
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _linkMenuScene = 0;

    private string key = "HR$2pIjHR$2pIj12jh3adTaF3bi23u9n7a";

    void Start()
    {
        LocalVideos.enabled = false;
        ErrorMessage.enabled = false;

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
        // Populate the Categories dropdown, and select the first category.

        foreach (var cat in Global.Instance.videoCategories)
        {
            Categories.options.Add(new Dropdown.OptionData() { text = cat.Name });
        }
        Categories.value = 1;
        Categories.value = 0;
    }

    void GetLocalVideos()
    {
        // If there are any local videos, add them to the LocalVideos list and select the first video. Else go back to the menu.

        if (!LocalVideos.enabled)
            LocalVideos.enabled = true;

        if (Global.Instance.localVideos.Count > 0)
        {
            foreach (var lv in Global.Instance.localVideos)
            {
                LocalVideos.options.Add(new Dropdown.OptionData() { text = lv.Name });
            }
            LocalVideos.value = 1;
            LocalVideos.value = 0;

            SelectVideo();
        }
        else
        {
            SceneLoader.Instance.CurrentScene = 0;
        }
    }

    void AddListeners()
    {
        // Add the button and dropdown listeners for the view.

        SaveButton.onClick.AddListener(SaveVideoDetails);
        UploadButton.onClick.AddListener(ConfirmUploadVideo);
        DeleteButton.onClick.AddListener(DeleteVideo);
        LocalVideos.onValueChanged.AddListener(
            delegate
            {
                SelectVideo();
            });
    }

    void DeleteVideo()
    {
        //If video file has been successfully deleted - delete the video from db and remove from list if previous scene was linkmenu / return to linkmenu if previous scene was record 

        //TODO: Add confirmDelete popup
        if (DeleteVideoFile())
        {
            if (_previousScene == _linkMenuScene)
            {
                StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                RemoveVideoFromList();
            }
            else if (_previousScene == _recordVideoScene)
            {
                SceneLoader.Instance.CurrentScene = 0;
            }

        }
    }

    bool DeleteVideoFile()
    {
        //delete the video file

#if UNITY_IPHONE
        if (File.Exists("/private" + _localPath))
        {
            File.Delete("/private" + _localPath);
            if (!File.Exists(_localPath))
            {
                return true;
            }
        }
#else
        if (File.Exists(_localPath))
        {
            File.Delete(_localPath);
            if (!File.Exists(_localPath))
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
        //Save the new or updated video details in the db.
        if (ValidInput())
        {
            ErrorMessage.enabled = false;
            if (_previousScene == _recordVideoScene)
            {
                _selectedVideo = new QrVideo(Name.text, Description.text, Global.Instance.videoPath, 0,
                    Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
                StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
            }

            if (_previousScene == _linkMenuScene)
            {
                UpdateSelectedVideoFromInputFields();
                StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
            }
        }
        else
        {
            ErrorMessage.enabled = true;
        }

    }

    void ConfirmUploadVideo()
    {
        //confirm if you want to upload the video

        if (ValidInput())
        {
            ErrorMessage.enabled = false;
            UpdateSelectedVideoFromInputFields();
            _confirmUploadMessage = Util.CancellableMessageBox(new Rect(0, 0, 300, 200),
                "Du er ved at uploade din video " + _selectedVideo.Name + ". Denne video er af typen \"" +
                Global.Instance.videoCategories[LocalVideos.value].Name +
                "\". Er du sikker på at du vil fortsætte med at uploade videoen?", true, Message.Type.Info,
                delegate (Message message, bool value)
                {
                    if (value)
                    {
                        _uploadProgressMessage = Util.MessageBox(new Rect(0, 0, 300, 200),
                            "Uploader video: " + _progress + "%", Message.Type.Info, false, true);
                        am.UploadProgressChanged += Progress;
                        UploadVideoToAzure();
                    }
                    else if (!value)
                    {

                    }
                    _confirmUploadMessage.Destroy();
                });
        }
        else
        {
            ErrorMessage.enabled = true;
        }

    }

    private void Progress(object sender, AzureManager.UploadProgressEventArgs e)
    {
        _progress = (e.Progress * 100).ToString();
        int index = _progress.IndexOf(".");
        if (index > 0)
        {
            _progress = _progress.Substring(0, index);
            _uploadProgressMessage.Text = "Uploader video: " + _progress + "%";
        }
        else if (e.Progress == 1)
        {
            _uploadProgressMessage.Text = "Uploader video: " + _progress + "%";
            UploadVideo();
            _uploadProgressMessage.Destroy();
            am.UploadProgressChanged -= Progress;
        }

        ////int.Parse((webRequest.uploadProgress * 100).ToString("F0"));
        //Debug.Log(sender.ToString());
    }

    void UploadVideoToAzure()
    {
        // Try to upload the video to the Azure blob. If successfull, make the video live (db) and delete the video file. If previous scene was linkmenu, remove from list. If previous scene was record, return to menu.

        if (File.Exists(_selectedVideo.Path) && isEncrypted)
        {
            DecryptVideoFile();
        }

        string blockBlobReference = _selectedVideo.Name.Replace(" ", "") + "_" + _selectedVideo.Id;
        _localPath = _selectedVideo.Path;
        _selectedVideo.Path = blockBlobReference;
        StartCoroutine(am.PutBlob(_localPath, blockBlobReference));
        ////TODO: dont continue untill the video has been successfully updated

    }

    void UploadVideo()
    {
        if (MakeVideoLive())
        {
            DeleteVideoFile();
        }
        if (_previousScene == _linkMenuScene)
            RemoveVideoFromList();
        else if (_previousScene == _recordVideoScene)
            SceneLoader.Instance.CurrentScene = 0;
    }

    void SelectVideo()
    {
        // Set _selectedVideo from the localVideos dropdown, and update the name, description and category fields to match the video.

        _selectedVideo = Global.Instance.localVideos[LocalVideos.value];
        Name.text = _selectedVideo.Name;
        Description.text = _selectedVideo.Description;
        Categories.value = _selectedVideo.VideoCategoryId - 1;
    }

    void RemoveVideoFromList()
    {
        // Remove video from the localVideos dropdown, and refresh localVideos.

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

    bool MakeVideoLive()
    {
        // Upload/Update video in db with new a release date. 

        _selectedVideo.ReleaseDate = DateTime.Now;

        if (_previousScene == _linkMenuScene)
        {

            StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
            return true;
        }
        else if (_previousScene == _recordVideoScene)
        {
            StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
            return true;
        }
        return false;
    }

    bool ValidInput()
    {
        if (Name.text.Length > 0 && Description.text.Length > 0)
            return true;
        return false;
    }




    void OnDestroy()
    {
        if (File.Exists(_selectedVideo.Path) && _selectedVideo.VideoCategoryId == 3 && isEncrypted)
        {
            EncyptVideoFile();
        }
    }

    //Refacotring
    public void EncyptVideoFile()
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        string orginalFilePath = _selectedVideo.Path;
        string file = Path.GetFileNameWithoutExtension(_selectedVideo.Path);
        string newPath = _selectedVideo.Path.Replace(file, file + "-Encrypted");
        ev.EncryptFile(_selectedVideo.Path, newPath, key, salt);
        if (File.Exists(_selectedVideo.Path))
        {
            File.Delete(_selectedVideo.Path);
        }
        File.Move(newPath, orginalFilePath);
    }

    public void DecryptVideoFile()
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        string orginalFilePath = _selectedVideo.Path;
        string file = Path.GetFileNameWithoutExtension(_selectedVideo.Path);
        string newPath = _selectedVideo.Path.Replace(file, file + "-Decrypted");
        ev.DecryptFile(_selectedVideo.Path, newPath, key, salt);
        if (File.Exists(_selectedVideo.Path))
        {
            File.Delete(_selectedVideo.Path);
        }
        File.Move(newPath, orginalFilePath);
    }

}
