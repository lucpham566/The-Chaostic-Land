using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item")]
public abstract class ItemClass: ScriptableObject
{
    public string itemName;
    public string itemId;
    public Sprite itemIcon;

    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
    public abstract EquipmentClass GetEquipment();
    public abstract MiscClass GetMisc();
    public abstract ConsumableClass GetConsumable();

}
