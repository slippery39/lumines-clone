using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField]
    private Button backToMainMenuButton;

    public Button BackToMainMenuButton { get => backToMainMenuButton; set => backToMainMenuButton = value; }

}
