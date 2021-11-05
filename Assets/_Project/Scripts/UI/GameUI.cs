using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameUI : MonoBehaviour
{

    [Header("Game State")]
    [SerializeField]
    private GameController gameController;

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
    private NextBlocks upcomingBlocks;
    // Start is called before the first frame update
    [Header("Scoring")]
    [SerializeField]
    private ScoreBoard scoreBoard;
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
             scoreBoard.AnimateScore(amount);
         });

        gameController.OnScoreMultiplierIncrease( (int amount) =>
        {
            scoreMultiplierNotification.SetMultiplier(amount);
            scoreMultiplierNotification.PlayAnimation();
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

        scoreBoard.CurrentTime = gameController.CurrentTime;
        scoreBoard.BlocksErased = gameController.erasedBlocksCount;
        scoreBoard.Score = gameController.score;

        
    }

    public void SetSkin(Skin skin)
    {

        SetBackground(skin.Background);
        SetBlockPiece(skin.BlockPiece);
        /*
         * Set the Background
         * Set the Block Piece
         * Set the Highlighted Block Piece
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
        //Setting the BlockPiece on the Cells Object
        cells.BlockPiecePrefab.Material1 = blockPiece.Material1;
        cells.BlockPiecePrefab.Material2 = blockPiece.Material2;
        cells.CreatedBlockPieces.ForEach((x, y, block) =>
        {
            if (block == null) { return; }
            block.Material1 = blockPiece.Material1;
            block.Material2 = blockPiece.Material2;
        });

        //Set the Block PIeces of the CUrrent Block.
        cells.SetCurrentBlockPieces(blockPiece);
        //Set the Block Piece of the Upcoming Blocks;
        upcomingBlocks.SetBlockPiece(blockPiece);
       //Need to set the BlockPiece on the UpcomingBlocks object and the CurrentBlock
    }
    private void SetHighlightedBlockPiece()
    {

    }
    /*TODO - Figure out how we should set the Music and BPM?*/

    private void SetTimeLinePosition()
    {
        timeLine.transform.localPosition = new Vector3(gameController.luminesGame.TimeLinePosition * gameController.luminesGame.Width, timeLine.transform.localPosition.y, timeLine.transform.localPosition.z);
    }

    private void SetBlockDropPreviewPosition()
    {
        dropPreview.transform.localPosition = new Vector3(gameController.luminesGame.CurrentBlock.X + 1, dropPreview.transform.localPosition.y, dropPreview.transform.localPosition.z);
    }


}
