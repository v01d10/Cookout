using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using static UnityEditor.Progress;

[System.Serializable]
public class WorkstationBase : MonoBehaviour {
    KitchenManager kitchenManager;

    public string Name;
    public WorkstationTypes WorkstationType;

    [SerializeField] public Item CurrentRecipe;

    [HideInInspector] public Transform StandingPoint;
    public bool Used;

    [Header("Cleanliness")]
    public float Dirtiness;

    private void Awake() {
        kitchenManager = FindObjectOfType<KitchenManager>();
        StandingPoint = GetComponentInChildren<StandingPoint>().transform;
    }

    public void StationStartWorking(Item item) {
        Used = true;
        SetRecipe(item);
    }

    public void SetRecipe(Item item) {
        CurrentRecipe = item;
    }

}

public enum WorkstationTypes {
    Oven,
    Cauldron,
    PrepareTable
}
