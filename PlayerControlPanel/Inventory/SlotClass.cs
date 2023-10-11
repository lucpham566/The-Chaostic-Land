using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField]private ItemClass item;
    [SerializeField] private int quantity=0;

    public SlotClass()
    {
        item = null;
        quantity = 0;
    }

    public SlotClass(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public ItemClass GetItem() { 
        return item; 
    }

    public void AddQuantity(int _quantity)
    {
        quantity += _quantity;
    }
    public void SubQuantity(int _quantity)
    {
        quantity -= _quantity;
    }
}
