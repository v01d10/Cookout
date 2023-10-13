using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Worker {
    public string WorkerName;
    public string WorkerID;
    public int WorkerType;

    public float WorkerHealth;
    public float WorkerMaxHealth;

    public int WorkerLevel;
    public float WorkerExp;
    public float WorkerExpNeeded;
    public float WorkerSkillPoint;

    public int WorkerCookingLevel;
    public int WorkerFightingLevel;
    public int WorkerSpeedLevel;

    public Vector3 WorkerPosition;
}
