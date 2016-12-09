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
    public UnityEngine.UI.Text StatusMessage;

    AzureManager am = new AzureManager();
    EncryptVideo ev = new EncryptVideo();
    private string _progress = "0";
    private string _localPath = "";
    private bool _isSavedInDB, _encryptOnDestroy = false;
    private Message _confirmUploadMessage, _conirmDeleteMessage ,_uploadProgressMessage;
    private string _uploadString;

    private QrVideo _selectedVideo;
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _linkMenuScene = 0;

    
#region Initialization
    //Initializes the various variables depending on last scene. 
    void Start()
    {
        LocalVideosRow.SetActive(false);
        StatusMessage.enabled = false;

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

    //Gets video Categories
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

    //Gets local Videos.
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
    //Adds Listeners
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
    //If video file has been successfully deleted - delete the video from db and remove from list if previous scene was linkmenu / return to linkmenu if previous scene was record 
    void DeleteVideo()
    {
        if (File.Exists(_selectedVideo.Path))
        {
            StatusMessage.enabled = false;
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
                                StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                                RemoveVideoFromList();
                            }
                            else if (_previousScene == _recordVideoScene)
                            {
                                if (_isSavedInDB)
                                {
                                    Global.Instance.localVideos.RemoveAt(Global.Instance.localVideos.Count - 1);
                                    StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
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
            StartCoroutine(DisplayStatusMessage("Filen du prøver at slette, kunne ikke findes", true, 0));
        }
    }
    #endregion

#region Save button
    void SaveVideoDetails()
    {
        //Save the new or updated video details in the db.
        if (ValidInput())
        {
            StatusMessage.enabled = false;
            DataManager.SuccessChanged += SaveVideoStatus;
            ButtonsInteractable(false);
            if (!_isSavedInDB)
            {
                _selectedVideo = new QrVideo(Guid.NewGuid(), Name.text, Description.text, Global.Instance.videoPath, 0,
                Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
                StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
            }
            else if (_isSavedInDB)
            {
                UpdateSelectedVideoFromInputFields();
                StartCoroutine(DataManager.UpdateQrVideo(_selectedVideo));
            }
        }
        else
        {
            StartCoroutine(DisplayStatusMessage("Der skete en fejl, videon blev ikke gemt", true, 0));
        }
    }

    void SaveVideoStatus(object sender, DataManager.SuccessEventArgs e)
    {
        DataManager.SuccessChanged -= SaveVideoStatus;
        if (e.Success)
        {
            StartCoroutine(DisplayStatusMessage("Videoen blev gemt", false, 5f));
            ButtonsInteractable(true);
            if (!_isSavedInDB)
            {
                Global.Instance.localVideos.Add(_selectedVideo); //Add to global - check if UploadQrVideo is successfull first?
                _encryptOnDestroy = true;
                _isSavedInDB = true;
            } else
            {
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

        } else
        {
            StartCoroutine(DisplayStatusMessage("Der skete en fejl, videon blev ikke gemt.", true, 0));
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
        if (File.Exists(_selectedVideo.Path))
        {
            if (ValidInput())
            {
                StatusMessage.enabled = false;
                UpdateSelectedVideoFromInputFields();
                _confirmUploadMessage = Util.CancellableMessageBox(new Rect(0, 0, 300, 200),
                    "Du er ved at uploade din video " + _selectedVideo.Name + ". Denne video er af typen \"" +
                    Global.Instance.videoCategories[_selectedVideo.VideoCategoryId-1].Name +
                    "\". Er du sikker på at du vil fortsætte med at uploade videoen?", true, Message.Type.Info,
                    delegate(Message message, bool value)
                    {
                        if (value)
                        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IOS
                            _uploadString = "Uploader video: " + _progress + "%";
#endif
#if UNITY_ANDROID
                            _uploadString = "Uploader video...";
#endif
                            _uploadProgressMessage = Util.MessageBox(new Rect(0, 0, 300, 200),
                                _uploadString, Message.Type.Info, false, true);
                            _uploadString = "";
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IOS
                            am.ProgressChanged += AzureUploadProgress;
#endif
#if UNITY_ANDROID
                            am.ProgressChanged += AzureUploadAndroid;
#endif
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
                                UploadVideoToAzure();
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
                StartCoroutine(DisplayStatusMessage("Venligst udfyldd alle felter før du forsøger at uploade.", true, 0));
            }
        }
        else
        {
            StartCoroutine(DisplayStatusMessage("Fejl: Kunne ikke finde filen du forsøgte at uploade", true, 0));
        }

    }

    IEnumerator DecryptAndUpload()
    {
        _uploadProgressMessage.Text = "Trin 1 af 2\n\nDekrypterer video...";
        yield return new WaitForSeconds(1f); // wait for gui to finish loading before going on to decrypt the video
        ev.DecryptFile(_selectedVideo.Path);
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IOS
        _uploadString = "Trin 2 af 2\n\n";
#endif
#if UNITY_ANDROID
        _uploadProgressMessage.Text = "Trin 2 af 2\n\nUploader video...";
         //yield return new WaitForSeconds(1f);
#endif
        UploadVideoToAzure();
            
    }

    void UploadVideoToAzure()
    {
        // Try to upload the video to the Azure blob. If successfull, make the video live (db) and delete the video file. If previous scene was linkmenu, remove from list. If previous scene was record, return to menu.
        string blockBlobReference = _selectedVideo.Name.Replace(" ", "") + "_" + _selectedVideo.Id;
        _localPath = _selectedVideo.Path;
        _selectedVideo.Path = blockBlobReference;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_IOS
        StartCoroutine(am.PutBlob(_localPath, blockBlobReference));
#endif
#if UNITY_ANDROID
        StartCoroutine(am.PutBlobAndroid(_localPath, blockBlobReference));
#endif

    }

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
            _uploadString = "";
        }
    }

    private void AzureUploadAndroid(object sender, AzureManager.ProgressEventArgs e)
    {
        if(e.Progress == 1)
        {
            am.ProgressChanged -= AzureUploadAndroid;
            am.ProgressBar = 0;
            _uploadProgressMessage.Destroy();
            UpdateVideoInDb();
        }
    }

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
            StartCoroutine(DisplayStatusMessage("Fejl: Kunne ikke uploade videon til databasen.", true, 0));
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
        _selectedVideo.VideoCategoryId = Categories.value +1;
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
            StartCoroutine(DisplayStatusMessage("Fejl: Kunne ikke finde filen du forsøge at uploade", true, 0));
        }
        return false;
    }

    void ButtonsInteractable(bool enabled)
    {
        if(enabled) {
            SaveButton.interactable = true;
            UploadButton.interactable = true;
            DeleteButton.interactable = true;
        } else
        {
            SaveButton.interactable = false;
            UploadButton.interactable = false;
            DeleteButton.interactable = false;
        }
    }

    IEnumerator DisplayStatusMessage(string message, bool error, float displayDuration)
    {
        // Display a status message - if displayDuration is 0, it won't expire

        StatusMessage.text = message;
        if (error)
            StatusMessage.color = Color.red;
        else
            StatusMessage.color = Color.green;
        StatusMessage.enabled = true;

        if (displayDuration > 0)
        {
            yield return new WaitForSeconds(displayDuration);
            StatusMessage.enabled = false;
        }
    }


}
