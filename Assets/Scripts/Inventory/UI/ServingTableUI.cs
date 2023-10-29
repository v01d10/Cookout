using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ServingTableUI : MonoBehaviour {
    private Inventory inventory;

    public RectTransform[] ServingTableSlots;
    public GameObject servingTableUI;

    private void Start() {
        RefreshSlots();
    }

    public void ShowServingTableUI() {
        servingTableUI.SetActive(true);
    }

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        ShowServingTableUI();
        RefreshSlots();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        RefreshSlots();
    }

    public void RefreshSlots() {
        foreach (var slot in ServingTableSlots) {
            Image icon = slot.Find("InventorySlotIcon").GetComponent<Image>();
            icon.gameObject.SetActive(false);

            TextMeshProUGUI amountText = slot.Find("AmountText").GetComponent<TextMeshProUGUI>();
            amountText.text = null;
        }

        for (int i = 0; i < inventory.GetItemList().Count; i++) {
            Item item = inventory.GetItemList()[i];
            Image icon = ServingTableSlots[i].Find("InventorySlotIcon").GetComponent<Image>();
            icon.gameObject.SetActive(true);
            icon.sprite = item.GetSprite();

            TextMeshProUGUI amountText = ServingTableSlots[i].Find("AmountText").GetComponent<TextMeshProUGUI>();
            amountText.SetText(item.Amount.ToString());

            Debug.Log("Refreshed inventory slot");
        }
    }
}
