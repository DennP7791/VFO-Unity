using UnityEngine;
using System.Collections;

/* ------------------------------------------------------------
 * Description: 
 * ------------------------------------------------------------
 * Example of the HUD used for placing an helper around 
 * a bed
 * 
 * ------------------------------------------------------------
 * Pre-Conditions:
 * ------------------------------------------------------------
 * - The scene contains a bed
 * - An Helper prefab exists in the resource folder
 * - HUD_Helper prefab exists in the resource folder
 * 
 * ------------------------------------------------------------
 * Post-Conditions:
 * ------------------------------------------------------------
 * - A HUD is displayed in the scene
 * - Helper instances can be placed around the bed at one 
 *   of the positions specified in Helper.Position:Enum
 * 
 */

public class Example_HUD_Helper : MonoBehaviour
{
	void Start () 
    {
        Helper helper = Util.InstantiateResource<Helper>("Helper");
        if(helper)
            helper.SetPosition(Helper.Position.BOTTOMCENTER);

        HUD hud = Util.ToggleResource<HUD>("HUD_Helper");
        if (hud)
        {
            hud.Buttons[(int)Helper.Position.BOTTOMCENTER].Disabled = true;
            hud.Buttons[(int)Helper.Position.UPPERLEFT].Correct = true;
            hud.Buttons[(int)Helper.Position.UPPERCENTER].Correct = true;
            hud.Buttons[(int)Helper.Position.UPPERRIGHT].Correct = true;
        }
	}

    void Awake()
    {
    }

	void Update () 
    {
	}
}
