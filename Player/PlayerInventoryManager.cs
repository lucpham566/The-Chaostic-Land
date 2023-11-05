using Spine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public string instance_id;
    public string item_id;
}

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items;
}


public class PlayerInventoryManager : MonoBehaviour
{
    public ItemClass itemAdd;
    public ItemClass itemRemove;
    public List<SlotClass> items = new List<SlotClass>();
    public TextAsset jsonFileName;

    // Start is called before the first frame update
    void Start()
    {

        // Deserialize JSON thành đối tượng PlayerData
        InventoryData inventory = JsonUtility.FromJson<InventoryData>(jsonFileName.text);

        foreach (var item in inventory.items)
        {
            Debug.Log("Instance ID: " + item.instance_id);
            Debug.Log("Item ID: " + item.item_id);
        }

        AddItem(itemAdd,5);
        RemoveItem(itemRemove,2);
    }
    void Update()
    {
       
    }
    public void AddItem(ItemClass item,int quantity)
    {
        if ( item is EquipmentClass)
        {
            items.Add(new SlotClass(item, 1));
        }
        else
        {
            SlotClass slot = ContainsItem(item);
            if (slot != null)
            {
                slot.AddQuantity(quantity);
            }
            else
            {
                items.Add(new SlotClass(item, quantity));
            }
        }
    }

    public void RemoveItem(ItemClass item, int quantity)
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null)
        {
            if (temp.GetQuantity()>1)
            {
                temp.SubQuantity(quantity);
            }
            else
            {
                SlotClass slotToRemove = new SlotClass();

                foreach (SlotClass slot in items)
                {
                    if (slot.GetItem() == item)
                    {
                        slotToRemove = slot;
                        break;
                    }
                }

                items.Remove(slotToRemove);
            }
        }
        else
        {
            return;
        }
      
    }

    private SlotClass ContainsItem (ItemClass item)
    {
        foreach (SlotClass slot in items) {
            if (slot.GetItem() == item)
            {
                return slot;
            }
        }
        return null;
    }
}
