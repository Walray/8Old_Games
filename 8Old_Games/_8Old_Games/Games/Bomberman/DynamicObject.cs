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

namespace _8Old_Games.Games.Bomberman {
    public class DynamicObject {
        public enum Type {
            TYPE_1P,
            TYPE_2P,
            TYPE_ENEMY,
            TYPE_NONE,
        };
        const int PLAYER_SPEED = 1000;
        const int ENEMY_SPEED = 500;
        private Type mType;
        private int mX;
        private int mY;
        private int mDirectionX;
        private int mDirectionY;
        private Random mRand;

        public DynamicObject() {
            mType = Type.TYPE_NONE;
            mRand = new Random();
        }
        //셀 좌표를 내부좌표로 치환
        public void set(int x, int y, Type type) {
            mX = x * 32000 + 16000;
            mY = y * 32000 + 16000;
            mType = type;
            if (mType == Type.TYPE_ENEMY) {
                mDirectionX = mDirectionY = 0;
                switch (mRand.Next(0,4)) {
                    case 0: mDirectionX = 1; break;
                    case 1: mDirectionX = -1; break;
                    case 2: mDirectionY = 1; break;
                    case 3: mDirectionY = -1; break;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Texture2D obj) {
            int destX = (mX - 16000 + 500) / 1000; //500 반올림
            int destY = (mY - 16000 + 500) / 1000;
            int srcX=0, srcY=0;
            switch (mType) {
                case Type.TYPE_1P: srcX = 0; srcY = 0; Console.WriteLine("{0},{1}", destX, destY); break;
                case Type.TYPE_2P: srcX = 16; srcY = 0; break;
                case Type.TYPE_ENEMY: srcX = 32; srcY = 16; break;
            }
            spriteBatch.Draw(obj, new Rectangle(destX, destY, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);
        }
        public void update(KeyboardState ks) {
            if (mType == Type.TYPE_ENEMY) {
                mX += mDirectionX * ENEMY_SPEED;
                mY += mDirectionY * ENEMY_SPEED;
            }
            else if (mType == Type.TYPE_1P) {
                int dx, dy;
                dx = dy = 0;
                if (ks.IsKeyDown(Keys.Up)) {
                    dy = -1;
                }
                if (ks.IsKeyDown(Keys.Down)) {
                    dy = 1;
                }
                if (ks.IsKeyDown(Keys.Left)) {
                    dx = -1;
                }
                if (ks.IsKeyDown(Keys.Right)) {
                    dx = 1;
                }
                mX += dx * PLAYER_SPEED;
                mY += dy * PLAYER_SPEED;
            }
            else if (mType == Type.TYPE_2P) { 
                int dx, dy;
                dx = dy = 0;
                if (ks.IsKeyDown(Keys.W)) {
                    dy = -1;
                }
                else if (ks.IsKeyDown(Keys.S)) {
                    dy = 1;
                }
                else if (ks.IsKeyDown(Keys.A)) {
                    dx = -1;
                }
                else if (ks.IsKeyDown(Keys.D)) {
                    dx = 1;
                }
                mX += dx * PLAYER_SPEED;
                mY += dy * PLAYER_SPEED;
            }

            const int X_MIN = 2*8000;
            const int X_MAX = 2*(320 * 1000 - 8000);
            const int Y_MIN = 2*8000;
            const int Y_MAX = 2 * (240 * 1000 - 8000);
            bool hit = false;
            if (mX < X_MIN) {
                mX = X_MIN;
                hit = true;
            }
            else if (mX > X_MAX) {
                mX = X_MAX;
                hit = true;
            }
            if (mY < Y_MIN) {
                mY = Y_MIN;
                hit = true;
            }
            else if (mY > Y_MAX) {
                mY = Y_MAX;
                hit = true;
            }

            if (hit && mType == Type.TYPE_ENEMY) {
                mDirectionX = mDirectionY = 0;
                switch (mRand.Next(0, 4)) {
                    case 0: mDirectionX = 1; break;
                    case 1: mDirectionX = -1; break;
                    case 2: mDirectionY = 1; break;
                    case 3: mDirectionY = -1; break;
                }
            }
        }
    }
}
