using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayfabRegister : MonoBehaviour
{
    [SerializeField] Canvas mainScreenCanvas;
    [SerializeField] Canvas loginCanvas;
    [SerializeField] TMP_InputField userName;
    [SerializeField] TMP_InputField userPassword;
    [SerializeField] TextMeshProUGUI errorText;

    [SerializeField] Button saveButton;
    [SerializeField] Button cancelButton;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();   
    }

    public void OnSaveButtonClick()
    {
        audioSource.Play();
        Invoke("Save", 1f);
    }

    public void OnCancelButtonClick()
    {
        audioSource.Play();
        Invoke("ClosePanel", 1f);
    }

    private void OnRegisterFailure(PlayFabError obj)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = obj.GenerateErrorReport();
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {
        PlayerPrefs.SetString("USERNAME", userName.text);
        PlayerPrefs.SetString("PASSWORD", userPassword.text);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName.text }, OnDisplayName, OnErrorDisplayName);
        gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(false);
        mainScreenCanvas.gameObject.SetActive(true);
    }

    private void OnErrorDisplayName(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {

    }

    void Save()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Username = userName.text, Password = userPassword.text, RequireBothUsernameAndEmail = false };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
