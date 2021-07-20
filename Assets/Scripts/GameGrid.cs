using UnityEngine;


public class GameGrid : MonoBehaviour {

    [SerializeField]
    private float cellSize = 1.5f;

    [SerializeField]
    private GameBlock blockPrefab;

    [SerializeField]
    private GameBlock[,] createdBlocks;

    [SerializeField]
    private GameObject[,] createdCells;

    [SerializeField]
    private Game.Game luminesGame;

    // Start is called before the first frame update
    void Awake()
    {

        luminesGame = new Game.Game();

        if (blockPrefab == null)
        {
            throw new System.Exception("Remember to set the block prefab in the GameGrid");
        }

        int width = luminesGame.Width;
        int height = luminesGame.Height;

        createdCells = new GameObject[width, height];
        createdBlocks = new GameBlock[width, height];
  
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = CreateCell(x,y, Color.green);
                createdCells[x, y] = cell;
                createdBlocks[x,y]=CreateBlock(x, y, cell,luminesGame.Board[x,y]);                
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            luminesGame.Tick();
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
                createdBlocks[x, y].BlockType = luminesGame.Board[x, y];
            }
        }
    }

    private GameBlock CreateBlock(int x, int y, GameObject cell,int value)
    {
        var block = Instantiate(blockPrefab.gameObject);
        block.transform.parent = cell.transform;
        block.transform.localPosition = new Vector3(cellSize / 2, cellSize / 2, 0);
        block.GetComponent<GameBlock>().BlockType = luminesGame.Board[x, y];
        block.name = "Game Block";

        return block.GetComponent<GameBlock>();
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
