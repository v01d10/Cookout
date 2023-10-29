using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryEquipment : KitchenEquipment {
    public int ModelIndex;

    [Space]
    [SerializeField] public Inventory inventory;
}
