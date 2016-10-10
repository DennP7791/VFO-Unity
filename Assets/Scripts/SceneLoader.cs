using UnityEngine;
using System.Collections;

//Singleton class intended for Scene loading
//Its functions are supposed to be called by the links in LinkMenu Script (initialized via the inspector);

public class SceneLoader : MonoBehaviour {

    private static SceneLoader _instance;

    private int _currentCategory = -1;
    private int _currentID = -1;
    private int _currentScene = -1;

    private string _loadThisScene = "";

    Message loadingBox;

    public int CurrentCategory
    {
        get { return _currentCategory; }
        set { _currentCategory = value; }
    }

    public int CurrentID
    {
        get { return _currentID; }
        set { _currentID = value; }
    }

    public int CurrentScene
    {
        get { return _currentScene; }
        set
        {
            Debug.Log("New Scene Value Set: " + value);
            //stop all audio from previous scene
            Text.Instance.StopAudio();

            _currentScene = value;
            Util.SetToolTipText("");
            loadingBox = Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("sceneloader_downloading") + " 0%", Message.Type.Info, false, true);
            if (value == 0)
            {
                BottomBarScript.SetNoButtons(true);
                this.Scene0();
            }else
            if (value > 0)
            {
                BottomBarScript.SetNoButtons(false);
                this.Container();
            }
        }
    }

    public static SceneLoader Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (SceneLoader)GameObject.FindObjectOfType(typeof(SceneLoader));

                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "SceneLoader";
                    _instance = (SceneLoader)container.AddComponent(typeof(SceneLoader));
                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
		
    }

    //Example funtions 
    public void Scene0()
    {
		Global.Instance.HasSimulationRun = false;
		Global.Instance.HasHelpOrTestRun = false;
		Global.Instance.HasPreasurePointsRun = false;
        Global.Instance.IsQrVideoMenu = false;
		
        Debug.Log("loading scene" + _currentScene);

        _loadThisScene = "scene00";

        BottomBarScript.EnableInfoButton(false);
       // BottomBarScript.EnableToolBoxButton(false);
		
        //Util.InstantiateResource<LinkMenu1>("LinkMenu1");
    }

    public void StartContainer()
    {
        loadingBox = Util.MessageBox(new Rect(0, 0, 300, 200), Text.Instance.GetString("sceneloader_downloading") + " 0%", Message.Type.Info, false, true);
        Container();
    }
	
	public void Container()
	{

        string p_level = "";
	    string s_level = "";

        int currentScene = CurrentScene;
		switch (currentScene)
		{
            case 1:
                Global.Instance.HasHelpOrTestRun = true;
                Global.Instance.HasPreasurePointsRun = true;
                s_level = "welcome";
                break;
            case 2:
                Global.Instance.HasHelpOrTestRun = true;
                Global.Instance.HasPreasurePointsRun = true;
                if (Global.Instance.ProgramLanguage == "sv-SE")
                    s_level = "Introduction_se";
                else
                    s_level = "Introduction";
                break;
            case 106:
                Global.Instance.HasHelpOrTestRun = true;
                Global.Instance.HasPreasurePointsRun = true;
                s_level = "function_level";
                break;
            case 107:
                Global.Instance.HasHelpOrTestRun = true;
                Global.Instance.HasPreasurePointsRun = true;
                s_level = "preasure_points";
                break;
            case 3:
                Global.Instance.InfoWindowText = "breafing_back_in_chair_a";
                s_level = "sim_back_in_chair_borger_a";
                break;
            case 4:
                Global.Instance.InfoWindowText = "breafing_back_in_chair_b";
                s_level = "sim_back_in_chair_borger_b";
                break;
            case 115:
                Global.Instance.InfoWindowText = "breafing_back_in_chair_c";
                s_level = "sim_back_in_chair_borger_c";
                break;
            case 6:
                Global.Instance.InfoWindowText = "breafing_higher_in_bed_a";
                s_level = "sim_higher_in_bed_borger_a";
                break;
            case 7:
                Global.Instance.InfoWindowText = "breafing_higher_in_bed_b";
                s_level = "sim_higher_in_bed_borger_b";
                break;
            case 8:
                Global.Instance.InfoWindowText = "breafing_higher_in_bed_c";
                s_level = "sim_higher_in_bed_borger_c";
                break;
            case 9:
                Global.Instance.InfoWindowText = "breafing_higher_in_bed_drawsheet";
                s_level = "sim_higher_in_bed_with_drawsheet";
                break;
            case 10:
                Global.Instance.InfoWindowText = "breafing_lie_down_a";
                s_level = "sim_lie_down_borger_a";
                break;
            case 11:
                Global.Instance.InfoWindowText = "breafing_lie_down_b";
                s_level = "sim_lie_down_borger_b";
                break;
            case 12:
                Global.Instance.InfoWindowText = "breafing_lie_down_c";
                s_level = "sim_lie_down_borger_c";
                break;
            case 13:
                Global.Instance.InfoWindowText = "breafing_lifting_bed_chair_floorlift";
                s_level = "sim_lifting_from_bed_to_chair_floorlift";
                break;
            case 14:
                Global.Instance.InfoWindowText = "breafing_lifting_bed_chair_rooflift";
                s_level = "sim_lifting_from_bed_to_chair_rooflift";
                break;
            case 15:
                Global.Instance.InfoWindowText = "breafing_lifting_chair_bed_floorlift";
                s_level = "sim_lifting_from_chair_to_bed_floorlift";
                break;
            case 16:
                Global.Instance.InfoWindowText = "breafing_place_sling_in_bed";
                s_level = "sim_place_sling_in_bed";
                break;
            case 17:
                Global.Instance.InfoWindowText = "breafing_place_sling_in_chair";
                s_level = "sim_place_sling_in_chair";
                break;
            case 18:
                Global.Instance.InfoWindowText = "breafing_sit_up_a";
                s_level = "sim_sit_up_borger_a";
                break;
            case 19:
                Global.Instance.InfoWindowText = "breafing_sit_up_b";
                s_level = "sim_sit_up_borger_b";
                break;
            case 20:
                Global.Instance.InfoWindowText = "breafing_sit_up_c";
                s_level = "sim_sit_up_borger_c";
                break;
            case 21:
                Global.Instance.InfoWindowText = "breafing_stand_up_a";
                s_level = "sim_stand_up_borger_a";
                break;
            case 22:
                Global.Instance.InfoWindowText = "breafing_stand_up_b";
                s_level = "sim_stand_up_borger_b";
                break;
            case 23:
                Global.Instance.InfoWindowText = "breafing_stand_up_c";
                s_level = "sim_stand_up_borger_c";
                break;
            case 24:
                Global.Instance.InfoWindowText = "breafing_side_a";
                s_level = "sim_to_side_in_bed_borger_a";
                break;	
            case 25:
                Global.Instance.InfoWindowText = "breafing_side_b";
                s_level = "sim_to_side_in_bed_borger_b";
                break;	
            case 26:
                Global.Instance.InfoWindowText = "breafing_side_c";
                s_level = "sim_to_side_in_bed_borger_c";
                break;	
            case 27:
                Global.Instance.InfoWindowText = "breafing_turn_a";
                s_level = "sim_turn_to_side_borger_a";
                break;
            case 28:
                Global.Instance.InfoWindowText = "breafing_turn_b";
                s_level = "sim_turn_to_side_borger_b";
                break;
            case 29:
                Global.Instance.InfoWindowText = "breafing_turn_c";
                s_level = "sim_turn_to_side_borger_c";
                break;
            case 30:
                Global.Instance.InfoWindowText = "breafing_walk_a";
                s_level = "sim_walk_borger_a";
                break;
            case 31:
                Global.Instance.InfoWindowText = "breafing_walk_b";
                s_level = "sim_walk_borger_b";
                break;
            case 32:
                Global.Instance.InfoWindowText = "breafing_walk_c";
                s_level = "sim_walk_borger_c";
                break;
            case 100:
                Global.Instance.InfoWindowText = "breefing_hor_higer_in_bed_pillow";
                s_level = "sim_hor_higer_in_bed_pillow";
                break;
            case 101:
                Global.Instance.InfoWindowText = "breefing_hor_higer_in_bed_slidemat";
                s_level = "sim_hor_higer_in_bed_slidemat";
                break;
            case 102:
                Global.Instance.InfoWindowText = "breefing_hor_turn_to_side";
                s_level = "sim_hor_turn_to_side";
                break;
            case 103:
                Global.Instance.InfoWindowText = "breefing_hor_sit_up_in_bed";
                s_level = "sim_hor_sit_up_in_bed";
                break;
            case 104:
                Global.Instance.InfoWindowText = "breefing_hor_sitting_to_standing";
                s_level = "sim_hor_sitting_to_standing";
                break;
            case 105:
                Global.Instance.InfoWindowText = "breefing_hor_sitting_to_lying";
                s_level = "sim_hor_sitting_to_lying";
                break;
			case 108:
				Global.Instance.InfoWindowText = "breafing_higher_in_bed_a_almbed";
				s_level = "sim_higher_in_bed_borger_a_almbed";
				break;
			case 109:
				Global.Instance.InfoWindowText = "breafing_place_drawsheet_in_bed";
				s_level = "sim_place_drawsheet_in_bed";
				break;
			case 110:
				Global.Instance.InfoWindowText = "breafing_place_sling_in_bed_single";
				s_level = "sim_place_sling_in_bed_single";
				break;
			case 111:
				Global.Instance.InfoWindowText = "breafing_lifting_bed_chair_rooflift_single";
				s_level = "sim_lifting_from_bed_to_chair_rooflift_single";
				break;
			case 112:
				Global.Instance.InfoWindowText = "breafing_remove_sling_in_chair";
				s_level = "sim_remove_sling_in_chair";
				break;
			case 113:
				Global.Instance.InfoWindowText = "breafing_remove_sling_in_chair_single";
				s_level = "sim_remove_sling_in_chair_single";
				break;
			case 114:
				Global.Instance.InfoWindowText = "breafing_remove_sling_in_bed";
				s_level = "sim_remove_sling_in_bed";
				break;
            case 5:
                Global.Instance.InfoWindowText = "breafing_back_in_chair_b";
                s_level = "sim_back_in_chair_borger_b_new";
                break;
            case 1001:
		        Global.Instance.IsQrVideoMenu = true;
                Global.Instance.InfoWindowText = "Scan QR Code";
                s_level = "scanQr";
                break;
            case 1002:
                Global.Instance.IsQrVideoMenu = true;
                Global.Instance.InfoWindowText = "Play QR videos";
                s_level = "video_player";
                break;
            default:
                s_level = "main";
                break;

		}

        p_level = "sim_preasure_points_help";

	    if (Global.Instance.IsQrVideoMenu)
	    {
	        _loadThisScene = s_level;
	    }
		else if(!Global.Instance.HasHelpOrTestRun)
		{
			_loadThisScene = "help_or_test";
		}
		else if(!Global.Instance.HasPreasurePointsRun)
		{
			_loadThisScene = p_level;
		}
		else
		{
			_loadThisScene = s_level;
		}
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_loadThisScene != "")
        {
            Debug.Log("Changing scene to: " + _loadThisScene);
            float _percentageLoaded = Application.GetStreamProgressForLevel(_loadThisScene) * 100;
            int percentageLoaded = (int)_percentageLoaded;
            loadingBox.Text = Text.Instance.GetString("sceneloader_downloading") + " " + percentageLoaded.ToString() + "%";

            if (Application.CanStreamedLevelBeLoaded(_loadThisScene))
            {
                string _s = _loadThisScene;
                _loadThisScene = "";
                Application.LoadLevel(_s);
            }
        }
	}
}
