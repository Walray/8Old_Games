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
    public class LastCroc {

        //맵 크기 및 리소스 크기 정보에 대한 상수들
        public const int WIDTH = 32;
        public const int HEIGHT = 32;

        //마지막 악어의 위치 정보(Y 좌표 고정)
        private int mX;
        private const int mY = 0;

        //악어 입 리소스 프레임(0~1)
        private int mSeq;
        //악어가 잠겨있는지의 여부(해당 GOAL이 안전한지)
        private bool mIsSunk;


        //타이밍 컨트롤용 변수
        private double mTimeSinceLastInput;
        private const double MIN_TIME = 1.5;
        
        public LastCroc() {; }

        public void initialize(int x) {
            mX = x;
            mIsSunk = true;
            mSeq = -1;
            mTimeSinceLastInput = 0.0;
        }

        //입력 및 경과 처리 함수
        public bool update(GameTime gameTime, Frog frog, Map map) {
            //해당 부분이 클리어가 됐으면(KING) 더는 악어가 나오지 않 게 처리
            if (map.isKing(mX, mY)) {
                mIsSunk = false;
                mSeq = -1;
                return false;
     
            }

            //악어가 떠올라있는데 개구리가 접근하면 개구리 사망처리
            if (!mIsSunk && frog.X == mX && frog.Y == mY) {
                frog.IsDead = true;
                return true;
            }

            //프레임에 따른 악어 모습 변화 처리
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                mSeq++;
                if (mSeq > 1) mSeq = -1;
                if (mSeq == -1) mIsSunk = true;
                else mIsSunk = false;

                mTimeSinceLastInput = 0.0;
            }
            return false;
        }

        //그리기 함수
        public void draw(Texture2D sprite, Texture2D dummy, SpriteBatch spriteBatch, Vector2 origin) {
            if (!mIsSunk) { 
                if (mSeq == -1) spriteBatch.Draw(dummy, new Rectangle((int)origin.X + mX * WIDTH, (int)origin.Y + mY * HEIGHT, WIDTH, HEIGHT), Color.White);
                else if(mSeq==0) spriteBatch.Draw(sprite, new Rectangle((int)origin.X + mX * WIDTH, (int)origin.Y + mY * HEIGHT, WIDTH, HEIGHT), new Rectangle(0,0,WIDTH, HEIGHT),Color.White);
                else if (mSeq == 1) spriteBatch.Draw(sprite, new Rectangle((int)origin.X + mX * WIDTH, (int)origin.Y + mY * HEIGHT, WIDTH, HEIGHT), new Rectangle(WIDTH, 0, WIDTH, HEIGHT), Color.White);
            }
        }
    }
}
