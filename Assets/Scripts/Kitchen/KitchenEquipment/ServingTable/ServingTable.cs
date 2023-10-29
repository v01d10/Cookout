using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ServingTable : MonoBehaviour {
    [SerializeField, HideInInspector] private InventoryUI servingTableUI;

    [SerializeField] public ServingTableBase thisServingTable;

    private void Start() {
        servingTableUI = GameObject.Find("ServingTableUI").GetComponent<InventoryUI>();
        thisServingTable.inventory = new Inventory();
    }

    private void OnMouseDown() {
        Debug.Log("Clicked serving table ! ");
        OpenServingTableUI();
    }

    public void OpenServingTableUI() {
        servingTableUI.SetInventory(thisServingTable.inventory);
    }
}
