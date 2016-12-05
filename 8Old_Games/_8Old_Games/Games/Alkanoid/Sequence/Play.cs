using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using _8Old_Games.Games.Alkanoid;

namespace _8Old_Games.Games.Alkanoid.Sequence
{
    public class Play : Sequence
    {

        private Paddle paddle;
        private Ball ball;
        private Wall wall;
        private int screenHeight;
        private int screenWidth;

        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        public Play() : base() {; }
        public override void initialize()
        {
            this.screenWidth = 800;
            this.screenHeight = 480;
            this.paddle = new Paddle(screenWidth, screenHeight);
            this.wall = new Wall(screenWidth, screenHeight);
            this.ball = new Ball(paddle, wall, screenWidth, screenHeight);

        }

        public override State update(GameTime gameTime, KeyboardState ks)
        {    //메뉴로 이동 처리(입력 타이밍 제어)
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0)
            {
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Space))
                {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU;
                }
                mTimeSinceLastInput = 0.0;
            }


            paddle.Update(gameTime);
            ball.Update(gameTime);
            return State.PLAY;
        }


        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }
        public void draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D ballImage, Texture2D paddleImage, Texture2D wallImage, SpriteFont font) {
            ball.Draw(spriteBatch, gameTime, ballImage);
            paddle.Draw(spriteBatch, gameTime, paddleImage);
            wall.Draw(spriteBatch, gameTime, wallImage, font);
        }
    }
}
