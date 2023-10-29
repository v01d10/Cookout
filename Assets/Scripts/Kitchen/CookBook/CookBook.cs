using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CookBook : MonoBehaviour {
    [SerializeField] public List<Item> DishRecipeList;
    [SerializeField] public List<Item> ProcessedRecipeList;

    public void StartLoadingRecipes() {
        StartCoroutine(LoadRecipes());
    }

    public IEnumerator LoadRecipes() {
        yield return new WaitUntil(() => ItemDB.instance.parsed);

        DishRecipeList = ItemDB.instance.LoadedDishes;
        ProcessedRecipeList = ItemDB.instance.LoadedIngredientsProcessed;

        for (int i = 0; i < DishRecipeList.Count; i++) {
            DishRecipeList[i].Amount = ItemDB.instance.DishRecipesAmounts[i];
        }

        for (int i = 0; i < ProcessedRecipeList.Count; i++) {
            ProcessedRecipeList[i].Amount = ItemDB.instance.ProcessedRecipesAmounts[i];
        }
    }

    public List<Item> GetRecipeList(bool dishOrNot) {
        if(dishOrNot) {
            return DishRecipeList;
        } else {
            return ProcessedRecipeList;
        }
    }
}
