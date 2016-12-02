using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    public static class GameMap
    {
        public static int[,] gameMap =
        { // 1이면 벽돌 보이고, 0이면 안보인다.
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,0,0,0,1,1,1,1},
        };

        public static int brickNum = 80; // 블록 남은 개수
    }
}
