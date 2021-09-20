using GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrottledInput))]
public class GameController : MonoBehaviour
{

    public Game luminesGame;
    // Start is called before the first frame update


    private float currentTime = 0.0f;

    public float CurrentTime { get { return currentTime; } }

    private float moveDownTime = 1.0f;
    private float nextMoveDownTime = 1.0f;

    private float nextGravityTime = 1.05f;

    private ThrottledInput customInputHandler;

    public int erasedBlocksCount;
  

    private void Awake()
    {
        luminesGame = new Game();
    }

    void Start()
    {
        InitializeInputHandlers();
        InitializeGameEventHandlers();
    }

    // Update is called once per frame
    void Update()
    {
        GameLoop();
    }

    private void InitializeInputHandlers()
    {
        customInputHandler = GetComponent<ThrottledInput>();
        customInputHandler.AddHandler(KeyCode.LeftArrow, luminesGame.MoveLeft);
        customInputHandler.AddHandler(KeyCode.RightArrow, luminesGame.MoveRight);
        customInputHandler.AddHandler(KeyCode.DownArrow, luminesGame.MoveDown);
        customInputHandler.AddHandler(KeyCode.Q, ()=> { luminesGame.CurrentBlock.RotateLeft(); });
        customInputHandler.AddHandler(KeyCode.W, ()=> { luminesGame.CurrentBlock.RotateRight(); });        
    }

    private void InitializeGameEventHandlers()
    {
        //To prevent players from accidently placing too many blocks in a row from holding down the down key
        luminesGame.OnBlockPlaced += (info) => customInputHandler.ResetThrottleDelayTime(KeyCode.DownArrow);
        luminesGame.OnDeletion += (info) => erasedBlocksCount += info.SquaresDeleted.Count;
    }

    private void GameLoop()
    {
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
        if (currentTime > nextGravityTime)
        {
            luminesGame.BoardGravity();
            nextGravityTime += 0.05f;
        }
    }
}
