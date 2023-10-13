using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CookBook : MonoBehaviour {
    [SerializeField] public List<Item> DishRecipeList;
    [SerializeField] public List<Item> ProcessedRecipeList;

    public List<Item> GetRecipeList(bool dishOrNot) {
        switch(dishOrNot) {
            default:
            case true: return DishRecipeList;
            case false: return ProcessedRecipeList;  
        }
    }
}
