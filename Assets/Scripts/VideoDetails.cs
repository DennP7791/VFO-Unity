using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Threading;

public class VideoDetails : MonoBehaviour
{
    public InputField Name, Description;
    public Dropdown LocalVideos, Categories;
    public Button SaveButton, UploadButton, DeleteButton;
    public GameObject LocalVideosRow;
    public UnityEngine.UI.Text ErrorMessage;

    AzureManager am = new AzureManager();
    EncryptVideo ev = new EncryptVideo();
    private string _progress = "0";
    private string _localPath = "";
    private bool _isSavedInDB, _encryptOnDestroy = false; //make false again
    private Message _confirmUploadMessage, _conirmDeleteMessage, _uploadProgressMessage;
    private string _uploadString;

    private QrVideo _selectedVideo;
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _linkMenuScene = 0;

    private string progressIndicator = "";


    #region Initialization
    void Start()
    {
        LocalVideosRow.SetActive(false);
        ErrorMessage.enabled = false;

        Debug.Log("previous scene: " + SceneLoader.Instance.PreviousScene);
        Debug.Log("current scene: " + SceneLoader.Instance.CurrentScene);

        _previousScene = SceneLoader.Instance.PreviousScene;
        GetCategories();

        _selectedVideo = new QrVideo(Global.Instance.videoPath); //instantiate new QrVideo with only a path. Used to store the path from the recorded video, to be able to use the same Upload/Decrypt methods as with local stored videos.
        if (_previousScene == _linkMenuScene)
        {
            _isSavedInDB = true;
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

        if (!LocalVideosRow.activeSelf)
            LocalVideosRow.SetActive(true);

        LocalVideos.ClearOptions();
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
    #endregion

    #region Delete button
    void DeleteVideo()
    {
        //If video file has been successfully deleted - delete the video from db and remove from list if previous scene was linkmenu / return to linkmenu if previous scene was record 

#if UNITY_IOS
        FileInfo info = new FileInfo(_selectedVideo.Path);
        if (info.Exists || info != null)
        {
            Debug.Log("[HANS] IOS exists");      
#else
        if (File.Exists(_selectedVideo.Path))
        {
            Debug.Log("[HANS] Android/Unity exists");      
#endif       
            ErrorMessage.enabled = false;
            _conirmDeleteMessage = Util.CancellableMessageBox(new Rect(0, 0, 300, 200),
                "Du er ved at slette din video " + _selectedVideo.Name +
                ". Er du sikker på at du vil slette denne video?",
                true, Message.Type.Info,
                delegate(Message message, bool value)
                {
                    if (value)
                    {
                        _localPath = _selectedVideo.Path;

                        if (DeleteVideoFile())
                        {
                            if (_previousScene == _linkMenuScene)
                            {
                                //StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                                RemoveVideoFromList();
                            }
                            else if (_previousScene == _recordVideoScene)
                            {
                                if (_isSavedInDB)
                                {
                                    Global.Instance.localVideos.RemoveAt(Global.Instance.localVideos.Count - 1);
                                    //StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                                }
                                SceneLoader.Instance.CurrentScene = 0;
                            }
                        }
                    }
                    _conirmDeleteMessage.Destroy();
                });
        }
        else
        {
            ErrorMessage.text = "Filen du prøver at slette, kunne ikke findes";
            ErrorMessage.enabled = true;
        }
    }

    #endregion

    #region Save button
    void SaveVideoDetails()
    {
        // Remove all '/' as they generate folders on azure
        Name.text.Replace("/", "-");
        //Save the new or updated video details in the db.
        if (ValidInput())
        {
            ErrorMessage.enabled = false;
            if (!_isSavedInDB)
            {
                _selectedVideo = new QrVideo(Guid.NewGuid(), Name.text, Description.text, Global.Instance.videoPath, 0,
                Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
                SaveButton.interactable = false;
                StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
                Global.Instance.localVideos.Add(_selectedVideo); //Add to global - check if UploadQrVideo is successfull first?
                SaveButton.interactable = true;
                _encryptOnDestroy = true;
                _isSavedInDB = true;
            }
            else if (_isSavedInDB)
            {
                UpdateSelectedVideoFromInputFields();
                StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
                if (_previousScene == _linkMenuScene)
                {
                    UpdateVideoList();
                }
                else
                {
                    int lastVideo = Global.Instance.localVideos.Count - 1;
                    Global.Instance.localVideos[lastVideo].Name = _selectedVideo.Name;
                    Global.Instance.localVideos[lastVideo].Description = _selectedVideo.Description;
                    Global.Instance.localVideos[lastVideo].VideoCategoryId = _selectedVideo.VideoCategoryId;
                }

            }
        }
        else
        {
            ErrorMessage.enabled = true;
        }
    }

    void UpdateVideoList()
    {
        // Update Global.localVideos to match _selectedVideo, and refresh LocalVideos.

        int tempValue = LocalVideos.value;

        Global.Instance.localVideos[LocalVideos.value].Name = _selectedVideo.Name;
        Global.Instance.localVideos[LocalVideos.value].Description = _selectedVideo.Description;
        Global.Instance.localVideos[LocalVideos.value].VideoCategoryId = _selectedVideo.VideoCategoryId;

        GetLocalVideos();
        LocalVideos.value = tempValue; //Select the previous selected video instead of the first one in the list, that would otherwise be selected by GetLocalVideos()
    }

    private void EncyptVideoFile()
    {
        ev.EncryptFile(_selectedVideo.Path);
    }

    void OnDestroy()
    {
        //Encrypt the newly recorded video after leaving the scene, if it hasn't been uploaded yet.

        if (_encryptOnDestroy && _previousScene == _recordVideoScene)
        {
            Thread thread = new Thread(EncyptVideoFile);
            thread.Start();
        }
    }
    #endregion

    #region Upload button
    void ConfirmUploadVideo()
    {
        //confirm if you want to upload the video
        Debug.Log("ConfirmUpLoadVideo [HANS]: " + _selectedVideo.Path);
        Debug.Log("[HANS]: " + LocalVideos.value);
        foreach (VideoCategory category in Global.Instance.videoCategories)
        {
            Debug.Log("[HANS] Category: " + "[" + category.Id + "]" + "[" + category.Name + "]");
        }

#if UNITY_IOS
        FileInfo info = new FileInfo(_selectedVideo.Path);
        if (info.Exists || info != null)
        {
            Debug.Log("[HANS] IOS exists");
            ValidateAndUpload();
        }
#else
        if (File.Exists(_selectedVideo.Path))
        {
            Debug.Log("[HANS] Android/Unity exists");
            ValidateAndUpload();
        }
#endif
        else
        {
            ErrorMessage.text = "Failed to find the video file you were trying to upload";
            ErrorMessage.enabled = true;
        }

    }

    void ValidateAndUpload()
    {
        if (ValidInput())
        {
            ErrorMessage.enabled = false;
            UpdateSelectedVideoFromInputFields();

            _confirmUploadMessage = Util.CancellableMessageBox(new Rect(0, 0, 300, 200),
                "Du er ved at uploade din video " + _selectedVideo.Name + ". Denne video er af typen \"" +
                Global.Instance.videoCategories[_selectedVideo.VideoCategoryId].Name +
                "\". Er du sikker på at du vil fortsætte med at uploade videoen?", true, Message.Type.Info,
                delegate(Message message, bool value)
                {
                    if (value)
                    {
                        _uploadString = "Uploader video: " + _progress + "%";
                        _uploadProgressMessage = Util.MessageBox(new Rect(0, 0, 300, 200),
                            _uploadString, Message.Type.Info, false, true);
                        _uploadString = "";
                        am.ProgressChanged += AzureUploadProgress;
                        if (_isSavedInDB || _previousScene == _linkMenuScene)
                        {
                            StartCoroutine(DecryptAndUpload());
                        }
                        else
                        {
                            if (!_isSavedInDB)
                            {
                                _selectedVideo = new QrVideo(Guid.NewGuid(), Name.text, Description.text,
                                    Global.Instance.videoPath, 0,
                                    Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
                            }
                            try
                            {
                                UploadVideoToAzure();
                            }
                            catch (Exception e)
                            {
                                Debug.Log("ConfirmUpLoadVideo [HANS]: " + e.Message);
                            }
                        }
                    }
                    else if (!value)
                    {

                    }
                    _confirmUploadMessage.Destroy();
                });
        }
        else
        {
            ErrorMessage.text = "Please fill out all the forms before trying to upload";
            ErrorMessage.enabled = true;
        }
    }

    IEnumerator DecryptAndUpload()
    {
        _uploadProgressMessage.Text = "Trin 1 af 2\n\nDekrypterer video...";
        yield return new WaitForSeconds(1f); // wait for gui to finish loading before going on to decrypt the video
        ev.DecryptFile(_selectedVideo.Path);
        _uploadString = "Trin 2 af 2\n\n";
        UploadVideoToAzure();
    }

    void UploadVideoToAzure()
    {
        // Try to upload the video to the Azure blob. If successfull, make the video live (db) and delete the video file. If previous scene was linkmenu, remove from list. If previous scene was record, return to menu.
        string blockBlobReference = _selectedVideo.Name.Replace(" ", "") + "_" + _selectedVideo.Id;
        _localPath = _selectedVideo.Path;
        _selectedVideo.Path = blockBlobReference;
        StartCoroutine(am.PutBlob(_localPath, blockBlobReference));
    }


#if UNITY_IOS
    private void AzureUploadProgress(object sender, AzureManager.ProgressEventArgs e)
    {
        progressIndicator += ".";
        _uploadProgressMessage.Text = _uploadString + "Uploader video: " + progressIndicator;

        if (progressIndicator.Length == 10)
        {
            progressIndicator = "";
        }
        else if (e.Progress == 2)
        {
            _uploadProgressMessage.Text = _uploadString + "Uploader video: " + progressIndicator;
            UpdateVideoInDb();
            _uploadProgressMessage.Destroy();
            am.ProgressChanged -= AzureUploadProgress;
        }
    }
#else
    private void AzureUploadProgress(object sender, AzureManager.ProgressEventArgs e)
    {
        _progress = (e.Progress * 100).ToString();
        int index = _progress.IndexOf(".");
        if (index > 0)
        {
            _progress = _progress.Substring(0, index);
            _uploadProgressMessage.Text = _uploadString + "Uploader video: " + _progress + "%";
        }
        else if (e.Progress == 1)
        {
            _uploadProgressMessage.Text = _uploadString + "Uploader video: " + _progress + "%";
            UpdateVideoInDb();
            _uploadProgressMessage.Destroy();
            am.ProgressChanged -= AzureUploadProgress;
        }
    }
#endif

    void UpdateVideoInDb()
    {
        //updates or uploads the video in the db

        DataManager.SuccessChanged += UpdateVideoInDbStatus;
        _selectedVideo.ReleaseDate = DateTime.Now;

        if (_isSavedInDB)
        {
            StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
        }
        else
        {
            StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
        }
    }

    void UpdateVideoInDbStatus(object sender, DataManager.SuccessEventArgs e)
    {
        DataManager.SuccessChanged -= UpdateVideoInDbStatus;
        if (e.Success)
        {
            if (DeleteVideoFile())
            {
                _encryptOnDestroy = false;
                if (_previousScene == _linkMenuScene)
                    RemoveVideoFromList();
                else if (_previousScene == _recordVideoScene)
                    SceneLoader.Instance.CurrentScene = 0;
            }
        }
        else
        {
            ErrorMessage.text = "Failed to upload video to database";
            ErrorMessage.enabled = true;
        }
    }
    #endregion

    void SelectVideo()
    {
        // Set _selectedVideo from the localVideos dropdown, and update the name, description and category fields to match the video.

        _selectedVideo = Global.Instance.localVideos[LocalVideos.value];
        Name.text = _selectedVideo.Name;
        Description.text = _selectedVideo.Description;
        Categories.value = _selectedVideo.VideoCategoryId - 1;
    }

    void UpdateSelectedVideoFromInputFields()
    {
        _selectedVideo.Name = Name.text;
        _selectedVideo.Description = Description.text;
        _selectedVideo.VideoCategoryId = Categories.value + 1;
    }

    bool ValidInput()
    {
        if (Name.text.Length > 0 && Description.text.Length > 0)
            return true;
        return false;
    }

    void RemoveVideoFromList()
    {
        // Remove video from the localVideos dropdown, and refresh LocalVideos.

        Global.Instance.localVideos.Remove(_selectedVideo);
        GetLocalVideos();
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
            ErrorMessage.text = "The file you were trying to upload could not be found";
            ErrorMessage.enabled = true;
            Debug.Log("File not found");
        }
        return false;
    }


}
