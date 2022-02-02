using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private bool _isPaused = false;
    //The parent game object that contains all the things corresponding to a lumines game.
    [SerializeField]
    private GameObject luminesGameParent;
    
    //our conductor object has special time considerations that we need to take into account when pausing / resuming.
    [SerializeField]
    private Conductor conductor;

    [SerializeField]
    private List<GameObject> gameComponents = new List<GameObject>();

    [SerializeField]
    ThrottledInput gameInputs;

    [SerializeField]
    PauseMenu pauseMenu;


    private void Start()
    {
        pauseMenu.ResumeButton.onClick.AddListener(() =>
        {
            ResumeGame();
        });

        pauseMenu.ExitButton.onClick.AddListener(() =>
        {
            BackToMainMenu();
        });
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (!_isPaused)
        {
            var luminesGameComponents = luminesGameParent.GetComponentsInChildren<ILuminesGameUpdateable>();
            //TODO - check this for performance concerns.
            foreach (var comp in luminesGameComponents)
            {
                    comp.LuminesGameUpdate();
            }
        }       
    }

    void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        gameInputs.enabled = false;
        pauseMenu.gameObject.SetActive(true);
    }

    void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        gameInputs.enabled = true;
        pauseMenu.gameObject.SetActive(false);
    }

    private void ResetStatics()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
    }

    void BackToMainMenu()
    {
        ResetStatics();
        SceneManager.LoadScene("MainMenu");
    }
}
