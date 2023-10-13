using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChefManager : MonoBehaviour {
    public GameObject ChefPrefab;

    public List<ChefBase> Workers = new List<ChefBase>();
    public List<Worker> LoadedWorkers = new List<Worker>();
    
    [HideInInspector] public AuthManager authManager;
    [HideInInspector] public DatabaseReference db;

    private void Awake() {
        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        db = FirebaseDatabase.DefaultInstance.RootReference;

        //for (int i = 0; i < 3; i++) {
        //    CreateNewWorker(0, "Worker: " + i);
        //}
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
                    Debug.Log(loadedWorker.WorkerID);
                    LoadedWorkers.Add(loadedWorker);
                }
                loaded = true;
            }
        });

        yield return new WaitUntil(() => loaded);
        foreach(Worker worker in LoadedWorkers) {
            SpawnWorker(worker);
        }
    }

    void SpawnWorker(Worker worker) {
        Debug.Log("Loaded: " + worker.WorkerID);
        GameObject w = Instantiate(ChefPrefab, transform);
        w.transform.name = "Worker " + worker.WorkerID;
        w.GetComponent<ChefBase>().ThisWorker = worker;
        w.transform.position = worker.WorkerPosition;
        Workers.Add(w.GetComponent<ChefBase>());
    }

    public void SaveWorkers() {
        if(Workers != null) { 
            foreach (ChefBase worker in Workers) {
                Worker w = worker.ThisWorker;
                string json = JsonUtility.ToJson(w);
                db.Child("Workers").Child(authManager.ThisPlayer.PlayerID).Child(w.WorkerID).SetRawJsonValueAsync(json);
            }
        }
    }

    public void DeleteWorker(string workerID) {
        db.Child("Workers").Child(workerID).RemoveValueAsync();
    }

    public void CreateNewWorker(int workerType, string workerName) {
        GameObject worker = Instantiate(ChefPrefab, transform);
        Worker w = worker.GetComponent<ChefBase>().ThisWorker;

        w.WorkerID = GetInstanceID().ToString();
        w.WorkerName = workerName;
        w.WorkerType = workerType;
        w.WorkerLevel = 1;
        w.WorkerExpNeeded = 333;

        w.WorkerCookingLevel = 1;
        w.WorkerFightingLevel = 1;
        w.WorkerSpeedLevel = 1;

        w.WorkerMaxHealth = 100;
        w.WorkerHealth = w.WorkerMaxHealth;

        Workers.Add(worker.GetComponent<ChefBase>());
    }

}

