using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseOverObject : MonoBehaviour {
	
	private bool mouseOver = false;
	private string hoverName = "";
	private string clickName = "";
	private string clickTag = "-1";
	private string oldClickTag = "";
	private string oldClickName = "";
	private Material cOver;
	private Material cExit;
	
	private bool hoverAntislideMat = false;
	private bool hoverDrawsheet = false;
    private bool hoverSlidemat = false;
    private bool walker = false;
	
	public bool HoverAntiSlideMat
	{
		get { return hoverAntislideMat; }
		set { hoverAntislideMat = value; }
	}

    public bool HoverSlidemat
    {
        get { return hoverSlidemat; }
        set { hoverSlidemat = value; }
    }
	
	public bool HoverDrawSheet
	{
		get { return hoverDrawsheet; }
		set { hoverDrawsheet = value; }
	}

    public bool HoverWalker
    {
        get { return walker; }
        set { walker = value; }
    }
	
	List<string> oldTags = new List<string>();	
	
	// Use this for initialization
	void Start () {
		mouseOver = false;
		
		cOver = (Material)Resources.Load("MouseHover");
		cExit = (Material)Resources.Load("MouseExit");
	}
	
	/// <summary>
    ///    Run when ever mouse is over the object containing the script
    /// </summary>
	private void OnMouseOverF()
	{
		if(!mouseOver)
		{
			mouseOver = true;
			
			int pos = clickName.IndexOf("_");
			if(pos != -1) hoverName = "hoover" + clickName.Substring(pos);
			
			string hoverTag = clickTag + "_hover";

            if (clickTag == "borger" || clickTag == "wheelchair" || clickTag == "bed" || clickTag == "comfortchair")
			{
				GameObject hover = GameObject.Find(hoverName);
					if(hover) {
						if(!hover.GetComponent<Renderer>().enabled)
							hover.GetComponent<Renderer>().enabled = true;

                            hover.GetComponent<Renderer>().material = cOver;
					}
			}
			else
			{
				GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(hoverTag);
				foreach(GameObject hover in tagObjects)
				{
					if(!hover.GetComponent<Renderer>().enabled)
						hover.GetComponent<Renderer>().enabled = true;
					hover.GetComponent<Renderer>().material = cOver;
				}
			}
			
			oldTags.Add(hoverTag);
			
			if(mouseOver && !States.Instance.GetStateValueB("TalkDialogActive") && !Util.AnyVisibleResource<Message>() && !Util.AnyVisibleResource<HUD>() && !Util.AnyVisibleResource<HUDWalker>() && !Util.AnyVisibleResource<HUDBed>() && !Util.AnyVisibleResource<HUDTiled>())
			{
                switch (clickTag)
                {
                    case "assistant":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_assistant"));
                        break;
                    case "borger":
                        if(hoverName == "hoover_head") {
						    Util.SetToolTipText(Text.Instance.GetString("mouse_over_patient_head"));
					    }
                        break;
                    case "sling":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_sling"));
                        break;
                    case "drawsheet":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_drawsheet"));
                        break;
                    case "bed":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_bed"));
                        break;
                    case "wheelchair":
                    case "comfortchair":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_wheelchair"));
                        break;
                    case "antislidemat":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_antislidemat"));
                        break;
                    case "slidemat":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_slidemat"));
                        break;
                    case "walker":
                        Util.SetToolTipText(Text.Instance.GetString("mouse_over_walker"));
                        break;
                }
			}
		}
		
		if(Input.GetMouseButtonDown(0) && mouseOver)
		{
			if(clickTag == "assistant")
				ActionOnClick.Instance.Action("clickedHelper"); // Calls sub class (class just forwards for now) with the mouse clicked event
			if(clickTag == "borger" || clickTag == "wheelchair" || clickTag == "bed" || clickTag == "comfortchair")
				ActionOnClick.Instance.Action(hoverName);
			if(clickTag == "sling")
				ActionOnClick.Instance.Action("clickedSling");
			if(clickTag == "drawsheet")
				ActionOnClick.Instance.Action("clickedDrawsheet");
			if(clickTag == "antislidemat")
				ActionOnClick.Instance.Action("clickedAntislidemat");
            if (clickTag == "slidemat")
                ActionOnClick.Instance.Action("clickedSlidemat");
            if (clickTag == "walker")
                ActionOnClick.Instance.Action("clickedWalker");
        }
	}
	
	/// <summary>
    ///    Run when ever the mouse exits the object containing the script
    /// </summary>	
	void OnMouseExitF()
	{
		if(mouseOver)
		{
			mouseOver = false;
			
			if(oldTags.Count > 0)
			{
				for(int i = 0; i < oldTags.Count; ++i)
				{
					GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(oldTags[i]);
					foreach(GameObject hover in tagObjects)
					{
						hover.GetComponent<Renderer>().material = cExit;
					}
				}
				
				oldTags.Clear();
				Util.SetToolTipText("");
			}
		}
	}

	
	void Update()
	{	
		if(!Util.AnyVisibleResource<ToolGrid>() && !States.Instance.GetStateValueB("TalkDialogActive"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   			RaycastHit hit;
			int myMask = 1<<12;
   			if (Physics.Raycast(ray, out hit, 50.0f, myMask))
			{
				clickTag = hit.collider.tag;
				clickName = hit.collider.name;
				
				if((clickTag == "antislidemat" && !hoverAntislideMat) || (clickTag == "drawsheet" && !hoverDrawsheet) || (clickTag == "slidemat" && !hoverSlidemat) || (clickTag == "walker" && !walker))
					return;
				
				if(clickTag != oldClickTag || clickName != oldClickName)
				{
					OnMouseExitF();
					oldClickTag = clickTag;
					oldClickName = clickName;
				}
	
    			OnMouseOverF();
   			}
			else
			{
				OnMouseExitF();
			}
		}
	}
	
	private static MouseOverObject _instance; //singleton
	
	public static MouseOverObject Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = (MouseOverObject)GameObject.FindObjectOfType(typeof(MouseOverObject));

                if (!_instance)
                {
                    Debug.LogError("Could not find mouseover object, please attach the script to the Borger object");
                } 
            }

            return _instance;
        }
    }
}
