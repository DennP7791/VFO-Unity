using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{

    public UnityEngine.UI.Text _name;
    public UnityEngine.UI.Text _count;
    public Image _thumbnail;
    public QrVideo _qrVideo;
    public Button videoDetails;
    public GameObject detailsPanel;

    // Use this for initialization
    void Start()
    {
        videoDetails.onClick.AddListener(ShowDetails);
    }

    /// <summary>
    /// Sets up the details panel.
    /// </summary>
    public void ShowDetails()
    {
        ListItemController lic = new ListItemController();
        ListItemController._selectedVideo = _qrVideo;
        lic.EnableDetails();

    }

}
