using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ListItemController : MonoBehaviour
{

    public InputField inputfield;
    public Dropdown dropdown;
    public Button searchButton;
    public Button rightButton, leftButton;
    public GameObject listItem;
    public Sprite[] spriteList;
    public GameObject contentPanel;

    private List<VideoCategory> videoCatagoryList;
    private List<VideoCategory> newVideoCatagoryList;

    private List<QrVideo> videoList;
    private List<QrVideo> searchList;
    private List<QrVideo> pagelist;

    public static GameObject detailsPanel;
    public static QrVideo _selectedVideo;
    public static UnityEngine.UI.Text _detailsName, _detailsDescription;
    public Button _loadvideoButton, _cancelButton;
    


    // Use this for initialization
    void Start()
    {
        detailsPanel = GameObject.Find("Details");
        _detailsName = GameObject.Find("DetailsName").GetComponent<UnityEngine.UI.Text>();
        _detailsDescription = GameObject.Find("DetailsDescription").GetComponent<UnityEngine.UI.Text>();
        detailsPanel.SetActive(false);
        Initialize();
    }

    public void EnableDetails()
    {
        detailsPanel.SetActive(true);
        _detailsName.text = _selectedVideo.Name;
        _detailsDescription.text = _selectedVideo.Description;
    }

    public void DisableDetails()
    {
        detailsPanel.SetActive(false);
    }

    public void ChangeScene()
    {
        Global.Instance.videoPath = _selectedVideo.Path;
        SceneLoader.Instance.CurrentScene = 1002;
    }

    void Initialize()
    {
        videoCatagoryList = Global.Instance.videoCategories;
        videoList = Global.Instance.RetrieveReleasedQrVideo;

        populateDropdown();
        populateVideoes(videoList);

        _cancelButton.onClick.AddListener(DisableDetails);
        _loadvideoButton.onClick.AddListener(ChangeScene);

        searchButton.onClick.AddListener(SearchVideo);
        rightButton.onClick.AddListener(NextPage);
        leftButton.onClick.AddListener(PreviousPage);
        inputfield.gameObject.GetComponent<InputField>();
        dropdown.gameObject.GetComponent<Dropdown>();
    }

    private void populateDropdown()
    {
        newVideoCatagoryList = new List<VideoCategory>();
        newVideoCatagoryList = videoCatagoryList;
        var videoCatagoryRemove = newVideoCatagoryList.SingleOrDefault(r => r.Id == 3);
        if (videoCatagoryRemove != null)
            newVideoCatagoryList.Remove(videoCatagoryRemove);
        foreach (var item in newVideoCatagoryList)
        {
            dropdown.options.Add(new Dropdown.OptionData(item.Name));
        }
    }

    void populateVideoes(List<QrVideo> qrVidList)
    {
        Debug.Log("populateVideoes");
        //qrVidList.Skip(2).Take(2).ToList();

        foreach (var item in qrVidList)
        {
            GameObject newListItem = GameObject.Instantiate(listItem);
            ListItem controller = newListItem.GetComponent<ListItem>();
            controller._name.text = item.Name;
            controller._qrVideo = item;
            if (item.VideoCategoryId == 1)
            {
                controller._thumbnail.sprite = spriteList[0];
            }
            else
            {
                controller._thumbnail.sprite = spriteList[1];
            }
            newListItem.transform.parent = contentPanel.transform;
        }
    }

    private void DestroyAllListItems()
    {
        var listItems = GameObject.FindGameObjectsWithTag("list_item");

        foreach (var item in listItems)
        {
            Destroy(item);
        }
    }

    private void SearchVideo()
    {
        string input = inputfield.text.ToLower();
        searchList = new List<QrVideo>();
        DestroyAllListItems();
        foreach (var item in videoList)
        {
            Debug.Log(item.VideoCategoryId);
            Debug.Log(dropdown.value);
            if (dropdown.value == item.VideoCategoryId && item.Name.ToLower().Contains(input))
            {
                //VEJLEDNING
                searchList.Add(item);
            }
            else if (dropdown.value == item.VideoCategoryId && item.Name.ToLower().Contains(input))
            {
                //FORFLYTNING
                searchList.Add(item);
            }
            else if (dropdown.value == 0 && item.Name.ToLower().Contains(input))
            {
                //ALT
                searchList.Add(item);
            }
        }
        
        populateVideoes(searchList);
        //Debug.Log(input);
        //Debug.Log(dropdown.value);
    }



    private void NextPage()
    {
        pagelist = new List<QrVideo>();
        
        if (videoList.Count > 24)
        {
            for (int i = 0; i < 24; i++)
            {
                pagelist = (List<QrVideo>)videoList.Skip(i * 24).Take(25);
            }
            DestroyAllListItems();
            populateVideoes(pagelist);
        }


    }

    private void PreviousPage()
    {

    }


}
