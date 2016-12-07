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

namespace _8Old_Games.Games.Bomberman.Sequence {
    public class Play2 : Sequence {
        protected const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;
        Stage stage;

        public Play2() : base() {; }
        public override void initialize() {
            stage = new Stage();
            stage.initialize(0);
        }
        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU1;
                }
                mTimeSinceLastInput = 0.0;
            }
            int state = stage.update(gameTime, ks);
            if (state == 1) return State.CLEAR;
            else if (state == -1) return State.SELECTION;
            return State.PLAY2;
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }


        public void draw(SpriteBatch spriteBatch, Texture2D sprite, SpriteFont sf, Vector2 origin) {
            stage.draw(spriteBatch, sprite, sf);
        }

    }
}
