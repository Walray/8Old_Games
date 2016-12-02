using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    public class Brick : Objects
    {
        public int appear = 1; // 1이면 나타나고, 0이면 사라짐 => 이걸로 블럭 닿았는지 판별

        public Brick(Vector2 position, Vector2 size) : base(position, size)
        {
        }
    }
}
