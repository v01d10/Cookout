using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorkerManager : MonoBehaviour {
    public GameObject ChefPrefab;

    public List<WorkerBase> Workers = new List<WorkerBase>();
    
    [HideInInspector] public AuthManager authManager;
    [HideInInspector] public DatabaseReference db;

    private void Awake() {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        db = FirebaseDatabase.DefaultInstance.RootReference;

        //CreateNewWorker(0, "Worker: ");
        for (int i = 0; i < 1; i++) {
        }
    }

    private void OnEnable() {
        AuthManager.OnPlayerLogin += StartLoadingWorkers;
    }

    private void OnDisable() {
        AuthManager.OnPlayerLogin -= StartLoadingWorkers;
    }

    private void OnApplicationQuit() {
        SaveWorkers();
    }

    public void StartLoadingWorkers() {
        StartCoroutine(LoadWorkers());
    }

    public IEnumerator LoadWorkers() {
        yield return new WaitForSeconds(1f);
        bool loaded = false;
        List<Worker> loadedWorkers = new();

        db.Child("Workers").Child(authManager.ThisPlayer.PlayerID).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                Worker loadedWorker = null;

                foreach (DataSnapshot childSnapshot in snapshot.Children) {
                    string childJson = childSnapshot.GetRawJsonValue();
                    loadedWorker = JsonUtility.FromJson<Worker>(childJson);
                    Debug.Log("Loaded worker: " + loadedWorker.WorkerID);
                    loadedWorkers.Add(loadedWorker);
                }
                loaded = true;
            }
        });

        yield return new WaitUntil(() => loaded);
        foreach(Worker worker in loadedWorkers) {
            SpawnWorker(worker);
        }
    }

    void SpawnWorker(Worker worker) {
        GameObject w = Instantiate(ChefPrefab, transform);
        w.GetComponent<WorkerBase>().ThisWorker = worker;
        w.GetComponent<WorkerBase>().ThisWorker.workerStats = worker.workerStats;
        
        w.transform.name = "Worker " + worker.WorkerID;
        w.transform.SetPositionAndRotation(worker.WorkerPosition, worker.WorkerRotation);
        Workers.Add(w.GetComponent<WorkerBase>());
    }

    public void SaveWorkers() {
        if(Workers != null) { 
            foreach (WorkerBase worker in Workers) {
                worker.SaveWorker();
            }
        }
    }

    public void DeleteWorker(string workerID) {
        db.Child("Workers").Child(workerID).RemoveValueAsync();
    }

    public void CreateNewWorker(int workerType, string workerName) {
        GameObject worker = Instantiate(ChefPrefab, transform);
        Worker w = worker.GetComponent<WorkerBase>().ThisWorker;
        WorkerStats wStats = w.workerStats;

        w.WorkerID = Workers.Count.ToString();
        w.WorkerName = workerName;
        w.WorkerType = workerType;
        wStats.WorkerLevel = 1;
        wStats.WorkerExpNeeded = 333;

        wStats.WorkerCookingLevel = 1;
        wStats.WorkerFightingLevel = 1;
        wStats.WorkerSpeedLevel = 1;

        wStats.WorkerMaxHealth = 100;
        wStats.WorkerHealth = wStats.WorkerMaxHealth;

        Workers.Add(worker.GetComponent<WorkerBase>());
    }

}

