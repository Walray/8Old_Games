using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    public static class GlobalData
    {
        // 여러가지 콘텐츠 사이즈들 설정
        public static Vector2 windowSize { get { return new Vector2(800, 480); } }
        public static Vector2 padSize { get { return new Vector2(104, 26); } }
        public static Vector2 padPosition { get { return new Vector2(300, 400); } }
        public static Vector2 padSpeed { get { return new Vector2(0, 20); } }
        public static Vector2 brickSize { get { return new Vector2(40, 40); } }
        public static Vector2 ballSize { get { return new Vector2(32, 32); } }
        public static Vector2 ballSpeed { get { return new Vector2(3, 1); } }
        public static Vector2 ballPosition { get { return new Vector2(20, 20); } }

        // 행열 설정
        public const int rows = 4;
        public const int cols = 15;

        public const int maxTime = 60; // 시간은 60초로 설정
        public static float gravity = 0.06F;
        public static float err = 15F; // 충돌 탐지
    }
}
