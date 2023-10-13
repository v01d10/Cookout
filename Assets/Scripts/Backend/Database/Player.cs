using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player {
    public string PlayerID;
    public string PlayerName;
    public string PlayerEmail;
    public string PlayerPassword;

    public int PlayerLevel = 1;
    public float PlayerExp = 0;
    public float PlayerExpNeeded = 333;

    public int Money = 100;
    public float PremiumCurrency = 10;

    public long LogOutTime;

    public Player(string name, string id, string email, string password, int level, float exp, float expNeeded, int money, float pCurrency) { 
        this.PlayerID = id;
        this.PlayerName = name;
        this.PlayerEmail = email;
        this.PlayerPassword = password;
        this.PlayerLevel = level;
        this.PlayerExp = exp;
        this.PlayerExpNeeded = expNeeded;
        this.Money = money;
        this.PremiumCurrency = pCurrency;
    }
}
