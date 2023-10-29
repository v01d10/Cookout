using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Worker {
    public string WorkerName;
    public string WorkerID;
    public int WorkerType;
    [Space]
    public float ChefSpeed;
    public bool Working;
    public Vector3 WorkerPosition;
    public Quaternion WorkerRotation;
    [Space]
    public WorkerStats workerStats;
}
