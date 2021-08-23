using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameBoardController : MonoBehaviour
{

    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private GameObject grid;
    [SerializeField]
    private GameObject cells;
    [SerializeField]
    private GameObject timeLine;

    [SerializeField]
    private GameObject dropPreview;

    [SerializeField]
    private NextBlocks upcomingBlocks;
    // Start is called before the first frame update

    [SerializeField]
    private Renderer gridRenderer;


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


        gridRenderer = grid.GetComponent<Renderer>();

        if (gridRenderer == null)
        {
            throw new System.Exception("Could not find the grid renderer on the grid game object");
        }

        cells.GetComponent<GameCellsController>().Initialize(gameController.luminesGame);      
        
    }

    // Update is called once per frame
    void Update()
    {
        SetTimeLinePosition();
        SetUpcomingBlocks();
        SetBlockDropPreviewPosition();
    }

    private void SetTimeLinePosition()
    {
        timeLine.transform.localPosition = new Vector3(gameController.luminesGame.TimeLinePosition * gameController.luminesGame.Width,timeLine.transform.localPosition.y,timeLine.transform.localPosition.z);
    }

    private void SetUpcomingBlocks()
    {
        upcomingBlocks.nextBlocks = gameController.luminesGame.UpcomingBlocks.ToList();
    }

    private void SetBlockDropPreviewPosition()
    {
        dropPreview.transform.localPosition = new Vector3(gameController.luminesGame.CurrentBlock.X + 1, dropPreview.transform.localPosition.y, dropPreview.transform.localPosition.z);
    }

    public void SetBPM(float bpm)
    {
        if (gridRenderer == null)
        {
            return;
        }
        //testing the BPM setting on our material;
        gridRenderer.material.SetFloat("_BPM", bpm);
    }
}
