using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Text : MonoBehaviour
{
    // use this class to get text from db and pass it to the program 

    private static Text _instance; //singleton

    public static Text Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Text)GameObject.FindObjectOfType(typeof(Text));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Text";
                    _instance = (Text)container.AddComponent(typeof(Text));
                }
            }

            return _instance;
        }
    }

    private string language = "dk"; //"se";
    private List<string> id = new List<string>();
    private List<string> str = new List<string>();
    private List<string> spk = new List<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        TextAsset t = (TextAsset)Resources.Load("language_" + language, typeof(TextAsset));
        XMLParser p = new XMLParser();
        XMLNode n = p.Parse(t.text);
        foreach (XMLNode node in n.GetNodeList("texts>0>text"))
        {
            XMLNode n1 = node.GetNode("id>0");
            XMLNode n2 = node.GetNode("string>0");
            XMLNode n3 = node.GetNode("speak>0");

            string _id = n1.GetValue("_text");
            if (id.Contains(_id))
            {
                Debug.LogWarning("Warning: There is alread an id named '" + _id + "' added to the text list");
            }
            id.Add(_id);

            if (n2 == null)
            {
                Debug.LogWarning("Couldn't find text for id: '" + _id + "'");
            }

            string _spk = "";
            if (n3 != null)
            {
                _spk = n3.GetValue("_text");
            }
            spk.Add(_spk);

            str.Add(n2.GetValue("_text"));
        }
    }

    //void PlaySpeakStream(string name) //MC 02-08-2016 - Testing method for streaming audio
    //{
    //    string url = "http://vfoclient.welfaredenmark.com" + "/speak_app/" + Global.Instance.ProgramLanguage + "/" + name + ".mp3";

    //    WWW www = new WWW(url);

    //    AudioSource audio = GetComponent<AudioSource>();
    //    audio.clip = www.GetAudioClip(false, true, AudioType.MPEG);
    //    audio.volume = 1.0f;
    //    audio.Play();
    //}

    IEnumerator PlaySpeak(string name)
    {
        GetComponent<AudioSource>().Stop();
        //string path = Application.dataPath + "/speak/" + Global.Instance.ProgramLanguage + "/" + name; //MC 26-07-2016 - Deprecated - Looks like sound files are stored online to limit application size

        string path = "http://vfoclient.welfaredenmark.com";

#if UNITY_ANDROID || UNITY_IOS
        path += "/speak_app/" + Global.Instance.ProgramLanguage + "/" + name + ".mp3"; ; //MC 27-07-2016 - Added to include new correct path to sound NB: Only danish sound files are located here, new path to speak_app in mp3.
#else
        path += "/speak/" + Global.Instance.ProgramLanguage + "/" + name + ".ogg";
#endif

        WWW wwwaudio = new WWW(path);

        GetComponent<AudioSource>().clip = wwwaudio.GetAudioClip(false, true); //MC introduced to do stream instead of download
        GetComponent<AudioSource>().spatialBlend = 0.0f;

        while (GetComponent<AudioSource>().clip.loadState != AudioDataLoadState.Loaded) //REFACTORED MC 21-06-2016 (!GetComponent<AudioSource>().clip.isReadyToPlay) --> (GetComponent<AudioSource>().clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return wwwaudio;
        }

        if (GetComponent<AudioSource>().time > GetComponent<AudioSource>().clip.length - 2)
        {
            GetComponent<AudioSource>().clip = wwwaudio.GetAudioClip(false, true); //MC introduced to do stream instead of download
        }

        GetComponent<AudioSource>().Play();
        wwwaudio.Dispose();
        wwwaudio = null;
    }

    public void StopAudio()
    {
        StopAllCoroutines();
        GetComponent<AudioSource>().Stop();
    }

    /// <summary>
    /// Get text string from the language xml file and plays speak
    /// </summary>
    /// <param name="_id">the text strings id</param>
    /// <returns></returns>
    public string GetStringAndPlaySpeak(string _id)
    {
        string s = "Not Found " + _id;

        int pos = id.IndexOf(_id);
        if (pos != -1)
        {
            s = str[pos];
            if (spk[pos] != "")
            {
                StopCoroutine("PlaySpeak");
                StartCoroutine("PlaySpeak", spk[pos]);
            }
        }

        return s;
    }

    /// <summary>
    /// Play speak from XML file
    /// </summary>
    /// <param name="_id">the text strings id</param>
    /// <returns></returns>
    public void PlaySpeakFromID(string _id)
    {
        int pos = id.IndexOf(_id);
        if (pos != -1)
        {
            if (spk[pos] != "")
            {
                StopCoroutine("PlaySpeak");
                StartCoroutine("PlaySpeak", spk[pos]);
            }
        }
    }

    /// <summary>
    /// Get text string from the language xml file
    /// </summary>
    /// <param name="_id">the text strings id</param>
    /// <returns></returns>
    public string GetString(string _id)
    {
        string s = "Not Found: " + _id;

        int pos = id.IndexOf(_id);
        if (pos != -1)
        {
            s = str[pos];
        }

        return s;
    }
}
