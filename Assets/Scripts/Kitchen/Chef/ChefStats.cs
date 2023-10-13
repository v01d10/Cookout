using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefStats : MonoBehaviour {
    ChefBase ThisChef;
    Worker ThisWorker;
    ChefManager chefManager;

    private void Awake() {
        ThisChef = GetComponent<ChefBase>();
        ThisWorker = ThisChef.ThisWorker;
        chefManager = GameObject.Find("ChefManager").GetComponent<ChefManager>();
    }
    
    public void AddExpWorker(float amount) {
        if(ThisWorker.WorkerExp + amount < ThisWorker.WorkerExpNeeded) {
            ThisWorker.WorkerExp += amount;
        } else {
            ThisWorker.WorkerLevel++;
            ThisWorker.WorkerSkillPoint++;
            ThisWorker.WorkerExp = ThisWorker.WorkerExp + amount - ThisWorker.WorkerExpNeeded;
            ThisWorker.WorkerExpNeeded *= 2;
            ThisWorker.WorkerMaxHealth *= 1.2f;
            ThisWorker.WorkerHealth = ThisWorker.WorkerMaxHealth;
        }
    }

    public void EditHealth(float amount) {
        if(ThisWorker.WorkerHealth - amount > 0 || ThisWorker.WorkerHealth + amount < ThisWorker.WorkerMaxHealth) {
            ThisWorker.WorkerHealth += amount;
        }
    }

    public void AddSkillLevel(int skillIndex) {
        if(skillIndex == 0) {
            ThisWorker.WorkerCookingLevel++;
        }
        if(skillIndex == 1) {
            ThisWorker.WorkerFightingLevel++;
            ThisWorker.WorkerMaxHealth += 10;

        }
        if (skillIndex == 2) {
            ThisWorker.WorkerSpeedLevel++;
        }
        ThisWorker.WorkerSkillPoint--;
    }

    public void Death() {
        chefManager.DeleteWorker(ThisWorker.WorkerID);
        Destroy(gameObject);
    }

}
