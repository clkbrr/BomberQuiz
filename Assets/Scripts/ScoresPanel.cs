using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class ScoresPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userScoreText;
    [SerializeField] Button closeButton;
    ScoreListing scoreListing;
    [SerializeField] GameObject userNameAndScorePrefab;
    [SerializeField] Transform scoreListingContainer;

    void Awake()
    {
        GetLeaderboard();
        userScoreText.text = "Your highest score is  : " + MainScreen.score.ToString();
    }

    // Make a request to get the leaderboard list(top ten users)
    void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "Score", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnErrorLeaderBoard);
    }

   void OnErrorLeaderBoard(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }

    // If get leaderboard request is successful, display the top ten users 
    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject scoreList = Instantiate(userNameAndScorePrefab, scoreListingContainer);
            scoreListing = scoreList.GetComponent<ScoreListing>();
            scoreListing.userNameText.text = player.DisplayName;
            scoreListing.scoreText.text = player.StatValue.ToString();
        }
    }

    public void OnCloseButtonClick()
    {
        gameObject.SetActive(false);

        if (scoreListingContainer.childCount > 0)
        {
            for (int i = scoreListingContainer.childCount; i >= 0; i--)
            {
                Destroy(scoreListingContainer.GetChild(i).gameObject);
            }
        }  
    }
}
