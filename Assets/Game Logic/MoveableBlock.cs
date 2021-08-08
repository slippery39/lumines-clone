using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameLogic
{ 
   
    public  class MoveableBlock
    {
        public int X { get; set; }
        public int Y { get; set; }

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

            block.data[0] = UnityEngine.Random.Range(1, 3);
            block.data[1] = UnityEngine.Random.Range(1, 3);
            block.data[2] = UnityEngine.Random.Range(1, 3);
            block.data[3] = UnityEngine.Random.Range(1, 3);

            return block;
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
