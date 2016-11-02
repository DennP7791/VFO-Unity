using System;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;
using Text = UnityEngine.UI.Text;

public class VideoDetails : MonoBehaviour
{
    public InputField Name;
    public InputField Description;
    public InputField Password;
    public GameObject PasswordRow;
    public Dropdown Categories;
    public Dropdown UserGroups;
    public Button SaveButton;
    public Button CancelButton;

    private List<string> _categoryList = new List<string>();
    private int _previousScene;
    private int _recordVideoScene = 1003;
    private int _uploadScene = 1005;

	void Start ()
	{
	    _previousScene = SceneLoader.Instance.PreviousScene;
        GetCategories();

	    if (_previousScene == _uploadScene)
	    {
	        Name.text = Global.Instance.CurrentVideo.Name;
            Description.text = Global.Instance.CurrentVideo.Description;
            Categories.value = Global.Instance.CurrentVideo.VideoCategoryId-1;
        }


	    PasswordRow.SetActive(false);
        //GetUserGroups();
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

    void GetUserGroups()
    {
        
    }

    void AddListeners()
    {
        //Categories.onValueChanged.AddListener(
        //    delegate
        //    {
        //        EnableDisablePassword();
        //    });
        SaveButton.onClick.AddListener(
            delegate
            {
                SaveVideoDetails();
            });
        CancelButton.onClick.AddListener(
            delegate
            {
                GoToPreviousScene();
            });

    }

    void EnableDisablePassword()
    {
        //TODO: currently we're disabling password if the category.value doesn't match 2 (individuel forflytning), find a better solution than hardcoding this value.
        if (Categories.value == 2)
        {
            PasswordRow.SetActive(true);
            Debug.Log("enabling password");
        }
        else
        {
            PasswordRow.SetActive(false);
            Debug.Log("diabling password");
        }
    }

    void SaveVideoDetails()
    {
        QrVideo video;
        Debug.Log(_previousScene);
        Debug.Log(Global.Instance.userGroup.GroupName);

        //1003 = record video
        if (_previousScene == _recordVideoScene)
        {
            video = new QrVideo(Name.text, Description.text, Global.Instance.videoPath, 0, Global.Instance.userGroup.Id, Global.Instance.UserId, null, Categories.value);
            StartCoroutine(DataManager.UploadQrVideo(video));
        }

        // upload video
        if (_previousScene == _uploadScene)
        {
            video = Global.Instance.CurrentVideo;
            video.Name = Name.text;
            video.Description = Description.text;
            video.VideoCategoryId = Categories.value + 1;

            StartCoroutine(DataManager.UpdateQrVideo(video));
        }


    }

    void GoToPreviousScene()
    {
        SceneLoader.Instance.CurrentScene = _previousScene;
    }

    void UpdateVideo()
    {
        
    }

    void Cancel()
    {
        
    }

}
