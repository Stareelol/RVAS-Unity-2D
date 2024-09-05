using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI clockWhite;
    public TextMeshProUGUI clockBlack;
    public TextMeshProUGUI checkmateText;
    public TextMeshProUGUI warningTextWhite;
    public TextMeshProUGUI warningTextBlack;

    GameScript gs;
    GameObject controller;
    GameObject panel;

    public float elapsedTimeWhite;
    public float elapsedTimeBlack;
    public float elapsedTimeWarningWhite;
    public float elapsedTimeWarningBlack;

    public int clockTimeMinutes = 15;
    public int clockTimeSeconds = 0;
    public int bonusSeconds = 5;

    public int warningTimeWhite = 20;
    public int warningTimeBlack = 20;

    int minutesWhite;
    int secondsWhite;
    int minutesBlack;
    int secondsBlack;
    float time;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        gs = controller.GetComponent<GameScript>();

        panel = GameObject.FindGameObjectWithTag("Panel");
        panel.SetActive(false);
        checkmateText.enabled = false;
        warningTextBlack.enabled = false;

        time = clockTimeMinutes * 60 + clockTimeSeconds;
        elapsedTimeWhite = time;
        elapsedTimeBlack = time;

        elapsedTimeWarningWhite = warningTimeWhite;
        elapsedTimeWarningBlack = warningTimeBlack;

        clockWhite.text = string.Format("{0:00}:{1:00}", clockTimeMinutes, clockTimeSeconds);
        clockBlack.text = string.Format("{0:00}:{1:00}", clockTimeMinutes, clockTimeSeconds);
    }
    void Update()
    {

        minutesWhite = Mathf.FloorToInt(elapsedTimeWhite / 60);
        secondsWhite = Mathf.FloorToInt(elapsedTimeWhite % 60);
        minutesBlack = Mathf.FloorToInt(elapsedTimeBlack / 60);
        secondsBlack = Mathf.FloorToInt(elapsedTimeBlack % 60);

        warningTimeWhite = Mathf.FloorToInt(elapsedTimeWarningWhite % 60);
        warningTimeBlack = Mathf.FloorToInt(elapsedTimeWarningBlack % 60);

        if (elapsedTimeWhite > 0 && gs.GetCurrentPlayer() == "white" && gs.startClockWhite == true)
        {
            elapsedTimeWhite -= Time.deltaTime;
        }

        if (elapsedTimeWhite > 0 && gs.GetCurrentPlayer() == "black" && gs.startClockBlack == true)
        {
            elapsedTimeBlack -= Time.deltaTime;
        }

        if (elapsedTimeBlack <= 0) 
        {
            SceneManager.LoadScene("Game");
        }

        if (elapsedTimeWhite <= 0) 
        {
            SceneManager.LoadScene("Game");
        }

        if (gs.startClockWhite == false)
        {
            elapsedTimeWarningWhite -= Time.deltaTime;
        }

        if (gs.startClockBlack == false && gs.startClockWhite)
        {
            elapsedTimeWarningBlack -= Time.deltaTime;
        }

        if (gs.startClockWhite) warningTextWhite.enabled = false;

        if (gs.startClockBlack == false && gs.startClockWhite) warningTextBlack.enabled = true;
        else warningTextBlack.enabled = false;

        if (gs.startClockWhite == false && elapsedTimeWarningWhite <= 0) ShowCheckmateScreen("warningWhite");

        warningTextWhite.text = string.Format("{0} sekundi za prvi potez!", warningTimeWhite);
        warningTextBlack.text = string.Format("{0} sekundi za prvi potez!", warningTimeBlack);
        clockWhite.text = string.Format("{0:00}:{1:00}", minutesWhite, secondsWhite);
        clockBlack.text = string.Format("{0:00}:{1:00}", minutesBlack, secondsBlack);

    }
    public void AddSecondsToClock(string player)
    {
        if (player == "white")
        {
            elapsedTimeWhite += bonusSeconds;
        }
        else if (player == "black")
        {
            elapsedTimeBlack += bonusSeconds;
        }
    }

    public void ShowCheckmateScreen(string player)
    {
        panel.SetActive(true);
        checkmateText.enabled = true;

        switch (player)
        {
            case "white":
                {
                    checkmateText.text = "Šah mat! Beli je pobedio!";
                    Time.timeScale = 0;
                }
                break;
            case "black":
                {
                    checkmateText.text = "Šah mat! Crni je pobedio!";
                    Time.timeScale = 0;
                }
                break;
            case "stalemate":
                {
                    checkmateText.text = "Pat!";
                    Time.timeScale = 0;
                }
                break;
            case "warningWhite":
                {
                    checkmateText.text = "Vreme je isteklo! Crni je pobedio!";
                    Time.timeScale = 0;
                }
                break;
            case "warningBlack":
                {
                    checkmateText.text = "Vreme je isteklo! Beli je pobedio!";
                    Time.timeScale = 0;
                }
                break;
        }
    }

    public void CloseAndRestart()
    {
        SceneManager.LoadScene("Game");
    }
}
