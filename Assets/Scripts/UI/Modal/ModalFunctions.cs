using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalFunctions : MonoBehaviour {
    public static ModalFunctions instance;

    private void Awake() {
        instance = this;
    }

    public void AnotherActionInProgress() {
        ModalHandler.instance.CloseModal();
    }

    public void NotEnoughIngredients() {
        ModalHandler.instance.CloseModal();
    }
}
