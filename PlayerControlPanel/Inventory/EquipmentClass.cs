using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    VuKhi,
    Ao,
    Mu,
    Quan,
    Gang,
    Giay,
    Nhan
    // Thêm các giá trị khác tại đây
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/ Equipment")]
public class EquipmentClass : ItemClass
{
    [Header("Equipment")]

    public ItemType itemType;
    public int physicsDamage;
    public int magicDamage;
    public int physicsDefense;
    public int magicDefense;
    public int hp;
    public int mana;

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return null; }
    public override EquipmentClass GetEquipment() { return this; }

}
