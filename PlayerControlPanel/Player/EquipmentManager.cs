using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentManager : MonoBehaviour
{
    public List<EquipmentClass> equipmentList;
    public GameObject slotsHolder;
    public GameObject[] slots;

    public PlayerEquipmentManager playerEquipmentManager;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];
        for (int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        playerEquipmentManager = PhotonPlayer.local.GetComponent<PlayerEquipmentManager>();

        equipmentList = playerEquipmentManager.equipmentList;

        RefreshUI();
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

            slotObject.transform.GetChild(1).GetComponent<Image>().enabled = true;
            slotObject.transform.GetChild(1).GetComponent<Image>().sprite = equipmentList[i].GetItem().itemIcon;

        }
    }

       
}
