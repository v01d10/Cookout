using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour {
    [SerializeField, HideInInspector] private InventoryUI inventoryUI;

    [SerializeField] public InventoryEquipment fridgeBase;    

    private void Start() {
        inventoryUI = GameObject.Find("UI").GetComponent<InventoryUI>();
        fridgeBase.inventory = new Inventory();
    }

    private void OnMouseDown() {
        Debug.Log("Clicked fridge!");
        OpenInventory();
    }

    public void OpenInventory() {
        inventoryUI.SetInventory(fridgeBase.inventory);
    }
}
