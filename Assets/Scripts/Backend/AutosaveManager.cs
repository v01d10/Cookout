using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AutosaveManager : MonoBehaviour {
    public float autosaveInterval;
    float autosaveTimer;

    AuthManager authManager;
    ChefManager chefManager;
    KitchenManager kitchenManager;

    private void Start() {
        authManager = GetComponent<AuthManager>();
        chefManager = GameObject.Find("ChefManager").GetComponent<ChefManager>();
        kitchenManager = GameObject.Find("KitchenManager").GetComponent<KitchenManager>();

        StartCoroutine(AutosaveTimer());
    }

    IEnumerator AutosaveTimer() {
        autosaveTimer = autosaveInterval * 60;
        yield return new WaitForSeconds(autosaveTimer);

        authManager.SavePlayer();
        chefManager.SaveWorkers();
        kitchenManager.SaveKitchen();
        Debug.Log("Autosave completed...");

        StartCoroutine(AutosaveTimer());
    }
}
