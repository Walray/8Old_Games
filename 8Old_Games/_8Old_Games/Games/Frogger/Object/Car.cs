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
    public class Car : Object {
        
        public Car() : base() {; }

        public override void initialize(bool origin, int x, int y, double speed, int length, Rectangle src) {
            mOrigin = origin;
            mX = x;
            mY = y;
            mSpeed = speed;
            mLength = length;
            mSrc = src;
            mTimeSinceLastInput = 0.0;
        }


        //입력 및 경과 처리 함수
        public override void update(GameTime gameTime) {
            //mSpeed에 맞춰 이동 방향에 알맞게 자신의 좌표(mX,mY)를 증감, 끝에 도달하면 처음으로 돌아감.
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= mSpeed) {
                if (mOrigin) { // 오른쪽
                    if (mX < COL - 1) { mX++; } else { mX = -(mLength - 1); }

                } else { // 왼쪽
                    if (mX > -(mLength - 1)) { mX--; } else { mX = COL - 1; }

                }
                mTimeSinceLastInput = 0.0f;
            }

        }

        //그리기 함수
        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin, GameTime gametime) {
            spriteBatch.Draw(sprite, new Rectangle(mX * WIDTH + (int)origin.X, mY * HEIGHT + (int)origin.Y, WIDTH * mLength, HEIGHT), mSrc, Color.White);
            
        }
    }
}
