Shader "Dorfer/Hybrid"
{
    Properties
    {   [KeywordEnum(UV, World Space)]
        _MappingTexture("Mapping Texture", Float) = 0
         [Space(30)]
    
        [Toggle] _Grayscale("GRAYSCALE", Float) = 0
            [Toggle] _GrayscaleFresnel("Grayscale fresnel", Float) = 1
            [Toggle] _GrayscaleSpecular("Grayscale specular", Float) = 1

        _grayscaleFactor("grayscaleFactor", Range(0,1)) = 1    //not draw in inspector

       _MainTex("Texture", 2D) = "white" {}
       _MainColor("Base Color", Color) = (1,1,1,1)
        
        _Vector("Vector Texture", Vector) = (0,0,0,0)           //not draw in inspector
        [Space(30)]
       // _EmissionTex("Emission Texture", 2D) = "white" {}
         

        [Space(30)]
        
        _Hue("Hue", Range(0, 1)) = 1.0
        _Contrast("Contrast", Range(0.3, 2)) = 1.0
        _Saturation("Saturation", Range(0, 2)) = 1.0
       
       //[HDR] _ColorLine ("Color Line", Color) = (1,1,1,1)
       // _WidthLine("Width Line", Range(0, 1)) = 1
        
        //_GSRampThreshold("GS Ramp Trashold", Range(-1, 1)) = 1
        //_GSRampSmooth("GS Ramp Smooth", Range(0, 1)) = 1
        
       
        [Header(LIGHTING)]
        [Space(30)]
        
		_HColor ("High Color", Color) = (1,1,1,1)
        _MainColor("Base Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.35,0.4,0.45,1.0)
		_RampThreshold ("Ramp Trashold", Range(0, 1)) = 0.8
        _RampSmooth ("Ramp Smooth", Range(0, 1)) = 1

        
        [Toggle] _Show_Gradient("GRADIENT", Float) = 0
       
        [Space(30)]

        [KeywordEnum(UV, World Space, Object Space)]
        _MappingGradient("Mapping Gradient", Float) = 2
		_Color2 ("Top Color", Color) = (1,1,1,1) 
        _Color1 ("Bottom Color", Color) = (0.35,0.4,0.45,1.0)
		_Height ("Center Gradient Line", Float) = 0
        _Smooth("Smooth Gradient", Range(0.001,1)) = 0.5
        _VectorGradient("Vector Gradient", Vector) = (0,1,0,0)
        

        [Toggle] _Show_Fresnel("FRESNEL", Float) = 0
        
        
        [Space(30)]

		  _FresnelColor ("Fresnel Color", Color) = (1,1,1,1) 
		  _FresnelExponent ("Fresnel Exponent", Range(0, 50)) = 2
		  _FresnelPower ("Fresnel Power", Range(0, 50)) = 3
          _SaturationCompensation("Saturation Compensation", Range(-1, 2)) = 1


           [Toggle] _Show_Specular("SPECULAR", Float) = 0
        
            [Space(30)]
           _SpecColors ("Specular Color", Color) = (1,1,1,1)
           _Shin("Shin", Range(1.5, 20)) = 3
           _smoothSpec("Shin Smooth", Range(0.01, 10)) = 1.5

           [Toggle] _EnvirLighting("Grayscale fresnel", Float) = 0

        
 
    

    }

   SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
             //"RenderType" = "Geometry"
            //"Queue" = "Geometry"
            
        }
 
               HLSLINCLUDE
               ENDHLSL
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward"

                    }
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
 
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _SHOW_GRADIENT_ON
            #pragma multi_compile _ _SHOW_FRESNEL_ON
            #pragma multi_compile _ _SHOW_SPECULAR_ON
            #pragma multi_compile _ _GRAYSCALE_ON
            #pragma multi_compile _ _GRAYSCALESPECULAR_ON 
            #pragma multi_compile _ _GRAYSCALEFRESNEL_ON 
            #pragma multi_compile _ _ENVIRLIGHTING_ON 
            #pragma multi_compile_fog
            #pragma enable_cbuffer

            #pragma shader_feature  _MAPPINGGRADIENT_UV _MAPPINGGRADIENT_WORLD_SPACE _MAPPINGGRADIENT_OBJECT_SPACE _HSV
            #pragma shader_feature  _MAPPINGTEXTURE_UV _MAPPINGTEXTURE_WORLD_SPACE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        
 
            CBUFFER_START(UnityPerMaterial)
            //TEXTURE2D(_MainTex);
			//SAMPLER(sampler_MainTex);
            sampler2D _EmissionTex;
            sampler2D _MainTex;
            
            uniform float4 _EmissionTex_ST;
            float4 _MainTex_ST;

            sampler2D _BaseMap;
            float4 _BaseMap_ST;

            float4 _ShadowColor;
            float4 _HColor, _MainColor;
            float4 _ColorLine;
			float  _RampThreshold, _RampSmooth, _WidthLine;
            float  _GSRampThreshold, _GSRampSmooth;
            float3 _Vector, _VectorGradient;
            float _Hue, _Contrast, _Saturation;

            float4 _Color1;
            float4 _Color2;
	        float  _Height, _Smooth;
	        

            float4 _SpecColors;
            float  _Shin;
                
            float  _smoothSpec;

            float4 _FresnelColor;
	        float _FresnelExponent;
	        float _FresnelPower;
	        float _SaturationCompensation;

            float _Speed;
            float _SwayMax;
            float _YOffset;
            float _ROffset;
            float _Rigidness;
            float _Radius;
            float _MaxWidth;
            float _weightObject,_HightOffset;

            float _grayscaleFactor;
            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
         
            CBUFFER_END
            

                
              
 
                                           //-------------------RGB2HVS-------------------//
            
		                                    float4x4 contrastMatrix (float c)
                                            {
                                                float t = (1.0 - c) * 0.5;
                                                return float4x4 (c, 0, 0, 0, 0, c, 0, 0, 0, 0, c, 0, t, t, t, 1);
                                            }
 
                                            float3 RGBToHSV(float3 c)
                                            {
                                                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                                                float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
                                                float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
                                                float d = q.x - min( q.w, q.y );
                                                float e = 1.0e-10;
                                                return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
                                            }
 
                                            float3 HSVToRGB( float3 c )
                                            {
                                                float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
                                                float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
                                                return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
                                            }
       
                                            float3 Hue( float3 p, float v )
                                            {
                                                p = RGBToHSV(p);
                                                p.x *= v;
                                                return HSVToRGB(p);
                                            }
       
                                            float3 Saturation( float3 p, float v )
                                            {
                                                p = RGBToHSV(p);
                                                p.y *= v;
                                                return HSVToRGB(p);
                                            }
       
                                            float3 Contrast( float3 p, float v )
                                            {
                                                return mul(float4(p,1.0), contrastMatrix(v)).rgb;
                                            }



            struct Attributes
            {
                float3 normalOS                 : NORMAL;
                float4 positionOS               : POSITION;
                 float4 positionCS               :POSITION;
                float2 uv		                : TEXCOORD0;
                float3 viewDir                  : TEXCOORD3;
               
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct Varyings
            {
                float3 normalWS                 : NORMAL;
                float4 positionCS               : SV_POSITION;
                float3 positionWS               : TEXCOORD2;
                float3 positionOS               : TEXCOORD6;
                float3 positionObj               : TEXCOORD5;
                float3 oPosN              : TEXCOORD4;
                float fogCoord                  : TEXCOORD1;
                float3 viewDir                  : TEXCOORD3;
                float2 uv		    : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
 
           
			
           

            Varyings vert (Attributes input)
            {
               
                Varyings output = (Varyings)0;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
  
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);      
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.positionOS = input.positionOS.xyz;

                output.viewDir = normalize( _WorldSpaceCameraPos.xyz - output.positionWS);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

                
                
                return output;
            }
 
            half4 frag(Varyings input) : SV_Target
            {
             
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
#if _MAPPINGTEXTURE_UV
                
                half4 MainTex = tex2D(_MainTex, input.uv) * (_MainColor * 1.5);
#elif _MAPPINGTEXTURE_WORLD_SPACE
               
                half4 MainTex = tex2D(_MainTex, (input.positionWS.xz * _MainTex_ST.xy) + _MainTex_ST.zw) * (_MainColor * 1.5);
#endif

                half4 EmissionTex = tex2D(_EmissionTex, (input.uv * _EmissionTex_ST.xy) + _EmissionTex_ST.zw);

                    //-------------------RGB2HVS-------------------//
                        MainTex.rgb = Hue(MainTex.rgb, _Hue);   
                        MainTex.rgb = Saturation(MainTex.rgb, _Saturation);
                        MainTex.rgb = Contrast(MainTex.rgb, _Contrast); 
               
#if _GRAYSCALE_ON                  
                        float intensity = MainTex.x * 0.299 + MainTex.y * 0.587 + MainTex.z * 0.114;

                        float4 GrayScaleMainTex = float4(lerp(MainTex.rgb, intensity, _grayscaleFactor), 0);
                        //float GSramp = smoothstep(_GSRampThreshold - _GSRampSmooth, _GSRampThreshold + _GSRampSmooth, mul(oPosN, _Vector));
              //float GSramp_1 = smoothstep(_GSRampThreshold + _WidthLine - _GSRampSmooth, _GSRampThreshold + _WidthLine + _GSRampSmooth, mul(oPosN, _Vector));

              //float4 ColorLine = lerp(MainTex, EmissionTex + _ColorLine, GSramp) ;
              //float4 VectorGrayScale = lerp(ColorLine, GrayScaleMainTex, GSramp_1) ;

                        MainTex = GrayScaleMainTex;
#endif

                half4 diffuseLight = half4(1,1,1,1);
                float4 gradient = half4(1,1,1,1);
                half4 color = half4(1,1,1,1);
                float4 fresnelColor = half4(1,1,1,1);
                half4 fog;
 

               //float3 oPosN = TransformObjectToWorldNormal(input.positionOS);
            
                VertexPositionInputs vertexInput = (VertexPositionInputs)0;
                vertexInput.positionWS = input.positionWS;
 
                float3 normal = normalize(input.normalWS);
                float4 shadowCoord = GetShadowCoord(vertexInput);
                half shadowAttenutation = MainLightRealtimeShadow(shadowCoord);
  
                float3 L = _MainLightPosition;
                half nl = dot(normal,L) * 0.5 + 0.5;

               
                float ramp = smoothstep(_RampThreshold - _RampSmooth * 0.5, _RampThreshold + _RampSmooth * 0.5, nl) * shadowAttenutation;

#if _GRAYSCALE_ON                  
                _ShadowColor = (_ShadowColor.x * 0.299 + _ShadowColor.y * 0.587 + _ShadowColor.z * 0.114) * 1.9;  //need test for coefficient
                _HColor = (_HColor.x * 0.299 + _HColor.y * 0.587 + _HColor.z * 0.114) * 0.88;

                
#endif    

                diffuseLight = lerp(_ShadowColor, _HColor, ramp);





                
                color = MainTex * diffuseLight ;

                //---------------------------------GRADIENT----------------------------------//
#if _SHOW_GRADIENT_ON
                #if _MAPPINGGRADIENT_UV
                float mapping = mul(input.uv, _VectorGradient);

                #elif _MAPPINGGRADIENT_WORLD_SPACE
                float mapping = mul(input.positionWS, _VectorGradient);

                #elif _MAPPINGGRADIENT_OBJECT_SPACE
                float mapping = mul(input.positionOS.xyz, _VectorGradient.xyz);
                
                #endif

                gradient = lerp(_Color1, _Color2, (mapping - _Height) / _Smooth);

               // color += gradient;
                color = lerp(color, gradient, 0.5);
#endif      
                //gradient = smoothstep((mapping * 0.001 + _Height * 0.001) - _Smooth * 0.01, (mapping * 0.001 + _Height * 0.001) + _Smooth * 0.01, _Color1);
                //gradient *= smoothstep((-mapping * 0.001 - _Height * 0.001) - _Smooth * 0.01, (-mapping * 0.001 - _Height * 0.001) + _Smooth * 0.01, _Color2);

           //---------------------------------GRADIENT-END------------------------------//

                

#if _SHOW_FRESNEL_ON
                float fresnel = 1 - saturate(dot(input.normalWS, input.viewDir));
                fresnel = saturate(pow(fresnel, _FresnelPower) * _FresnelExponent);
                fresnelColor = fresnel * _FresnelColor * _SaturationCompensation;
    #if _GRAYSCALE_ON  
        #if _GRAYSCALEFRESNEL_ON                  
                fresnelColor = (fresnelColor.x * 0.299 + fresnelColor.y * 0.587 + fresnelColor.z * 0.114);  //need test for coefficient

        #endif

    #endif 
                color += fresnelColor;
#endif

#if _SHOW_SPECULAR_ON
                float3 halfVector = normalize(_MainLightPosition + input.viewDir);
                float NdotH = dot(normal, halfVector);
                float lightIntensity = smoothstep(0, 0.01, NdotH);
                float specularIntensity = pow(NdotH * lightIntensity, _Shin * _Shin);
                float specularIntensitySmooth = smoothstep(0.005, _smoothSpec, specularIntensity);

                float4 specular = specularIntensitySmooth * _SpecColors * shadowAttenutation;
    #if _GRAYSCALE_ON      
            #if _GRAYSCALESPECULAR_ON
                specular = (specular.x * 0.299 + specular.y * 0.587 + specular.z * 0.114);  //need test for coefficient
               
            #endif

    #endif    

                color += specular;
#endif

                //color = MainTex * diffuseLight * gradient + fresnelColor + specular;
                color.rgb = MixFogColor(color.rgb, unity_FogColor, input.fogCoord);
#if _ENVIRLIGHTING_ON
                color += half4(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w, 0);
#endif
                return color;
            }
 


           
            ENDHLSL
        }

        
     


    
        Pass{

           
                Tags { "LightMode" = "ShadowCaster" }

           HLSLPROGRAM

           #pragma vertex ShadowPassVertex
    #pragma fragment ShadowPassFragment

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
   
            #pragma multi_compile_fog
            #pragma enable_cbuffer

            
          
           #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"


         

                struct VertexInput
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;

#if _ALPHATEST_ON
                float2 uv     : TEXCOORD0;
#endif

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
#if _ALPHATEST_ON
                float2 uv     : TEXCOORD0;
#endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
                    UNITY_VERTEX_OUTPUT_STEREO

            };

            VertexOutput ShadowPassVertex(VertexInput v)
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
                float3 normalWS = TransformObjectToWorldNormal(v.normal.xyz);

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));

                o.vertex = positionCS;


                return o;
            }

            half4 ShadowPassFragment(VertexOutput i) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                #if _ALPHATEST_ON
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);;
                clip(col.a - _Alpha);
                #endif

                return 0;
            }

                ENDHLSL

        }

    } 

   
    
    CustomEditor "OldCustomEditor"
    
}