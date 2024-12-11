using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

namespace Snippets.Items.Demo
{
    /// <summary>
    /// Very simple showcase
    /// </summary>
    public class ItemsDemo : MonoBehaviour
    {
        [Header("Look")]
        [SerializeField] ItemsInventory inventory;

        [Header("UI")]
        [SerializeField] Button getStoneButton;
        [SerializeField] Button getWoodButton;
        [SerializeField] Button craftPickaxeButton;

        [Header("Items")]
        [SerializeField] Item stone;
        [SerializeField] Item wood;
        [SerializeField] Item pick;
        [SerializeField] Item[] requiredToGetPick;

        const string saveKey = "items.demo";
        const string resourcesPath = "ItemsDemo";

        bool isInited;

        private void Start()
        {
            Load();

            var configs = Resources.LoadAll<ItemConfig>(resourcesPath);
            inventory.Init(configs);

            // Subscribe UI buttons
            getStoneButton.onClick.AddListener(() =>
            {
                inventory.Add(stone);
            });

            getWoodButton.onClick.AddListener(() => 
            {
                inventory.Add(wood);
            });

            craftPickaxeButton.onClick.AddListener(() =>
            {
                inventory.Subtract(requiredToGetPick);
                inventory.Add(pick);
            });

            isInited = true;
        }

        private void Update()
        {
            if (!isInited) return;
            // Enable or disable craft button
            craftPickaxeButton.interactable = inventory.IsEnough(requiredToGetPick);
        }

        private void OnDestroy() 
        {
            Save();
        }

        // You don't need to save or load manually when using PlayerData
        private void Save()
        {
            SaveExtension.Save(inventory, saveKey);
        }

        private void Load()
        {
            inventory = SaveExtension.Load(saveKey, new ItemsInventory());
        }
    }
}
