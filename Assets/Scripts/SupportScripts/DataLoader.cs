using System;
using UnityEngine;
using System.Collections;

public class DataLoader : MonoBehaviour {

    private bool noNewVersion = false;
    private bool checkNewVersion = false;
	// Use this for initialization
	void Start () {
        BottomBarScript.SetNoButtons(true);

        Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("data_loader_getting_data"), Message.Type.Info, false, true);

        StartCoroutine(RetrieveVersion());
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (noNewVersion)
        {
            if (!checkNewVersion)
            {
                StartCoroutine(DataManager.RetrieveData());
                StartCoroutine(DataManager.RetrieveQrVideoData());
                StartCoroutine(DataManager.RetrieveVideoCategoryData());
                checkNewVersion = true;
            }
        }
	}

    public static void DownloadNewVersion(Message message, bool value)
    {
        Application.OpenURL("http://vfo.welfaredenmark.com/download/index?lang=" + Global.Instance.ProgramLanguage);
        Application.Quit();
    }

    public IEnumerator RetrieveVersion()
    {
        string url = "https://vfo.welfaredenmark.com/version.txt";

        WWW www = new WWW(url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Current Version: " + Global.Instance.ProgramVersion.ToString() + " - New Version: " + www.text);
            if (isVersionBigger(www.text))
            {
                noNewVersion = false;
                Debug.LogWarning("New version available");
                Util.OkMessageBox(new Rect(0, 0, 400, 200), "Der er en ny version tilgængelig.\n\nDu skal hente og installere den nye version før du kan forsætte.\n\nTryk på ok for at afslutte programmet og åbne websiden med den nye version.", true, Message.Type.Info, DownloadNewVersion);
            }
            else
            {
                noNewVersion = true;
            }
        }
        else
        {
            Debug.Log("No new version");
            noNewVersion = true;
        }
    }

    public bool isVersionBigger(string v)
    {
        bool _v = false;

        float newVersion = -1.0f;
        if (float.TryParse(v, out newVersion))
        {
            if (newVersion > Global.Instance.ProgramVersion)
                _v = true;
        }

        return _v;
    }
}
