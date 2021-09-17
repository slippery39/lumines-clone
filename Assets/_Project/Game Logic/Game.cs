﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using System.IO;
using Game_Logic;

namespace GameLogic
{

    [System.Serializable]
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

        private Queue<MoveableBlock> nextBlocks = new Queue<MoveableBlock>();
        public Queue<MoveableBlock> UpcomingBlocks { get { return nextBlocks; } }


        public bool[,] timeLineMarked;



        private int bufferedHeight = 3; //height at the top of the game grid where the movable blocks will spawn.


        public event Action<GameEventInfo> OnBlockPlaced;
        public event Action<GameEventInfo> OnNewSquareFormed;
        public event Action<GameEventInfo> OnNewBlock;


        private float _timeLinePosition = 0.0f;
        public float TimeLinePosition { get { return _timeLinePosition; } }

        private Dictionary<string, int> markedSquares = new Dictionary<string, int>();
        public Game()
        {
            board = new int[width, height + bufferedHeight];
            markedForDeletion = new bool[width, height];
            timeLineMarked = new bool[width, height];


            //Below is temporary just for testing.
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    board[i, j] = 0;
                    markedForDeletion[i, j] = false;
                    timeLineMarked[i, j] = false;
                }
            }

            currentBlock = CreateMoveableBlock();
            //store a list of the next blocks

            nextBlocks.Enqueue(CreateMoveableBlock());
            nextBlocks.Enqueue(CreateMoveableBlock());
            nextBlocks.Enqueue(CreateMoveableBlock());
        }

        //temporary function to move our timeline, it should actually be synced to a beat.
        public void MoveTimeLine(float normalizedAmt)
        {

            //move by the normalized amount
            //check to see if we would have passed a grid position
            //(i.e. TimeLineMark and TimeLineCheckDeletions2 will need to have a value inputted)

            //check to see if we passed a grid boundary.
            int currentGridPos = Mathf.FloorToInt(_timeLinePosition * Width);
            _timeLinePosition += normalizedAmt;
            _timeLinePosition = Mathf.Repeat(_timeLinePosition, 1);

            int nextGridPos = Mathf.FloorToInt(_timeLinePosition * Width);            

            if ( (nextGridPos > currentGridPos) || (nextGridPos == 0 && currentGridPos!=0) )
            {
                TimeLineCheckDeletions(nextGridPos);
                MarkByTimeLine(nextGridPos);                
            }
        }


        public void BoardGravity()
        {
            //more all blocks down one unit.
            for (var x = 0; x < Width; x++)
            {
                for (var y = 1; y < Height; y++)
                {
                    //if a block has been marked by the timeline already then it should be unmoveable
                    if (board[x, y - 1] == 0 && !timeLineMarked[x,y] )
                    {
                        board[x, y - 1] = board[x, y];
                        board[x, y] = 0;  
                        
                        
                        
                    }
                  }
            }
            //TODO - Check for new squares formed.
        }

        public void MoveLeft()
        {
            if (currentBlock.X == 0)
            {
                return;
            }

            if (board[CurrentBlock.X - 1, CurrentBlock.Y] > 0 || board[CurrentBlock.X - 1, CurrentBlock.Y - 1] > 0)
            {
                return;
            }

            currentBlock.X = Math.Max(currentBlock.X - 1, 0);
        }

        public void MoveRight()
        {

            if (currentBlock.X == Width - 2)
            {
                return;
            }

            if (board[CurrentBlock.X + 2, CurrentBlock.Y] > 0 || board[CurrentBlock.X + 2, CurrentBlock.Y - 1] > 0)
            {
                return;
            }

            currentBlock.X = Math.Min(Width - 2, currentBlock.X + 1);
        }

        public void MoveDown()
        {
            //Check to see if it would collide with any blocks.

            if (currentBlock.Y - 2 < 0)
            {
                SetToBoard();
                return;
            }

            //Need to have a check in here in case it goes past the board.
            if (board[currentBlock.X, currentBlock.Y - 2] > 0 || board[currentBlock.X + 1, currentBlock.Y - 2] > 0) //TODO - what does this mean?
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

            if (CurrentBlock.Y >= Height)
            {
                Debug.LogWarning("GAME OVER");
                return;
            }

            board[CurrentBlock.X, CurrentBlock.Y] = CurrentBlock.Data[0];
            board[CurrentBlock.X + 1, CurrentBlock.Y] = CurrentBlock.Data[1];
            board[CurrentBlock.X, CurrentBlock.Y - 1] = CurrentBlock.Data[2];
            board[CurrentBlock.X + 1, CurrentBlock.Y - 1] = CurrentBlock.Data[3];


            var info = new GameEventInfo
            {
                CurrentSquares = GetAllSquares()
            };


            //TODO - pass information to the OnBlockPlaced

            info.previousNextBlocks = this.nextBlocks.ToList();            
            currentBlock = nextBlocks.Dequeue();
            nextBlocks.Enqueue(CreateMoveableBlock());
            info.nextBlocks = this.nextBlocks.ToList();

            OnBlockPlaced?.Invoke(info);
            OnNewBlock?.Invoke(info);
        }

        private MoveableBlock CreateMoveableBlock()
        {

            var block = MoveableBlock.CreateRandom();
            block.X = (int)Math.Floor(width / 2.0);
            block.Y = Height + bufferedHeight - 1;

            return block;

        }

        public bool IsInFreeFall(int x, int y)
        {

            if (x >=Width || y >= Height)
            {
                throw new Exception("Invalid arguments for IsInFreeFall. Outside the bounds : " + x + "," + y);
            }

            if (y == 0)
            {
                return false;
            }

            if (timeLineMarked[x, y])
            {
                return false;
            }

            if (board[x, y] > 0)
            {
                for (var yy = y; yy >= 0; yy--)
                {
                    if (timeLineMarked[x, yy])
                    {
                        continue;
                    }
                    if (board[x, yy] == 0)
                    {
                        return true; //will return true if any block underneath is 0;
                    }
                }
            }
            return false;
        }

        public void MarkByTimeLine(int x)
        {
           for (var y = 0; y < Height; y++)
            {
                if (markedForDeletion[x, y])
                {
                    timeLineMarked[x, y] = true;
                }
            }
        }

        private int GetAmountToDelete(int columnIndex)
        {
            int amount = 0;

            for (var y = 0; y < Height; y++)
            {
                if (markedForDeletion[columnIndex, y])
                {
                    amount++;   
                }
            }
            return amount;
        }

        private void VisitGrid(Action<int,int> func)
        {
            for (var x = 0;x< Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    func(x, y);
                }
            }
        }

        private void DeleteCell(int x, int y)
        {
            board[x, y] = 0;
            markedForDeletion[x, y] = false;
            timeLineMarked[x, y] = false;
        }

        public void TimeLineCheckDeletions(int x)
        {
             //We only want to delete squares after we have fully passed them,
            //to do this we will always check the previous column intead of the curret one.
            if (x == 0)
            {
                x = Width - 1;
            }
            else
            {
                x--;
            }

            if (x < 0)
            {
                return;
            }

            //New algorithm, check to see if the next column has any marked squares we should not delete if that is the case.
            var nextCol = (x + 1 == Width ? 0 : x + 1);

            if (nextCol!=0 && GetAmountToDelete(nextCol) > 0)
            {
                return;
            }
            else
            {
                VisitGrid((xx, yy) =>
                {
                    //not sure if we need this xx<=x.
                    if (xx<= x && timeLineMarked[xx,yy])
                    {
                        DeleteCell(xx, yy);
                    }
                });
            }
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

            markedSquares.Clear();

            for (var x = 0; x < Width - 1; x++)
            {
                for (var y = 0; y < Height - 1; y++)
                {
                    if (!IsInFreeFall(x, y) && CheckSquare(x, y))
                    {
                        markedForDeletion[x, y] = true;
                        markedForDeletion[x, y + 1] = true;
                        markedForDeletion[x + 1, y] = true;
                        markedForDeletion[x + 1, y + 1] = true;

                        markedSquares.Add(new Vector2Int(x, y + 1).ToString(), board[x, y+1]);
                    }
                }
            }
        }

        //not in use currently but could be useful in the future.
        private List<Vector2Int> FindTouchingSquares(Vector2Int coords, int value, Dictionary<string, int> visited)
        {
            var key = coords.ToString();
            //x,y,z,w


            if (visited == null)
            {
                visited = new Dictionary<string, int>();
            }

            if (visited.ContainsKey(key))
            {
                return new List<Vector2Int>();
            }

            visited.Add(key, value);


            if (!markedSquares.ContainsKey(key))
            {
                return new List<Vector2Int>();
            }

            var x = coords.x;
            var y = coords.y;

            var right = new Vector2Int(x + 1, y);
            var left = new Vector2Int(x - 1, y);
            var top = new Vector2Int(x, y + 1);
            var bottom = new Vector2Int(x, y - 1);

            //need to check diagonally as well
            var downLeft = new Vector2Int(x - 1, y - 1);
            var downRight = new Vector2Int(x + 1, y - 1);
            var upLeft = new Vector2Int(x - 1, y + 1);
            var upRight = new Vector2Int(x + 1, y + 1);
          


            //we should have a key here
            if (markedSquares[key] == value)
            {

                var squares = new List<Vector2Int> { coords }
                .Concat(FindTouchingSquares(left, value, visited))
                .Concat(FindTouchingSquares(right, value, visited))
                .Concat(FindTouchingSquares(top, value, visited))
                .Concat(FindTouchingSquares(bottom, value, visited))
                .Concat(FindTouchingSquares(downLeft, value, visited))
                .Concat(FindTouchingSquares(downRight, value, visited))
                .Concat(FindTouchingSquares(upLeft, value, visited))
                .Concat(FindTouchingSquares(upRight,value,visited))
                .ToList();

                return squares;
            }
            else
            {
                return new List<Vector2Int>();
            }

            //recursive function, keep checking until there is no other coords visited.
        }

        public void DeleteMarkedSquares()
        {
            for (var x = 0; x < Width; x++)
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

        private List<Square> GetAllSquares()
        {
            //we need to get he value.
            var squares = new List<Square>();

            VisitGrid((x, y) =>
            {
               if (CheckSquare(x, y))
                {
                    squares.Add(new Square(x, y, this.board[x, y]));
                }
            });


            return squares;

            //Use case 1 <-- do this one first.
            //be able in the ui to highlight with a white outline all the individual squares that are marked for deletion
            
            //A square can be determined by its Bottom Left coordinate.

            //Use case 2
            //WHen a block is placed, we should see a numer telling us how many squares are currently in the spot the block was placed.
            //Also included gravity placed blocks.


            //Starting from the bottom left
            //Go through the grid
            //Check if a square exists in that spot
            //if it does add it to the list.

        }


        private bool CheckSquare(int x, int y)
        {

            //Should not count pieces that are in free fall on the right;

            if (x+1 >=Width || y+1 >=Height)
            {
                return false;
            }
            if (IsInFreeFall(x + 1, y) || IsInFreeFall(x + 1, y + 1))
            {
                return false;
            }


            bool checkColor1 = board[x, y] == 1 && board[x, y + 1] == 1 && board[x + 1, y] == 1 && board[x + 1, y + 1] == 1;
            bool checkColor2 = board[x, y] == 2 && board[x, y + 1] == 2 && board[x + 1, y] == 2 && board[x + 1, y + 1] == 2;

            return checkColor1 || checkColor2;

        }


    }
}
