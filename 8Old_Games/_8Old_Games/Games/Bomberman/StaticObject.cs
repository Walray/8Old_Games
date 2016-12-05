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
    public class StaticObject {
        public enum Flag {
            FLAG_WALL=(1<<0),
            FLAG_BRICK=(1<<1),
            FLAG_ITEM_BOMB=(1<<2),
            FLAG_ITEM_POWER=(1<<3),
            FLAG_NOTHING = (1 <<4),
        };

        private uint mFlags;

        public StaticObject() {
            setFlag(Flag.FLAG_NOTHING);
        }


        public void initialze() {;
        }

        public void draw(SpriteBatch spriteBatch, int x, int y, Texture2D obj) {
            int srcX = -1, srcY = -1;
            bool floor = false;
            if(checkFlag(Flag.FLAG_WALL)) {
                srcX = 48;
                srcY = 16;
            }
            else if (checkFlag(Flag.FLAG_BRICK)) {
                srcX = 0;
                srcY = 32;
            }
            else {
                Console.WriteLine("뿅");
                srcX = 16;
                srcY = 32;
                floor = true;
            }
            spriteBatch.Draw(obj, new Rectangle(x * 32, y *32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);

            if(true || floor) {
                srcX = -1;
                if(checkFlag(Flag.FLAG_ITEM_BOMB)) {
                    srcX = 32;
                    srcY = 0;
                }

                else if (checkFlag(Flag.FLAG_ITEM_POWER)) {
                    srcX = 48;
                    srcY = 0;
                }

                if (srcX != -1) {
                    spriteBatch.Draw(obj, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);
                }
            }
            
        }

        public bool checkFlag(Flag f) {
            return (mFlags &(uint)f) != 0;
        }

        public void setFlag(Flag f) {
            mFlags |= (uint)f;
        }
        public void resetFlag(Flag f) {
            mFlags &= ~(uint)f;
        }


    }
}
