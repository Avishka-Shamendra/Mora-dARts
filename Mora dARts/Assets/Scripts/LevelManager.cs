using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadGame()
    {
        // GameScene
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        // MainMenuScene
        SceneManager.LoadScene(0);
    }

    public void LoadGameOver()
    {
        // GameOverScene
        SceneManager.LoadScene(2);
    }

    public void LoadRules()
    {
        // RulesScene
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
