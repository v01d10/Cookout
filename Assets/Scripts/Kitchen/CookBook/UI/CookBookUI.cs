using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookBookUI : MonoBehaviour {
    KitchenManager kitchenManager;
    CookBook cookBook;

    public GameObject cookBookUI;

    public Transform RecipeContainer;
    public Transform RecipeTemplate;

    private void Start() {
        kitchenManager = FindObjectOfType<KitchenManager>();
        cookBook = GameObject.Find("CookBook").GetComponent<CookBook>();
    }

    public void ShowCookBook(bool dishOrNot) {
        cookBookUI.SetActive(true);
        RefreshRecipes(dishOrNot);
    }

    public void HideCookBook() {
        cookBookUI?.SetActive(false);
    }

    public void RefreshRecipes(bool dishOrNot) {
        foreach (Transform child in RecipeContainer) {
            Destroy(child.gameObject);
        }

        foreach (Item recipe in cookBook.GetRecipeList(dishOrNot)) {
            RectTransform recipeRectTransform = Instantiate(RecipeTemplate, RecipeContainer).GetComponent<RectTransform>();
            recipeRectTransform.gameObject.SetActive(true);

            Image icon = recipeRectTransform.Find("RecipeIcon").Find("Icon").GetComponentInChildren<Image>();
            icon.sprite = recipe.GetSprite();

            TextMeshProUGUI nameText = recipeRectTransform.Find("RecipeName").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI timeText = recipeRectTransform.Find("RecipeTime").GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI amountText = recipeRectTransform.Find("RecipeAmount").GetComponentInChildren<TextMeshProUGUI>();

            nameText.SetText(recipe.ItemName);
            timeText.SetText(recipe.PrepareTime.ToString());
            amountText.SetText(recipe.Amount.ToString());

            Button button = recipeRectTransform.GetComponent<Button>();
            button.onClick.AddListener(() => {
                Workstation workstation = kitchenManager.SelectedEquipment.GetComponent<Workstation>();
                workstation.StartWorking(recipe);
            });

            Debug.Log("Spawned inventory slot");
        }
    }
}
