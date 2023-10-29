using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LoadedRecipes {
    [SerializeField] public List<Item> DishRecipes;
    [SerializeField] public List<Item> IngredientProcessedRecipes;
}
