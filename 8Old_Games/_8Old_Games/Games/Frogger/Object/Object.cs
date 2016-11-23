using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _8Old_Games.Games.Frogger.Object {
    //오브젝트(차, 거북이, 나무, 악어)의 뼈대가 되는 abstract 클래스.
    public abstract class Object {
        //맵 크기 및 리소스 크기 정보에 대한 상수들
        public const int WIDTH = 32;
        public const int HEIGHT = 32;
        public const int ROW = 13;
        public const int COL = 14;

        //오브젝트의 방향, true면 오른쪽,  false면 왼쪽
        protected bool mOrigin;
        //오브젝트의 위치
        protected int mX;
        protected int mY;
        //오브젝트의 길이
        protected int mLength;

        //오브젝트의 속도, update당 mSpeed/0.166(1/60)/60.
        protected double mSpeed;
        //오브젝트의 리소스 파일 내 위치
        protected Rectangle mSrc;
        //입력 타이밍 컨트롤용 변수
        protected double mTimeSinceLastInput;

        //getter, setter 정의
        public int X { get { return mX; } set { mX = value; } }
        public int Y { get { return mY; } set { mY = value; } }
        public int Length { get { return mLength; } set { mLength = value; } }
        
        protected Object() { }

        abstract public void initialize(bool origin, int x, int y, double speed, int length, Rectangle src);
        abstract public void update(GameTime gameTime);
        abstract public void draw(SpriteBatch spriteBatch, Texture2D sprite,Vector2 origin, GameTime gametime);
    }
}