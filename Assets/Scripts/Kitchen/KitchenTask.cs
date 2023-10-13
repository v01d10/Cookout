using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KitchenTask {
    public DateTime StartTime;
    public DateTime FinishTime;
    public float FinishAmount;
    public Recipe recipe;
}
