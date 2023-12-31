using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotButton: MonoBehaviour
{
    public ItemClass itemClass;
    public InventoryManager inventoryManager;
    public EquipmentManager equipmentManager;

    private void Update()
    {
    }

    public void Onclick()
    {
        resetActiveSlot();
        gameObject.GetComponent<Image>().color = new Color32(172, 255, 174, 255);
        if (itemClass != null)
        {   
            inventoryManager.itemSelected = itemClass.GetItem();
        }
        else
        {
            inventoryManager.itemSelected = null ;

        }
        inventoryManager.showInfomation();
        inventoryManager.showButtonInteractive();
    }
    private void resetActiveSlot()
    {
        foreach (GameObject slot in inventoryManager.slots)
        {
            slot.GetComponent<Image>().color = new Color(255, 255, 255);
        }

        Debug.Log(equipmentManager.slots + "equipmentManager.slots");
        foreach (GameObject slot in equipmentManager.slots)
        {
            slot.GetComponent<Image>().color = new Color(255, 255, 255);
        }
    }

}
