using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        //TODO - show pause menu.
    }

    void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        gameInputs.enabled = true;
    }
}
