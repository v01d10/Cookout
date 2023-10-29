using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnStartingEquipment : MonoBehaviour {
    public GameObject WorkstationPrefab;
    public GameObject FridgePrefab;
    public GameObject ServingTablePrefab;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.S)) 
            Spawn();
    }

    public void Spawn() {
        SpawnFridge(0);

        for (int i = 0; i < 2; i++) {
            SpawnWorkstations(i);
            SpawnServingTables(i);
        }
    }

    public void SpawnWorkstations(int id) {
        GameObject station = Instantiate(WorkstationPrefab, transform);
        station.transform.SetPositionAndRotation(new Vector3(3.5f, 1, id + 5), Quaternion.identity);

        WorkstationBase stationBase = new();
        stationBase.ID = id.ToString();
        stationBase.WorkstationType = id == 0 ? WorkstationTypes.Stove : WorkstationTypes.PrepareTable;
        station.GetComponent<Workstation>().thisWorkstation = stationBase;
        KitchenManager.instance.LoadedWorkstations.Add(stationBase);

        List<GameObject> EquipmentList = new List<GameObject>();
        switch (id) {
            default:
            case 0:
                EquipmentList = KitchenEquipmentAssets.instance.ModelsOven;
                break;
            case 1:
                EquipmentList = KitchenEquipmentAssets.instance.ModelsPrepareTable;
                break;
        }

        GameObject model = Instantiate(EquipmentList[0], station.transform);
        model.transform.SetPositionAndRotation(station.transform.position, station.transform.rotation);
    }

    public void SpawnFridge(int id) {
        GameObject newFridge = Instantiate(FridgePrefab);
        newFridge.transform.SetPositionAndRotation(new Vector3(3.5f, 1, -5), Quaternion.identity);
        newFridge.GetComponent<Fridge>().fridgeBase = new();
        KitchenManager.instance.fridge = newFridge.GetComponent<Fridge>();

        GameObject model = Instantiate(KitchenEquipmentAssets.instance.ModelsFridge[id], newFridge.transform);
        model.transform.SetPositionAndRotation(newFridge.transform.position, newFridge.transform.rotation);
        Debug.Log("Spawned fridge!");
    }

    public void SpawnServingTables(int id) {
        GameObject table = Instantiate(ServingTablePrefab, transform);
        table.transform.SetPositionAndRotation(new Vector3(id - 3, 1 , id), Quaternion.identity);

        ServingTableBase servingTable = new();
        servingTable.ID = id.ToString();
        table.GetComponent<ServingTable>().thisServingTable = servingTable;
        KitchenManager.instance.LoadedServingTables.Add(servingTable);

        GameObject model = Instantiate(KitchenEquipmentAssets.instance.ModelsServingTable[0], table.transform);
        model.transform.SetPositionAndRotation(table.transform.position, table.transform.rotation);
    }
}
