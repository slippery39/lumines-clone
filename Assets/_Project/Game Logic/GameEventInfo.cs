using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Logic
{
    public class GameEventInfo
    {
        public List<Square> CurrentSquares { get; set; }

        public List<MoveableBlock> previousNextBlocks { get; set; }
        public List<MoveableBlock> nextBlocks { get; set; }
        public GameEventInfo()
        {

        }
    }
}
