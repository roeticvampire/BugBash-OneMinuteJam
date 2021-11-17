using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class HomeScreenCode : MonoBehaviour
{
    [SerializeField] private Image howtoPlay;
    [SerializeField] private TextMeshProUGUI highScore;
    private void Start()
    {

        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/gameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        howtoPlay.enabled = !howtoPlay.enabled;
    }
}
