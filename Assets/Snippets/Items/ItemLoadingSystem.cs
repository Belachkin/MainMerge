using Kuhpik;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemLoadingSystem : GameSystem
{
    [InfoBox("Resources path to Item Configs")]
    [SerializeField] string loadingPath;

    public override void OnInit()
    {
        var configs = Resources.LoadAll<ItemConfig>(loadingPath);

        // You probably wanna do something like this.
        // player.items ??= new ItemsInventory();
        // player.items.Init(configs);
    }
}
