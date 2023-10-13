using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    private Inventory inventory;
    public Transform ItemSlotContainer;
    public Transform ItemSlotTemplate;

    private void Awake() {
        //ItemSlotContainer = transform.Find("ItemSlotContainer");
    }

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems() {
        foreach (Transform child in ItemSlotContainer) {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.GetItemList()) {
            RectTransform itemSlotRectTransform = Instantiate(ItemSlotTemplate, ItemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            Image icon = itemSlotRectTransform.Find("InventorySlotIcon").GetComponent<Image>();
            icon.sprite = item.GetSprite();

            TextMeshProUGUI amountText = itemSlotRectTransform.Find("AmountText").GetComponent<TextMeshProUGUI>();
            amountText.SetText(item.Amount.ToString());

            Debug.Log("Spawned inventory slot");
        }
    }
}
