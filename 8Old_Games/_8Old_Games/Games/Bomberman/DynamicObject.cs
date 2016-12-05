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
        private Type mType;
        private int mX;
        private int mY;
        public DynamicObject() {
            mType = Type.TYPE_NONE;
        }
        //셀 좌표를 내부좌표로 치환
        public void set(int x, int y, Type type) {
            mX = x * 32000 + 16000;
            mY = y * 32000 + 16000;
            mType = type;
        }

        public void draw(SpriteBatch spriteBatch, Texture2D obj) {
            int destX = (mX - 16000 + 500) / 1000; //500 반올림
            int destY = (mY - 16000 + 500) / 1000;
            int srcX=0, srcY=0;

            switch (mType) {
                case Type.TYPE_1P: srcX = 0; srcY = 0; break;
                case Type.TYPE_2P: srcX = 16; srcY = 0; break;
                case Type.TYPE_ENEMY: srcX = 32; srcY = 16; break;
            }
            spriteBatch.Draw(obj, new Rectangle(destX * 32, destY * 32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);
        }
    }
}
