using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

[Serializable]
public class WorkerBase : MonoBehaviour {
    [HideInInspector] public DatabaseReference dbReference;
    [HideInInspector] public AuthManager authManager;

    [SerializeField] public Worker ThisWorker;

    [HideInInspector] public WorkerNavigation ThisNavigation;

    private void Start() {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        ThisNavigation = GetComponent<WorkerNavigation>();
    }

    public void SetUpWorker(string name) {
        ThisWorker.WorkerName = name;
        ThisWorker.WorkerID = GetInstanceID().ToString();

        ThisWorker.workerStats.WorkerMaxHealth = 100;
        ThisWorker.workerStats.WorkerHealth = ThisWorker.workerStats.WorkerMaxHealth;
        ThisWorker.workerStats.WorkerLevel = 1;
        ThisWorker.workerStats.WorkerExpNeeded = 333;
        ThisWorker.workerStats.WorkerExp = 0;
    }

    public void SaveWorker() {
        string json = JsonUtility.ToJson(ThisWorker);
        dbReference.Child("Workers").Child(authManager.ThisPlayer.PlayerID).Child(ThisWorker.WorkerID).SetRawJsonValueAsync(json);
        Debug.Log(json);
    }
}
