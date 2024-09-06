using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject main;
    public GameObject TimeControl;

    public int time;
    public int bonus;
    void Start()
    {
        TimeControl.SetActive(false);
    }

    public void OnMainButtonPress()
    {
        main.SetActive(false);
        TimeControl.SetActive(true);
    }

    public void OnTimeButtonPress()
    {
        GameObject timeObject = GameObject.Find("/Canvas/TimeControl/Time");
        GameObject bonusObject = GameObject.Find("/Canvas/TimeControl/Bonus");

        if (bonusObject != null && timeObject != null)
        {
            int time_index, bonus_index;

            time_index = timeObject.GetComponent<TMP_Dropdown>().value;
            bonus_index = bonusObject.GetComponent<TMP_Dropdown>().value;

            List<TMP_Dropdown.OptionData> menuOptionsTime = timeObject.GetComponent<TMP_Dropdown>().options;
            List<TMP_Dropdown.OptionData> menuOptionsBonus = bonusObject.GetComponent<TMP_Dropdown>().options;

            time = int.Parse(menuOptionsTime[time_index].text);
            bonus = int.Parse(menuOptionsBonus[bonus_index].text);

            PlayerPrefs.SetInt("time", time);
            PlayerPrefs.SetInt("bonus", bonus);
        }

        SceneManager.LoadScene("Game");
    } 
}
