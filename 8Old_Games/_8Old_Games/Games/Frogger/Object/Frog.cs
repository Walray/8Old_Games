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
    public class Frog {
        //맵 크기 및 리소스 크기 정보에 대한 상수들
        public const int WIDTH = 32;
        public const int HEIGHT = 32;
        public const int ROW = 13;
        public const int COL = 14;

        //개구리의 위치 정보
        private int mX;
        private int mY;

        //개구리 사망 여부
        private bool misDead;

        //입력 타이밍 컨트롤용 변수 초당 MIN_TIME/0.0166(1/60) 만큼의 입력을 받는다.
        private double mTimeSinceLastInput;
        private const float MIN_TIME = 0.09f;

        //개구리가 해당 지역을 지나갔는지 여부를 저장하는 배열(점수 중복 가산 방지)
        private bool[] mTrail;

        //getter, setter 정의
        public int X { get { return mX; } set { mX = value; } }
        public int Y { get { return mY; } set { mY = value; } }
        public bool IsDead { get { return misDead; } set { misDead = value; } }
   
        public Frog() {;  }

        public void initialize() {
            mX = COL / 2;
            mY = ROW - 1;
            misDead = false;
            mTimeSinceLastInput = 0.0f;
            mTrail = new bool[COL];
        }

        //오브젝트와 충돌 검사
        public bool checkCollision(Object obj) {
            for (int x = 0; x < obj.Length; x++) {
                if (obj.X + x == mX && obj.Y == mY) {
                    misDead = true;
                    return true;
                }
            }
            return false;
        }
        
        //입력 및 경과 처리 함수
        public void update(GameTime gameTime, KeyboardState ks, Map map,out int deltaScore) {
            deltaScore = 0;
            /*키에 따른 이동처리 루틴
            * 해당 방향 버튼 키를 누르면 이동가능여부를 검사하고 해당 방향으로 좌표(mX,mY) 증감
            * Up 키에는 별도의 점수계산 부분 삽입, mTrail로 점수 중복가산 방지.
            */
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                if (ks.IsKeyDown(Keys.Left) ) {
                    if (map.getWalk(mX - 1, mY)) { 
                        if (mX > 0) {
                            mX--;
                        }
                    }
                } 
                else if (ks.IsKeyDown(Keys.Right)) {
                    if (map.getWalk(mX + 1, mY)) { 
                        if (mX < COL - 1) {
                            mX++;
                        }
                    }
                } 
                else if (ks.IsKeyDown(Keys.Up) ) {
                    if (map.getWalk(mX, mY - 1)) {
                        mY--;
                        if (mY > 0) {
                            if (mY>=1 && mY<=5 && !mTrail[mY]) {
                                mTrail[mY] = true;
                                deltaScore = 50;
                            }   
                            else if (mY>=7 &&mY<=11 && !mTrail[mY]) {
                                mTrail[mY] = true;
                                deltaScore = 30;
                            }
                        }
                    }
                }
                else if (ks.IsKeyDown(Keys.Down)) {
                    if (map.getWalk(mX, mY + 1)) { 
                        if (mY < ROW - 1) {
                            mY++;
                        }
                    }
                }
                mTimeSinceLastInput = 0.0f;
            }
        }

        //그리기 함수
        public void draw(Texture2D sprite, SpriteBatch spriteBatch, Vector2 origin) {
             spriteBatch.Draw(sprite, new Rectangle(mX*WIDTH+(int)origin.X, mY* HEIGHT + (int)origin.Y, WIDTH, HEIGHT), Color.White);
        }
    }
}
