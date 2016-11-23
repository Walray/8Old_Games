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

namespace _8Old_Games.Games.TicTacToe.Sequence {
    public class Menu1 : Sequence {
        private double mTimeAfterPlay = WAIT_TIME;
        private const double WAIT_TIME = 0.15;
        private int chStage;
        private bool isFirst = true;
        public Menu1() : base() {; }
        public override void initialize() {

        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterPlay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterPlay <= 0) {
                if (ks.IsKeyDown(Keys.Space)) {
                    mTimeAfterPlay = WAIT_TIME;
                    return State.PLAY1;
                } else if (ks.IsKeyDown(Keys.N)) {
                    return State.SELECTION;

                } else if (ks.IsKeyDown(Keys.Escape)) {
                    return State.EXIT;
                }
                mTimeSinceLastInput = 0.0f;
            }
            return State.MENU1;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }




    }
}
