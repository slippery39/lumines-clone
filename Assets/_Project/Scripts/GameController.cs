using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomInput))]
public class GameController : MonoBehaviour
{

    public Game luminesGame;
    // Start is called before the first frame update


    private float currentTime = 0.0f;
    private float moveDownTime = 1.0f;
    private float nextMoveDownTime = 1.0f;


    private CustomInput customInputHandler;

    private void Awake()
    {
        luminesGame = new Game();
    }

    void Start()
    {
        //initialize our keydownTime dict.
        customInputHandler = GetComponent<CustomInput>();

        customInputHandler.AddHandler(KeyCode.LeftArrow, luminesGame.MoveLeft);
        customInputHandler.AddHandler(KeyCode.RightArrow, luminesGame.MoveRight);
        customInputHandler.AddHandler(KeyCode.DownArrow, luminesGame.MoveDown);

        //To prevent players from accidently placing too many blocks in a row from holding down the down key
        luminesGame.OnBlockPlaced += () => customInputHandler.ResetThrottleTime(KeyCode.DownArrow);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            luminesGame.DeleteMarkedSquares();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            luminesGame.BoardGravity();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            luminesGame.CurrentBlock.RotateLeft();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            luminesGame.CurrentBlock.RotateRight();
        } 

        //Our Loop Is Here
        luminesGame.MarkDeletions();

        //Automatic Current Block Movement
        currentTime += Time.deltaTime;

        if (currentTime > nextMoveDownTime)
        {
            luminesGame.MoveDown();
            nextMoveDownTime += moveDownTime;
        }

        //Gravity Tick - frame count is temporary.
        if (Time.frameCount % 4 == 0)
        {
            luminesGame.BoardGravity();
        }

    }
}
