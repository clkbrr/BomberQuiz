using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class MainScreen : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button scoresButton;
    [SerializeField] GameObject scoresPanel;
    [SerializeField] Canvas loginCanvas;

    public static List<QuestionList> questions;

    public static int score;

    AudioSource audioSource;

    string userName;
    string userPassword;

    [SerializeField] TextAsset jsonFile; // The json file which contains the questions

    void Start()
    {
        GetQuestions();
        audioSource = GetComponent<AudioSource>();

        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "16D0E";
        }

        // If user has already registered login automatically
        if (PlayerPrefs.HasKey("USERNAME"))
        {
            userName = PlayerPrefs.GetString("USERNAME");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithPlayFabRequest { Username = userName, Password = userPassword };
            PlayFabClientAPI.LoginWithPlayFab(request, onLoginSuccess, onLoginFailure);
        }

        else // IF not registered open login canvas, so that he/she can register
        {
            gameObject.SetActive(false);
            loginCanvas.gameObject.SetActive(true);
        }        
    }

    void GetQuestions()
    {
        ProcessJsonData(jsonFile);
    }

    // Get questions from the json file and assign them to "questions" list
    void ProcessJsonData(TextAsset _jsonFile)
    {
        JsonDataClass jsonData = JsonUtility.FromJson<JsonDataClass>(_jsonFile.text);
        questions = jsonData.results;
    }

    // If user can login succesfully, then get the user's highest score from "Playfab"
    private void onLoginSuccess(LoginResult obj)
    {
        loginCanvas.gameObject.SetActive(false);
        GetUserScore();
    }

    // If login fails, then activate the login canvas so that user can login
    private void onLoginFailure(PlayFabError obj)
    {
        gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(true);
    }

    // Make request to get the user's highest score from "Playfab"
    public void GetUserScore()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), OnGetStats,
            error => error.GenerateErrorReport()
            );
    }

    // If request is successful, get the user's highest score from "Playfab" and assign it to a static score variable
    void OnGetStats(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "Score":
                    score = eachStat.Value;
                    break;
                default:
                    break;
            }
        }
    }

    public void OnPlayButtonClick()
    {
        audioSource.Play();
        StartCoroutine("WaitForPlay");
    }

    public void OnQuitButtonClick()
    {
        audioSource.Play();
        StartCoroutine("WaitForQuit");
    }

    public void onScoresButtonClick()
    {
        audioSource.Play();
        scoresPanel.SetActive(true);
    }

    // When the user clicks continue button, scene will change after 1.5 seconds.
    IEnumerator WaitForPlay()
    {
        yield return new WaitForSeconds(1.5f);
        LoadScene("QuizScene");
    }

    // When the user clicks quit button, game will quit after 1.5 seconds.
    IEnumerator WaitForQuit()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
