Shader "Dorfer/MeshFog"
{
    Properties
    {
       _Color("Fog Color", Color) = (1, 1, 1, .5)
       _IntersectionThresholdMax("Fog Depth",  Range(0,0.7)) = 0.5


       _NoiseTex ("Noise Texture", 2D) = "white" {}
		_FogXSpeed ("Fog Horizontal Speed", Float) = 0.1
		_FogYSpeed ("Fog Vertical Speed", Float) = 0.1
		_NoiseAmount ("Noise Amount", Float) = 1


        
		//_UVWaveSpeed("Speed", Float) = 1
		//_UVWaveAmplitude("Amplitude", Range(0.001,0.5)) = 0.05
		//_UVWaveFrequency("Frequency", Range(0,10)) = 1

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
   Blend SrcAlpha OneMinusSrcAlpha
           ColorMask RGB
           Cull Off 
           Lighting Off 
           ZWrite Off
        Pass
        {
          
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile_fog
           #include "UnityCG.cginc"
  
           struct appdata
           {
               float4 vertex : POSITION;
                fixed4 color : COLOR;
               float2 uv : TEXCOORD0;
           };
  
           struct v2f
           {
               float4 scrPos : TEXCOORD0;
               UNITY_FOG_COORDS(1)
               float4 vertex : SV_POSITION;
               fixed4 color : COLOR;
               float2 uv : TEXCOORD2;
           };
  
           sampler2D _CameraDepthTexture;
           float4 _Color;
           float4 _IntersectionColor;
           float _IntersectionThresholdMax;

            sampler2D _NoiseTex;
            half _FogXSpeed;
            half _FogYSpeed;
            half _NoiseAmount;
  
           v2f vert(appdata v)
           {
               v2f o;
               o.vertex = UnityObjectToClipPos(v.vertex);
               o.scrPos = ComputeScreenPos(o.vertex);
               o.uv = v.uv;
               UNITY_TRANSFER_FOG(o,o.vertex);
               
               return o;   
           }
  
  
            half4 frag(v2f i) : SV_TARGET
            {
               float depth = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
               float diff = saturate(_IntersectionThresholdMax * (depth - i.scrPos.w));
  
               fixed4 col = lerp(fixed4(_Color.rgb, 0.0), _Color, diff * diff * diff * (diff * (6 * diff - 15) + 10));
  
               UNITY_APPLY_FOG(i.fogCoord, col);
               

               float2 speed = _Time.y * float2(_FogXSpeed, _FogYSpeed);
                float noise = (tex2D(_NoiseTex, i.uv + speed).r - 0.5) * _NoiseAmount;
               return col * (1 - noise);
            }
  
            ENDCG
        }
    }
}