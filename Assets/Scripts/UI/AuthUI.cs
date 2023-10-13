using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthUI : MonoBehaviour {
    public GameObject RegisterUI;
    public GameObject LoginUI;

    public void OpenRegisterUI() {
        RegisterUI.SetActive(true);
    }
    public void CloseRegisterUI() {
        RegisterUI.SetActive(false);
    }

    public void OpenLoginUI() {
        LoginUI.SetActive(true);
    }
    public void CloseLoginUI() {
        LoginUI.SetActive(false);
    }

    public void HandleRegisterUI() {
        if(RegisterUI.activeInHierarchy)
            RegisterUI.SetActive(false);
        else
            RegisterUI.SetActive(true);
    }

    public void HandleLoginUI() {
        if(LoginUI.activeInHierarchy)
            LoginUI.SetActive(false);
        else
            LoginUI.SetActive(true);
    }
}
