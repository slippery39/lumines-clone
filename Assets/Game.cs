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
        private int[,] board;      
        public int[,] Board { get { return board; } }
        private bool[,] markedForDeletion;
        public bool[,] Deletions { get { return markedForDeletion; } }
        public int Width { get { return width; } }
        int width = 16;
        public int Height { get { return height; } }
        int height = 10;

        private int[] currentBlock;
        public int[] CurrentBlock { get { return currentBlock; } }
        public Game()
        {
            board = new int[width, height];
            markedForDeletion = new bool[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i, j] = UnityEngine.Random.RandomRange(0, 3);
                    markedForDeletion[i, j] = false;
                }
            }

            currentBlock = CreateRandomBlock();
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

            MarkDeletions();
        }

        public int[] CreateRandomBlock()
        {
            int blockSize = 4;
            int[] block = new int[4];

            for (int i = 0; i < blockSize; i++)
            {
                block[i] = UnityEngine.Random.Range(1, 3); 
            }
            return block;
        }

        public void MarkDeletions()
        {
            //Remove all blocks that are "squares"
            //A block can be a part of multiple squares

            for (var x = 0; x < Width - 1; x++)
            {
                for (var y = 0; y < Height - 1; y++)
                {
                    markedForDeletion[x, y] = false;
                }
            }

            for (var x = 0; x < Width-1; x++)
            {
                for (var y = 0; y < Height-1; y++)
                {
                    if (CheckSquare(x, y))
                    {
                        markedForDeletion[x, y] = true;
                        markedForDeletion[x, y + 1] = true;
                        markedForDeletion[x + 1, y] = true;
                        markedForDeletion[x+1 , y + 1] = true;
                    }
                  }
            }
        }

        public void DeleteMarkedSquares()
        {
            for (var x= 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (markedForDeletion[x, y])
                    {
                        DeleteSquare(x, y);
                    }
                }
            }
        }

        private void DeleteSquare(int x, int y)
        {
            markedForDeletion[x, y] = false;
            board[x, y] = 0;
        }


        private bool CheckSquare(int x,int y)
        {

            bool checkColor1 = board[x, y] == 1 && board[x, y + 1] == 1 && board[x + 1, y] == 1 && board[x + 1, y + 1] == 1;
            bool checkColor2 = board[x, y] == 2 && board[x, y + 1] == 2 && board[x + 1, y] == 2 && board[x + 1, y + 1] == 2;

            return checkColor1 || checkColor2;           
            
        }


    }
}
