Shader "Transparent/Illuminated offset 3" {
    
Properties {
    _MainTex ("Texture (A = Transparency)", 2D) = ""
}

SubShader {
	Tags {"Queue" = "transparent+3"}
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

