using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModalHandler : MonoBehaviour {
    public static ModalHandler instance;

    public GameObject ModalPanel;
    public TextMeshProUGUI ModalText;

    [Space]
    public Button OkayButton;
    public Button ContinueButton;
    public Button CancelButton;

    [Header("Databases")]
    [TextArea] public List<string> AvailableTexts;

    private void Awake() {
        instance = this;
    }

    public void OpenModal(bool singleButton, string text, UnityAction action0) {
        ModalPanel.SetActive(true);

        OkayButton.onClick.RemoveAllListeners();
        ContinueButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();

        if ( singleButton ) {
            ContinueButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
            OkayButton.gameObject.SetActive(true);

            OkayButton.GetComponent<Button>().onClick.AddListener(action0);
        } else {
            OkayButton.gameObject.SetActive(false);
            ContinueButton.gameObject.SetActive(true);
            CancelButton.gameObject.SetActive(true);

        }

        ModalText.SetText(text);
    }
    public void CloseModal() {
        ModalPanel.SetActive(false);
    }

    public void SetUpSingleButton() {

    }
    public void SetUpDoubeButton() {

    }
}
