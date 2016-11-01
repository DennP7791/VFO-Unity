using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

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
    private int PreviousScene;

	void Start ()
	{
	    PreviousScene = SceneLoader.Instance.PreviousScene;
	    PasswordRow.SetActive(false);
        GetCategories();
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
                ClickSave();
            });
        CancelButton.onClick.AddListener(
            delegate
            {
                ClickCancel();
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

    void ClickSave()
    {
        //save if previous scene was record video
        if (PreviousScene == 1003)
        {
            
        }
    }

    void ClickCancel()
    {
        SceneLoader.Instance.CurrentScene = PreviousScene;
    }

    void UpdateVideo()
    {
        
    }

    void Cancel()
    {
        
    }

}
