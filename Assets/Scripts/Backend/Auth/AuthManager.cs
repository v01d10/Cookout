using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Database;
using System.Security.Cryptography;
using Unity.VisualScripting;

[System.Serializable]
public class AuthManager : MonoBehaviour {
    private DatabaseReference db;
    private Firebase.Auth.FirebaseAuth auth;
    public AuthEmail authEmail;
    DataManager dataManager;
    AuthUI authUI;

    public ChefManager chefManager;
    public KitchenManager kitchenManager;

    [SerializeField] public Player ThisPlayer;

    public char[] chars;
    int playerCount;

    List<Player> loadedPlayers = new List<Player>();
    bool playersLoaded;

    public delegate void PlayerLogin();
    public static event PlayerLogin OnPlayerLogin;

    string pHash;

    private void Start() {
        db = FirebaseDatabase.DefaultInstance.RootReference;
        authEmail = GetComponent<AuthEmail>();
        dataManager = GetComponentInParent<DataManager>();
        authUI = GameObject.Find("UI").GetComponent<AuthUI>();

        chefManager = GameObject.Find("ChefManager").GetComponent<ChefManager>();
        kitchenManager = GameObject.Find("KitchenManager").GetComponent<KitchenManager>();

        if(dataManager.CheckLocalPlayer()) {
            StartCoroutine(LoadPlayer(dataManager.localID.UID));
        } else {
            authUI.OpenRegisterUI();
        }
    }

    private void OnDestroy() {
        var timestamp = Convert.ToInt64(ServerValue.Timestamp);
        //long time = Convert.ToInt64(timestamp);
        Debug.Log("Log out time: " + timestamp);
        ThisPlayer.LogOutTime = timestamp;
        SavePlayer();
    }

    public void Register() {
        //RegisterMail(authEmail.nameRegisterField.text, authEmail.emailRegisterField.text, authEmail.passwordRegisterField.text, authEmail.confirmPasswordRegisterField.text);
        StartCoroutine(RegisterMail("test", "test@test.com", "testtest", "testtest"));
    }

    public void Login() {
        StartCoroutine(LoginMail(authEmail.emailLoginField.text, authEmail.passwordLoginField.text));
    }

    IEnumerator RegisterMail(string name ,string email, string password, string confirmPassword) {
        StartCoroutine(GetPlayers());
        yield return new WaitUntil(() => playersLoaded);
            
        foreach (Player p in loadedPlayers) {
            if(p.PlayerName == name) {
                Debug.Log("Player name already exist!");
                yield break;
            }
            if(p.PlayerEmail == email) {
                Debug.Log("Player email already exist!");
                yield break;
            }
        }

        if (name == "") {
            Debug.Log("Enter name!");
            yield return null;
        }

        if (email == "" || !email.Contains("@") || !email.Contains(".")) {
            Debug.Log("Enter valid email!");
            yield return null;
        }

        foreach (char ch in chars) {
            if(password.Contains(ch)) {
                Debug.Log("Password contains: " + ch);
                yield return null;
            }
        }
        if (password.Length < 5) {
            Debug.Log("Password must have at least 5 characters!");
            yield return null;
        }
        if(password != confirmPassword) {
            Debug.Log("Passwords doesn't match!");
            yield return null;
        }

        var getPlayerCount = db.Child("PlayerCount").GetValueAsync();
        yield return new WaitUntil(() => getPlayerCount.IsCompleted);
        playerCount = int.Parse(getPlayerCount.Result.Value.ToString());

        PasswordHash(password);
        ThisPlayer = new Player(name, playerCount.ToString(), email, pHash, 1 ,0, 333, 100, 10);
        string player = JsonUtility.ToJson(ThisPlayer);
        db.Child("Players").Child(ThisPlayer.PlayerID).SetRawJsonValueAsync(player);

        Debug.Log("Player registered! " + playerCount);
        db.Child("PlayerCount").SetValueAsync(playerCount + 1);

        authUI.CloseRegisterUI();
        authUI.OpenLoginUI();
    }

    IEnumerator LoginMail(string email, string password) {
        StartCoroutine(GetPlayers());
        yield return new WaitUntil(() => playersLoaded);

        foreach (Player p in loadedPlayers) {
            if(p.PlayerEmail == email) {
                Debug.Log("User found");
                VerifyPassword(password, p.PlayerPassword, p);
            } else {
                Debug.Log("No user found!");
                yield break;
            }
        }
    }

    IEnumerator GetPlayers() {
        loadedPlayers.Clear();
        playersLoaded = false;

        var getPlayers = db.Child("Players").GetValueAsync();
        yield return new WaitUntil(() => getPlayers.IsCompleted);

        DataSnapshot snapshot = getPlayers.Result;
        Player loadedPlayer = null;
        foreach (DataSnapshot childSnapshot in snapshot.Children) {
            string childJson = childSnapshot.GetRawJsonValue();
            loadedPlayer = JsonUtility.FromJson<Player>(childJson);
            loadedPlayers.Add(loadedPlayer);
        }

        Debug.Log("Players loaded... " + loadedPlayers.Count);
        playersLoaded = true;
    }

    void PasswordHash(string password) {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        pHash = Convert.ToBase64String(hashBytes);
    }

    void VerifyPassword(string password, string h, Player player) {
        byte[] hashBytes = Convert.FromBase64String(h);

        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++) {
            if (hashBytes[i + 16] != hash[i]) {
                Debug.Log("Password verification failed.");
                return;
            }
        }

        Debug.Log("Password verification succeeded!");
        StartCoroutine(LoadPlayer(player.PlayerID));
        if (authEmail.stayLoggedToggle) {
            dataManager.localID.UID = ThisPlayer.PlayerID;
            dataManager.SavePlayer();
        }
        authUI.HandleLoginUI();
    }

    public IEnumerator LoadPlayer(string id) {
        var getPlayer = db.Child("Players").Child(id).GetValueAsync();
       yield return new WaitUntil(() => getPlayer.IsCompleted);

        DataSnapshot snapshot = getPlayer.Result;
        string json = snapshot.GetRawJsonValue();
        Player player = JsonUtility.FromJson<Player>(json);
        ThisPlayer = player;
        Debug.Log("Loaded player: " + player.PlayerID);

        OnPlayerLogin();
    }

    public void SavePlayer() {
        Player player = ThisPlayer;
        string json = JsonUtility.ToJson(player);
        db.Child("Players").Child(ThisPlayer.PlayerID).SetRawJsonValueAsync(json);
        Debug.Log("Player saved...");
    }

    DateTime GetTime() {
        var time = DateTime.UtcNow;

        FirebaseDatabase.DefaultInstance.GetReference(".info/serverTimeOffset").GetValueAsync().ContinueWith(task => {
            if(task.IsCompleted) {
                var offset = long.Parse(task.Result.Value.ToString());
                time = DateTime.UtcNow.AddMilliseconds(offset);
            }
        });
        return time;
        //var timeFirst = DateTime.ParseExact("2023-10-07 14:00:00,000", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);

    }

    int GetElapsedTime(DateTime timeStart) {
        DateTime timeNow = GetTime();
        TimeSpan elapsed = timeStart - timeNow;
        int hours = Math.Abs((int)elapsed.TotalHours);
        Debug.Log(elapsed + " = " + hours);
        return hours;
    }
}


