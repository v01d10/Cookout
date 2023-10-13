using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookBookUI : MonoBehaviour {
    CookBook cookBook;

    public Transform RecipeContainer;
    public Transform RecipeTemplate;

    public void RefreshRecipes(bool dishOrNot) {
        foreach (Transform child in RecipeContainer) {
            Destroy(child.gameObject);
        }

        foreach (Item recipe in cookBook.GetRecipeList(dishOrNot)) {
            RectTransform recipeRectTransform = Instantiate(RecipeTemplate, RecipeContainer).GetComponent<RectTransform>();
            recipeRectTransform.gameObject.SetActive(true);

            Image icon = recipeRectTransform.Find("RecipeIcon").GetComponentInChildren<Image>();
            icon.sprite = recipe.GetSprite();

            TextMeshProUGUI nameText = recipeRectTransform.Find("RecipeName").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI timeText = recipeRectTransform.Find("RecipeTime").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI amountText = recipeRectTransform.Find("RecipeAmount").GetComponentInChildren<TextMeshProUGUI>();

            nameText.SetText(recipe.ItemName);
            timeText.SetText(recipe.PrepareTime.ToString());
            amountText.SetText(recipe.Amount.ToString());

            Debug.Log("Spawned inventory slot");
        }
    }
}
