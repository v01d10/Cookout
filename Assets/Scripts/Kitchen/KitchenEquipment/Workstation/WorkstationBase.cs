using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;
using static UnityEditor.Progress;

[System.Serializable]
public class WorkstationBase : KitchenEquipment {
    public string Name;
    public WorkstationTypes WorkstationType;

    [SerializeField] public KitchenTask CurrentTask;
    public Worker CurrentWorker;
}

public enum WorkstationTypes {
    Stove,
    PrepareTable
}
