using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

[Serializable]
public class KitchenManager : MonoBehaviour {
    public static KitchenManager instance;
    public GameObject SelectedEquipment;

    [Header("Prefabs")]
    public GameObject WorkstationPrefab;
    public GameObject InventoryPrefab;

    [Header("Models")]
    public KitchenEquipmentAssets equipmentAssets;

    [Header("Loaded")]
    [SerializeField] public List<WorkstationBase> LoadedWorkstations;
    [SerializeField] public List<ServingTableBase> LoadedServingTables;

    [Space]
    public Fridge fridge;
    public List<ServingTable> ServingTables;

    [HideInInspector] public AuthManager authManager;
    [HideInInspector] public DatabaseReference db;

    bool WorkstationsLoaded;
    bool FridgeLoaded;
    bool ServingTablesLoaded;

    private void Awake() {
        instance = this;
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
        equipmentAssets = GetComponent<KitchenEquipmentAssets>();
        authManager = GameObject.Find("AuthManager").GetComponent<AuthManager>();
        db = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnEnable() { AuthManager.OnPlayerLogin += LoadKitchen; }

    private void OnDisable() { AuthManager.OnPlayerLogin -= LoadKitchen; }

    private void OnDestroy() { SaveKitchen(); }

    public void SaveKitchen() {
        SaveWorkstations();
        SaveFridge();
        SaveServingTables();
    }

    public void SaveWorkstations() {
        if (LoadedWorkstations == null) {
            Debug.LogWarning("No equipment to save!");
            return;
        }

        foreach (WorkstationBase workstation in LoadedWorkstations) {
            string json = JsonUtility.ToJson(workstation);
            db.Child("Kitchen").Child(authManager.ThisPlayer.PlayerID).Child("Workstations").Child(workstation.ID).SetRawJsonValueAsync(json);
            Debug.Log("Workstation saved... " + workstation.ID);
        }
    }

    public void SaveFridge() {
        string json = JsonUtility.ToJson(fridge.fridgeBase);
        db.Child("Kitchen").Child(authManager.ThisPlayer.PlayerID).Child("Fridge").SetRawJsonValueAsync(json);
        Debug.Log("Fridge saved... ");
    }

    public void SaveServingTables() {
        foreach (ServingTableBase servingTable in LoadedServingTables) {
            string json = JsonUtility.ToJson(servingTable);
            db.Child("Kitchen").Child(authManager.ThisPlayer.PlayerID).Child("ServingTables").Child(servingTable.ID).SetRawJsonValueAsync(json);
            Debug.Log("Inventory saved... " + servingTable.ID);
        }
    }

    public void LoadKitchen() {
        StartCoroutine(LoadWorkstations());
        StartCoroutine(LoadFridge());
        StartCoroutine(LoadServingTables());
    }

    public IEnumerator LoadWorkstations() {
        WorkstationBase workstation = null;
        db.Child("Kitchens").Child(authManager.ThisPlayer.PlayerID).Child("Workstations").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot childSnapshot in snapshot.Children) {
                    string childJson = childSnapshot.GetRawJsonValue();
                    workstation = JsonUtility.FromJson<WorkstationBase>(childJson);
                    Debug.Log(workstation);
                    LoadedWorkstations.Add(workstation);
                }
                WorkstationsLoaded = true;
            }
        });

        yield return new WaitUntil(() => WorkstationsLoaded);
        SpawnWorkstations();
    }

    public IEnumerator LoadFridge() {
        InventoryEquipment newFridgeBase = new();
        db.Child("Kitchens").Child(authManager.ThisPlayer.PlayerID).Child("Fridge").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;

                string json = snapshot.GetRawJsonValue();
                newFridgeBase = JsonUtility.FromJson<InventoryEquipment>(json);

                FridgeLoaded = true;
            }
        });

        yield return new WaitUntil(() => FridgeLoaded);
        SpawnFridge(newFridgeBase);
    }

    public IEnumerator LoadServingTables() {
        ServingTableBase servingTable = null;
        db.Child("Kitchens").Child(authManager.ThisPlayer.PlayerID).Child("ServingTables").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.ToString());
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot childSnapshot in snapshot.Children) {
                    string childJson = childSnapshot.GetRawJsonValue();
                    servingTable = JsonUtility.FromJson<ServingTableBase>(childJson);
                    LoadedServingTables.Add(servingTable);
                }
                ServingTablesLoaded = true;
            }
        });

        yield return new WaitUntil(() => ServingTablesLoaded);
        SpawnServingTables();
    }

    void SpawnWorkstations() {
        foreach (var workstation in LoadedWorkstations) {
            GameObject station = Instantiate(WorkstationPrefab, transform);
            station.transform.SetPositionAndRotation(workstation.placementHandler.Position, workstation.placementHandler.Rotation);
            station.GetComponent<Workstation>().thisWorkstation = workstation;

            string id = workstation.ID[^2..];
            char ch = id[0];
            if (ch.ToString() == "0") { id = workstation.ID[2].ToString(); }

            int workstationType = int.Parse(workstation.ID[0].ToString());

            List<GameObject> EquipmentList = new List<GameObject>();
            switch (workstationType) {
                default:
                case 0:
                    EquipmentList = equipmentAssets.ModelsOven;
                    break;
                case 1:
                    EquipmentList = equipmentAssets.ModelsPrepareTable;
                    break;
            }

            GameObject model = Instantiate(EquipmentList[int.Parse(id)], station.transform);
            model.transform.SetPositionAndRotation(station.transform.position, station.transform.rotation);

            station.GetComponent<Workstation>().CheckFinishedRecipe();
        }
    }

    void SpawnFridge(InventoryEquipment newFridgeBase) {
        GameObject newFridge = Instantiate(equipmentAssets.ModelsFridge[newFridgeBase.ModelIndex]);
        newFridge.transform.SetPositionAndRotation(newFridgeBase.placementHandler.Position, newFridgeBase.placementHandler.Rotation);
        newFridge.GetComponent<Fridge>().fridgeBase = newFridgeBase;
        fridge = newFridge.GetComponent<Fridge>();
        Debug.Log("Spawned fridge!");
    }

    void SpawnServingTables() {
        foreach (var servingTable in LoadedServingTables) {
            GameObject table = Instantiate(InventoryPrefab, transform);
            table.transform.SetPositionAndRotation(servingTable.placementHandler.Position, servingTable.placementHandler.Rotation);
            table.GetComponent<ServingTable>().thisServingTable = servingTable;

            GameObject model = Instantiate(equipmentAssets.ModelsServingTable[int.Parse(GetID(servingTable.ID))], table.transform);
            model.transform.SetPositionAndRotation(table.transform.position, table.transform.rotation);

            table.GetComponent<Workstation>().CheckFinishedRecipe();
        }
    }

    public string GetID(string inputID) {
        string id = inputID[^2..];
        char ch = id[0];
        if (ch.ToString() == "0") { id = inputID[2].ToString(); }
        return id;
    }
}
