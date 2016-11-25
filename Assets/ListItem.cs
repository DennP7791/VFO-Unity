using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{

    public UnityEngine.UI.Text _name, _detailsName, _detailsDescription;
    public Image _thumbnail;
    public QrVideo _qrVideo;
    public Button videoDetails;


    // Use this for initialization
    void Start()
    {

        videoDetails.onClick.AddListener(ShowDetails);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDetails()
    {

    }

}
