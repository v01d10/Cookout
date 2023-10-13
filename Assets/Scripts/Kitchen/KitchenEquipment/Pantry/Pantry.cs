using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pantry : MonoBehaviour {
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] public Inventory PantryInventory;

    private void Awake() {
        PantryInventory = new Inventory();
    }

    public void OpenInventory() {
        inventoryUI.SetInventory(PantryInventory);
    }
}
