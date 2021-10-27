using System;
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

        //We are experimenting with this IGrid interface internally for this class and if its good enough we will expand it and use it instead of a 2d Array externally as well.
        private IGrid<int> _board;        
        public int[,] Board { get => _board.To2dArray(); }
        private IGrid<bool> _markedForDeletion;
        public bool[,] Deletions { get => _markedForDeletion.To2dArray();  }
        public int Width { get { return _width; } }
        private int _width = 16;
        public int Height { get { return _height; } }
        private int _height = 10;

        private MoveableBlock _currentBlock;
        public MoveableBlock CurrentBlock { get =>  _currentBlock;  }

        private Queue<MoveableBlock> _upcomingBlocks = new Queue<MoveableBlock>();
        public Queue<MoveableBlock> UpcomingBlocks { get =>  _upcomingBlocks;  }

        private IGrid<bool> _timelineMarked;
        public bool[,] TimelineMarked { get => _timelineMarked.To2dArray(); }



        public event Action<GameEventInfo> OnBlockPlaced;
        public event Action<GameEventInfo> OnNewBlock;
        public event Action<GameEventInfo> OnDeletion;
        public event Action<GameEventInfo> OnSoftDrop;
        public event Action<GameEventInfo> OnTimeLineEnd;

        private float _timeLinePosition = 0.0f;
        public float TimeLinePosition { get =>  _timeLinePosition;  }



        private int _squaresDeletedThisTurn;

        private Dictionary<string, int> _markedSquares = new Dictionary<string, int>();
        public Game()
        {
            _board = new Grid<int>(_width, _height, (x, y) => { return 0; });
            _markedForDeletion = new Grid<bool>(_width, _height, (x, y) => { return false; });
            _timelineMarked = new Grid<bool>(_width, _height, (x, y) =>{ return false; });


            _currentBlock = CreateMoveableBlock();
            //store a list of the next blocks

            _upcomingBlocks.Enqueue(CreateMoveableBlock());
            _upcomingBlocks.Enqueue(CreateMoveableBlock());
            _upcomingBlocks.Enqueue(CreateMoveableBlock());
        }

        //Client code will control the timeline movement.
        public void MoveTimeLine(float normalizedAmt)
        {
            //move by the normalized amount
            //check to see if we would have passed a grid position
            //(i.e. TimeLineMark and TimeLineCheckDeletions2 will need to have a value inputted)

            //check to see if we passed a grid boundary.
            int currentGridPos = Mathf.FloorToInt(_timeLinePosition * Width);
            _timeLinePosition += normalizedAmt;

            bool reachedEnd = false;

            if (_timeLinePosition > 1)
            {
                reachedEnd = true;
            }

            _timeLinePosition = Mathf.Repeat(_timeLinePosition, 1);

            int nextGridPos = Mathf.FloorToInt(_timeLinePosition * Width);            

            if ( (nextGridPos > currentGridPos) || (nextGridPos == 0 && currentGridPos!=0) )
            {
                TimeLineCheckDeletions(nextGridPos);
                MarkByTimeLine(nextGridPos);   
                
                if (reachedEnd)
                {
                    OnTimeLineEnd?.Invoke(new GameEventInfo() { SquaresDeletedThisTurn = _squaresDeletedThisTurn });
                    _squaresDeletedThisTurn = 0;
                }
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
                    if (_board[x, y - 1] == 0 && !_timelineMarked[x,y] )
                    {
                        _board[x, y - 1] = _board[x, y];
                        _board[x, y] = 0;               
                     
                        
                    }
                  }
            }
        }

        public void MoveLeft()
        {
            if (_currentBlock.WillCollideLeft(_board))
            {
                return;
            }

            _currentBlock.X = Math.Max(_currentBlock.X - 1, 0);
        }

        public void MoveRight()
        {

            if (_currentBlock.WillCollideRight(_board))
            {
                return;
            }

            _currentBlock.X = Math.Min(Width - 2, _currentBlock.X + 1);
        }

        public void Gravity()
        {
            MoveDown();
        }

        public void SoftDrop()
        {
            MoveDown();            
            OnSoftDrop?.Invoke( new GameEventInfo() );
        }

        //Client code will either use Gravity() or SoftDrop().
        //Gravity is for the automatic nove down
        //Soft Drop is for when the user presses a key or whatever to manually move the block down.
        private void MoveDown()
        {
            //Check to see if it would collide with any blocks.
            if (_currentBlock.WillCollideBelow(_board))
            {
                SetToBoard();
                return;
            }
            _currentBlock.Y--;
        }

        private void SetToBoard()
        {
            /*
             * //order is top left, top right, bottom left, bottom right
             */

            //Game is over, need to return here or else we will get array out of bounds errors.
            if (CurrentBlock.Y >= Height)
            {
                Debug.LogWarning("GAME OVER");
                return;
            }
 
            //order is top left, top right, bottom left, bottom right
            _board[CurrentBlock.X, CurrentBlock.Y] = CurrentBlock.Data[0];
            _board[CurrentBlock.X + 1, CurrentBlock.Y] = CurrentBlock.Data[1];
            _board[CurrentBlock.X, CurrentBlock.Y - 1] = CurrentBlock.Data[2];
            _board[CurrentBlock.X + 1, CurrentBlock.Y - 1] = CurrentBlock.Data[3];


            var info = new GameEventInfo();


            //TODO - pass information to the OnBlockPlaced

            info.PreviousUpcomingBlocks = this._upcomingBlocks.ToList();            
            _currentBlock = _upcomingBlocks.Dequeue();
            _upcomingBlocks.Enqueue(CreateMoveableBlock());
            info.UpcomingBlocks = this._upcomingBlocks.ToList();
            //TODO - get a list of all squares.
  

            OnBlockPlaced?.Invoke(info);
            OnNewBlock?.Invoke(info);
        }

        private MoveableBlock CreateMoveableBlock()
        {
            int startingHeightAboveGrid = 3;
            var block = MoveableBlock.CreateRandom();
            block.X = (int)Math.Floor(_width / 2.0);
            block.Y = Height + startingHeightAboveGrid - 1;

            return block;

        }


        //Checks if a cell on the grid should have gravity applied to it.
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

            if (_timelineMarked[x, y])
            {
                return false;
            }

            if (_board[x, y] > 0)
            {
                for (var yy = y; yy >= 0; yy--)
                {
                    if (_timelineMarked[x, yy])
                    {
                        continue;
                    }
                    if (_board[x, yy] == 0)
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
                if (_markedForDeletion[x, y])
                {
                    _timelineMarked[x, y] = true;
                }
            }
        }

        private int GetAmountToDelete(int columnIndex)
        {
            int amount = 0;

            for (var y = 0; y < Height; y++)
            {
                if (_markedForDeletion[columnIndex, y])
                {
                    amount++;   
                }
            }
            return amount;
        }

        private void ForEachCell(Action<int,int> func)
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
            _board.Delete(x, y);
            _markedForDeletion.Delete(x, y);
            _timelineMarked.Delete(x, y);
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
                //Delete all marked cells. Note that this currently runs even if there is nothing marked by the time line.
                //Perhaps have an early exit for that case.
                bool squareListProcessed = false;
                ForEachCell((xx, yy) =>
                {

                    

                    //not sure if we need this xx<=x.
                    if (xx<= x && _timelineMarked[xx,yy])
                    {
                        //We only want this to run once or else the amount of squares will change b
                        if (!squareListProcessed)
                        {
                            var squares = GetAllTimeLineMarkedSquares();
                            _squaresDeletedThisTurn += squares.Count;
                            squareListProcessed = true;
                            this.OnDeletion?.Invoke(new GameEventInfo {SquaresDeletedThisTurn = _squaresDeletedThisTurn, SquaresDeleted = squares,Board = (int[,])Board.Clone()});
                        }
                        DeleteCell(xx, yy);
                    }
                });
            }
        }

    

        public void MarkDeletions()
        {
            //Remove all blocks that are "squares"
            //A block can be a part of multiple squares
            _markedForDeletion.ClearAll();
            _markedSquares.Clear();

            for (var x = 0; x < Width - 1; x++)
            {
                for (var y = 0; y < Height - 1; y++)
                {
                    if (!IsInFreeFall(x, y) && CheckIfSquare(x, y))
                    {
                        _markedForDeletion[x, y] = true;
                        _markedForDeletion[x, y + 1] = true;
                        _markedForDeletion[x + 1, y] = true;
                        _markedForDeletion[x + 1, y + 1] = true;

                        _markedSquares.Add(new Vector2Int(x, y + 1).ToString(), _board[x, y+1]);
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


            if (!_markedSquares.ContainsKey(key))
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
            if (_markedSquares[key] == value)
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

        private List<Square> GetAllTimeLineMarkedSquares()
        {
            var squares = new List<Square>();

            ForEachCell((x, y) =>
            {
                if (CheckIfSquare(x, y) && _timelineMarked[x,y])
                {
                    squares.Add(new Square(x, y, this._board[x, y]));
                }
            });


            return squares;
        }

        public List<Square> GetAllSquares()
        {
            var squares = new List<Square>();

            ForEachCell((x, y) =>
            {
                if (CheckIfSquare(x, y))
                {
                    squares.Add(new Square(x, y, this._board[x, y]));
                }
            });


            return squares;
        }


        private bool CheckIfSquare(int x, int y)
        {

            //Should not count pieces that are in free fall on the right;

            if (x+1 >=Width || y+1 >=Height)
            {
                return false;
            }
            if (IsInFreeFall(x,y) || IsInFreeFall(x + 1, y) || IsInFreeFall(x + 1, y + 1))
            {
                return false;
            }


            bool checkColor1 = _board[x, y] == 1 && _board[x, y + 1] == 1 && _board[x + 1, y] == 1 && _board[x + 1, y + 1] == 1;
            bool checkColor2 = _board[x, y] == 2 && _board[x, y + 1] == 2 && _board[x + 1, y] == 2 && _board[x + 1, y + 1] == 2;

            return checkColor1 || checkColor2;

        }


    }
}
