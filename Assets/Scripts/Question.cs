using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;
using System.Linq;

public class Question : MonoBehaviour
{
    [Header("Question")]
    [SerializeField] TextMeshProUGUI questionText; // Question text area of quiz canvas.    
    string question; // question object of QuestionSO which is a scriptable object. 
    int questionsCount; // Counts the number of displayed questions

    [Header("Answer")]
    [SerializeField] List<Button> answerText = new List<Button>(); // Answer text areas of quiz canvas.
    int correctAnswerIndex; // will hold the index of correct answer of a question.
    string correctAnswer; // It will contain the correct answer for a specific question
    List<string> incorrectAnswers = new List<string>(); // The list will contain the incorrect answers for a specific question

    [Header("Sounds")]
    [SerializeField] AudioClip correctAnswerSound;
    [SerializeField] AudioClip wrongAnswerSound;

    Timer timer; // will be used to determine if the times up or not.

    AudioSource audioSource;

    int quesitonLimit = 10; // Number of questions to be displayed.

    [HideInInspector]
    public bool answerSelected; // if the user selected an answer or not.

    [HideInInspector]
    public bool isAnswerTrue; // is the selected answer true ?

    ScoreKeeper score; // User's current score

    float timeToShowAnswerResultScreen = 2f; // The screen which shows a answer's result (true or not), will be showed to the user for 2 seconds.

    void Start()
    {
        questionsCount = 0;
        audioSource = GetComponent<AudioSource>();
        timer = FindObjectOfType<Timer>();
        score = FindObjectOfType<ScoreKeeper>();
        DisplayQuestion(); // At start, displays a question.
    }

    void Update()
    {
        // if user selects an answer, the screen that shows its result will be showed for 2 seconds.
        if (answerSelected)
        {
            timeToShowAnswerResultScreen -= 1 * Time.deltaTime;
        }

        // if the showing an answer's result timer is equal or less than 0, display a new question and reset that timer.
        if (timeToShowAnswerResultScreen <= 0)
        {
            timeToShowAnswerResultScreen = 2f;
            DisplayQuestion();
        }
    }

    void DisplayQuestion()
    {
        answerSelected = false; // when display a new question, make "is user selected an answer" to false.

        if (questionsCount < quesitonLimit) // If the number of displayed questions less than the number of questions to be displayed.
        {
            if (MainScreen.questions.Count > 0) // If the questions list is not empty
            {
                int questionIndex = Random.Range(0, MainScreen.questions.Count); // Select a random question index
                question = MainScreen.questions[questionIndex].question; // Assign a question by using the random index.
                correctAnswer = MainScreen.questions[questionIndex].correct_answer; // Get and assign the correct answer of the question
                incorrectAnswers = MainScreen.questions[questionIndex].incorrect_answers; // Get and assign the incorrect answers of the question
                questionText.text = question; // Display the question text

                SetAnswers();

                questionsCount++;
                MainScreen.questions.RemoveAt(questionIndex);
            }
        }
    }

    void SetAnswers()
    {        
        correctAnswerIndex = Random.Range(0, answerText.Count); // Select a random answer index
        answerText[correctAnswerIndex].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer; // Assign the correct answer to an answer text field by using the index.
        answerText[correctAnswerIndex].interactable = true; // Make the correct answer button interactable
        List<Button> incorrectButtons = new List<Button>(); // The list will contain incorrect answers of the question
        incorrectButtons = answerText.Where(a => a != answerText[correctAnswerIndex]).ToList(); // Assign the incorrect answers to the list, but exclude the button which holds the correct answer

        for (int i = 0; i < incorrectAnswers.Count; i++)
        {
            incorrectButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = incorrectAnswers[i]; // Assign the incorrect answers to the answer text of buttons which does not contain the correct answer button
            incorrectButtons[i].interactable = true; // Make the buttons interactable
        }
    }

    // when the user selects an answer
    public void onAnswerSelected(int index)
    {
        answerSelected = true;

        // if the user's answer is true...
        if (index == correctAnswerIndex)
        {
            audioSource.clip = correctAnswerSound;
            audioSource.Play();
            questionText.text = "Congratulations, your answer is true! :)";

            // make the answers uninteractable, so that when an answer selected, user can not click answer buttons again.
            for (int i = 0; i < answerText.Count; i++)
            {
                answerText[i].interactable = false;
            }

            isAnswerTrue = true;
            score.CalculateScore();
        }
        else // if the user's answer is false
        {
            audioSource.clip = wrongAnswerSound;
            audioSource.Play();
            string correctAnswer = answerText[correctAnswerIndex].GetComponentInChildren<TextMeshProUGUI>().text; // get the correct answer text.
            questionText.text = "Sorry :( but the correct answer is \n" + $"<color=red>{correctAnswer}</color>"; // display the correct answer on screen

            // make the answers uninteractable, so that when an answer selected, user can not click answer buttons again.
            for (int i = 0; i < answerText.Count; i++)
            {
                answerText[i].interactable = false;
            }

            isAnswerTrue = false;
        }

        // if there are no questions to display and user's time is not up, wait 2 seconds then load the end game scene.
        if (questionsCount == quesitonLimit && !timer.timesUp)
        {
            StartCoroutine("WaitForEndGameScene");            
        }
    }

    // wait 2 seconds then load the end game scene.
    IEnumerator WaitForEndGameScene()
    {
        yield return new WaitForSeconds(2f);
        LoadScene("EndGameScene");
    }
}
