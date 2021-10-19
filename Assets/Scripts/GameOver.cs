using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;

public class GameOver : MonoBehaviour
{
    [SerializeField] Button playAgainButton;
    [SerializeField] Button quitGameButton;

    [SerializeField] AudioClip grenadeSound;
    [SerializeField] AudioClip normalButtonSound;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = grenadeSound;
        audioSource.Play();
    }

    public void onPlayAgainClick()
    {
        audioSource.clip = normalButtonSound;
        audioSource.Play();
        StartCoroutine("waitForMainMenu");
    }

    public void onQuitGameClick()
    {
        audioSource.clip = normalButtonSound;
        audioSource.Play();
        StartCoroutine("waitForQuit");
    }

    // wait 1 seconds then load the main menu scene.
    IEnumerator waitForMainMenu()
    {
        yield return new WaitForSeconds(1f);
        LoadScene("MainScene");
    }

    // wait 1 seconds then quit the game.
    IEnumerator waitForQuit()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
}
