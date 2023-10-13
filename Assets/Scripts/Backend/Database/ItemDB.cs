using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Unity.VisualScripting;
using System;
using Firebase.Storage;
using UnityEditor.SceneManagement;
using System.Threading;
using Firebase.Firestore;
using System.IO;
using UnityEngine.SocialPlatforms;
using Firebase.Extensions;
using UnityEngine.Networking;
using System.Net;

public class ItemDB : MonoBehaviour {
    FirebaseStorage storage;
    StorageReference storageReference;

    [TextArea] public string ItemDatabaseFileUrl;
    public List<Item> LoadedItems;

    private void Start() {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl(ItemDatabaseFileUrl);

        StartCoroutine(GetItemDB());
    }

    public IEnumerator GetItemDB() {
        string destination = Application.persistentDataPath;
        string filePath = destination + "/ItemDatabase.json";

        //var getFile = storageReference.GetFileAsync(destination);

        //yield return new WaitUntil(() => getFile.IsCompleted);
        //if (!getFile.IsFaulted && !getFile.IsCanceled) {
        //    Debug.Log("File downloaded.");
        //}

        // Fetch the download URL
        var getFile = storageReference.GetDownloadUrlAsync();
        if (!getFile.IsFaulted && !getFile.IsCanceled) {
            Debug.Log("Download URL: " + getFile.Result);

            using (UnityWebRequest www = UnityWebRequest.Get(getFile.Result)) {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) {
                    Debug.Log(www.error);
                }
                else {
                    File.WriteAllBytes(filePath, www.downloadHandler.data);
                }
            }

        }

        yield return new WaitForSeconds(1);

        if (System.IO.File.Exists(filePath)) {
            string json = DataManager.Load(filePath);
            LoadedItems  = JsonUtility.FromJson<List<Item>>(json);
            Debug.Log("File DB found!");
        }
        else {
            Debug.Log("No file DB found!");
        }
    }
}
