using Game_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLogic
{ 
   [System.Serializable]
    public  class MoveableBlock
    {
        public int X { get; set; }
        public int Y { get; set; }

        //Note that the game grid height is 0 at the bottom and goes up vertically.
        public int LowerY { get => Y - 1; }
        public int RightX { get => X + 1; }

        [SerializeField]
        private int[] data = new int[4]; //order is top left, top right, bottom left, bottom right
        public int[] Data { get { return data; } }

        public MoveableBlock()
        {
            data[0] = 1;
            data[1] = 1;
            data[2] = 1;
            data[3] = 1;
        }

        

        public static MoveableBlock CreateRandom()
        {
            var block = new MoveableBlock();

            block.data[0] = GameRNG.RandomInt(1, 3);
            block.data[1] = GameRNG.RandomInt(1, 3);
            block.data[2] = GameRNG.RandomInt(1, 3);
            block.data[3] = GameRNG.RandomInt(1, 3);

            return block;
        }

        public bool WillCollideLeft(IGrid<int> _board)
        {
            //It is already on the left most boundary
            if (X == 0)
            {
                return true;
            }

            //The Moveable Block is not yet in the grid area
            if (Y > _board.Height)
            {
                return false;
            }
            //The moveable block is half inside the grid area, and half outside the grid area.
            if (Y == _board.Height)
            {
                return _board[X - 1, LowerY] > 0;
   
            }
            else
            {
                return _board[X - 1, Y] > 0 || _board[X - 1, LowerY] > 0;
            }
        }
        public bool WillCollideRight(IGrid<int> _board)
        {
            //We are already on the right most boundary
            if (RightX >= _board.Width - 1)
            {
                return true;
            }
            //The Moveable Block is not yet in the grid area
            if (Y > _board.Height)
            {
                return false;
            }
            //The moveable block is half inside the grid area, and half outside the grid area.
            if (Y == _board.Height)
            {
                return _board[X + 1, LowerY] > 0;

            }
            else
            {
                return _board[X + 1, Y] > 0 || _board[X + 1, LowerY] > 0;
            }
        }

        public bool WillCollideBelow(IGrid<int> _board)
        {
            //Need this check first so that we don't get any array out of bounds errors.
            if (Y > _board.Height)
            {
                return false;
            }
            //Has reached the very bottom of the grid.
            if (Y - 2 < 0) {
                return true;
            }
            //Is about to slam into a grid piece.
            if ( _board[X, Y - 2] > 0 || _board[X + 1, Y - 2] > 0)
            {
                return true;
            }

            return false;

        }

        public void RotateLeft()
        {
            var newData = new int[4];

            newData[0] = data[2];
            newData[1] = data[0];
            newData[2] = data[3];
            newData[3] = data[1];

            data = newData;
 
        }
        public void RotateRight()
        {
            var newData = new int[4];

            newData[0] = data[1];
            newData[1] = data[3];
            newData[2] = data[0];
            newData[3] = data[2];


            data = newData; 
        }

    }
}
