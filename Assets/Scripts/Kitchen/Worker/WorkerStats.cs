using System;
using UnityEngine;

[Serializable]
public class WorkerStats {
    public float WorkerHealth;
    public float WorkerMaxHealth;
    [Space]
    public int WorkerLevel;
    public float WorkerExp;
    public float WorkerExpNeeded;
    public float WorkerSkillPoint;
    [Space]
    public int WorkerCookingLevel;
    public int WorkerFightingLevel;
    public int WorkerSpeedLevel;
    
    public void AddExpWorker(float amount) {
        if(WorkerExp + amount < WorkerExpNeeded) {
            WorkerExp += amount;
        } else {
            WorkerLevel++;
            WorkerSkillPoint++;
            WorkerExp = WorkerExp + amount - WorkerExpNeeded;
            WorkerExpNeeded *= 2;
            WorkerMaxHealth *= 1.2f;
            WorkerHealth = WorkerMaxHealth;
        }
    }

    public void EditHealth(float amount) {
        if(WorkerHealth - amount > 0 || WorkerHealth + amount < WorkerMaxHealth) {
            WorkerHealth += amount;
        }
    }

    public void AddSkillLevel(int skillIndex) {
        if(skillIndex == 0) {
            WorkerCookingLevel++;
        }
        if(skillIndex == 1) {
            WorkerFightingLevel++;
            WorkerMaxHealth += 10;

        }
        if (skillIndex == 2) {
            WorkerSpeedLevel++;
        }
        WorkerSkillPoint--;
    }
}
