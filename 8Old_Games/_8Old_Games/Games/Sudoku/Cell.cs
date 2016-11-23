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

namespace _8Old_Games.Games.Sudoku {
    public class Cell
    {
        int input;
        int answer;
        bool visible;
        int x, y;
        Rectangle rect;

        public Cell() {; }

        public void initialize(int x, int y, int input, int answer, bool visible)
        {
            this.input = visible ? answer : input ;
            this.answer = answer;
            this.visible = visible;

            this.x = x;
            this.y = y;
            this.rect = new Rectangle(this.x * 45, this.y * 45, 45, 45);

        }

        public void update(int x, int y, bool isRight)
        {
            if (visible) return;
            if (isRight)
            {
                if (rect.Contains(x, y))
                {
                    input++;
                    if (input > 9)
                        input = 0;
                }
            }
            else
            {
                if (rect.Contains(x, y))
                {
                    input--;
                    if (input < 0)
                        input = 9;

                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Texture2D cell, SpriteFont sf_bold, SpriteFont sf) 
        {
            spriteBatch.Draw(cell, new Rectangle((x * 45)+38, (y * 45)+38, 45, 45), Color.White);
            if (visible)
                spriteBatch.DrawString(sf_bold, "" + input, new Vector2((x * 45) + 55, (y * 45) + 48), Color.DeepPink);
            else
                spriteBatch.DrawString(sf, "" + input, new Vector2((x * 45) + 55, (y * 45) + 48), Color.Black);
        }


        public bool checkclear() {

            if (input == answer)
                return true;
            else
                return false;

        }

            


        

    }
}
