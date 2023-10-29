using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KitchenTask {
    public string StartTime;
    public string FinishTime;
    public float FinishExperience;
    public Item recipe;
    public string workstationID;
}
