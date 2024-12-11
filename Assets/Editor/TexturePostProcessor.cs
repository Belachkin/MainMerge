// Greg Lukosek 2015
// lukos86@gmail.com

using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

public class TexturePostProcessor : AssetPostprocessor
{
    const int maxSize = 2048;
    string[] platforms = new string[] { "Android", "iPhone", "iOS" };

    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;

        // importer.anisoLevel = 1;
        // importer.mipmapEnabled = false;
        // importer.spritePixelsPerUnit = 100;
        // importer.filterMode = FilterMode.Bilinear;
        // importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;

        if (importer.maxTextureSize > maxSize)
            importer.maxTextureSize = maxSize;

        foreach (var platform in platforms)
        {
            TryOverridePlatformSettings(importer, platform);
        }

        Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));

        if (asset)
        {
            EditorUtility.SetDirty(asset);
            importer.SaveAndReimport();
        }
    }

    void TryOverridePlatformSettings(TextureImporter importer, string platform)
    {
        try
        {
            if (importer.GetPlatformTextureSettings(platform, out var platformMaxTextureSize, out var platformTextureFmt,
                 out var platformCompressionQuality, out var platformAllowsAlphaSplit))
            {
                var preferredSize = platformMaxTextureSize > maxSize ? maxSize : platformMaxTextureSize;
                importer.SetPlatformTextureSettings(platform, preferredSize, platformTextureFmt);
            }
        }

        catch (Exception e) { }
    }
}