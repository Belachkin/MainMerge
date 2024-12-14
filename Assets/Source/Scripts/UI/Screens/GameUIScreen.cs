using Kuhpik;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;
using Supyrb;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Source.Scripts.Data;
using TMPro;
using TextMeshProUGUI = TMPro.TextMeshProUGUI;

public class GameUIScreen : UIScreen
{
    public float animationScale = 1.5f; 
    public float animationDuration = 0.2f;
    
    public GameObject taskUIPrefab;
    public Transform taskPanel;

    
    public SerializedDictionary<MergeObjectType, TextMeshProUGUI> taskTexts;

    public TextMeshProUGUI MoneyText;
    
    

    
    
}
