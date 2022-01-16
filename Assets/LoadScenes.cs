using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{
    public Button startGameButton;
    public Button quitGameButton;

    public void Start()
    {
        startGameButton.onClick.AddListener(LoadGame);
    }

    public void LoadGame()
    {
        //TODO - change name of the game scene to Game instead of Main.
        SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
