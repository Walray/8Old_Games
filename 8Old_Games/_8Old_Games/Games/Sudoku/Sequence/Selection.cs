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

namespace _8Old_Games.Games.Sudoku.Sequence {
    public class Selection : Sequence {

        Rectangle button1;
        Rectangle button2;
        Rectangle button3;
        Rectangle button4;
        const int WIDTH = 150;
        const int HEIGHT = 70;
        Random random;

        public Selection() : base() {
            button1 = new Rectangle(50, 200, WIDTH, HEIGHT);
            button2 = new Rectangle(235, 200, WIDTH, HEIGHT);
            button3 = new Rectangle(420, 200, WIDTH, HEIGHT);
            button4 = new Rectangle(605, 200, WIDTH, HEIGHT);
            random = new Random();
        }

        public override void initialize() {

        }

        public override State update(GameTime gameTime, KeyboardState ks) { return State.NOTHING;  }


        public State update(GameTime gameTime, KeyboardState ks, out int stage, out int num) {
            
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                MouseState ms = Mouse.GetState();
                if(button1.Contains(ms.X, ms.Y) && ms.LeftButton==ButtonState.Pressed) {
                    stage = 0;
                    num = random.Next(0, 5);
                    return State.LOAD;
                }
                else if (button2.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                    stage = 1;
                    num = random.Next(0, 5);
                    return State.LOAD;
                } 
                else if (button3.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                    stage = 2;
                    num = random.Next(0, 5);
                    return State.LOAD;
                } 
                else if (button4.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                    stage = 3;
                    num = random.Next(0, 5);
                    return State.LOAD;

                }
                mTimeSinceLastInput = 0.0f;
            }
            stage = 0;
            num = random.Next(0, 5);
            return State.SELECTION;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {;}

        public void draw(SpriteBatch spriteBatch, Texture2D easy, Texture2D normal, Texture2D hard, Texture2D extreme, Vector2 origin) {
            spriteBatch.Draw(easy, button1, Color.LightCyan);
            spriteBatch.Draw(normal, button2, Color.Honeydew);
            spriteBatch.Draw(hard, button3, Color.Lavender);
            spriteBatch.Draw(extreme, button4, Color.LemonChiffon);
        }

    }
}
