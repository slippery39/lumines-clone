using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameGrid : MonoBehaviour { 

    [SerializeField]
    private int cellSize = 2;

    // Start is called before the first frame update
    void Awake()
    {
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
                DrawCell(x,y, Color.green);
            }
        }


    }

    void DrawCell(int x, int y, Color color)
    {

        var x1 = x * cellSize;
        var x2 = (x * cellSize) + cellSize;
        var y1 = y * cellSize;
        var y2 = (y * cellSize + cellSize);
        var cell = new GameObject();
        cell.name = $"Cell {x},{y}";
        cell.transform.parent = this.transform;
        DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y1, 0), color).transform.parent = cell.transform;
        //Horizonal Line Bottom
        DrawLine(new Vector3(x1, y2, 0), new Vector3(x2, y2, 0), color).transform.parent = cell.transform;
        DrawLine(new Vector3(x1, y1, 0), new Vector3(x1, y2, 0), color).transform.parent = cell.transform;
        //Vertical Line Bottom
        DrawLine(new Vector3(x2, y1, 0), new Vector3(x2, y2, 0), color).transform.parent = cell.transform;
    }


    GameObject DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("GUI/Text Shader"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        return myLine;
    }

}
