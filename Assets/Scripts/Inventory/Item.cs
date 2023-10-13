using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Dish,
    IngredientRaw,
    IngredientProcessed
}

[Serializable]
public class Item {
    public string ItemName;
    public int ItemID;
    public ItemType itemType;
    public int Amount;
    public float PrepareTime;
    public int RequiredLevel;
    public float Price;
    public List<int> RequiredItems;

    public Sprite GetSprite() {
        switch (itemType) {
            default:
            case ItemType.Dish:
                return ItemAssets.Instance.DishSprites[ItemID];
            case ItemType.IngredientRaw:
                return ItemAssets.Instance.IngredientRawSprites[ItemID];
            case ItemType.IngredientProcessed:
                return ItemAssets.Instance.IngredientProcessedSprites[ItemID];
        }
    }
}
