using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEquipmentManager : NetworkBehaviour
{
    public List<EquipmentClass> equipmentList;
    public PlayerCharacter playerCharacter;
    public GearEquipper gearEquipper;
    [Networked(OnChanged = nameof(OnchangedTest))] public int ABCD { get; set; }

    public static void OnchangedTest(Changed<PlayerEquipmentManager> changedd)
    {
        Debug.Log("đã thay đổi nè" + changedd.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        gearEquipper = GetComponent<GearEquipper>();
        ABCD = 1;
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
        ABCD = ABCD * -2;

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

        UpdateProperties();

        // Cập nhật các thuộc tính của nhân vật sau khi tháo item ra khỏi trang bị.
    }

    private void UpdateProperties()
    {
        playerCharacter.AddEquipmentStats();
        gearEquipper.LoadSkin();
    }
}
