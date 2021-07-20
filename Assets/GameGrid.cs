using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGrid : MonoBehaviour {

    [SerializeField]
    private float cellSize = 1.5f;

    [SerializeField]
    private GameObject blockPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        if (blockPrefab == null)
        {
            throw new System.Exception("Remember to set the block prefab in the GameGrid");
        }

        int width = 16;
        int height = 10;

  
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var x1 = x * cellSize;
                var x2 = (x * cellSize) + cellSize;
                var y1 = y * cellSize;
                var y2 = (y * cellSize + cellSize);
                var cell = DrawCell(x,y, Color.green);

                var block = Instantiate(blockPrefab);
                block.transform.parent = cell.transform;
                block.transform.localPosition= new Vector3(cellSize/2, cellSize/2, 0);
                block.name = "Game Block";
            }
        }
    }

    GameObject DrawCell(int x, int y, Color color)
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
