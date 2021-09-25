using GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ThrottledInput))]
public class GameController : MonoBehaviour
{

    public Game luminesGame;
    // Start is called before the first frame update

    private float _currentTime = 0.0f;
    public float CurrentTime { get { return _currentTime; } }

    private float _moveDownTime = 1.0f;
    private float _nextMoveDownTime = 1.0f;

    private float _nextGravityTime = 1.05f;

    private ThrottledInput customInputHandler;

    public int erasedBlocksCount;
    public int score = 0;


    private Scorer scorer = new Scorer();
  

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

    public void OnScoreAdded(Action<int> handler)
    {
        scorer.OnScoreAdded += handler;
    }

    public void OnScoreMultiplierIncrease(Action<int> handler)
    {
        scorer.OnScoreMultiplierIncrease += handler;
    }

    private void InitializeInputHandlers()
    {
        customInputHandler = GetComponent<ThrottledInput>();
        customInputHandler.AddHandler(KeyCode.LeftArrow, luminesGame.MoveLeft);
        customInputHandler.AddHandler(KeyCode.RightArrow, luminesGame.MoveRight);
        customInputHandler.AddHandler(KeyCode.DownArrow, luminesGame.SoftDrop,0.1f,0.01f);
        customInputHandler.AddHandler(KeyCode.Q, ()=> { luminesGame.CurrentBlock.RotateLeft(); });
        customInputHandler.AddHandler(KeyCode.W, ()=> { luminesGame.CurrentBlock.RotateRight(); });        
    }

    

    private void InitializeGameEventHandlers()
    {
        //To prevent players from accidently placing too many blocks in a row from holding down the down key
        luminesGame.OnBlockPlaced += (info) => customInputHandler.ResetThrottleDelayTime(KeyCode.DownArrow);
        luminesGame.OnDeletion += (info) => erasedBlocksCount += info.SquaresDeleted.Count;

        luminesGame.OnDeletion += (info) => score+=scorer.OnBlockDeleted(info);
        luminesGame.OnTimeLineEnd += (info) => score+=scorer.OnTimeLineEnd(info);
        luminesGame.OnSoftDrop += (info) => score+=scorer.OnSoftDrop(info);        
    }
    private void GameLoop()
    {
        //Our Loop Is Here
        luminesGame.MarkDeletions();

        //Automatic Current Block Movement
        _currentTime += Time.deltaTime;

        if (_currentTime > _nextMoveDownTime)
        {
            luminesGame.Gravity();
            _nextMoveDownTime += _moveDownTime;
        }

        //Gravity Tick - frame count is temporary.
        if (_currentTime > _nextGravityTime)
        {
            luminesGame.BoardGravity();
            _nextGravityTime += 0.05f;
        }
    }
}
