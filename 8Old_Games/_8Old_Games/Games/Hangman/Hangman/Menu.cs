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

namespace _8Old_Games.Games.Hangman.Hangman
{
    public class Menu : Sequence
    {
        private double mTimeAfterPlay = WAIT_TIME;
        private const double WAIT_TIME = 0.10;
        public Menu() : base() {; }

        public override void initialize() {     }

        public override State update(GameTime gameTime, KeyboardState ks)
        {

            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterPlay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterPlay <= 0)
            {
                if (ks.IsKeyDown(Keys.M))
                { // 게임선택메뉴
                    return State.EXIT;
                }
                else if (ks.IsKeyDown(Keys.S))
                { // 게임 다시 시작
                    return State.START;
                }
                else if (ks.IsKeyDown(Keys.Space))
                {  //게임으로 돌아가기
                    mTimeAfterPlay = WAIT_TIME;
                    return State.PLAY;
                }
                mTimeSinceLastInput = 0.0f;
            }
            return State.MENU;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin)
        {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);

        }

    }


}
