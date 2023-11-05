using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder;
    public ItemClass itemAdd;
    public ItemClass itemRemove;
    public List<SlotClass> items = new List<SlotClass>();

    public PlayerInventoryManager playerInventoryManager;


    public GameObject[] slots;
    public GameObject itemInforUI;
    public GameObject buttonContainer;
    public ItemClass itemSelected;
    public PlayerEquipmentManager playerEquipmentManager;
    public EquipmentManager equipmentManager;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];

        for (int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        playerInventoryManager = PhotonPlayer.local.GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = PhotonPlayer.local.GetComponent<PlayerEquipmentManager>();
        items = playerInventoryManager.items;
        
        RefreshUI();
    }
    void Update()
    {

        //showButtonInteractive();
    }
    private void RefreshUI()
    {
        List<string> equipmentIDs = playerEquipmentManager.equipmentList.Select(equipment => equipment.itemId).ToList();
        List<SlotClass> itemListShow = new List<SlotClass>(items);
        itemListShow.RemoveAll(item => equipmentIDs.Contains(item.GetItem().itemId));

        for (int i = 0; i < slots.Length; i++)
        {
            SlotButton slotButton = slots[i].GetComponent<SlotButton>();
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemListShow[i].GetItem().itemIcon;
                if (itemListShow[i].GetQuantity() > 1)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemListShow[i].GetQuantity().ToString();
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                slotButton.itemClass = itemListShow[i].GetItem();

            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                slotButton.itemClass = null;
            }
        }
    }

    public void AddItem(ItemClass item, int quantity)
    {
        playerInventoryManager.AddItem(item, quantity);

        RefreshUI();
    }

    public void RemoveItem(ItemClass item, int quantity)
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
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

    private SlotClass ContainsItem(ItemClass item)
    {
        foreach (SlotClass slot in items)
        {
            if (slot.GetItem() == item)
            {
                return slot;
            }
        }
        return null;
    }

    public void UserSellectedItem()
    {
        Debug.Log("cos sanr pham ne");

        if (itemSelected is EquipmentClass)
        {
            Debug.Log("cos sanr pham ne" + itemSelected.itemName);
            playerEquipmentManager.RPC_EquipItem((EquipmentClass)itemSelected);
            RefreshUI();
            equipmentManager.RefreshUI();
            resetSelectedItem();
        }
    }

    public void UnequipSellectedItem()
    {
        if (itemSelected is EquipmentClass)
        {
            playerEquipmentManager.UnequipItem((EquipmentClass)itemSelected);
            AddItem(itemSelected, 1);
            RefreshUI();
            equipmentManager.RefreshUI();
            resetSelectedItem();
        }
    }
    public void resetSelectedItem()
    {
        itemSelected = null;
        showButtonInteractive();
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
                if (playerEquipmentManager.equipmentList.Contains(itemSelected))
                {
                    buttonContainer.transform.Find("ButtonUnequip").gameObject.SetActive(true);
                    buttonContainer.transform.Find("ButtonUse").gameObject.SetActive(false);
                }
                else
                {
                    buttonContainer.transform.Find("ButtonUse").gameObject.SetActive(true);
                    buttonContainer.transform.Find("ButtonUnequip").gameObject.SetActive(false);
                }
            }
            else
            {
                buttonContainer.transform.Find("ButtonUnequip").gameObject.SetActive(false);
                buttonContainer.transform.Find("ButtonUse").gameObject.SetActive(false);
            }
        }
        else
        {
            buttonContainer.SetActive(false);
        }
    }
}
