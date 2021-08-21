using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public static class GameRNG
    {

        private static Random rng = new Random();

        public static int RandomInt(int min, int max)
        {
            return rng.Next(min, max);
        }

    }
}
