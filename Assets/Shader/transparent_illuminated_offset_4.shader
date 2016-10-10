Shader "Transparent/Illuminated offset 4" {
    
Properties {
    _MainTex ("Texture (A = Transparency)", 2D) = ""
}

SubShader {
	Tags {"Queue" = "transparent+1"}
	ZTest LEqual
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		SetTexture[_MainTex]
		{
			Matrix[_Matrix]
		}
	}
}

}

