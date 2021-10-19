using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] Button signupButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject signupPanel;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnLoginButtonClick()
    {
        audioSource.Play();
        Invoke("OpenLoginPanel", 1f);
    }

    public void OnSignupButtonClick()
    {
        audioSource.Play();
        Invoke("OpenSignupPanel", 1f);
    }

    public void OnQuitButtonClick()
    {
        audioSource.Play();
        StartCoroutine("WaitForQuit");
    }

    void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
    }

    void OpenSignupPanel()
    {
        signupPanel.SetActive(true);
    }

    // When the user clicks quit button, game will quit after 1.5 seconds.
    IEnumerator WaitForQuit()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
