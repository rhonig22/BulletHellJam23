using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUiController : MonoBehaviour
{
    public static readonly int menuScene = 1;
    public static readonly int optionsScene = 2;
    public static readonly int creditsScene = 3;
    public static readonly int leaderboardScene = 4;
    public static readonly int gameOverScene = 5;
    public static readonly int gamePlayScene = 6;
    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void OnStartGamePressed()
    {
        SceneManager.LoadScene(gamePlayScene);
    }

    public void OnCreditsPressed()
    {
        SceneManager.LoadScene(creditsScene);
    }

    public void OnOptionsPressed()
    {
        SceneManager.LoadScene(optionsScene);
    }

    public void OnLeaderboardPressed()
    {
        SceneManager.LoadScene(leaderboardScene);
    }

    public void SetMusicVolume(float vol)
    {
        GameObject.Find("MusicPlayer").GetComponent<AudioSource>().volume = vol;
    }

    public void SetEffectsVolume(float vol)
    {
        DataManager.Instance.SetEffectsVolume(vol);
    }
}
