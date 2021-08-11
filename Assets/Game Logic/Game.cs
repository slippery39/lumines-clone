using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace GameLogic
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

        private MoveableBlock currentBlock;        
        public MoveableBlock CurrentBlock { get { return currentBlock; } }

        private int bufferedHeight = 3; //height at the top of the game grid where the movable blocks will spawn.

        public Game()
        {
            board = new int[width, height+bufferedHeight];
            markedForDeletion = new bool[width, height];

            AutoFill2dArr(board, 0); 

            //Below is temporary just for testing.
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i, j] = UnityEngine.Random.RandomRange(0, 3);
                    markedForDeletion[i, j] = false;
                }
            }

            currentBlock = CreateMoveableBlock();
        }


        public void BoardGravity()
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

        private void AutoFill2dArr(int[,] arr,int value)
        {
            for (var i = 0; i < arr.GetUpperBound(0); i++)
            {
                for (var j = 0; j < arr.GetUpperBound(1); j++)
                {
                   arr[i, j] = value;
                }
            }
        }

        public void MoveLeft()
        {

            Debug.Log(CurrentBlock.Y);
            if (currentBlock.X == 0)
            {
                return;
            }

            if (board[CurrentBlock.X-1,CurrentBlock.Y]>0 || board[CurrentBlock.X-1, CurrentBlock.Y - 1] > 0)
            {
                return;
            }

            currentBlock.X = Math.Max(currentBlock.X - 1, 0);

            //TODO - Need to check for collisions right;
        }

        public void MoveRight()
        {

            if (currentBlock.X == Width - 2)
            {
                return;
            }

            if (board[CurrentBlock.X+2, CurrentBlock.Y] > 0 || board[CurrentBlock.X+2, CurrentBlock.Y - 1] > 0)
            {
                return;
            }

            currentBlock.X = Math.Min(Width - 2, currentBlock.X + 1);

            //TODO - Need to check for collisions right;
        }

        public void MoveDown()
        {
            //Check to see if it would collide with any blocks.

            if (currentBlock.Y-2 < 0)
            {
                SetToBoard();
                return;
            }

            //Need to have a check in here in case it goes past the board.
            if (board[currentBlock.X, currentBlock.Y - 2]>0 || board[currentBlock.X+1,currentBlock.Y-2]>0) //TODO - what does this mean?
            {
                SetToBoard();
                return;
                //TODO - collision detected, block should be baked into the board and gravity should start being applied.
            }

            currentBlock.Y--;
            
        }

        private void SetToBoard()
        {
            /*
             * //order is top left, top right, bottom left, bottom right
             */
            board[CurrentBlock.X, CurrentBlock.Y] = CurrentBlock.Data[0];
            board[CurrentBlock.X + 1, CurrentBlock.Y] = CurrentBlock.Data[1];
            board[CurrentBlock.X, CurrentBlock.Y - 1] = CurrentBlock.Data[2];
            board[CurrentBlock.X+1, CurrentBlock.Y - 1] = CurrentBlock.Data[3];
            currentBlock = CreateMoveableBlock();
        }

        private MoveableBlock CreateMoveableBlock()
        {

            var block = MoveableBlock.CreateRandom();
            block.X = (int)Math.Floor(width / 2.0);
            block.Y = Height + bufferedHeight - 1;

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
