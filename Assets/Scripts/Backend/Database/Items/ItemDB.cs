using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;
using System.IO;
using System.Net;
using Firebase.Firestore;
using Unity.VisualScripting;
using System;
using Newtonsoft.Json;
using static UnityEditor.Progress;

[Serializable]
public class ItemDB : MonoBehaviour {
    public static ItemDB instance;
    CookBook cookBook;

    FirebaseStorage storage;
    StorageReference storageReference;

    [TextArea] public string ItemDatabaseFileUrl;
    public string IconURL;

    string destination;
    string filePath;

    public List<Item> LoadedDishes;
    public List<Item> LoadedIngredientsRaw;
    public List<Item> LoadedIngredientsProcessed;

    [Space]
    public List<int> DishRecipesAmounts;
    public List<int> ProcessedRecipesAmounts;

    bool downloaded;
    bool fileExist;
    [HideInInspector] public bool parsed;

    private void Start() {
        instance = this;
        storage = FirebaseStorage.DefaultInstance;
        cookBook = GameObject.Find("CookBook").GetComponent<CookBook>();

        for (int i = 0; i < 3; i++) {
            StartCoroutine(LoadItemDB(i));
        }
    }

    public IEnumerator LoadItemDB(int i) {
        yield return new WaitForSeconds(1);
        destination = Application.persistentDataPath;

        switch(i) {
            default:
            case 0: 
                filePath = Path.Combine(destination, "DishDatabase.json");
                break;
            case 1:
                filePath = Path.Combine(destination, "IngredientRawDatabase.json");
                break;
            case 2:
                filePath = Path.Combine(destination, "IngredientProcessedDatabase.json");
                break;
        }

        ParseFile(i);

        yield return new WaitUntil(() => parsed);
        cookBook.StartLoadingRecipes();
    }

    void ParseFile(int i) {
        if (!File.Exists(filePath)) {
            Debug.LogWarning("File not found! " + filePath);
            return;
        }

        string json = DataManager.Load(filePath);
        //Debug.Log(json);

        switch(i) {
            default:
            case 0:
                LoadedDishes = JsonConvert.DeserializeObject<List<Item>>(json);
                break;
            case 1:
                LoadedIngredientsRaw = JsonConvert.DeserializeObject<List<Item>>(json);
                break;
            case 2:
                LoadedIngredientsProcessed = JsonConvert.DeserializeObject<List<Item>>(json);
                parsed = true;
                break;
        }
    }

    //public IEnumerator GetItemDB(int i) {
    //    destination = Application.persistentDataPath;

    //    if (i == 0) filePath = destination + "/DishDatabase.json";
    //    if (i == 1) filePath = destination + "/IngredientRawhDatabase.json";
    //    if (i == 2) filePath = destination + "/IngredientProcessedDatabase.json";

    //    storageReference = storage.GetReferenceFromUrl(ItemDatabaseFileUrl);
    //    StartCoroutine(GetURL());

    //    yield return new WaitUntil(() => downloaded);
    //    CheckFile(filePath);

    //    yield return new WaitUntil(() => fileExist);
    //    ParseFile(i);
    //}

    //public IEnumerator GetURL() {
    //    var getFile = storageReference.GetDownloadUrlAsync();
    //    yield return new WaitUntil(() => getFile.IsCompleted);
    //    if (!getFile.IsFaulted && !getFile.IsCanceled) {
    //        Debug.Log("Download URL: " + getFile.Result);
    //        Uri url = getFile.Result;
    //        DownloadFile(url);
    //    }
    //}

    //public void DownloadFile(Uri url) {
    //    using (WebClient webClient = new WebClient()) {
    //        webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadCompletedCallback);
    //        webClient.DownloadFileAsync(url, filePath);
    //    }
    //}

    //private void DownloadCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
    //    if (e.Error != null) {
    //        Debug.Log("Download failed!");
    //        downloaded = false;
    //    }
    //    else {
    //        Debug.Log("Download complete!");
    //        downloaded = true;
    //    }
    //}

    //public void CheckFile(string filePath) {
    //    if (!System.IO.File.Exists(filePath)) {
    //        Debug.Log("No file DB found!");
    //        return;
    //    }

    //    Debug.Log("File DB found!");
    //    fileExist = true;
    //}


}
