using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Game luminesGame;
    // Start is called before the first frame update


    private float currentTime = 0.0f;
    private float moveDownTime = 1.0f;
    private float nextMoveDownTime = 1.0f;

    private void Awake()
    {
        luminesGame = new Game();
    }

    void Start()
    {
        
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            luminesGame.MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            luminesGame.MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            luminesGame.MoveDown();
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
