using System;
using UnityEngine;

[Serializable]
public class Item
{
    [field: SerializeField] public ItemConfig Config { get; private set; }
    [HideInInspector] [SerializeField] string id;
    [SerializeField] float count;

    public event Action<float> OnValueChanged;
    public float Count => count;

    public string Id
    {
        get
        {
            if (Config == null) return id;
            else if (string.IsNullOrEmpty(id) || id != Config.Id) SetConfig(Config);
            return id;
        }
    }

    public Item() { }

    public Item(ItemConfig config) : this() { SetConfig(config); }

    public Item(ItemConfig config, float count) : this(config) { this.count = count; }

    public void SetConfig(ItemConfig config)
    {
        Config = config;
        id = config.Id;
    }

    /// <summary>
    /// Adds count to current item
    /// </summary>
    /// <param name="count"></param>
    public void Add(float count)
    {
        this.count += count;

        if (Config.HasMaxValue)
        {
            this.count = Mathf.Clamp(this.count, 0, Config.MaxValue);
        }

        OnChange();
    }

    /// <summary>
    /// Adds passed item count to current item
    /// </summary>
    /// <param name="item"></param>
    public void Add(Item item)
    {
        if (IsTheSameItemType(item))
        {
            Add(item.Count);
        }
    }

    /// <summary>
    /// Removes count from current item. With minimum value clamped at 0
    /// </summary>
    /// <param name="count"></param>
    public void Subtract(float count)
    {
        if (!CompareCount(count))
        {
            Debug.LogError("You are trying to Subtract count that is bigger than count of this item. " +
            "Consider using CompareCount() method before any Subtract operations. " +
            $"This ussue adressed to the item with id: <color=orange>{ Config.Id }</color>");

            return;
        }

        this.count = Mathf.Clamp(this.count - count, 0, float.MaxValue);
        OnChange();
    }

    /// <summary>
    /// Removes passed item count from current item. With minimum value clamped at 0
    /// </summary>
    /// <param name="item"></param>
    public void Subtract(Item item)
    {
        if (IsTheSameItemType(item))
        {
            Subtract(item.Count);
        }
    }

    /// <summary>
    /// Compares current item count with passed count
    /// </summary>
    /// <returns>Returns true if item count is bigger or equals to passed count</returns>
    public bool CompareCount(float count)
    {
        return this.count >= count;
    }

    /// <summary>
    /// Compares current item count with passed item count
    /// </summary>
    /// <param name="item">Other item</param>
    /// <returns>Returns true if item count is bigger or equals to passed item count</returns>
    public bool CompareCount(Item item)
    {
        if (!IsTheSameItemType(item))
        {
            return false;
        }

        return CompareCount(item.Count);
    }

    bool IsTheSameItemType(Item item)
    {
        bool Identity = Config.Id == item.Config.Id;

        if (Identity == false)
        {
            Debug.LogError($"You are trying to make math operation between two items with different type." +
            $"<color=orange>{Config.Id}</color> and <color=orange>{item.Config.Id}</color>");
        }

        return Identity;
    }

    void OnChange()
    {
        OnValueChanged?.Invoke(count);
    }
}
