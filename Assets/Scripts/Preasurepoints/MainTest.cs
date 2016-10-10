using UnityEngine;
using System.Collections;

public class MainTest : MonoBehaviour 
{
	private ArrayList 	Idle_Back;
	private ArrayList 	Idle_Side;
	private ArrayList 	Idle_Sitting;
	private ArrayList 	Idle_Stol;
	
	private ArrayList 	Points;
	
	private ArrayList 	MyAnimations;
	private int 		CurrentAnimationPos;
	
	private bool 		Done = false;
	private bool 		Success = false;
	
	public Material 	lit;
	public Material 	litGlow;
	
	public Rect 		buttonRect;
	
	
	public void MouseOverPoint(string point)
	{
		if(!Points.Contains(point))
		{
			Points.Add(point);
			GameObject.Find(point).GetComponent<Renderer>().material = litGlow;
		}
		else
		{
			int pos = Points.IndexOf((string)point);
			Points.RemoveAt(pos);
			GameObject.Find(point).GetComponent<Renderer>().material = lit;
		}
	}
	
	private void UnlitAll()
	{
		GameObject.Find("Point_Head").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Hand").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Gluteus_M").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Elbow").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Deltoid").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Calf").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Ankle").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Heel").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Hip").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Shoulder").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Ankle").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Calf").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Deltoid").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Elbow").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Gluteus_M").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Hand").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Heel").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Hip").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Shoulder").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_Tail_Bone").GetComponent<Renderer>().material = lit;
	}

    public void InfoClicked(Message message, bool value)
	{
        if (value)
        {
            BottomBarScript.EnableInfoButton(true);

            CurrentAnimationPos = Global.Instance.PreasurePointsAnimation - 1;
            GetComponent<Animation>().Play((string)MyAnimations[Global.Instance.PreasurePointsAnimation - 1]);

            Util.OkMessageBox(new Rect(Screen.width - 320, 40, 300, 200), "Du skal nu selv vælge de vigtige trykpunkter til forflytningen.\n\nTryk på hver punkt på modellen til venstre så de lyser op, og tryk derefter ok for at forsætte.", ButtonPressed);
        }
	}

	// Use this for initialization
	void Start () 
	{
		UnlitAll(); 
		
		BottomBarScript.EnableInfoButton(false);
		Message msg = Util.InfoWindow(new Rect(0, 0, 600, 400), Global.Instance.InfoWindowText, InfoClicked);
		msg.Depth = int.MaxValue - 50;

		
		MyAnimations = new ArrayList();
		MyAnimations.Add("Idle_back_100f");
		MyAnimations.Add("Idle_Side_100f");
		MyAnimations.Add("Idle_Sitting_100f");
		MyAnimations.Add("idle_stol_c");
		
		Idle_Back = new ArrayList();
		Idle_Back.Add("Point_Head");
		Idle_Back.Add("Point_L_Hand");
		Idle_Back.Add("Point_L_Gluteus_M");
		Idle_Back.Add("Point_L_Elbow");
		Idle_Back.Add("Point_L_Calf");
		Idle_Back.Add("Point_L_Heel");
		Idle_Back.Add("Point_L_Shoulder");
		Idle_Back.Add("Point_R_Hand");
		Idle_Back.Add("Point_R_Gluteus_M");
		Idle_Back.Add("Point_R_Elbow");
		Idle_Back.Add("Point_R_Calf");
		Idle_Back.Add("Point_R_Heel");
		Idle_Back.Add("Point_R_Shoulder");
		Idle_Back.Add("Point_Tail_Bone");
		
		Idle_Side = new ArrayList();
		Idle_Side.Add("Point_L_Deltoid");
		Idle_Side.Add("Point_L_Hip");
		Idle_Side.Add("Point_L_Ankle");
		Idle_Side.Add("Point_R_Hand");
		Idle_Side.Add("Point_L_Elbow");
		
		Idle_Sitting = new ArrayList();
		Idle_Sitting.Add("Point_L_Gluteus_M");;
		Idle_Sitting.Add("Point_R_Gluteus_M");
		
		Idle_Stol = new ArrayList();
		Idle_Stol.Add("Point_L_Elbow");
		Idle_Stol.Add("Point_R_Elbow");
		Idle_Stol.Add("Point_R_Gluteus_M");
		Idle_Stol.Add("Point_L_Gluteus_M");

		
		Points = new ArrayList();
		
		// For testing this scene on its own
		if(Global.Instance.PreasurePointsAnimation == -1)
			Global.Instance.PreasurePointsAnimation = 4;
		
		if(Global.Instance.PreasurePointsAnimation < 4)
		{
			GameObject go = GameObject.Find("Wheelchair dummy");
			if(go) GameObject.Destroy(go);
			else Debug.LogError("argh");
		}
		else
		{
			GameObject go = GameObject.Find("Bed");
			if(go)	GameObject.Destroy(go);		
			else Debug.LogError("argh2");
			
			GameObject go2 = GameObject.Find("Plane");
			if(go2) GameObject.Destroy(go2);
		}
		
	}

    public void ButtonPressed(Message message, bool value)
	{
        if (value)
        {
            bool compared = true;

            ArrayList ComparePoints = new ArrayList();

            string currentAnimation = (string)MyAnimations[CurrentAnimationPos];

            ComparePoints =
                currentAnimation == "Idle_back_100f" ? Idle_Back :
                currentAnimation == "Idle_Side_100f" ? Idle_Side :
                currentAnimation == "Idle_Sitting_100f" ? Idle_Sitting : Idle_Stol;


            if (ComparePoints.Count == Points.Count)
            {
                for (int i = 0; i < Points.Count; ++i)
                {
                    if (!ComparePoints.Contains((string)Points[i])) compared = false;
                }
            }
            else
            {
                compared = false;
            }

            Success = compared;


            if (Success && !Done)
            {
                Util.OkMessageBox(new Rect(10, 40, 200, 200), "Tillykke, du valgte de rigtige trykpunkter.\n\nTryk på Ok for at gå videre til forflytningen", DoneButton);
            }
            else if (!Success && !Done)
            {
                Util.OkMessageBox(new Rect(10, 40, 200, 200), "Du valgte detsværre ikke de rigtige trykpunkter.\n\nTryk på Ok for at prøve igen.", DoneButton);
            }
        }
	}

    public void DoneButton(Message message, bool value)
	{
        if (value)
        {
            if (Success)
            {
                Global.Instance.HasPreasurePointsRun = true;
                SceneLoader.Instance.Container();
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
	}
	
	
	// Update is called once per frame
	void Update () 
	{
	}
}
