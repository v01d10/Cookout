using UnityEngine;
using System.IO;
using System.Globalization;

public class DataManager : MonoBehaviour {
    [SerializeField] public PlayerLocalID localID = new PlayerLocalID();

    public bool CheckLocalPlayer() {
        string fileName = Application.persistentDataPath + "/playerID";
        if (File.Exists(fileName)) {
            string json = Load(fileName);
            localID = JsonUtility.FromJson<PlayerLocalID>(json);
            Debug.Log("Local player found! Loading... " + json);
            return true;
        }
        else {
            Debug.Log("Local player not found! Creating...");
            return false;
        }
    }

    public void SavePlayer() {
        Save(Application.persistentDataPath + "/playerID", JsonUtility.ToJson(localID));
        Debug.Log("Saved local player... : " + localID.UID);
    }

    public static void Save(string filePath, string jsonData) {
        using (StreamWriter streamWriter = new StreamWriter(filePath)) {
            streamWriter.Write(jsonData);
        }
    }

    public static string Load(string filePath) {
        using (StreamReader streamReader = new StreamReader(filePath)) {
            return streamReader.ReadToEnd();
        }
    }
}