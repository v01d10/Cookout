using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Unity.VisualScripting;

[System.Serializable]
public class KitchenManager : MonoBehaviour {
    public GameObject KitchenPrefab;

[Header("Models")]
    public List<GameObject> AvailableOvens;
    public List<GameObject> AvailablePrepareTables;
    public List<GameObject> AvailableServingTables;
    public List<GameObject> AvailableFloors;
    public List<GameObject> AvailableWalls;
    public List<GameObject> AvailableDecorations;
    public List<GameObject> AvailableChairs;
    public List<GameObject> AvailableTables;

[Header("Loaded")]
    [SerializeField] public List<KitchenPlacementHandler> LoadedHandlers;

    [HideInInspector] public AuthManager authManager;
    [HideInInspector] public DatabaseReference db;

    bool FinishedLoading;

    private void Start() {
        authManager = GameObject.Find("Backend").GetComponent<AuthManager>();
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnEnable() {
        AuthManager.OnPlayerLogin += StartLoadingKitchen;
    }

    private void OnDisable() {
        AuthManager.OnPlayerLogin -= StartLoadingKitchen;
    }

    private void OnDestroy() {
        SaveKitchen();
    }

    public void StartLoadingKitchen() {
        StartCoroutine(LoadKitchen());
    }

    public IEnumerator LoadKitchen() {
        db.Child("Kitchens").Child(SystemInfo.deviceUniqueIdentifier.ToString()).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                KitchenPlacementHandler handler = new();

                foreach (DataSnapshot childSnapshot in snapshot.Children) {
                    string childJson = childSnapshot.GetRawJsonValue();
                    handler = JsonUtility.FromJson<KitchenPlacementHandler>(childJson);
                    Debug.Log(handler.ID);
                    LoadedHandlers.Add(handler);
                }

                FinishedLoading = true;
            }
        });
        
        yield return new WaitUntil(() => FinishedLoading);
        SpawnKitchen();
    }

    public void SpawnKitchen() {
        foreach(var handler in LoadedHandlers) {
            GameObject ws = Instantiate(KitchenPrefab, handler.Position, handler.Rotation);
            ws.transform.parent = transform;
            ws.GetComponent<KitchenThing>().placementHandler = handler;

            string id = handler.ID[^2..];
            char ch = id[0];
            if (ch.ToString() == "0") { id = handler.ID[2].ToString(); }

            GameObject model = null;

            if (handler.Type == 0) {
                model = Instantiate(AvailableOvens[int.Parse(id)], ws.transform);
            } else if (handler.Type == 1) {
                model = Instantiate(AvailablePrepareTables[int.Parse(id)], ws.transform);
            } else if (handler.Type == 2) {
                model = Instantiate(AvailableServingTables[int.Parse(id)], ws.transform);
            } else if (handler.Type == 3) {
                model = Instantiate(AvailableFloors[int.Parse(id)], ws.transform);
            } else if (handler.Type == 4) {
                model = Instantiate(AvailableWalls[int.Parse(id)], ws.transform);
            } else if (handler.Type == 5) {
                model = Instantiate(AvailableDecorations[int.Parse(id)], ws.transform);
            } else if (handler.Type == 6) {
                model = Instantiate(AvailableChairs[int.Parse(id)], ws.transform);
            } else if (handler.Type == 7) {
                model = Instantiate(AvailableTables[int.Parse(id)], ws.transform);
            }
                
            model.transform.SetPositionAndRotation(handler.Position, handler.Rotation);
        }
    }

    public void SaveKitchen() {
        if (LoadedHandlers != null) {
            foreach (KitchenPlacementHandler kitchen in LoadedHandlers) {
                string json = JsonUtility.ToJson(kitchen);
                db.Child("Kitchens").Child(SystemInfo.deviceUniqueIdentifier.ToString()).Child(kitchen.ID).SetRawJsonValueAsync(json);
                Debug.Log("Kitchen saved...");
            }
        }
    }
}
