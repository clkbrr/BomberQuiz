using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.SceneManagement.SceneManager;

public class Timer : MonoBehaviour
{
    [SerializeField] ParticleSystem sparkle;
    [SerializeField] Slider slider;
    AudioSource audioSource;
    Question question;

    [HideInInspector]
    public float sliderValue;

    float decreaseAmountPerSecond = 1;

    [HideInInspector]
    public bool timesUp;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        question = FindObjectOfType<Question>();
        sliderValue = 22;
    }

    void Update()
    {
        DecreaseTimer();
    }

    void DecreaseTimer()
    {
        if (!question.answerSelected)
        {
            if (slider.value >= 7 && !timesUp)
            {
                slider.value = slider.value - (decreaseAmountPerSecond * Time.deltaTime);

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }                
            }
            
            if(slider.value <= 7)
            {
                timesUp = true;
                audioSource.Stop();
                LoadScene("GameOverScene");
            }
        }
        else
        {
            slider.value = slider.maxValue;
            audioSource.Stop();
        }

        sliderValue = slider.value;
    }

    void PlayWickSound()
    {
        if (!timesUp)
        {
            audioSource.Play();
        }
    }
}
