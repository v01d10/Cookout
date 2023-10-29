using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour {
    TimeHandler timeHandler;
    KitchenManager kitchenManager;
    CookBookUI cookBookUI;

    public WorkstationBase thisWorkstation;

    [HideInInspector] public Transform StandingPoint;

    private void Start() {
        kitchenManager = FindObjectOfType<KitchenManager>();
        timeHandler = FindObjectOfType<TimeHandler>();
        cookBookUI = GameObject.Find("UI").GetComponent<CookBookUI>();
        StandingPoint = GetComponentInChildren<StandingPoint>().transform;
    }

    private void OnMouseDown() {
        kitchenManager.SelectedEquipment = this.gameObject;
        OpenCookBook();
    }

    void OpenCookBook() {
        bool dishOrNot = thisWorkstation.WorkstationType == WorkstationTypes.Stove ? true : false;
        cookBookUI.ShowCookBook(dishOrNot);
    }

    public void StartWorking(Item item) {
        if(thisWorkstation.CurrentTask.StartTime != "") {
            ModalHandler.instance.OpenModal(true, ModalHandler.instance.AvailableTexts[0], () => { 
                ModalFunctions.instance.AnotherActionInProgress();
            });
            return;
        }

        List<Item> inventory = kitchenManager.fridge.fridgeBase.inventory.ItemList;
        for (int i = 0; i < item.RequiredItems.Count; i++) {
            for (int u = 0; u < inventory.Count; u++) {
                if (inventory[u].Amount < item.RequiredItemsAmounts[i]) {
                    ModalHandler.instance.OpenModal(true, ModalHandler.instance.AvailableTexts[1], () => {
                        ModalFunctions.instance.NotEnoughIngredients();
                    });
                    return;
                }
            }
        }

        SetRecipe(item);

        Debug.Log("Starting work: " + item);
        cookBookUI.HideCookBook();
    }

    public void SetRecipe(Item item) {
        KitchenTask task = new KitchenTask();
        DateTime startTime = timeHandler.GetTime();
        DateTime finishTime = timeHandler.GetFinishTime(item.PrepareTime);

        task.StartTime = startTime.ToString();
        task.FinishTime = finishTime.ToString();
        task.recipe = item;
        task.workstationID = thisWorkstation.ID;
        task.FinishExperience = item.FinishExperince;

        thisWorkstation.CurrentTask = task;
    }

    public void CheckFinishedRecipe() {
        if (thisWorkstation.CurrentTask.StartTime == "") {
            Debug.Log("No task to check! " + thisWorkstation.ID);
            return;
        }

        string timeFinishString = thisWorkstation.CurrentTask.FinishTime;
        DateTime timeFinish = DateTime.Parse(timeFinishString);

        Debug.Log("Finish time: " + timeFinish);
        int remainingTime = TimeHandler.instance.GetRemainingTime(timeFinish);

        if (remainingTime > 0) {
            Debug.Log("Equipment: " + thisWorkstation.ID + " Recipe: " + thisWorkstation.CurrentTask + " Remaining time: " + remainingTime);
        } 
        else {
            Debug.Log("Task Finished!! " + "Equipment: " + thisWorkstation.ID + " Recipe: " + thisWorkstation.CurrentTask);
            FinishWorking();
        }
    }

    public void FinishWorking() {
        KitchenTask task = thisWorkstation.CurrentTask;
        Inventory inventory = KitchenManager.instance.fridge.fridgeBase.inventory;

        inventory.AddItem(task.recipe);
        AuthManager.instance.ThisPlayer.AddExp(task.FinishExperience);

        thisWorkstation.CurrentTask = null;
    }

}
