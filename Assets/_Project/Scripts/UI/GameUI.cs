using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameUI : MonoBehaviour
{

    [Header("Game State")]
    [SerializeField]
    private GameController gameController;

    [Header("Misc")]
    [SerializeField]
    private bool willSetConductor = false;

    [Header("Skin")]
    [SerializeField]
    private Skin skin;

    [Header ("Game Board")]
    [SerializeField]
    private GameCellsController cells;

    [SerializeField]
    private GameObject grid;

    [SerializeField]
    private TimeLine timeLine;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject dropPreview;

    [SerializeField]
    private GameObject _beatNumbers;

    private GameObject _scoreBoard;

    [SerializeField]
    private NextBlocks upcomingBlocks;
    // Start is called before the first frame update
    [SerializeField]
    private ScoreMultiplierNotification scoreMultiplierNotification;


    private void Awake()
    {
        
    }

    void Start()
    {

        if (gameController == null)
        {
            throw new System.Exception("Please ensure you have connected the game to the GameBoardController in the inspector");
        }
        if (gameController.luminesGame == null)
        {
            throw new System.Exception("the lumines game has not been created yet");
        }
        if (grid == null)
        {
            throw new System.Exception("Please ensure you have connected the grid to the GameBoardController in the inspector");
        }
        if (cells == null)
        {
            throw new System.Exception("Please ensure you have connected the cells to the GameBoardController in the inspector");
        }
        if (timeLine == null)
        {
            throw new System.Exception("Please ensure you have connected the timeLine to the GameBoardController in the inspector");
        }

        if (upcomingBlocks == null)
        {
            throw new System.Exception("Please ensure you have connected the upcomingBlocks to the GameBoardController in the inspector");
        }

        if (dropPreview == null)
        {
            throw new System.Exception("Please ensure the drop preview object is connected to the GameBoardController in the inspector");
        }

        scoreMultiplierNotification.EnsureInitialized(this); 

        cells.Initialize(gameController.luminesGame);

        SetSkin(skin);

        upcomingBlocks.SetNextBlocks(gameController.luminesGame.UpcomingBlocks.ToList());


        gameController.luminesGame.OnNewBlock += ((onNewBlockInfo) =>
          {
              upcomingBlocks.UpdateNextBlocks(onNewBlockInfo.UpcomingBlocks);
          });

        gameController.OnScoreAdded((int amount) =>
         {
             foreach(var score in GetComponentsInChildren<IUsesScore>())
             {
                 score.OnScoreAdded(amount);
             }
         });

        gameController.OnScoreMultiplierIncrease( (int amount) =>
        {
            foreach (var score in GetComponentsInChildren<IUsesScoreMultiplier>())
            {
                score.SetMultiplier(amount);
            }
        });

        
        gameController.OnDeletion((info) =>
        {
            timeLine.AmountDeleted = info.SquaresDeletedThisTurn;
        });

        gameController.OnTimeLineEnd((info) =>
        {
            timeLine.AmountDeleted = 0;
        });

        


    }

    // Update is called once per frame
    void Update()
    {
        SetTimeLinePosition();
        SetBlockDropPreviewPosition();

        foreach(var score in GetComponentsInChildren<IUsesScore>())
        {
            score.CurrentTime = gameController.CurrentTime;
            score.BlocksErased = gameController.erasedBlocksCount;
            score.Score = gameController.score;
        }
    }

    public void SetSkin(Skin skin)
    {

        SetBackground(skin.Background);
        SetBlockPiece(skin.BlockPiece);
        SetHighlightedBlockPiece(skin.HighlightedSquare);
        SetBeatNumbers(skin.BeatNumbers);
        SetScoreBoard(skin.ScoreBoard);

        //Hacky solution so that any duplicate UI's do not accidently set the conductor timings.
        if (willSetConductor) 
            Conductor.Instance.SetFromSkin(skin);
        /*
         * Set the Background [DONE]
         * Set the Block Piece [DONE]
         * Set the Highlighted Block Piece [PROGRESS]
         * Set the Music?? (But Music is Controlled by the Conductor??)
         * Set the BPM?? (Again this is controlled by the Conductor).
         */
    }

    
    private void SetBackground(GameObject background)
    {
        var backgroundMaterial = background.GetComponent<Renderer>().material;
        this.background.GetComponent<Renderer>().material = backgroundMaterial;
    }
    private void SetBlockPiece(GameBlockPiece blockPiece)
    {
        var thingsThatUseBlocks = GetComponentsInChildren<IUsesBlocks>();
        thingsThatUseBlocks.ToList().ForEach(thing => thing.SetBlock(blockPiece));
    }
    private void SetHighlightedBlockPiece(HighlightedSquare highlightedSquareInfo)
    {
        var thingsThatHaveHighlightedSquares = GetComponentsInChildren<IUsesHighlightedSquares>();
        thingsThatHaveHighlightedSquares.ToList().ForEach(thing => thing.SetHighlightedSquare(highlightedSquareInfo));
    }
    /*TODO - Figure out how we should set the Music and BPM?*/
    private void SetBeatNumbers(GameObject beatNumbers)
    {
        Destroy(_beatNumbers.gameObject);
        _beatNumbers = Instantiate(beatNumbers);
        _beatNumbers.transform.SetParent(this.transform);
    }

    private void SetScoreBoard(GameObject scoreBoard)
    {
        if (_scoreBoard!=null)
        Destroy(_scoreBoard.gameObject);        
        _scoreBoard = Instantiate(scoreBoard);
        var previousPosition = _scoreBoard.transform.localPosition;   



        _scoreBoard.transform.SetParent(this.transform);
        _scoreBoard.transform.localPosition = previousPosition;
    }

    private void SetTimeLinePosition()
    {
        timeLine.transform.localPosition = new Vector3(gameController.luminesGame.TimeLinePosition * gameController.luminesGame.Width, timeLine.transform.localPosition.y, timeLine.transform.localPosition.z);
    }

    private void SetBlockDropPreviewPosition()
    {
        dropPreview.transform.localPosition = new Vector3(gameController.luminesGame.CurrentBlock.X + 1, dropPreview.transform.localPosition.y, dropPreview.transform.localPosition.z);
    }


}
