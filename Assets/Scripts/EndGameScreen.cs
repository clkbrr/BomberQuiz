using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button MainMenuButton;
    [SerializeField] Button QuitGameButton;
    [SerializeField] AudioClip fireworksSound;
    [SerializeField] AudioClip normalButtonSound;
    AudioSource audioSource;

    int highestScore = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireworksSound;
        audioSource.Play();
        CalculateScore();
        SetScore();
    }

    void CalculateScore()
    {
        scoreText.text = "Your score is: " + ScoreKeeper.score;
        SavePlayerScore();
    }

    // Set the user's score stat which is recoreded in Playfab with the current final score
    void SetScore()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate{StatisticName = "Score", Value = highestScore},
            },
        },
        result => { Debug.Log("Statistics updated."); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void OnMainMenuButtonClicked()
    {
        audioSource.clip = normalButtonSound;
        audioSource.Play();
        StartCoroutine("WaitForMainMenu");
    }

    public void OnQuitButtonClicked()
    {
        audioSource.clip = normalButtonSound;
        audioSource.Play();
        StartCoroutine("WaitForQuit");
    }

    // wait 1 seconds then load the main menu scene.
    IEnumerator WaitForMainMenu()
    {
        yield return new WaitForSeconds(1f);
        LoadScene("MainScene");
    }

    // wait 1 seconds then quit the game.
    IEnumerator WaitForQuit()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }

    void SavePlayerScore()
    {
        // If player's new score is greater than his previous score OR player has not have a previous score
        if ((PlayerPrefs.HasKey("Score") && (ScoreKeeper.score > PlayerPrefs.GetInt("Score"))) || !PlayerPrefs.HasKey("Score"))
        {
            // Save the player final score
            PlayerPrefs.SetInt("Score", ScoreKeeper.score);
            PlayerPrefs.Save();
            highestScore = ScoreKeeper.score;
        }

        // If the user's last recorded highest score is less then the current score, make the current score the new highest score
        if (MainScreen.score < ScoreKeeper.score)
        {
            MainScreen.score = ScoreKeeper.score;
            highestScore = ScoreKeeper.score;
        }
        else
        {
            highestScore = MainScreen.score;
        }
    }
}
