﻿using System;
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
        public Play2() : base() {; }
        public override void initialize() {

        }
        //스페이스 바를 누르면 LOAD 시퀀스로 이동
        public override State update(GameTime gameTime, KeyboardState ks) {

            Console.WriteLine("{0}, {1}", Mouse.GetState().X, Mouse.GetState().Y);
            //메뉴로 이동 처리(입력 타이밍 제어)
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU2;
                }
                mTimeSinceLastInput = 0.0;
            }

            return State.PLAY2;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }

    }
}
