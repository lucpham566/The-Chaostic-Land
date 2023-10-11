using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class EquipmentManager : MonoBehaviour
{
    public List<EquipmentClass> equipmentList;
    public UICharacter UICharacter;
    public PlayerCharacter playerCharacter;
    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
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

        UpdateProperties();

    }

    public void UnequipItem(ItemType itemType)
    {
        // Tìm và xóa itemType ra khỏi danh sách
        for (int i = 0; i < equipmentList.Count; i++)
        {
            if (equipmentList[i].itemType == itemType)
            {
                equipmentList.RemoveAt(i);
                break;
            }
        }

        UpdateProperties();

        // Cập nhật các thuộc tính của nhân vật sau khi tháo item ra khỏi trang bị.
    }

    private void UpdateProperties()
    {
        UICharacter.RefreshUI();
        playerCharacter.AddEquipmentStats();
    }
}
