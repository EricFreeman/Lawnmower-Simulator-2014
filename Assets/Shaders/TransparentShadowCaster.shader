Shader "Custom/TransparentShadowCaster" {
	Subshader
    {
       UsePass "VertexLit/SHADOWCOLLECTOR"    
       UsePass "VertexLit/SHADOWCASTER"
    }
 
    Fallback off
}
