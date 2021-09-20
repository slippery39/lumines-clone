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

        public List<Square> SquaresDeleted { get; set; }
        //These are used for the Next Blocks Preview.
        public List<MoveableBlock> PreviousUpcomingBlocks { get; set; }
        public List<MoveableBlock> UpcomingBlocks { get; set; }
        public GameEventInfo()
        {

        }
    }
}
