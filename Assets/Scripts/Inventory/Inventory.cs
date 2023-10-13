using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory {
    public event EventHandler OnItemListChanged;

    [SerializeField] public List<Item> ItemList;

    public Inventory() {
        ItemList = new List<Item>();

        AddItem(new Item { ItemID = 2, ItemName = "Dish", itemType = ItemType.Dish, Amount = 1});
        AddItem(new Item { ItemID = 0, ItemName = "Processed", itemType = ItemType.IngredientProcessed, Amount = 1});
        AddItem(new Item {ItemID = 1, ItemName = "Raw", itemType = ItemType.IngredientRaw, Amount = 1});
        Debug.Log(ItemList.Count);
    }

    public void AddItem(Item item) {
        bool itemInInventory = false;
        foreach(Item inventoryItem in ItemList) {
            if(inventoryItem.ItemName == item.ItemName) {
                inventoryItem.Amount += item.Amount;
                itemInInventory = true;
            }
        }
        if(!itemInInventory) {
            ItemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item) {
        Item itemInInventory = null;
        foreach (Item inventoryItem in ItemList) {
            if (inventoryItem.ItemName == item.ItemName) {
                inventoryItem.Amount -= item.Amount;
                itemInInventory = inventoryItem;
            }
        }
        if (itemInInventory != null && itemInInventory.Amount <= 0) {
            ItemList.Remove(itemInInventory);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return ItemList;
    }
}
