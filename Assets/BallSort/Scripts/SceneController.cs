using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToNextLevel()
    {
        int currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        SceneManager.LoadScene("Level "+ currentUnlockedLevel);
    }
        public void GoToSameLevel()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void GoToLevelMenu()
    {
        SceneManager.LoadScene("LevelsMenu");
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("Start");
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
