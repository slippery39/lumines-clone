using UnityEngine;

using GameLogic;
using System.Collections.Generic;
using Game_Logic;

public class GameCellsController : MonoBehaviour {

    [SerializeField]
    private float cellSize = 1.0f;

    [SerializeField]
    private GameBlockPiece blockPiecePrefab;
    [SerializeField]
    private GameBlockPiece[,] createdBlockPieces;
    [SerializeField]
    private GameBlock gameBlockPrefab;


    [SerializeField]
    private HighlightedSquare highlightedSquarePrefab;

    [SerializeField]
    private HighlightedSquare[,] createdSquares;

    [SerializeField]
    private GameBlock currentBlock;

    [SerializeField]
    private GameObject[,] createdCells;

    [SerializeField]
    private Game luminesGame;

    private void Update()
    {
        UpdateBoard();
    }

    public void Initialize(Game luminesGame)
    {
        if (blockPiecePrefab == null)
        {
            throw new System.Exception("Remember to set the block piece prefab in the GameGrid");
        }

        if (gameBlockPrefab == null)
        {
            throw new System.Exception("Remember to set the block prefab in the game grid");
        }
        if (luminesGame == null)
        {
            throw new System.Exception("The Lumines game is undefined in the grid, make sure it is programmed so that the grid is built after the game is created.");
        }
        this.EnsureInitialized(highlightedSquarePrefab);

        this.luminesGame = luminesGame;

        InstantiateCells();
        InstantiateCurrentBlock();
        InstantiateSquares();
    }

    private void InstantiateCells()
    {

        int width = luminesGame.Width;
        int height = luminesGame.Height;

        createdCells = new GameObject[width, height];
        createdBlockPieces = new GameBlockPiece[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = CreateCell(x, y);
                createdCells[x, y] = cell;
                createdBlockPieces[x, y] = CreateBlock(x, y, cell);
            }
        }
    }

    private void InstantiateCurrentBlock()
    {
        //instantiate the game block 
        currentBlock = Instantiate(gameBlockPrefab);
        currentBlock.name = "Current Block";
        currentBlock.transform.parent = this.transform;
    }

    private void UpdateBoard()
    {
        int width = luminesGame.Width;
        int height = luminesGame.Height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (luminesGame.timeLineMarked[x, y] && luminesGame.Board[x,y]!=0)
                {
                    createdBlockPieces[x, y].BlockType = BlockTypes.DeletionInProgress;
                    continue;
                }
                if (luminesGame.timeLineMarked[x,y] && luminesGame.Board[x, y] == 0)
                {

                    Debug.LogError($"Error found with our game at {x},{y}");
                }
                createdBlockPieces[x, y].BlockType = luminesGame.Deletions[x, y] ? (BlockTypes)luminesGame.Board[x, y] + 2 : (BlockTypes)luminesGame.Board[x, y];
                
            }
        }
        UpdateSquares();
        UpdateCurrentBlock();
    }


    private void InstantiateSquares()
    {
        int width = luminesGame.Width;
        int height = luminesGame.Height;

        createdSquares = new HighlightedSquare[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                createdSquares[x, y] = Instantiate(highlightedSquarePrefab);
                createdSquares[x, y].transform.SetParent(createdCells[x, y].transform);
                createdSquares[x, y].transform.localPosition = new Vector3(1, 1, -0.01f + ( -0.001f * y + (-0.001f * x) ) );
                createdSquares[x, y].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSquares() { 

        var squares = luminesGame.GetAllSquares();

        int width = luminesGame.Width;
        int height = luminesGame.Height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                createdSquares[x, y].gameObject.SetActive(false);
            }
        }


        squares.ForEach(square =>
        {
            createdSquares[square.X, square.Y].gameObject.SetActive(true);
        });
    }




    private void UpdateCurrentBlock()
    {
        currentBlock.transform.localPosition = new Vector3(0.5f + luminesGame.CurrentBlock.X * cellSize, -0.5f + luminesGame.CurrentBlock.Y * cellSize, 0);
        currentBlock.GetComponent<GameBlock>().SetColors(luminesGame.CurrentBlock.Data);
    }

    private GameBlockPiece CreateBlock(int x, int y, GameObject cell)
    {
        var block = Instantiate(blockPiecePrefab.gameObject);
        block.transform.parent = cell.transform;
        block.transform.localPosition = new Vector3(cellSize / 2, cellSize / 2, 0);
        block.GetComponent<GameBlockPiece>().BlockType = (BlockTypes)luminesGame.Board[x, y];
        block.name = "Game Block";

        return block.GetComponent<GameBlockPiece>();
    }

    GameObject CreateCell(int x, int y)
    {
        var cell = new GameObject();
        cell.name = $"Cell {x},{y}";
        cell.transform.parent = this.transform;

        cell.transform.localPosition = new Vector3(x * cellSize, y * cellSize, 0);
        return cell;
    }



}
