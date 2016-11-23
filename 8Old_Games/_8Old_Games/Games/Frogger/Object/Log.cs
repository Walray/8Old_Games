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
    public class Log : Object {
        //통나무인지 여부, true면 통나무, false면 악어
        private bool mIsLog;
        //개구리가 현재 통나무 위에 있는지 여부
        private bool mFrogOn;
        //해당 부분이 악어의 입인지의 여부
        private bool mIsMouth;

        //악어 입 리소스 프레임(0~1)
        private int mSeq;

        public bool IsLog { get { return mIsLog; } set { mIsLog = value; } }
        
        public Log() : base() {; }

        public override void initialize(bool origin, int x, int y, double speed, int length, Rectangle src) {; }

        public void initialize(bool origin, int x, int y, double speed, int length, Rectangle src, bool isLog, bool isMouth=false) {
            mOrigin = origin;
            mX = x;
            mY = y;
            mSpeed = speed;
            mLength = length;
            mSrc = src;
            mTimeSinceLastInput = 0.0;
            mIsLog = isLog;
            mFrogOn = false;
            mIsMouth = isMouth;
            mSeq = 0;
        }


        public override void update(GameTime gameTime) {; }

        //입력 및 경과 처리 함수
        public void update(GameTime gameTime, Map map, Frog frog) {

            //개구리가 거북이 위에 있는지 검사, 만약 악어 입 위면 개구리 사망처리
            if (frog.X == mX && frog.Y == mY) {
                mFrogOn = true;
                if (mIsMouth) frog.IsDead = true;
            } 
            else mFrogOn = false;

            //mSpeed에 맞춰 이동 방향에 알맞게 자신의 좌표(mX,mY)를 증감
            //끝에 도달하면 처음으로 돌아가고 만약 개구리가 위에 있으면 개구리 사망처리
            //거북이가 물 위를 이동함에 따라 걷기 가능 여부도 변경
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if(mTimeSinceLastInput >= mSpeed / 1.5) {
                if (mSeq == 0) mSeq = 1;
                else mSeq = 0;
            }
            if (mTimeSinceLastInput >= mSpeed) {
                map.resetWalk(mX, mY);
                if (mOrigin) { // 오른쪽
                    if (mX < COL ) {
                        mX++;
                        if (mFrogOn) {
                            frog.X++;
                            if (frog.X > COL - 1) frog.IsDead = true;
                        }
                    } else {
                        mX = -(mLength - 1);
                        if (mFrogOn) {
                            frog.IsDead = true;
                        }
                    }
                } else { // 왼쪽
                    if (mX > -(mLength - 1)) {
                        mX--;
                        if (mFrogOn) {
                            frog.X--;
                            if (frog.X < 0) frog.IsDead = true;
                        }
                    } else {
                        mX = COL + mLength;
                        if (mFrogOn) {
                            frog.IsDead = true;
                        }
                    }
                }
                mTimeSinceLastInput = 0.0f;
            }
            map.setWalk(mX, mY);
        }

        //그리기 함수
        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin, GameTime gametime) {
           if (IsLog) spriteBatch.Draw(sprite, new Rectangle(mX * WIDTH + (int)origin.X , mY * HEIGHT + (int)origin.Y , WIDTH, HEIGHT), mSrc, Color.White);
            else {
                if(!mIsMouth) spriteBatch.Draw(sprite, new Rectangle(mX * WIDTH + (int)origin.X , mY * HEIGHT + (int)origin.Y , WIDTH, HEIGHT), mSrc, Color.White);
                else { spriteBatch.Draw(sprite, new Rectangle(mX * WIDTH + (int)origin.X , mY * HEIGHT + (int)origin.Y , WIDTH, HEIGHT), new Rectangle(mSrc.X+WIDTH*mSeq, mSrc.Y, WIDTH, HEIGHT), Color.White); }
            }
             
        }
    }

}
