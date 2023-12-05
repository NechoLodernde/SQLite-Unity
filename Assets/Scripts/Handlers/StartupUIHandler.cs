using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupUIHandler : MonoBehaviour
{
    readonly private string MainMenuSceneName = "MainMenuScene";

    private void Start()
    {
        GoToMainMenu();
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
