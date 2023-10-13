using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

[System.Serializable]
public class ChefBase : MonoBehaviour {
    [HideInInspector] public ChefNavigation ThisNavigation;
    public WorkstationBase CurrentWorkstation;
    public float ChefSpeed;
    public bool Working;

    [SerializeField] public Worker ThisWorker = new Worker();
    [HideInInspector] public DatabaseReference dbReference;
    [HideInInspector] public AuthManager authManager;

    private void Start() {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        authManager = GameObject.Find("Backend").GetComponent<AuthManager>();
        ThisNavigation = GetComponent<ChefNavigation>();
    }

    public void SetUpWorker(string name) {
        ThisWorker.WorkerName = name;
        ThisWorker.WorkerID = GetInstanceID().ToString();
        ThisWorker.WorkerMaxHealth = 100;
        ThisWorker.WorkerHealth = ThisWorker.WorkerMaxHealth;
        ThisWorker.WorkerLevel = 1;
        ThisWorker.WorkerExpNeeded = 333;
        ThisWorker.WorkerExp = 0;

        string json = JsonUtility.ToJson(ThisWorker);
        dbReference.Child("workers").Child(authManager.ThisPlayer.PlayerID).Child(ThisWorker.WorkerID).SetRawJsonValueAsync(json);
        Debug.Log(json);
    }

    public void LoadWorker(string id) {
        ThisWorker.WorkerID = id;
        Debug.Log("Worker loaded");
    }


}
