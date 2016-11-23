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
    public class Turtle : Object {
        //잠기는 거북이 인지에 대한 여부
        private bool mIsSunk;
        //거북이가 완전히 잠겼는지에 대한 여부
        private bool mIsGone;
        //개구리가 현재 거북이 위에 있는지 여부
        private bool mFrogOn;
        //거북이 리소스 프레임(0~5)
        private int mSeq;
        //프레임이 바뀌는 시간에 대한 컨트롤 변수
        private double mFrameTime; 
        private double mTimeSinceLastFrame;

        public Turtle() : base() {; }
        
        public override void initialize(bool origin, int x, int y, double speed, int length, Rectangle src) {; }
        public void initialize(bool origin, int x, int y, double speed, int length, Rectangle src, bool isSunk) {
            mOrigin = origin;
            mX = x;
            mY = y;
            mSpeed = speed;
            mLength = length;
            mSrc = src;
            mIsSunk = isSunk;
            mSeq = 0;
            mTimeSinceLastInput = 0.0;
            mFrogOn = false;
            mIsGone = false;
            mFrameTime = mSpeed *2.5;
            mTimeSinceLastFrame = 0.0;
        }

        public override void update(GameTime gameTime) {; }

        //입력 및 경과 처리 함수
        public void update(GameTime gameTime, Map map, Frog frog) {
            
            //프레임 변화를 처리하는 루틴(start)
            //시간이 경과하면 프레임(0~5)을 증가, 만약 5 프레임(완전히 잠김)에 도달하고 개구리가 거북이 위에 있으면 사망처리
            mTimeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastFrame >= mFrameTime) {
                if (mIsSunk && !mIsGone) {
                    mSeq++;
                    if (mSeq >= 5) {
                        mIsGone = true;
                        if (mFrogOn) frog.IsDead = true;
                    }
                }
                mTimeSinceLastFrame = 0.0;
            }

            //프레임 변화를 처리하는 루틴(end)

            //개구리가 거북이 위에 있는지 검사
            if (frog.X == mX && frog.Y == mY) { mFrogOn = true; } else mFrogOn = false;


            //mSpeed에 맞춰 이동 방향에 알맞게 자신의 좌표(mX,mY)를 증감
            //끝에 도달하면 처음으로 돌아가고 만약 개구리가 위에 있으면 개구리 사망처리
            //거북이가 물 위를 이동함에 따라 걷기 가능 여부도 변경
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= mSpeed) {
               map.resetWalk(mX, mY);
                if (mOrigin) { // 오른쪽
                    if (mX < (COL - 1)) {
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
                        mX = COL;
                        mSeq = 0;
                        mIsGone = false;
                        if (mFrogOn) {
                            frog.IsDead = true;
                        }
                    }
                }
                mTimeSinceLastInput = 0.0f;
            }
            map.setWalk(mX, mY);
        }


        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin, GameTime gametime) {; }
        //그리기 함수
        public void draw(SpriteBatch spriteBatch, Texture2D sprite, bool isRight, Vector2 origin, GameTime gametime) {
            if (isRight) spriteBatch.Draw(sprite, new Rectangle(mX*WIDTH + (int)origin.X, mY*HEIGHT + (int)origin.Y, WIDTH, HEIGHT), new Rectangle(mSrc.X + WIDTH * mSeq, mSrc.Y, WIDTH, HEIGHT), Color.White);
            else spriteBatch.Draw(sprite, new Rectangle(mX*WIDTH + (int)origin.X, mY*HEIGHT + (int)origin.Y, WIDTH, HEIGHT), new Rectangle(mSrc.X + WIDTH * mSeq, mSrc.Y, WIDTH, HEIGHT), Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, (float)SpriteSortMode.Immediate);

        }

    }
}
