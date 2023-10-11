using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder;
    public ItemClass itemAdd;
    public ItemClass itemRemove;
    public List<SlotClass> items = new List<SlotClass>();

    public GameObject[] slots;
    public GameObject itemInforUI;
    public GameObject buttonContainer;
    public ItemClass itemSelected;
    public EquipmentManager equipmentManager;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];

        for (int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        AddItem(itemAdd,5);
        RemoveItem(itemRemove,2);
        RefreshUI();
    }
    void Update()
    {
        
        //showButtonInteractive();
    }
    private void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            SlotButton slotButton = slots[i].GetComponent<SlotButton>();
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                slotButton.slotClass = items[i];

            }
            catch {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                slotButton.slotClass = null;
            }
        }
    }

    public void AddItem(ItemClass item,int quantity)
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
        RefreshUI();
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
      
        RefreshUI();
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

    public void UserSellectedItem()
    {
        if (itemSelected is EquipmentClass)
        {
            equipmentManager.EquipItem((EquipmentClass)itemSelected);
        }
    }

    public void showInfomation()
    {
        if (itemSelected)
        {
            itemInforUI.SetActive(true);
            Transform itemName = itemInforUI.transform.Find("name");
            itemName.GetComponent<TextMeshProUGUI>().text = itemSelected.name;
        }
        else
        {
            itemInforUI.SetActive(false);
        }
    }
    public void showButtonInteractive()
    {
        if (itemSelected)
        {
            buttonContainer.SetActive(true);
            if (itemSelected is EquipmentClass)
            {
                buttonContainer.transform.Find("ButtonUse").gameObject.SetActive(true);
            }
            else
            {
                buttonContainer.transform.Find("ButtonUse").gameObject.SetActive(false);
            }
        }
        else
        {
            buttonContainer.SetActive(false);
        }
    }
}
