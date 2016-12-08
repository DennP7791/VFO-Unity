using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class ListItemController : MonoBehaviour
{

    public InputField inputfield;
    public Dropdown dropdown;
    public Button searchButton;
    public Button rightButton, leftButton;
    public GameObject listItem;
    public Sprite[] spriteList;
    public GameObject contentPanel;
    public UnityEngine.UI.Text videoCount;
    public UnityEngine.UI.Text pageNumber;
    public UnityEngine.UI.Text noVideoes;
    int pageNr = 1;

    private List<VideoCategory> videoCatagoryList;
    private List<VideoCategory> newVideoCatagoryList;
    private List<QrVideo> maxNumberPrPage;

    private List<QrVideo> videoList;
    private List<QrVideo> searchList;
    private List<QrVideo> pagelist;

    public static GameObject detailsPanel;
    public static QrVideo _selectedVideo;
    public static UnityEngine.UI.Text _detailsName, _detailsDescription;
    public Button _loadvideoButton, _cancelButton;

    private bool nextPageClicked = false;
    private bool isSearched = false;

    /// <summary>
    /// Used to initialized variables by finding the correct gameobjects in the hierachy.
    /// </summary>
    void Start()
    {
        detailsPanel = GameObject.Find("Details");
        _detailsName = GameObject.Find("DetailsName").GetComponent<UnityEngine.UI.Text>();
        _detailsDescription = GameObject.Find("DetailsDescription").GetComponent<UnityEngine.UI.Text>();
        detailsPanel.SetActive(false);
        Initialize();
    }

    /// <summary>
    /// Used to enable Details on video click and to set the text of the gameObjects.
    /// </summary>
    public void EnableDetails()
    {
        detailsPanel.SetActive(true);
        _detailsName.text = _selectedVideo.Name;
        _detailsDescription.text = _selectedVideo.Description;
    }
    /// <summary>
    /// Disables details.
    /// </summary>
    public void DisableDetails()
    {
        detailsPanel.SetActive(false);
    }
    /// <summary>
    /// Sets the correct video path and changes scene to video player, when called.
    /// </summary>
    public void ChangeScene()
    {
        Global.Instance.videoPath = _selectedVideo.Path;
        SceneLoader.Instance.CurrentScene = 1002;
    }

    /// <summary>
    /// Used to Initialize variables, and gameObjects.
    /// </summary>
    void Initialize()
    {
        maxNumberPrPage = new List<QrVideo>();
        videoCatagoryList = Global.Instance.videoCategories;
        videoList = Global.Instance.qrVideos;

        populateDropdown();
        maxNumberPrPage = videoList.Take(25).ToList();
        populateVideoes(maxNumberPrPage);
        videoCount.text = maxNumberPrPage.Count.ToString() + "/" + videoList.Count.ToString();
        if (pageNr == 1)
        {
            var pnb = 0;
            var count = pnb * 25 + maxNumberPrPage.Count;
            setPageNumber(count, videoList); //set pageNumber
        }

        _cancelButton.onClick.AddListener(DisableDetails);
        _loadvideoButton.onClick.AddListener(ChangeScene);

        searchButton.onClick.AddListener(SearchVideo);
        rightButton.onClick.AddListener(NextPage);
        leftButton.onClick.AddListener(PreviousPage);
        noVideoes.enabled = false;
    }
    /// <summary>
    /// Instantiates list of catagories, and populates our dropdown.
    /// </summary>
    private void populateDropdown()
    {
        List<VideoCategory> newVCL;
        newVCL = videoCatagoryList;
        if (newVCL != null)
        {
            var videoCatagoryRemove = newVCL.Take(2).ToList();
            foreach (var item in videoCatagoryRemove)
            {
                dropdown.options.Add(new Dropdown.OptionData(item.Name));
            }
        }
    }
    /// <summary>
    /// Instantiates List items from script. 
    /// </summary>
    /// <param name="qrVidList"></param>
    void populateVideoes(List<QrVideo> qrVidList)
    {
        foreach (var item in qrVidList)
        {
            GameObject newListItem = GameObject.Instantiate(listItem);
            ListItem controller = newListItem.GetComponent<ListItem>();
            controller._name.text = item.Name;
            controller._count.text = item.Count.ToString();
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
            newListItem.transform.localScale = contentPanel.transform.localScale;
        }
    }

    /// <summary>
    /// Used to clear all listItems.
    /// </summary>
    private void DestroyAllListItems()
    {
        var listItems = GameObject.FindGameObjectsWithTag("list_item");

        foreach (var item in listItems)
        {
            Destroy(item);
        }
    }

    /// <summary>
    /// Search logic, used to search by Text and downdown value.
    /// </summary>
    private void SearchVideo()
    {
        string input = inputfield.text.ToLower();
        maxNumberPrPage = new List<QrVideo>();
        searchList = new List<QrVideo>();
        DestroyAllListItems();
        foreach (var item in videoList)
        {
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
        if (searchList.Count != 0)
        {
            isSearched = true;
            pageNr = 1;
            maxNumberPrPage = searchList.Take(25).ToList();
            populateVideoes(maxNumberPrPage);
            videoCount.text = maxNumberPrPage.Count.ToString() + "/" + searchList.Count.ToString();
            noVideoes.enabled = false;
            if (pageNr == 1)
            {
                var pnb = 0;
                var count = pnb * 25 + maxNumberPrPage.Count;
                setPageNumber(count, searchList); //set pageNumber
            }
        }
        else
        {
            videoCount.text = maxNumberPrPage.Count.ToString() + "/" + searchList.Count.ToString();
            noVideoes.enabled = true;
            if (pageNr == 1)
            {
                var pnb = 0;
                var count = pnb * 25 + maxNumberPrPage.Count;
                setPageNumber(count, searchList); //set pageNumber
            }
        }

    }
    /// <summary>
    /// Changes page, and fills the page with the correct list Items.
    /// </summary>
    private void NextPage()
    {

        if (pageNr == 0)
        {
            pageNr = 1;
        }

        if (!isSearched)
        {
            if (videoList.Count >= pageNr * 25)
            {
                SetUpList();
                pageNr++;
                nextPageClicked = true;
            }
        }
        else
        {
            if (searchList.Count >= pageNr * 25)
            {
                SetUpList();
                pageNr++;
                nextPageClicked = true;
            }
        }
    }
    /// <summary>
    /// Sets up the list, depending on the page nr.
    /// </summary>
    private void SetUpList()
    {
        pagelist = new List<QrVideo>();

        if (searchList != null)
        {
            if (searchList.Count > 25)
            {
                for (int i = 0; i < 25; i++)
                {
                    pagelist = searchList.Skip(pageNr * 25).Take(25).ToList();
                }
                DestroyAllListItems();
                populateVideoes(pagelist);
                var count = pageNr * 25 + pagelist.Count;
                videoCount.text = count + "/" + searchList.Count.ToString();
                setPageNumber(count, searchList); //set pageNumber
            }
        }
        else
        {
            if (videoList.Count > 25)
            {
                for (int i = 0; i < 25; i++)
                {
                    pagelist = videoList.Skip(pageNr * 25).Take(25).ToList();
                }
                DestroyAllListItems();
                populateVideoes(pagelist);
                var count = pageNr * 25 + pagelist.Count;
                videoCount.text = count + "/" + videoList.Count.ToString();
                setPageNumber(count, videoList); //set pageNumber
            }
        }
    }

    /// <summary>
    /// Reduces page nr by 1, if the page is not page 0.
    /// </summary>
    private void PreviousPage()
    {
        if (nextPageClicked)
        {
            pageNr--;
            nextPageClicked = false;
        }

        if (pageNr >= 1)
        {
            pageNr--;
            SetUpList();
        }
    }
    /// <summary>
    /// Sets the page number in the button of the panel.
    /// </summary>
    /// <param name="allPageVideos"></param>
    /// <param name="allvideos"></param>
    private void setPageNumber(int allPageVideos, List<QrVideo> allvideos)
    {
        if (pageNr == 0)
        {
            pageNr++;
        }
        pageNumber.text = Math.Ceiling((double)allPageVideos / 25) + "/" + Math.Ceiling((double)allvideos.Count / 25);
    }


}
