using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour {
    public static ItemAssets Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Sprite> DishSprites;
    public List<Sprite> IngredientRawSprites;
    public List<Sprite> IngredientProcessedSprites;
}
