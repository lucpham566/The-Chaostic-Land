using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slotsHolder;
    public EquipmentManager equipmentManager;
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setItem(EquipmentClass equipment)
    {
        ItemType type = equipment.itemType;

        Transform slotTransform = slotsHolder.transform.Find(type.ToString());
        GameObject slot = slotTransform.gameObject;
        slotTransform.GetChild(1).GetComponent<Image>().enabled = true;
        slotTransform.GetChild(1).GetComponent<Image>().sprite = equipment.GetItem().itemIcon;
    }

    public void RefreshUI()
    {
        List<EquipmentClass> equipmentList = equipmentManager.equipmentList;
        for (int i = 0; i < equipmentList.Count; i++)
        {
            setItem(equipmentList[i]);
        }
    }
}
