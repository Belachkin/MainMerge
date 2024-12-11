using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Item")]
public class ItemConfig : ScriptableObject
{
    [Header("Base")]
    [SerializeField] Sprite icon;
    [SerializeField] GameObject prefab;
    [SerializeField] bool hasMaxValue;
    [SerializeField, ShowIf("hasMaxValue")] float maxValue;

    public string Id => name?? string.Empty;

    public Sprite Icon => icon;
    public GameObject Prefab => prefab;
    public bool HasMaxValue => hasMaxValue;
    public float MaxValue => maxValue;
}
