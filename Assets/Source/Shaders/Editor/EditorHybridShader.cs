using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class OldCustomEditor : ShaderGUI
{
    
    public override void OnGUI(MaterialEditor editor, MaterialProperty[] prop)
    {
        //---------------------------------SEARCH_PROPERTIES-------------------------------------------//
        MaterialProperty _MappingTex = ShaderGUI.FindProperty("_MappingTexture", prop);
        MaterialProperty _Vector = ShaderGUI.FindProperty("_Vector", prop);
        MaterialProperty _MainTex = ShaderGUI.FindProperty("_MainTex", prop);
        MaterialProperty _MainColor = ShaderGUI.FindProperty("_MainColor", prop);
        MaterialProperty _Grayscale = ShaderGUI.FindProperty("_Grayscale", prop);
        MaterialProperty _GrayscaleFresnel = ShaderGUI.FindProperty("_GrayscaleFresnel", prop);
        MaterialProperty _GrayscaleSpecular = ShaderGUI.FindProperty("_GrayscaleSpecular", prop);
        MaterialProperty _grayscaleFactor = ShaderGUI.FindProperty("_grayscaleFactor", prop);

        //----------HSV-----------//
        MaterialProperty _Hue = ShaderGUI.FindProperty("_Hue", prop);
        MaterialProperty _Contrast = ShaderGUI.FindProperty("_Contrast", prop);
        MaterialProperty _Saturation = ShaderGUI.FindProperty("_Saturation", prop);

        MaterialProperty[] HSV_Block = { _Hue, _Contrast, _Saturation };

        //----------LIGHTING-----------//
        MaterialProperty _HColor = ShaderGUI.FindProperty("_HColor", prop);
        MaterialProperty _ShadowColor = ShaderGUI.FindProperty("_ShadowColor", prop);
        MaterialProperty _RampThreshold = ShaderGUI.FindProperty("_RampThreshold", prop);
        MaterialProperty _RampSmooth = ShaderGUI.FindProperty("_RampSmooth", prop);

        MaterialProperty[] LIGHTING_Block = { _HColor, _ShadowColor, _RampThreshold, _RampSmooth };

        //----------GRADIENT-----------//
        MaterialProperty _Show_Gradient = ShaderGUI.FindProperty("_Show_Gradient", prop);
        MaterialProperty _MappingGradient = ShaderGUI.FindProperty("_MappingGradient", prop);

        MaterialProperty _Color2 = ShaderGUI.FindProperty("_Color2", prop);
        MaterialProperty _Color1 = ShaderGUI.FindProperty("_Color1", prop);
        MaterialProperty _Height = ShaderGUI.FindProperty("_Height", prop);
        MaterialProperty _Smooth = ShaderGUI.FindProperty("_Smooth", prop);
        MaterialProperty _VectorGradient = ShaderGUI.FindProperty("_VectorGradient", prop);

        MaterialProperty[] GRADIENT_Block = { _MappingGradient, _Color2, _Color1, _Height, _Smooth, _VectorGradient };

        //----------FRESNEL-----------//
        MaterialProperty _Show_Fresnel = ShaderGUI.FindProperty("_Show_Fresnel", prop);

        MaterialProperty _FresnelColor = ShaderGUI.FindProperty("_FresnelColor", prop);
        MaterialProperty _FresnelExponent = ShaderGUI.FindProperty("_FresnelExponent", prop);
        MaterialProperty _FresnelPower = ShaderGUI.FindProperty("_FresnelPower", prop);
        MaterialProperty _SaturationCompensation = ShaderGUI.FindProperty("_SaturationCompensation", prop);

        MaterialProperty[] FRESNEL_Block = { _FresnelColor, _FresnelExponent, _FresnelPower, _SaturationCompensation };

        //----------SPECULAR-----------//
        MaterialProperty _Show_Specular = ShaderGUI.FindProperty("_Show_Specular", prop);

        MaterialProperty _SpecColors = ShaderGUI.FindProperty("_SpecColors", prop);
        MaterialProperty _Shin = ShaderGUI.FindProperty("_Shin", prop);
        MaterialProperty _smoothSpec = ShaderGUI.FindProperty("_smoothSpec", prop);

        MaterialProperty _EnvirLighting = ShaderGUI.FindProperty("_EnvirLighting", prop);

        MaterialProperty[] SPECULAR_Block = { _SpecColors, _Shin, _smoothSpec };

        //---------------------------------DRAW_PROPERTIES--------------------------------------------------//


        //----TEXTURE----//
        editor.ShaderProperty(_MappingTex, "Mapping texture");
        
        
        editor.ShaderProperty(_MainTex, "Texure");
        editor.ShaderProperty(_MainColor, "Main Color");
        editor.ShaderProperty(_Grayscale, "Grayscale");

        if (_Grayscale.floatValue == 1)
        {
            editor.ShaderProperty(_GrayscaleFresnel, _GrayscaleFresnel.displayName);
            editor.ShaderProperty(_GrayscaleSpecular, _GrayscaleSpecular.displayName);
        }
            //editor.ShaderProperty(_grayscaleFactor, _grayscaleFactor.displayName);


        //----HSV----//
            for (int i = 0; i < HSV_Block.Length; i++)
                editor.ShaderProperty(HSV_Block[i], HSV_Block[i].displayName);  

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //---LINE

        //----LIGHTING----//
        
        for (int i = 0; i < LIGHTING_Block.Length; i++)
            editor.ShaderProperty(LIGHTING_Block[i], LIGHTING_Block[i].displayName);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //---LINE


        //----Gradient----//

        editor.ShaderProperty(_Show_Gradient, _Show_Gradient.displayName);

        if (_Show_Gradient.floatValue == 1)
            for (int i = 0; i < GRADIENT_Block.Length; i++) 
                editor.ShaderProperty(GRADIENT_Block[i], GRADIENT_Block[i].displayName);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //---LINE

        //----FRESNEL----//
       
        editor.ShaderProperty(_Show_Fresnel, _Show_Fresnel.displayName);

        if (_Show_Fresnel.floatValue == 1) 
            for (int i = 0; i < FRESNEL_Block.Length; i++)
                editor.ShaderProperty(FRESNEL_Block[i], FRESNEL_Block[i].displayName);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //---LINE

        //----SPECULAR----//
       
        editor.ShaderProperty(_Show_Specular, _Show_Specular.displayName);
        if (_Show_Specular.floatValue == 1)
             for (int i = 0; i < SPECULAR_Block.Length; i++)
                editor.ShaderProperty(SPECULAR_Block[i], SPECULAR_Block[i].displayName);
   
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); //---LINE
        editor.ShaderProperty(_EnvirLighting, "Environment Lighting");
        editor.RenderQueueField();
        editor.EnableInstancingField();
        editor.DoubleSidedGIField();


    }





}


