using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ItemsInventory
{
    [SerializeField] List<Item> itemsToSave;

    Dictionary<string, Item> items;

    public void Init(IEnumerable<ItemConfig> itemConfigs)
    {
        itemsToSave ??= new List<Item>();
        items = itemsToSave.ToDictionary(x => x.Id, x => x);

        foreach (var config in itemConfigs)
        {
            CreateOrInitItem(config);
        }
    }

    public Item this[string id]
    {
        get
        {
            if (items == null)
            {
                Debug.LogError("Inventory was not initialized");
                return null;
            }

            else if (!items.ContainsKey(id))
            {
                Debug.LogError($"Item with ID: {id} not found. Check your init function");
                return null;
            }

            return items[id];
        }
    }

    public void Add(Item item)
    {
        this[item.Id].Add(item);
    }

    public void Add(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public void Subtract(Item item)
    {
        this[item.Id].Subtract(item);
    }

    public void Subtract(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            Subtract(item);
        }
    }

    public bool IsEnough(Item item)
    {
        return this[item.Id].CompareCount(item);
    }

    /// <summary>
    /// Fast check
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public bool IsEnough(IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            if (!IsEnough(item))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsEnough(IEnumerable<Item> items, out IEnumerable<Item> missingItems)
    {
        var requiredItems = new List<Item>();

        foreach (var item in items)
        {
            if (!IsEnough(item))
            {
                requiredItems.Add(item);
            }
        }

        missingItems = requiredItems;
        return requiredItems.Count == 0;
    }

    void CreateOrInitItem(ItemConfig config)
    {
        if (!items.ContainsKey(config.Id))
        {
            var id = config.Id;
            var item = new Item(config, 0);

            items.Add(id, item);
            itemsToSave.Add(item);
        }
        else
        {
            this[config.Id].SetConfig(config);
        }
    }
}
