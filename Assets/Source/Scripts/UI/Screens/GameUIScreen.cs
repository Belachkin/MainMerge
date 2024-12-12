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
    private const float animationScale = 1.5f; 
    private const float animationDuration = 0.2f;
    
    public GameObject taskUIPrefab;
    public Transform taskPanel;

    public SerializedDictionary<MergeObjectType, int> tasks;
    public SerializedDictionary<MergeObjectType, TextMeshProUGUI> taskTexts;
    
    public void InitTasks(
        SerializedDictionary<MergeObjectType, int> levelTasks, 
        SerializedDictionary<MergeObjectType, Sprite> taskIcons)
    {

        for (int i = taskPanel.childCount - 1; i >= 0; i--)
        {
            Transform child = taskPanel.GetChild(i);
            Destroy(child.gameObject);
        }
        
        tasks.Clear();
        taskTexts.Clear();
        
        foreach (var task in levelTasks)    
        {
            tasks.Add(task.Key, task.Value);
            
            var newTaskUI = Instantiate(taskUIPrefab, taskPanel);
            
            newTaskUI.GetComponent<Image>().sprite = taskIcons[task.Key];
            
            var newTaskText = newTaskUI.GetComponentInChildren<TextMeshProUGUI>();
            newTaskText.text = task.Value.ToString();
            
            taskTexts.Add(task.Key, newTaskText);
            
        }
    }

    public void UpdateTask(MergeObjectType mergeObjectType)
    {
        GameObject animateObject = null;
        if (tasks[mergeObjectType] <= 0)
        {
            var img = taskTexts[mergeObjectType].gameObject.GetComponentInChildren<Image>();
            
            img.enabled = true;
            
            animateObject = img.gameObject;
            taskTexts[mergeObjectType].enabled = false;
        }
        else
        {
            taskTexts[mergeObjectType].text = tasks[mergeObjectType].ToString();
            animateObject = taskTexts[mergeObjectType].gameObject;
        }
        
        animateObject.transform
            .DOScale(animationScale, animationDuration) 
            .OnComplete(() => 
                animateObject.transform.DOScale(1f, animationDuration) 
            );
    }
    
}
