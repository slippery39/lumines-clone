using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour
{
    public Button startGameButton;
    public Button quitGameButton;
    public Button howToPlayButton;
    public Button backToMainMenuButton;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject howToPlay;

    public void Start()
    {
        startGameButton.onClick.AddListener(LoadGame);
        quitGameButton.onClick.AddListener(QuitGame);
        backToMainMenuButton.onClick.AddListener(ShowMainMenu);
        howToPlayButton.onClick.AddListener(ShowHowToPlay);
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

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        howToPlay.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(true);
    }
}
