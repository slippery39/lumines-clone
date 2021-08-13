using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
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

        cells.GetComponent<GameGrid>().Initialize(gameController.luminesGame);
        
    }

    // Update is called once per frame
    void Update()
    {
        gameController.luminesGame.MoveTimeLine();
        SetTimeLinePosition();
    }

    private void SetTimeLinePosition()
    {
        timeLine.transform.localPosition = new Vector3(gameController.luminesGame.TimeLinePosition * gameController.luminesGame.Width,timeLine.transform.localPosition.y,timeLine.transform.localPosition.z);
    }
}
