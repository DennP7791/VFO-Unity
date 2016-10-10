using UnityEngine;
using System.Collections;

public class HelpOrTest : BaseWindow {
	
	GUISkin guiSkin;
	
	public override void WinStart()
	{
		guiSkin = (GUISkin)Resources.Load("VirtuelForflytningTemp");
	}
	
	public override void WinOnGUI()
	{
		Rect position = new Rect(Screen.width * 0.5f - 200.0f, 0.0f, 400.0f, Screen.height);
        Position = position;
		Box(new Rect(0, 0, Position.width, Position.height), "", guiSkin.GetStyle("Window"));
		
		AnswerWindow(1);
	}
	
	public override void WinUpdate()
	{
		
	}
	
	void AnswerWindow(int windowId)
	{		
		if(Button(new Rect(25, (Screen.height * 0.5f) -75, 350, 50), Text.Instance.GetString("help_or_test_help"), guiSkin.GetStyle("Button")))
		{
			Global.Instance.RunSimulationWithHelp = true;	
			Global.Instance.HasHelpOrTestRun = true;
			SceneLoader.Instance.StartContainer();
		}
        if (Button(new Rect(25, (Screen.height * 0.5f) + 25, 350, 50), Text.Instance.GetString("help_or_test_test"), guiSkin.GetStyle("Button")))
		{
			Global.Instance.RunSimulationWithHelp = false;
			Global.Instance.HasHelpOrTestRun = true;
			SceneLoader.Instance.StartContainer();
		}
	}
}
