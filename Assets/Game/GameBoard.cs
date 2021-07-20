using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Game
{
    [System.Serializable]
    public class GameBoard
    {
        private int width = 0;
        private int height = 0;
        private int cellSize = 2;
        private int[,] gridArray;
        public Shader shader;
        public GameBoard(int width,int height)
        {
            this.width = width;
            this.height = height;
            gridArray = new int[width,height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    var x1 = x * cellSize;
                    var x2 = (x * cellSize) + cellSize;
                    var y1 = y * cellSize;
                    var y2 = (y * cellSize + cellSize);
                    DrawCell(x1, x2, y1, y2, Color.green);
                }            
            }
        }


        void DrawCell(int x1,int x2,int y1, int y2, Color color)
        {
            DrawLine(new Vector3(x1, y1, 0), new Vector3(x2, y1, 0), color);
            //Horizonal Line Bottom
            DrawLine(new Vector3(x1, y2, 0), new Vector3(x2, y2, 0),color);
            //Vertical Line Top
            DrawLine(new Vector3(x1, y1, 0), new Vector3(x1, y2, 0), color);
            //Vertical Line Bottom
            DrawLine(new Vector3(x2, y1, 0), new Vector3(x2, y2, 0),color);
        }


        void DrawLine(Vector3 start, Vector3 end, Color color)
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
        }
    }


}
