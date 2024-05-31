using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSeeScoreClick() {
        SceneManager.LoadScene("See score");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
