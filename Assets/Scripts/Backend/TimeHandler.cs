using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using System;
using Firebase.Database;

public class TimeHandler : MonoBehaviour {
    public static TimeHandler instance;

    private void Awake() {
        instance = this;        
    }

    public DateTime GetTime() {
        var time = DateTime.UtcNow;
        FirebaseDatabase.DefaultInstance.GetReference(".info/serverTimeOffset").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted) {
                var offset = long.Parse(task.Result.Value.ToString());
                time = DateTime.UtcNow.AddMilliseconds(offset);
                Debug.Log("Current server time: " + time.ToString());
            }
        });
        return time;
    }

    public DateTime GetFinishTime(float timeToAdd) {
        DateTime startTime = GetTime();
        return startTime.AddHours(timeToAdd);
    }

    public int GetElapsedTime(DateTime timeStart) {
        DateTime timeNow = GetTime();
        TimeSpan elapsed = timeStart - timeNow;
        int hours = Math.Abs((int)elapsed.TotalHours);
        Debug.Log(elapsed + " = " + hours);
        return hours;
    }

    public int GetRemainingTime(DateTime timeFinish) {
        DateTime timeCurrent = GetTime();
        TimeSpan remaining = (timeFinish - timeCurrent);

        int minutes = (int)remaining.TotalMinutes;
        Debug.Log("Remaining minutes: " + minutes);
        return minutes;

    }
}
