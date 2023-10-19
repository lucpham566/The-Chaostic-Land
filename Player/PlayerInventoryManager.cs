using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    public ItemClass itemAdd;
    public ItemClass itemRemove;
    public List<SlotClass> items = new List<SlotClass>();

    // Start is called before the first frame update
    void Start()
    {
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
