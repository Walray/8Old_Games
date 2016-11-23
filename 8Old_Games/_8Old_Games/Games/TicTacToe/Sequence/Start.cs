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
    public class Start : Sequence {

        public Start() : base() {; }
        public override void initialize() {

        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                if (ks.IsKeyDown(Keys.Space)) {
                    return State.SELECTION;
                }
                mTimeSinceLastInput = 0.0f;
            }
            return State.START;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }

    }
}
