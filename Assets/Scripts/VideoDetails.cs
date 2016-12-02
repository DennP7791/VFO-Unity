using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Threading;

public class VideoDetails : MonoBehaviour
{
    public InputField Name;
    public InputField Description;
    public Dropdown LocalVideos;
    public Dropdown Categories;
    public Button SaveButton;
    public Button UploadButton;
    public Button DeleteButton;
    public GameObject LocalVideosRow;
    public UnityEngine.UI.Text ErrorMessage;

    AzureManager am = new AzureManager();
    EncryptVideo ev = new EncryptVideo();
    private string _progress = "0";
    private string _localPath = "";
    private bool _isSavedInDB = false; //make false again
    private Message _confirmUploadMessage;
    private Message _uploadProgressMessage;
    private string _uploadString;

    private QrVideo _selectedVideo;
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _linkMenuScene = 0;

    

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

    void DeleteVideo()
    {
        //If video file has been successfully deleted - delete the video from db and remove from list if previous scene was linkmenu / return to linkmenu if previous scene was record 

        _localPath = _selectedVideo.Path;

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
                if (_isSavedInDB)
                {
                    Global.Instance.localVideos.RemoveAt(Global.Instance.localVideos.Count-1);
                    StartCoroutine(DataManager.DeleteVideo(_selectedVideo.Id));
                }
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
            if (!_isSavedInDB)
            {
                //TODO: only encrypt when leaving scene and no upload was made?
                _selectedVideo = new QrVideo(Guid.NewGuid(), Name.text, Description.text, Global.Instance.videoPath, 0,
                Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value + 1);
                SaveButton.interactable = false; //indicate that button is disabled?
                StartCoroutine(DataManager.UploadQrVideo(_selectedVideo));
                Global.Instance.localVideos.Add(_selectedVideo); //Add to global - check if UploadQrVideo is successfull first?
                SaveButton.interactable = true;
                Thread thread = new Thread(EncyptVideoFile);
                thread.Start();
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

    void ConfirmUploadVideo()
    {
        //confirm if you want to upload the video
        if (ValidInput() && File.Exists(_selectedVideo.Path))
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
                        _uploadString = "Uploader video: " + _progress + "%";
                        _uploadProgressMessage = Util.MessageBox(new Rect(0, 0, 300, 200),
                            _uploadString, Message.Type.Info, false, true);
                        _uploadString = "";
                        am.ProgressChanged += Progress;
                        if (_isSavedInDB || _previousScene == _linkMenuScene)
                        {
                            StartCoroutine(DecryptAndUpload());
                        }
                        else
                        {
                            if (!_isSavedInDB)
                            {
                                _selectedVideo = new QrVideo(Guid.NewGuid(), Name.text, Description.text, Global.Instance.videoPath, 0,
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

    private void Progress(object sender, AzureManager.ProgressEventArgs e)
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
            UploadVideo();
            _uploadProgressMessage.Destroy();
            am.ProgressChanged -= Progress;
        }
    }

    void UploadVideoToAzure()
    {
        // Try to upload the video to the Azure blob. If successfull, make the video live (db) and delete the video file. If previous scene was linkmenu, remove from list. If previous scene was record, return to menu.
        string blockBlobReference = _selectedVideo.Name.Replace(" ", "") + "_" + _selectedVideo.Id;
        _localPath = _selectedVideo.Path;
        _selectedVideo.Path = blockBlobReference;
        StartCoroutine(am.PutBlob(_localPath, blockBlobReference));
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
            // TODO: online upload if not saved, otherwise update
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

    void RemoveVideoFromList()
    {
        // Remove video from the localVideos dropdown, and refresh LocalVideos.

        Global.Instance.localVideos.Remove(_selectedVideo);
        GetLocalVideos();
    }

    private void EncyptVideoFile()
    {
        ev.EncryptFile(_selectedVideo.Path);
    }
}
