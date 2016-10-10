
// pseudo image effect that displays useful info for your image effects

#pragma strict

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script AddComponentMenu ("Image Effects/Camera Info")

class CameraInfo extends MonoBehaviour {

	// display current depth texture mode
	public var currentDepthMode : DepthTextureMode;
	// render path
	public var currentRenderPath : RenderingPath;
	// number of official image fx used
	public var currentPostFxCount : int = 0;
	
#if UNITY_EDITOR	
	function Start () {
		UpdateInfo ();		
	}

	function Update () {
		if (currentDepthMode != gameObject.GetComponent.<Camera>().depthTextureMode)
			gameObject.GetComponent.<Camera>().depthTextureMode = currentDepthMode;
		if (currentRenderPath != gameObject.GetComponent.<Camera>().actualRenderingPath)
			gameObject.GetComponent.<Camera>().renderingPath = currentRenderPath;
			
		UpdateInfo ();
	}
	
	function UpdateInfo () {
	    currentDepthMode = GetComponent.<Camera>().depthTextureMode; //gameObject.GetComponent<Camera>().depthTextureMode; 
		currentRenderPath = gameObject.GetComponent.<Camera>().actualRenderingPath;
		var fx : PostEffectsBase[] = GetComponents.<PostEffectsBase>(); 
		var fxCount : int = 0;
		for (var post : PostEffectsBase in fx) 
			if (post.enabled)
				fxCount++;
		currentPostFxCount = fxCount;		
	}
#endif
}
