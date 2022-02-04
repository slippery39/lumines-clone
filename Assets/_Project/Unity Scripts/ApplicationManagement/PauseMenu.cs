using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button exitButton;
    public Button ResumeButton { get => resumeButton; set => resumeButton = value; }
    public Button ExitButton { get => exitButton; set => exitButton = value; }
}
