using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayfabLogin : MonoBehaviour
{
    [SerializeField] Canvas mainScreenCanvas;
    [SerializeField] Canvas loginCanvas;
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField userPassword;
    [SerializeField] TextMeshProUGUI errorText;

    [SerializeField] Button loginButton;
    [SerializeField] Button cancelButton;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = obj.GenerateErrorReport();
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        PlayerPrefs.SetString("USERNAME", userName.text);
        PlayerPrefs.SetString("PASSWORD", userPassword.text);

        gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(false);
        mainScreenCanvas.gameObject.SetActive(true);
    }

    public void onClickLogin()
    {
        audioSource.Play();
        Invoke("Login", 1f);
    }

    public void OncancelButtonClick()
    {
        audioSource.Play();
        Invoke("ClosePanel", 1f);
    }

    void Login()
    {
        var request = new LoginWithPlayFabRequest { Username = userName.text, Password = userPassword.text };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
