using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using ExerciseCollections;
using System;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{

    static string url = "";

    private static bool DEBUG = false;


    static void InitializeUrl()
    {
        //static string url = "https://vfo.welfaredenmark.com/Service/"; //Production environment service

        //url = "http://localhost:59477/Service/"; //LOCAL SERVICE - Comment for release version

        url = "http://vfo-staging-webapp.azurewebsites.net/Service/"; //STAGING SERVICE - Comment for release version
        //url = "http://vfo-staging-webapp.azurewebsites.net/Service/";
#if UNITY_EDITOR
        url = "http://localhost:59477/Service/";
#endif

    }

    //Classes used in Json Data Serialization and Deserialization
    #region Json Data container classes

    public class JsonRequestArgs
    {
        public string method;
        public string @params;
        public int id;
    }

    //public class JsonExercisePart
    //{
    //    public int Id;
    //    public string Name;
    //    public bool Completed;
    //    public double Time;

    //    public JsonExercisePart()
    //    {
    //    }

    //    public JsonExercisePart(int id, string name, bool completed, double time)
    //    {
    //        Id = id;
    //        Name = name;
    //        Completed = completed;
    //        Time = time;
    //    }

    //    public override string ToString()
    //    {
    //        return Id + " " + Name + " " + Completed + " " + Time + "; ";
    //    }
    //}

    public class JsonBaseExercise
    {
        public int Id;
        public double Score;
        public string Name;

        public JsonBaseExercise()
        {
        }

        public JsonBaseExercise(int id, string name, double score)
        {
            Id = id;
            Score = score;
            Name = name;
        }

        public override string ToString()
        {
            return "( " + Id + " " + Score + " " + Name + " )";
        }
    }

    public class JsonCategoryCollection
    {
        public int UserId;
        public JsonCategory[] Categories;

        public override string ToString()
        {
            string result = UserId + " < ";
            foreach (JsonCategory c in Categories)
            {
                result += c.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }

    public class JsonQrVideoCollection
    {
        public JsonQrVideo[] QrVideos;
        public override string ToString()
        {
            string result = "";
            foreach (JsonQrVideo qrv in QrVideos)
            {
                result += qrv.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }


    public class JsonVideoCategoryCollection
    {
        public JsonVideoCategory[] VideoCategories;

        public override string ToString()
        {
            string result = "";
            foreach (JsonVideoCategory qrv in VideoCategories)
            {
                result += qrv.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }

    public class JsonVideoUserViewCollection
    {
        public JsonQrVideoUserView[] VideoUserView;

        public override string ToString()
        {
            string result = "";
            foreach (JsonQrVideoUserView jsonVUV in VideoUserView)
            {
                result += jsonVUV.ToString() + " ";
            }
            result += ">";
            return result;
        }
    }

    public class JsonQrVideoUserView
    {
        public Guid VideoId;
        public int UserId;
        public DateTime ViewDate;

        public JsonQrVideoUserView()
        {

        }

        public JsonQrVideoUserView(Guid videoId, int userId, DateTime viewDate)
        {
            VideoId = videoId;
            UserId = userId;
            ViewDate = viewDate;
        }

        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";

            result += "}";
            return result;
        }
    }

    public class JsonQrVideo
    {
        public Guid Id;
        public string Name;
        public string Description;
        public string Path;
        public int Count;
        public int UserGroupId;
        public int UserId;
        public DateTime? ReleaseDate;
        public int VideoCategoryId;

        public JsonQrVideo()
        {
        }

        public JsonQrVideo(Guid id)
        {
            Id = id;
        }

        public JsonQrVideo(Guid id, string name, string description, string path, int count, int userGroupId, int userId, DateTime? releaseDate, int videoCategoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Path = path;
            Count = count;
            UserGroupId = userGroupId;
            UserId = userId;
            ReleaseDate = releaseDate;
            VideoCategoryId = videoCategoryId;
        }

        public JsonQrVideo(string name, string description, string path, int userId, int videoCategoryId)
        {
            Name = name;
            Description = description;
            Path = path;
            UserId = userId;
            VideoCategoryId = videoCategoryId;
        }

        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";

            result += "}";
            return result;
        }
    }

    public class JsonUserGroup
    {
        public int Id;
        public string GroupName;
        public int? CustomerId;
    }

    public class JsonCategory : JsonBaseExercise
    {
        public JsonExercise[] Exercises;

        public JsonCategory()
        {
        }

        public JsonCategory(int id, string name, double score)
            : base(id, name, score)
        {
        }

        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";
            foreach (JsonExercise e in Exercises)
            {
                result += e.ToString() + " ";
            }
            result += "}";
            return result;
        }
    }

    public class JsonVideoCategory
    {
        public int Id;
        public string Name;
        public JsonVideoCategory()
        {
        }

        public JsonVideoCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            string result = base.ToString() + " " + "{ ";

            result += "}";
            return result;
        }
    }

    public class JsonExercise : JsonBaseExercise
    {
        public int SceneFunction;
        public bool Attempted;
        //public JsonExercisePart[] Parts;


        public JsonExercise()
        {
        }

        public JsonExercise(int id, string name, double score, int sceneFunction, bool attempted)
            : base(id, name, score)
        {
            SceneFunction = sceneFunction;
            Attempted = attempted;
        }

        public override string ToString()
        {
            string result = base.ToString() + "Function " + SceneFunction + "[ ";
            //foreach (JsonExercisePart p in Parts)
            //{
            //    result +=p.ToString() + " ";
            //}
            result += "]";
            return result;
        }
    }

    public class JsonUserGroupVideoCredential
    {
        public int Id;
        public int VideoCategoryId;
        public int UserGroupId;
        public string Password;
        public string Salt;

        public JsonUserGroupVideoCredential()
        {

        }

        public JsonUserGroupVideoCredential(int id, int videoCategoryId, int userGroupId, string password, string salt)
        {
            Id = id;
            VideoCategoryId = videoCategoryId;
            UserGroupId = userGroupId;
            Password = password;
            Salt = salt;
        }
    }

    public class JsonCredentials
    {
        public string Username = "";
        public string Password = "";

        public JsonCredentials(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override string ToString()
        {
            return "Username: " + Username + ", Password: " + Password + "";
        }
    }


    #endregion

    public static void WrongUserClicked(Message message, bool value)
    {
        if (Global.Instance.ProgramLanguage == "sv-SE")
            Application.OpenURL("http://vfo.welfaresverige.se");
        else
            Application.OpenURL("https://vfo.welfaredenmark.com");

        Application.Quit();
    }

    public static IEnumerator ValidateCredentials(JsonCredentials credentials)
    {
        InitializeUrl();

        string _url = url + "Authorize/";
        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/Authorize/"; //OutComment if release version
        }

        Debug.Log("Validating Credentials -> " + credentials);

        string serialized = JsonWriter.Serialize(credentials);

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);

        WWW www = new WWW(_url, bytes);

        yield return www;
        // check for errors

        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Global.Instance.UserId = int.Parse(www.text);
            if (Global.Instance.UserId == -1)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("data_manager_wrong_username_password"), Message.Type.Error, true, true);
            }
            else if (Global.Instance.UserId == -1000)
            {
                Util.OkMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("data_manager_admin_loging_error"), true, Message.Type.Info, WrongUserClicked);
            }
            else if (Global.Instance.UserId == -9000)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("data_manager_general_error"), Message.Type.Error, true, true);
            }
            else
            {
                Debug.Log("Upload Complete.");
                Application.LoadLevel("Loading");
            }
        }
        else
        {
            Debug.Log(www.error);
            Util.MessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("data_manager_connect_error"), Message.Type.Error, true, true);
        }

        GameObject go = GameObject.Find("infomessage");
        if (go)
            GameObject.Destroy(go);
    }

    public static IEnumerator RetrieveData()
    {
        InitializeUrl();
        string requestMethod = "GET";
        string _url = url + "GetExercises/" + Global.Instance.UserId + "/" + "da-DK";
        Uri uri = new Uri(_url);
        using (UnityWebRequest webRequest = new UnityWebRequest())
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            webRequest.downloadHandler = downloadHandler;

            webRequest.method = requestMethod;
            webRequest.url = uri.ToString();

            webRequest.Send();

            while (!webRequest.isDone)
            {
                yield return null;
            }
            if (webRequest.isDone && webRequest.error == null)
            {
                JsonCategoryCollection cc = JsonReader.Deserialize<JsonCategoryCollection>(webRequest.downloadHandler.text);
                Debug.Log("Deserialized:\n" + cc.ToString());
                Global.Instance.categoryCollection = JsonCategoryCollectionToExerciseCategoryCollection(cc);
                Debug.Log("Converted to ExercizeCategoryCollection:\n" + Global.Instance.categoryCollection.ToString());
                Global.Instance.LoadMain();
            }
        }

    }

    //public static IEnumerator RetrieveData()
    //{
    //    Debug.Log("Retrieving Data");

    //    string url = "https://vfo.welfaredenmark.com/Service/GetExercises/" + Global.Instance.UserId + "/" + "da-DK"; //Production environment service
    //    url = "http://localhost:59477/Service/GetExercises/" + Global.Instance.UserId + "/" + "da-DK"; //LOCAL SERVICE - Comment for release version
    //    //url = "http://vfo-staging-webapp.azurewebsites.net/Service/GetExercises/" + Global.Instance.UserId + "/" + "da-DK"; //STAGING SERVICE - Comment for release version

    //    if (Global.Instance.ProgramLanguage == "sv-SE")
    //    {
    //        //url = "http://vfo.welfaresverige.se/Service/GetExercises/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
    //    }

    //    WWW www = new WWW(url);

    //    yield return www;

    //    if (www.error == null)
    //    {
    //        Debug.Log("Result:\n" + www.text);
    //        try
    //        {
    //            JsonCategoryCollection cc = JsonReader.Deserialize<JsonCategoryCollection>(www.text);
    //            Debug.Log("Deserialized:\n" + cc.ToString());
    //            Global.Instance.categoryCollection = JsonCategoryCollectionToExerciseCategoryCollection(cc);
    //            Debug.Log("Converted to ExercizeCategoryCollection:\n" + Global.Instance.categoryCollection.ToString());
    //            Global.Instance.LoadMain();
    //        }
    //        catch (Exception e)
    //        {
    //            Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("WWW Error: " + www.error);
    //    }
    //}

    public static IEnumerator UploadData()
    {
        InitializeUrl();
        string _url = url + "SaveData/";

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/SaveData/"; //OutComment if release version
        }

        Debug.Log("Uploading Data:\n" + Global.Instance.categoryCollection);
        JsonCategoryCollection cc = ExerciseCategoryCollectionToJsonCategoryCollection(Global.Instance.categoryCollection);

        Debug.Log("Converted To Json Container:\n" + cc.ToString());
        string serialized = JsonWriter.Serialize(cc);

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);

        WWW www = new WWW(_url, bytes);

        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Debug.Log("Upload Complete.");
        }
        else
        {
            Debug.Log("Upload Error: " + www.error);
        }
    }

    public static IEnumerator RetrieveVideoCategoryData()
    {
        InitializeUrl();
        Debug.Log("Retrieving QrVideoCategoryData");
        string _url = url + "GetVideoCategories/";

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetVideoCategories/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonVideoCategoryCollection qrvC = JsonReader.Deserialize<JsonVideoCategoryCollection>(www.text);
                Debug.Log("After Deserialize");
                Global.Instance.videoCategories = JsonVideoCategoryToVideoCategory(qrvC);
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator RetrieveQrVideoData()
    {
        InitializeUrl();
        Debug.Log("Retrieving QrVideoData");
        string _url = url + "GetQrVideos/" + Global.Instance.UserId;

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetQrVideos/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonQrVideoCollection qrvC = JsonReader.Deserialize<JsonQrVideoCollection>(www.text);
                Global.Instance.qrVideos = JsonQrVideoToQrVideo(qrvC);
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator RetrieveVideoPathData()
    {
        InitializeUrl();
        Debug.Log("Retrieving Video Paths");
        string _url = url + "GetVideoPaths/" + Global.Instance.UserId;

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetVideoPaths/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonQrVideoCollection qrvC = JsonReader.Deserialize<JsonQrVideoCollection>(www.text);
                Global.Instance.localVideos = JsonQrVideoToQrVideo(qrvC);
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator UploadQrVideo(QrVideo qrVid)
    {
        InitializeUrl();
        Debug.Log("UploadQrVideo");
        string _url = url + "SaveQrVideo/";

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/SaveQrVideo/"; //OutComment if release version
        }

        JsonQrVideo qrVideos = QrVideoToJsonQrVideo(qrVid);



        Debug.Log("Converted To Json Container:\n" + qrVideos.ToString());
        string serialized = JsonWriter.Serialize(qrVideos);

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);

        WWW www = new WWW(_url, bytes);

        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Debug.Log("Upload Complete.");
            Success = true;
        }
        else
        {
            Success = false;
            Debug.Log("Upload Error: " + www.error);
        }
    }

    public static IEnumerator UpdateQrVideo(QrVideo qrVideo)
    {
        InitializeUrl();
        Debug.Log("UpdateQRVideo");
        string _url = url + "UpdateQrVideo/";
        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/UpdateQrVideo/"; //OutComment if release version
        }

        JsonQrVideo jsonVid = QrVideoToJsonQrVideo(qrVideo);

        Debug.Log("Converted To Json Container:\n" + jsonVid.ToString());
        string serialized = JsonWriter.Serialize(jsonVid);

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);

        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "PUT");

        WWW www = new WWW(_url, bytes, headers);

        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Debug.Log("Update Complete.");
            Success = true;
        }
        else
        {
            Success = false;
            Debug.Log("Update Error: " + www.error);
        }
    }

    public static IEnumerator UpdateVideoCount(Guid id, int count)
    {
        InitializeUrl();
        Debug.Log("UpdateVideoCount");
        string _url = url + "UpdateVideoCount/"+ id +"?Count=" + count;
        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/"UpdateVideoCount/"+ id +"?Count=" + count; //OutComment if release version
        }

        Debug.Log("Converted To Json Container:\n" + id.ToString() + count.ToString());
        string serialized = JsonWriter.Serialize(id.ToString() + count.ToString());

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);

        var headers = new Dictionary<string, string>();
        headers.Add("X-HTTP-Method-Override", "PUT");

        WWW www = new WWW(_url, bytes, headers);

        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Debug.Log("Update Complete.");
        }
        else
        {
            Debug.Log("Update Error: " + www.error);
        }
    }

    public static IEnumerator UploadQrVideoUserView(QrVideoUserView view)
    {
        InitializeUrl();
        Debug.Log("Upload Video User View");
        string _url = url + "SaveVideoUserViewData/";

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/SaveVideoUserViewData/"; //OutComment if release version
        }
        
        JsonQrVideoUserView qrVideoUserView = QrVideoUserViewToJsonQrVideoUserView(view);

        Debug.Log("Converted To Json Container:\n" + qrVideoUserView.ToString());
        string serialized = JsonWriter.Serialize(qrVideoUserView);

        Debug.Log("Serialized:\n" + serialized);
        Encoding encoding = Encoding.UTF8;
        byte[] bytes = encoding.GetBytes(serialized);
        
        WWW www = new WWW(_url, bytes);

        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("Result: " + www.text);
            Debug.Log("Upload Complete.");
        }
        else
        {
            Debug.Log("Upload Error: " + www.error);
        }
    }

    public static IEnumerator GetVideoIdByPath()
    {
        InitializeUrl();
        Debug.Log("GetUserGroup");
        string _url = url + "GetVideoIdByPath?path=" + Global.Instance.videoPath;


        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetVideoIdByPath?path=" + Global.Instance.videoPath + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;


        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);

            try
            {

                JsonQrVideo ug = JsonReader.Deserialize<JsonQrVideo>(www.text);
                Global.Instance.qrVideoId = ug.Id;
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator GetUserGroup()
    {
        InitializeUrl();
        Debug.Log("GetUserGroup");
        string _url = url + "GetUserGroup/" + Global.Instance.UserId;


        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetUserGroup/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonUserGroup ug = JsonReader.Deserialize<JsonUserGroup>(www.text);
                Global.Instance.userGroup = JsonUserGroupToUserGroup(ug);
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator GetVideoCount()
    {
        Global.Instance.ready = false;
        InitializeUrl();
        Debug.Log("Getting Video Count");
        string _url = url + "GetVideoCount/" + Global.Instance.qrVideoId;

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetUserGroup/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }
        WWW www = new WWW(_url);
        yield return www;

        if(www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonVideoUserViewCollection jsonVUV = JsonReader.Deserialize<JsonVideoUserViewCollection>(www.text);
                Global.Instance.getVideoUserViewCount = JsonVideoUserView(jsonVUV);
                Global.Instance.ready = true;
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator DeleteVideo(Guid id)
    {
        InitializeUrl();
        Debug.Log("GetUserGroup");
        string _url = url + "DeleteVideo/" + id;
        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/DeleteVideo/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        UnityWebRequest request = UnityWebRequest.Delete(_url);

        yield return request.Send();

        if (request.error == null)
        {
            //Debug.Log("Result: " + request.text);
            Debug.Log("Video Deleted");
        }
        else
        {
            Debug.Log("Delete Error: " + request.error);
        }
    }

    static public IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Result: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    static ExerciseCategoryCollection JsonCategoryCollectionToExerciseCategoryCollection(JsonCategoryCollection collection)
    {
        Global.Instance.UserId = collection.UserId;
        ExerciseCategoryCollection ecc = new ExerciseCategoryCollection();
        foreach (JsonCategory c in collection.Categories)
        {
            ExerciseCategory tmpCategory = new ExerciseCategory(c.Id, c.Name, c.Score);
            ecc.Add(tmpCategory);
            foreach (JsonExercise e in c.Exercises)
            {
                Exercise tmpExercise = new Exercise(e.Id, e.Name, e.Score, e.SceneFunction);
                tmpCategory.Add(tmpExercise);
                //foreach (JsonExercisePart p in e.Parts)
                //{
                //    ExercisePart tmpPart = new ExercisePart(p.Id, p.Name, p.Completed, p.Time);
                //    tmpExercise.Add(tmpPart);
                //}
            }
        }
        return ecc;
    }

    static JsonCategoryCollection ExerciseCategoryCollectionToJsonCategoryCollection(ExerciseCategoryCollection collection)
    {
        JsonCategoryCollection jcc = new JsonCategoryCollection();
        jcc.UserId = Global.Instance.UserId;
        List<JsonCategory> categoryList = new List<JsonCategory>();

        foreach (ExerciseCategory c in collection)
        {
            JsonCategory tmpCategory = new JsonCategory(c.ID, c.Name, c.Score);
            List<JsonExercise> exerciseList = new List<JsonExercise>();
            foreach (Exercise e in c)
            {
                JsonExercise tmpExercise = new JsonExercise(e.ID, e.Name, e.Score, e.Function, e.Attempted);

                // Resets attempted each time the data is set, so only current attempted exercise is sent
                e.Attempted = false;

                //List<JsonExercisePart> partList = new List<JsonExercisePart>();
                //foreach (ExercisePart p in e)
                //{
                //    JsonExercisePart tmpPart = new JsonExercisePart(p.ID, p.Name, p.Complete, p.Time);
                //    partList.Add(tmpPart);
                //}
                //tmpExercise.Parts = partList.ToArray();
                exerciseList.Add(tmpExercise);
            }
            tmpCategory.Exercises = exerciseList.ToArray();
            categoryList.Add(tmpCategory);
        }
        jcc.Categories = categoryList.ToArray();
        return jcc;
    }

    static List<QrVideo> JsonQrVideoToQrVideo(JsonQrVideoCollection qrvCol)
    {
        List<QrVideo> qrvList = new List<QrVideo>();
        foreach (JsonQrVideo qrv in qrvCol.QrVideos)
        {
            QrVideo tmpQrVideo = new QrVideo(qrv.Id, qrv.Name, qrv.Description, qrv.Path, qrv.Count, qrv.UserGroupId, qrv.UserId, qrv.ReleaseDate, qrv.VideoCategoryId);
            qrvList.Add(tmpQrVideo);
        }
        return qrvList;
    }

    static List<QrVideoUserView> JsonVideoUserView(JsonVideoUserViewCollection qrvCol)
    {
        List<QrVideoUserView> qrvList = new List<QrVideoUserView>();
        foreach(JsonQrVideoUserView qrvuv in qrvCol.VideoUserView)
        {
            QrVideoUserView tmpQrVUV = new QrVideoUserView(qrvuv.VideoId, qrvuv.UserId, qrvuv.ViewDate);
            qrvList.Add(tmpQrVUV);
        }
        return qrvList;
    }

    static UserGroup JsonUserGroupToUserGroup(JsonUserGroup ug)
    {
        UserGroup userGroup = new UserGroup(ug.Id, ug.GroupName, ug.CustomerId);
        return userGroup;
    }

    static List<VideoCategory> JsonVideoCategoryToVideoCategory(JsonVideoCategoryCollection jsonVidCol)
    {
        List<VideoCategory> vcList = new List<VideoCategory>();
        foreach (JsonVideoCategory qrv in jsonVidCol.VideoCategories)
        {
            VideoCategory videoCategory = new VideoCategory(qrv.Id, qrv.Name);
            vcList.Add(videoCategory);
        }
        return vcList;
    }

    static JsonQrVideo QrVideoToJsonQrVideo(QrVideo vid)
    {
        JsonQrVideo jsonQrVideo = new JsonQrVideo(vid.Id, vid.Name, vid.Description, vid.Path, vid.Count, vid.UserGroupId, vid.UserId, vid.ReleaseDate, vid.VideoCategoryId);
        return jsonQrVideo;
    }

    static JsonQrVideoUserView QrVideoUserViewToJsonQrVideoUserView(QrVideoUserView view)
    {
        JsonQrVideoUserView jsonQrVideoUserView = new JsonQrVideoUserView(view.VideoId, view.UserId, view.ViewDate);
        return jsonQrVideoUserView;
    }



    //
    //Encryption
    //

    static UserGroupVideoCredential JsonUserGroupVideoCatagoryCredintial(JsonUserGroupVideoCredential ugcv)
    {
        UserGroupVideoCredential userGroupVideoCredintial = new UserGroupVideoCredential(ugcv.Id, ugcv.VideoCategoryId, ugcv.UserGroupId, ugcv.Password, ugcv.Salt);
        return userGroupVideoCredintial;
    }


    public static IEnumerator validateSeureQrVideo(string path)
    {
        InitializeUrl();
        Debug.Log("Retrieving Secure Qr video Data");
        string _url = url + "GetSecureQrVideo/" + Global.Instance.UserId + "?Path=" + path;
        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/GetSecureQrVideo/" + Global.Instance.UserId + "?Path=" + path; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            if (Convert.ToBoolean(www.text))
            {
                QrCamController qrCam = new QrCamController();
                qrCam.onSuccess(path);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public static IEnumerator getUserGroupCredential()
    {
        InitializeUrl();
        Debug.Log("GetUserGroupCredential");
        string _url = url + "GetUserGroupCredential/" + Global.Instance.UserId;

        if (Global.Instance.ProgramLanguage == "sv-SE")
        {
            //url = "http://vfo.welfaresverige.se/Service/getUserGroupCredential/" + Global.Instance.UserId + "/" + "sv-SE"; //OutComment if release version
        }

        WWW www = new WWW(_url);

        yield return www;

        if (www.error == null)
        {
            Debug.Log("Result:\n" + www.text);
            try
            {
                JsonUserGroupVideoCredential ugvc = JsonReader.Deserialize<JsonUserGroupVideoCredential>(www.text);
                Global.Instance.userGroupVideoCredintial = JsonUserGroupVideoCatagoryCredintial(ugvc);
            }
            catch (Exception e)
            {
                Util.MessageBox(new Rect(0, 0, 400, 200), "Error: " + e.Message + "\n\nPlease try to restart the application!", Message.Type.Error, false, true);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }




    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

#region Event to check if request was successful
    private static bool _success;
    public static event EventHandler<SuccessEventArgs> SuccessChanged;

    public static bool Success
    {
        get { return _success; }
        set
        {
            _success = value;
            SuccessEventArgs e = new SuccessEventArgs { Success = value };
            OnSuccessChanged(e);
        }
    }

    public class SuccessEventArgs : EventArgs
    {
        public bool Success { get; set; }
    }

    protected static void OnSuccessChanged(SuccessEventArgs e)
    {
        if (SuccessChanged != null)
            SuccessChanged(null, e);
    }
#endregion
}
