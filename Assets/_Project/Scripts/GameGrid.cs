using UnityEngine;

using GameLogic;


public class GameGrid : MonoBehaviour {

    [SerializeField]
    private float cellSize = 1.0f;

    [SerializeField]
    private GameBlockPiece blockPiecePrefab;
    [SerializeField]
    private GameBlockPiece[,] createdBlockPieces;
    [SerializeField]
    private GameBlock gameBlockPrefab;

    [SerializeField]
    private GameBlock currentBlock;

    [SerializeField]
    private GameObject[,] createdCells;

    [SerializeField]
    private Color gridLineColors = Color.gray;

    [SerializeField]
    private Game luminesGame;





    // Start is called before the first frame update
    void Awake()
    {
  
    }

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

        this.luminesGame = luminesGame;

        int width = luminesGame.Width;
        int height = luminesGame.Height;

        createdCells = new GameObject[width, height];
        createdBlockPieces = new GameBlockPiece[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = CreateCell(x, y, gridLineColors);
                createdCells[x, y] = cell;
                createdBlockPieces[x, y] = CreateBlock(x, y, cell);
            }
        }

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
                createdBlockPieces[x, y].BlockType = luminesGame.Deletions[x, y] ? luminesGame.Board[x, y] + 2 : luminesGame.Board[x, y];
            }
        }

        UpdateCurrentBlock();
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
        block.GetComponent<GameBlockPiece>().BlockType = luminesGame.Board[x, y];
        block.name = "Game Block";

        return block.GetComponent<GameBlockPiece>();
    }

    GameObject CreateCell(int x, int y, Color color)
    {
        var cell = new GameObject();
        cell.name = $"Cell {x},{y}";
        cell.transform.parent = this.transform;

        cell.transform.localPosition = new Vector3(x * cellSize, y * cellSize, 0);
        return cell;
    }

}
