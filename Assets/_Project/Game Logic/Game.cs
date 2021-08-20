using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using System.IO;

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

        public bool[,] timeLineMarked;



        private int bufferedHeight = 3; //height at the top of the game grid where the movable blocks will spawn.


        public event Action OnBlockPlaced;



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
                TimeLineMark(nextGridPos);
                TimeLineCheckDeletions(nextGridPos);
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

            //TODO - Need to check for collisions right;
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

            //TODO - Need to check for collisions right;
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

            OnBlockPlaced?.Invoke();

            currentBlock = CreateMoveableBlock();
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

        public void TimeLineMark(int x)
        {
           for (var y = 0; y < Height; y++)
            {
                if (markedForDeletion[x, y])
                {
                    timeLineMarked[x, y] = true;
                }
            }
        }

        private string Arr2dToString<T>(T[,] arr)
        {
            var result = string.Empty;
            var maxX = arr.GetLength(0);
            var maxY = arr.GetLength(1);
            for (var y = 0; y < maxY; y++)
            {
                result += "{";
                for (var x = 0; x < maxX; x++)
                {
                    result += $"{arr[x, y]},";
                }

                result += "}" + Environment.NewLine;
            }


            return result;
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
                x = x - 1;
            }

            if (x < 0)
            {
                return;
            }


            for (var y = 0; y < Height; y++)
            {
                if (timeLineMarked[x, y])
                {
                    var value = board[x, y];
                    var visited = new Dictionary<string, int>();

                    //there are 4 squares this could be a part of here. top left, top right, bottom left, bottom right
                    //should only be checking to the right of the time line.
                    var topLeft = new Vector2Int(x - 1, y + 1);
                    var topRight = new Vector2Int(x, y + 1);
                    var bottomLeft = new Vector2Int(x, y - 1);
                    var bottomRight = new Vector2Int(x + 1, y - 1);

                    var touchingSquares =
                         FindTouchingSquares(new Vector2Int(x, y), value, visited)
                        .Concat(FindTouchingSquares(topLeft, value, visited))
                        .Concat(FindTouchingSquares(topRight, value, visited))
                        .Concat(FindTouchingSquares(bottomLeft, value, visited))
                        .Concat(FindTouchingSquares(bottomRight, value, visited))
                        .ToList();

                    if (touchingSquares.Count == 0)
                    {
                        //this should never happen, once something has been marked by the timeline, there should always
                        //be something to delete.
                        Debug.LogWarning("no touching squares found even though timeline marked an area for " + x + "," + y);
                        Debug.LogWarning($"value :{value} markedForDeletion: {markedForDeletion[x, y]}");



                        ThrottledLogger.LogToFile($"TouchingSquareErr-[{x}{y}]",
                            $"{x},{y}\r\n" +
                            $"Value : {board[x,y]} \r\n"+
                            $"Marked in TimeLine? {timeLineMarked[x, y]} \r\n" +
                            $"Marked for Deletion? {markedForDeletion[x, y]} \r\n" +
                            " ----Board----- \r\n\r\n" +
                            Arr2dToString(board) + Environment.NewLine +
                            "---Deletions----\r\n\r\n" +
                            Arr2dToString(markedForDeletion) + Environment.NewLine +
                            "--TimeLine Marked---\r\n\r\n" +
                            Arr2dToString(timeLineMarked));

                            

                            

                        continue;
                        //todo possibly download board states when this happens.
                    }


                   

                    var maxSquareX = touchingSquares.Max(sq => sq.x);               

                    if (x > maxSquareX)
                    {
                        touchingSquares.ForEach(sq =>
                        {
                            var xx = sq.x;
                            var yy = sq.y;

                            //we actually have to check for each part, since we only want to delete things that the timeline 
                            //has marked.

                            var coords = new List<Vector2Int>
                            {
                                new Vector2Int(xx,yy),
                                new Vector2Int(xx+1,yy),
                                new Vector2Int(xx,yy-1),
                                new Vector2Int(xx+1,yy-1)
                            };

                            coords.ForEach(coord =>
                            {
                                if (timeLineMarked[coord.x, coord.y])
                                {
                                    board[coord.x, coord.y] = 0;
                                    markedForDeletion[coord.x, coord.y] = false;
                                    timeLineMarked[coord.x, coord.y] = false;
                                }
                                else
                                {
                                    markedForDeletion[coord.x, coord.y] = false;                                    
                                }
                            });
                        });
                    }
                    //go back and check all the other parts of the square that were marked.
                }
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

                        //todo - add the square to the list, any other squares that touch this one

                        //timeline moves over a deletion marked piece - we mark it.
                        //once the timeline completely moves over the square, we delete the whole square
                        //but this also includes multiple squares

                        //so we need to make some sort of graph of squares. 

                        //


                    }
                }
            }
        }

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


        private bool CheckSquare(int x, int y)
        {

            //Should not count pieces that are in free fall on the right;

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
