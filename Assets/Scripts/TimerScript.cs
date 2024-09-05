using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI clockWhite;
    public TextMeshProUGUI clockBlack;

    GameScript gs;
    CalculateAllMoves cam;
    GameObject controller;

    public float elapsedTimeWhite;
    public float elapsedTimeBlack;

    public int clockTimeMinutes = 15;
    public int clockTimeSeconds = 0;
    public int bonusSeconds = 5;

    int minutesWhite;
    int secondsWhite;
    int minutesBlack;
    int secondsBlack;
    float time;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        gs = controller.GetComponent<GameScript>();
        cam = controller.GetComponent<CalculateAllMoves>();

        time = clockTimeMinutes * 60 + clockTimeSeconds;
        elapsedTimeWhite = time;
        elapsedTimeBlack = time;

        clockWhite.text = string.Format("{0:00}:{1:00}", clockTimeMinutes, clockTimeSeconds);
        clockBlack.text = string.Format("{0:00}:{1:00}", clockTimeMinutes, clockTimeSeconds);
    }
    void Update()
    {

        minutesWhite = Mathf.FloorToInt(elapsedTimeWhite / 60);
        secondsWhite = Mathf.FloorToInt(elapsedTimeWhite % 60);
        minutesBlack = Mathf.FloorToInt(elapsedTimeBlack / 60);
        secondsBlack = Mathf.FloorToInt(elapsedTimeBlack % 60);

        if (elapsedTimeWhite > 0 && gs.GetCurrentPlayer() == "white" && gs.startClock == true)
        {
            elapsedTimeWhite -= Time.deltaTime;
        }

        if (elapsedTimeWhite > 0 && gs.GetCurrentPlayer() == "black")
        {
            elapsedTimeBlack -= Time.deltaTime;
        }

        if (elapsedTimeBlack <= 0 || cam.blackChecked)
        {
            SceneManager.LoadScene("Game");
        }

        if (elapsedTimeWhite <= 0 || cam.whiteChecked)
        {
            SceneManager.LoadScene("Game");
        }

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
}
