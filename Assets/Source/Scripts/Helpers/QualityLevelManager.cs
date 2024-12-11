using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manager responsible for setting the DeviceQuality level of the current device.
/// This sets automatically the QualityLevel + targetframerate of the application
/// </summary>
public class QualityLevelManager : MonoBehaviour
{

    private void Start()
    {
        InitializeAutodetect();
    }

    public enum DeviceQualityLevel
    {
        LowEnd,
        MediumEnd,
        HighEnd
    }

    public DeviceQualityLevel Quality { get; private set; }


    public void InitializeAutodetect()
    {
        float batteryLevel = SystemInfo.batteryLevel;
        int systemMemorySize = SystemInfo.systemMemorySize;

        if (systemMemorySize <= 2048)
        {
            Quality = DeviceQualityLevel.LowEnd;
            Debug.Log($"RAM <= 2048 Bytes ({systemMemorySize}) : Quality index set to '{Quality}'");
        }
        else if (systemMemorySize <= 3072)
        {
            Quality = DeviceQualityLevel.MediumEnd;
            Debug.Log($"RAM between 2048 Bytes and 3072 Bytes ({systemMemorySize}): Quality index set to '{Quality}'");
        }
        else
        {
            Quality = DeviceQualityLevel.HighEnd;
            Debug.Log($"RAM > 3072 Bytes ({systemMemorySize}): Quality settings set to '{Quality}'");
        }

        if (batteryLevel <= .15f)
        {
            if (Quality > DeviceQualityLevel.LowEnd)
            {
                Quality -= 1;
                Debug.Log($"Battery level <= 15% ({batteryLevel}), reducing quality to '{Quality}'");
            }
        }

        ApplyQualitySettings(Quality);
    }

    private void ApplyQualitySettings(DeviceQualityLevel quality)
    {
        int qualityLevel;
        int targetFPS = 60;

        switch (Quality)
        {
            case DeviceQualityLevel.LowEnd:
                qualityLevel = 0;
                targetFPS = 30;
                break;
            case DeviceQualityLevel.MediumEnd:
                qualityLevel = 1;
                targetFPS = 60;
                break;
            case DeviceQualityLevel.HighEnd:
            default:
                qualityLevel = 2;
                targetFPS = 60;
                break;
        }

        Debug.Log($"Quality index : {Quality}, Quality level : {qualityLevel}, targetFramerate : {targetFPS}");
        QualitySettings.SetQualityLevel(qualityLevel, true);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }
}