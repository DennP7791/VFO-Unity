#define MYDEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExerciseCollections;

public class Global : MonoBehaviour {

    public ToolButton[] toolButtonArray = new ToolButton[0];
    public ExerciseCategoryCollection categoryCollection = new ExerciseCategoryCollection();
    public List<QrVideo> qrVideos = new List<QrVideo>();
    public string videoUrl = "";
    public List<VideoCategory> videoCategories = new List<VideoCategory>();

    private static Global _instance; //singleton
    private Dictionary<string,GameObject> guiObjects; //list of the
    // Singleton
    public static Global Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (Global)GameObject.FindObjectOfType(typeof(Global));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "Global";
                    _instance = (Global)container.AddComponent(typeof(Global));
                }
            }

            return _instance;
        }
    }

    public string ProgramLanguage = "da-DK"; //"sv-SE";
    public float ProgramVersion = 3.68f;

    public int MaxStars = 3;
	
	private float gameScore = 0.0f;
	private int userId = -1;
	
	private int preasurePointsAnimation 	= -1;
	private bool hasPreasurePointsRun 		= false;
	private bool hasSimulationRun			= false;
	private bool hasHelpOrTestRun			= false;
	private bool runSimulationWithHelp 		= false;
    private bool isQrVideoMenu              = false;
	
	private string infoWindowText			= "";

    private string _loadThisScene = "";
	
	public float GameScore
	{
		get { return gameScore; }
		set { gameScore = value; }
	}
	
	public int UserId
	{
		get { return userId; }
		set { userId = value; }
	}
	
	public int PreasurePointsAnimation
	{
		get { return preasurePointsAnimation; }
		set { preasurePointsAnimation = value; }
	}
	
	public bool HasPreasurePointsRun
	{
		get { return hasPreasurePointsRun; }
		set { hasPreasurePointsRun = value; }
	}
	
	public bool HasSimulationRun
	{
		get { return hasSimulationRun; }
		set { hasSimulationRun = value; }
	}
	
	public bool RunSimulationWithHelp
	{
		get { return runSimulationWithHelp; }
		set { runSimulationWithHelp = value; }
	}
	
	public bool HasHelpOrTestRun
	{
		get { return hasHelpOrTestRun; }
		set { hasHelpOrTestRun = value; }
	}

    public bool IsQrVideoMenu
    {
        get { return isQrVideoMenu; }
        set { isQrVideoMenu = value; }
    }
	
	public string InfoWindowText
	{
		get { return infoWindowText; }
		set { infoWindowText = value; }
	}

    public string GetTextFromXml(string id)
    {
        return Text.Instance.GetString(id);
    }   
	
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_loadThisScene != "")
        {
            if (Application.CanStreamedLevelBeLoaded(_loadThisScene))
            {
                string _s = _loadThisScene;
                _loadThisScene = "";
                Application.LoadLevel(_s);
            }
        }
	}

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
	
	public void ToggleSound()
	{
		AudioListener.volume = (AudioListener.volume == 0)? 1 : 0;
	}
	
    public void updateScore(double score)
    {
        ExerciseCategory category = Global.Instance.categoryCollection[SceneLoader.Instance.CurrentCategory];
        if (category != null)
        {
            Exercise exercise = category[SceneLoader.Instance.CurrentID];
            if (exercise != null)
            {
                exercise.Score = score;
                exercise.Attempted = true;
                category.Update();


                foreach (var qrVideo in Global.Instance.qrVideos)
                {
                    Debug.Log("video data " + qrVideo);
                }
                //StartCoroutine(DataManager.UploadData());
                //StartCoroutine(DataManager.UploadQRVideo());
                //StartCoroutine(DataManager.UpdateQRVideo());
            }
        }
        else
            Debug.Log("This is terrible, something went completely wrong!");
    }


    public void CloseInfoWinIfVisible()
    {
        Debug.Log("Checking for instances of InfoWin...");
        Message iw = Util.FindInstancesWhoseNameStartsWith<Message>("InfoWindow");
        if (iw)
        {
            Debug.Log("Found: " + iw.name);
            GameObject.Destroy(iw.gameObject);
            BottomBarScript.EnableInfoButton(true);
            return;
        }
        Debug.Log("Not found.");
    }

    public void ToggleInfoWin()
    {
		BottomBarScript.EnableInfoButton(false);
        Message iw = Util.InfoWindow(new Rect(0, 0, 800, 470), Text.Instance.GetString(infoWindowText), true, Message.Type.Info, false, true, true, BottomBarScript.EnableInfoButton);
        iw.Depth = 99;
    }

    public void ToggleToolBox()
    {
        CloseInfoWinIfVisible();
        if(!Util.ToggleResource<ToolBox>("ToolBox"))
        {
            Debug.LogWarning("ToolBox is not an existent resource.");
        }
    }

    public void LoadMain()
    {
        Debug.Log("Loading Main");
        _loadThisScene = "main";
    }

    private void DiplayToolWindow<T>(string name, ToolButton button) where T : BaseWindow
    {
        T tool = Util.ToggleResource<T>(name);
        button.Target = tool.gameObject;
        if (!tool)
            Debug.LogWarning(name + " is not an existent resource.");
        if (tool.enabled)
        {
            if (!button.Selected)
            {
                tool.enabled = false;
                return;
            }
            WindowHandler.BringWindowToFront(tool);
            ToolBox tb = (ToolBox)GameObject.FindObjectOfType(typeof(ToolBox));
            if(tb)
            {
                WindowHandler.BringWindowToFront(tb);
            }
        }
    }

    public void DisplayRemote(ToolButton button)
    {
        DiplayToolWindow<RemoteController>("Remote", button);
    }

    public void DisplayRemoteBed(ToolButton button)
    {
        DiplayToolWindow<Remote>("Remote_Bed", button);
    }

    public void DisplayRemoteBed2(ToolButton button)
    {
        DiplayToolWindow<Remote>("Remote_Bed_2", button);
    }

    public void DisplayRemoteFloorLift(ToolButton button)
    {
        DiplayToolWindow<Remote>("Remote_FloorLift", button);
    }

    public void DisplayRemoteLoftLift(ToolButton button)
    {
        DiplayToolWindow<Remote>("Remote_LoftLift", button);
    }

    public void DisplayHelperHUD(ToolButton button)
    {
        DiplayToolWindow<HUD>("HUD_Helper", button);
    }

    public void DisplaySlideMatHUD(ToolButton button)
	{
        DiplayToolWindow<HUD>("HUD_SlideMat", button);
	}

    public void DisplaySlideMatHUD2(ToolButton button)
    {
        DiplayToolWindow<HUD>("HUD_SlideMat2", button);
    }

    public void DisplaySlideMatPillow(ToolButton button)
    {
        DiplayToolWindow<HUD>("HUD_SlideMatPillow", button);
    }
	
	public void DisplayBedsheetHUD(ToolButton button)
	{
        DiplayToolWindow<HUDTiled>("HUD_Bedsheet", button);
	}

    public void DisplayBedsheetHUD2(ToolButton button)
    {
        DiplayToolWindow<HUDTiled>("HUD_Bedsheet2", button);
    }
	
	public void DisplayWalkerSlideMatHUD(ToolButton button)
	{
        DiplayToolWindow<HUDTiled>("HUD_Wheel_Chair", button);
	}

    public void DisplayWalkerHUD(ToolButton button)
	{
        DiplayToolWindow<HUDTiled>("HUD_Walker2", button);
	}

    public void DisplayAntiSlideMatHUD(ToolButton button)
	{
        DiplayToolWindow<HUD>("HUD_AntiSlideMat", button);
	}

    public void DisplayAntiSlideMatHUD2(ToolButton button)
    {
        DiplayToolWindow<HUD>("HUD_AntiSlideMat2", button);
    }

    public void DisplayBoxHUD(ToolButton button)
    {
        DiplayToolWindow<HUDTiled>("HUD_Box", button);
    }

    public void DisplayWheelChairMat(SlideMatWheelChair.Position pos)
    {
        GameObject go = GameObject.Find("Wheelchair");
        if (go)
        {
            SlideMatWheelChair smwc = go.GetComponent<SlideMatWheelChair>();
            if (smwc)
            {
                smwc.SetPosition(pos);
            }
        }
    }

    public void DisplayWheelChairMatLeft()
    {
        DisplayWheelChairMat(SlideMatWheelChair.Position.LEFT);
		GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "slidematPlacedL");
    }

    public void DisplayWheelChairMatRight()
    {
        DisplayWheelChairMat(SlideMatWheelChair.Position.RIGHT);
		GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "slidematPlacedR");
    }

    public void InsertHelper(Helper.Position position)
    {
        // Helper is always in the scene, the callback resets his position from the definition script.
        GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "helperPlaced" + position.ToString());
    }

    public void InsertSlideMat(SlideMat.Position position)
    {
        SlideMat slideMat = Util.InstantiateResource<SlideMat>("SlideMat");
        if (slideMat)
		{
            slideMat.SetPosition(position);
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "slidemat_" + position.ToString());
		}
        else
            Debug.LogWarning("SlideMat is not an existent resource.");
    }

    public void InsertAntiSlideMat(AntiSlideMat.Position position)
    {
        AntiSlideMat asm = Util.InstantiateResource<AntiSlideMat>("AntiSlideMat");
        if (asm)
		{
            asm.SetPosition(position);
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "antislidemat_" + position.ToString());
		}
        else
            Debug.LogWarning("SlideMat is not an existent resource.");
    }

    public void InsertAntiSlideMat2(AntiSlideMat.Position position)
    {
        AntiSlideMat2 asm = Util.ToggleResource<AntiSlideMat2>("AntiSlideMat2");
        if (asm)
        {
            AnimateAntiSlideMat.Instance.ResetPositionAndRotation();
            GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "antislidemat_" + position.ToString());
        }
        else
            Debug.LogWarning("SlideMat is not an existent resource.");
    }


    public void InsertWalker()
    {

    }

    public void InsertBox()
    {
        Box box = Util.ToggleResource<Box>("Box");
        if (box)
        {
            //do something
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "boxPlaced");
        }
        else
            Debug.LogWarning("Box is not an existent resource");
    }

    public void InsertBedSheet()
    {
        BedSheet bs = Util.InstantiateResource<BedSheet>("BedSheet");
        if (bs)
        {
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "bedsheet_" + "DEFAULT");
            //do something
        }
        else
            Debug.LogWarning("BedSheet is not an existent resource.");
    }

    public void InsertBedSheet2()
    {

    }

    [System.Obsolete("This function should not be used until the scenes are created")]
    public void MaleHelperMessageBox()
    {

        Util.CancellableMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("global_place_helper"), InsertMaleHelper);
    }

    public void InsertMaleHelper(Message message, bool value)
    {
        if (value)
        {
            Util.InstantiateResource<Helper>("Helper");
        }
    }


    public void InsertToolModel(string name, string state)
    {
        if (GameObject.Find(name))
            return;

        Object resourceObj = Resources.Load(name);
        if (resourceObj)
        {
            GameObject go = (GameObject)Object.Instantiate(resourceObj);
            GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", state);
        }
    }

    public void SlingMessageBox(ToolButton button)
    {
        if (button.Selected)
        {
            Message msg = Util.CancellableMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("global_place_sling"), true, Message.Type.Info, InsertSling);
            if (msg)
            {
                msg.name = "SlingBox";
                button.Target = msg.gameObject;
            }
            return;
        }
        GameObject go = GameObject.Find("SlingBox");
        if (go)
            GameObject.Destroy(go);
    }

    public void InsertSling(Message message, bool value)
    {
        if (value)
        {
            // Sling is always in the scene, and the callback moves it to the correct position when it receives the callback
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "slingPlaced");
        }
    }

    public void FloorLiftMessageBox(ToolButton button)
    {
        if (button.Selected)
        {
            Message msg = Util.CancellableMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("global_place_floorlift"), true, Message.Type.Info, InsertFloorLift);
            if (msg)
            {
                msg.name = "FloorLiftBox";
                button.Target = msg.gameObject;
            }
            return;
        }
        GameObject go = GameObject.Find("FloorLiftBox");
        if (go)
            GameObject.Destroy(go.gameObject);
    }

    public void InsertFloorLift(Message message, bool value)
    {
        if (value)
        {
            InsertToolModel("FloorLift", "floorLiftPlaced");
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "floorliftPlaced");
        }
    }
	
	public void ChairMessageBox(ToolButton button)
    {
        if (button.Selected)
        {
            Message msg = Util.CancellableMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("global_place_chair"), true, Message.Type.Info, InsertChair);
            if (msg)
            {
                msg.name = "ChairBox";
                button.Target = msg.gameObject;
            }
            return;
        }
        GameObject go = GameObject.Find("ChairBox");
        if (go)
            GameObject.Destroy(go);
    }
	
	public void InsertChair(Message message, bool value)
    {
        if (value)
        {
            InsertToolModel("Small_chairs", "chairPlaced");
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "chairPlaced");
        }
    }

    public void WheelchairMessageBox(ToolButton button)
    {
        if (button.Selected)
        {
            Message msg = Util.CancellableMessageBox(new Rect(0, 0, 400, 200), Text.Instance.GetString("global_place_wheelchair"), true, Message.Type.Info, InsertWheelchair);
            if (msg)
            {
                msg.name = "WheelCharBox";
                button.Target = msg.gameObject;
            }
            return;
        }
        GameObject go = GameObject.Find("WheelCharBox");
        if (go)
            GameObject.Destroy(go);
    }

    public void InsertWheelchair(Message message, bool value)
    {
        if (value)
        {
            //InsertToolModel("Wheelchair", "wheelchairPlaced");
			GameObject.Find(States.Instance.GetStateValue("actionCallbackGameObjectName")).SendMessage("SimCallback", "wheelchairPlaced");
        }
    }

    public void BackToHome(Message message, bool value)
    {
        if(value)
            SceneLoader.Instance.CurrentScene = 0;
        //Enable Home button
        BottomBarScript.SetButtonDisabled(0, false);
    }

    public void RefreshScene(Message message, bool value)
    {

        if (value)
        {
            SceneLoader.Instance.CurrentScene = SceneLoader.Instance.CurrentScene;
        }
            //Enable Refresh button
            BottomBarScript.SetButtonDisabled(3, false);
            BottomBarScript.SetButtonDisabled(0, false);
      
        /*

        {
            Util.ToggleResource<ToolBox>("ToolBox");
        }*/ 
    }
	
	public void BackToHomeMessage()
    {
        CloseInfoWinIfVisible();
        if(Util.CancellableMessageBox(new Rect(100, 100, 300, 200), Text.Instance.GetString("global_back_to_home"), true, Message.Type.Warning, BackToHome))
            BottomBarScript.SetButtonDisabled(0, true);
    }

    public void RefreshSceneMessage()
    {
        CloseInfoWinIfVisible();
        if(Util.CancellableMessageBox(new Rect(100, 100, 300, 200), Text.Instance.GetString("global_start_over"), true, Message.Type.Warning, RefreshScene))
            BottomBarScript.SetButtonDisabled(3, true);
    }
	
}
