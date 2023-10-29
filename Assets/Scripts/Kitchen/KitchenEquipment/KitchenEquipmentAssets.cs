using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenEquipmentAssets : MonoBehaviour {
    public static KitchenEquipmentAssets instance;

    [Header("Models"), Space]
    [Header("Kitchen Equipment")]
    public List<GameObject> ModelsOven;
    public List<GameObject> ModelsPrepareTable;
    [Header("Inventory Equipment")]
    public List<GameObject> ModelsFridge;
    public List<GameObject> ModelsServingTable;
    [Header("Decorations")]
    public List<GameObject> ModelsFloor;
    public List<GameObject> ModelsWall;
    public List<GameObject> ModelsDecorations;
    [Header("Restaurant Equipment")]
    public List<GameObject> ModelsChair;
    public List<GameObject> ModelsTable;

    private void Awake() {
        instance = this;
    }
}
