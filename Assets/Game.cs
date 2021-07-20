using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Game
{
    public class Game
    {
        int[,] board;

        public int[,] Board { get { return board; } }

        public int Width { get { return width; } }
        int width = 16;
        public int Height { get { return height; } }
        int height = 10;
        public Game()
        {
            board = new int[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i, j] = UnityEngine.Random.RandomRange(0, 3);
                }
            }
        }


        public void Tick()
        {
            //more all blocks down one unit.
            for (var x = 0; x < Width; x++)
            {
                for (var y = 1; y < Height; y++)
                {
                    if (board[x,y-1] == 0)
                    {
                        board[x, y - 1] = board[x, y];
                        board[x, y] = 0;
                    }
                }
            }

        }


    }
}
