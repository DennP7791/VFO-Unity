Shader "Transparent/Illuminated 2" {
    
Properties {
    _MainTex ("Texture (A = Transparency)", 2D) = ""
}

SubShader {
	Tags {"Queue" = "transparent+2"}
	ZTest LEqual
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		SetTexture[_MainTex]
	}
}

}

