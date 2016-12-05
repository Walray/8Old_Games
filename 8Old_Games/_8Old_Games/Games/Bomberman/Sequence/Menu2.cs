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
    public class Menu2 : Sequence {
        //PLAY<->MENU 시퀀스 전환 시 입력 속도 제어용 변수
        private double mTimeAfterPlay = WAIT_TIME;
        private const double WAIT_TIME = 0.15;


        public Menu2() : base() {; }
        public override void initialize() {

        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            //스페이스를 누르면 다시 PLAY 시퀀스로 이동
            //Enter를 누르면 방향키로 고른 스테이지 정보와 함께 LOAD 시퀀스로 이동
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterPlay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                if (mTimeAfterPlay <= 0) {
                    if (ks.IsKeyDown(Keys.Space)) {
                        mTimeAfterPlay = WAIT_TIME;
                        return State.PLAY2;
                    }
                    else if (ks.IsKeyDown(Keys.Enter)) {
                        return State.LOAD;
                    }
                    else if (ks.IsKeyDown(Keys.Escape)) {
                        return State.EXIT;
                    }
                }
                mTimeSinceLastInput = 0.0f;
            }

            return State.MENU2;
        }


        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }



    }
}
