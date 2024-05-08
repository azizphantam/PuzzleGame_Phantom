using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameLoose : MonoBehaviour
{
    public static GameLoose intance;
    public float timeLeft = 30.0f; // Initial time in seconds
    private bool timerRunning = true;

    public GameObject LooseCanvas;
    public TMP_Text TimeLeftText;
    public Slider TimerSlider;

    void Start()
    {
        if (intance == null)
        {
            intance = this;
        }
        
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (timerRunning)
        {
            yield return new WaitForSeconds(1.0f); // Wait for 1 second

            timeLeft -= 1.0f; // Decrement timeLeft by 1
            TimeLeftText.text = "Timer Left : " + timeLeft;
            TimerSlider.value-= 1.0f;
            // If timeLeft is less than or equal to 0, stop the timer
            if (timeLeft <= 0.0f)
            {
                timeLeft = 0.0f; // Ensure timer doesn't go negative
                timerRunning = false;
                LooseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            
        }
    }

    public void RestartTimer()
    {
        timeLeft = 30.0f;
        timerRunning = true;
        TimerSlider.value = 30.0f;
    }

    public void ReplayLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuScreen()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}