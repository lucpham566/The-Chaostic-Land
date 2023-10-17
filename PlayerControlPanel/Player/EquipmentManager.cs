using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class EquipmentManager : MonoBehaviour
{
    public List<EquipmentClass> equipmentList;
    public UICharacter UICharacter;
    public PlayerCharacter playerCharacter;
    public GearEquipper gearEquipper;
    public GameObject slotsHolder;
    public GameObject[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];
        for (int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        playerCharacter = GetComponent<PlayerCharacter>();
        gearEquipper = GetComponent<GearEquipper>();
        RefreshUI();
    }
    public void EquipItem(EquipmentClass item)
    {
        bool itemTypeExists = false;

        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (equipmentList[i].itemType == item.itemType)
            {
                // Nếu đã tồn tại, thay thế nó bằng item mới
                equipmentList[i] = item;
                itemTypeExists = true;
                break;
            }
        }

        if (!itemTypeExists)
        {
            // Nếu itemType chưa tồn tại, thêm item vào danh sách
            equipmentList.Add(item);
        }

        RefreshUI();
        UpdateProperties();

    }

    public void UnequipItem(EquipmentClass item)
    {
        // Tìm và xóa itemType ra khỏi danh sách
        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (equipmentList[i].itemType == item.itemType)
            {
                equipmentList.RemoveAt(i);
                break;
            }
        }

        RefreshUI();
        UpdateProperties();

        // Cập nhật các thuộc tính của nhân vật sau khi tháo item ra khỏi trang bị.
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            SlotButton slotButton = slots[i].GetComponent<SlotButton>();
            slotButton.itemClass = null;
            slots[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            slots[i].transform.GetChild(1).GetComponent<Image>().sprite = null;
        }

        for (int i = 0; i < equipmentList.Count; i++)
        {
            GameObject slotObject = slotsHolder.transform.Find(equipmentList[i].itemType.ToString()).gameObject;
            SlotButton slotButton = slotObject.GetComponent<SlotButton>();
            slotButton.itemClass = equipmentList[i].GetItem();

        }
    }

        private void UpdateProperties()
    {
        UICharacter.RefreshUI();
        playerCharacter.AddEquipmentStats();
        gearEquipper.LoadSkin();
    }
}
