using UnityEngine;


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
    private GameLogic.Game luminesGame;

    [SerializeField]
    private Color gridLineColors = Color.gray;


    private float currentTime = 0.0f;
    private float moveDownTime = 1.0f;
    private float nextMoveDownTime = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        luminesGame = new GameLogic.Game();

        if (blockPiecePrefab == null)
        {
            throw new System.Exception("Remember to set the block piece prefab in the GameGrid");
        }

        if (gameBlockPrefab == null)
        {
            throw new System.Exception("Remember to set the block prefab in the game grid");
        }

        int width = luminesGame.Width;
        int height = luminesGame.Height;

        createdCells = new GameObject[width, height];
        createdBlockPieces = new GameBlockPiece[width, height];
  
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = CreateCell(x,y, gridLineColors);
                createdCells[x, y] = cell;
                createdBlockPieces[x,y]=CreateBlock(x, y, cell,luminesGame.Board[x,y]);                
            }
        }

        //instantiate the game block 
        currentBlock = Instantiate(gameBlockPrefab);
        currentBlock.name = "Current Block";
        currentBlock.transform.parent = this.transform;     
    }

    private void Update()
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
        if (Time.frameCount % 4 ==0)
        {
            luminesGame.BoardGravity();
        }



        UpdateBoard();
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

    private GameBlockPiece CreateBlock(int x, int y, GameObject cell,int value)
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

        var x1 = 0;
        var x2 = cellSize;
        var y1 = 0;
        var y2 = cellSize;
        var cell = new GameObject();
        cell.name = $"Cell {x},{y}";
        cell.transform.parent = this.transform;

        cell.transform.localPosition = new Vector3(x * cellSize, y * cellSize, 0);

        var topLine = DrawLine(new Vector3(0, 0, 0), new Vector3(x2, y1, 0), color,cell);
          topLine.name = "Top Horizontal Line";
        //Horizonal Line Bottom
        var bottomLine = DrawLine(new Vector3(x1, y2, 0), new Vector3(x2, y2, 0), color,cell);
         bottomLine.name = "Bottom Horizontal Line";

        var leftLine = DrawLine(new Vector3(x1, y1, 0), new Vector3(x1, y2, 0), color,cell);
        leftLine.name = "Left Vertical Line";

        //Vertical Line Bottom
        var rightLine = DrawLine(new Vector3(x2, y1, 0), new Vector3(x2, y2, 0), color,cell);
        rightLine.name = "Right Vertical Line";

        return cell;
    }


    GameObject DrawLine(Vector3 start, Vector3 end, Color color,GameObject parent)
    {
        GameObject myLine = new GameObject();
        myLine.transform.parent = parent.transform;
        myLine.transform.localPosition = new Vector3(0, 0, 0);
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.material = new Material(Shader.Find("GUI/Text Shader"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        return myLine;
    }

}
