using UnityEngine;
using System.Collections;

public class AnimateAndHighlight : MonoBehaviour {
	
	public Material lit;
	public Material unlit;
	
	private float timer = 0.0f;
	private float highlighttime = 3.5f;
	private bool highlight = false;
	
	void lay()
	{
		GameObject.Find("Point_Head").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Hand").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Gluteus_M").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Elbow").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Deltoid").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Calf").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Ankle").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Heel").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_L_Hip").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Shoulder").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Ankle").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Calf").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Deltoid").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Elbow").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Gluteus_M").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Hand").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Heel").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_R_Hip").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Shoulder").GetComponent<Renderer>().material = lit;
		GameObject.Find("Point_Tail_Bone").GetComponent<Renderer>().material = lit;
	}
	
	void unlitAll()
	{
		GameObject.Find("Point_Head").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Hand").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Gluteus_M").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Elbow").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Deltoid").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Calf").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Ankle").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Heel").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Hip").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_L_Shoulder").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Ankle").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Calf").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Deltoid").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Elbow").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Gluteus_M").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Hand").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Heel").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Hip").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_R_Shoulder").GetComponent<Renderer>().material = unlit;
		GameObject.Find("Point_Tail_Bone").GetComponent<Renderer>().material = unlit;
	}
	
	void Start () {
		lay();
	}
	
	
	void Update () 
	{
		if(Input.GetKey(KeyCode.Alpha1))
		{
			GetComponent<Animation>().Play("Right_leg_bend_relax");
			GameObject.Find("Point_R_Calf").GetComponent<Renderer>().material = unlit;
			highlight = true;
		}
		if(Input.GetKey(KeyCode.Alpha2))
		{
			GetComponent<Animation>().Play("left_turn");
			unlitAll();
			GameObject.Find("Point_L_Ankle").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Deltoid").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Elbow").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Hip").GetComponent<Renderer>().material = lit;
					
			highlight = true;
		}
		if(Input.GetKey(KeyCode.Alpha3))
		{
			GetComponent<Animation>().Play("Sit_up_down");
			unlitAll();
			GameObject.Find("Point_L_Calf").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Gluteus_M").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Hand").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_L_Heel").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_R_Calf").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_R_Gluteus_M").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_R_Hand").GetComponent<Renderer>().material = lit;
			GameObject.Find("Point_R_Heel").GetComponent<Renderer>().material = lit;
			
			highlight = true;
		}
		
		if(highlight)
		{
			timer += Time.deltaTime;
			if(timer > highlighttime)
			{
				highlight = false;
				lay();
				timer = 0.0f;
			}
		}
	}
}
