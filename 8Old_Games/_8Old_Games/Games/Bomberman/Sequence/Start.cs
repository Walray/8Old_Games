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
    public class Start : Sequence {
        public Start() : base() {; }
        public override void initialize() {

        }
        //스페이스 바를 누르면 LOAD 시퀀스로 이동
        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                if (ks.IsKeyDown(Keys.Space)) {
                    return State.SELECTION;
                }
                else if (ks.IsKeyDown(Keys.Enter)) {
                    return State.EXIT;
                }
                mTimeSinceLastInput = 0.0f;
            }
            return State.START;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }

    }
}
