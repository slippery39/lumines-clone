using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    int width = 16;
    int height = 10;

    [SerializeField]
    GameObject cellPrefab;

    // Start is called before the first frame update
    void Start()
    {

        if (cellPrefab == null)
        {
            throw new System.Exception("Grid prefab was not defined for the board");

        }
        for (int row = 0; row < width; row++)
        {
            for (int col = 0; col < height; col++)
            {
                var cell= Instantiate(cellPrefab);
                cell.name = $"Grid Cell {row},{col}";
                cell.transform.position = new Vector3(row *2, col * 2, 0);
                cell.transform.parent = this.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
