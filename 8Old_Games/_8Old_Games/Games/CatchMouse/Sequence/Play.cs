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


namespace _8Old_Games.Games.CatchMouse.Sequence
{
    public class Play : Sequence
    {
        protected const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        private int SIZE= 50;

        //초록
        Random rand = new Random(); //랜덤값을 생성한다.(랜덤 위치)

        List<Rectangle> skullSquare; // 해골이 그려질 곳을 지정한다.
        Rectangle currentSquare1; // 쥐가 그려질 곳을 지정한다.

        int playerScore = 0; // 점수

        float timeRemaining = 0.0f; // 사각형이 지워지기 까지의 시간
        const float TimePerSquare = 0.70f; // 나타나는 시간
        private Clock clock;

        int cSkull = 100; //기준점수(해골 등장 수)
        int cSize = 150; // 기준점수(사이즈 감소)
        int deltaSkull = 100;//점수 증감량
        int deltaSize = 150;//점수 증감량
        public Play() : base() {
            ; }

        public override void initialize(){
            skullSquare = new List<Rectangle>();
            skullSquare.Add(new Rectangle(
                                      rand.Next(0, 800 - 25),
                                      rand.Next(0, 480 - 25), SIZE, SIZE));
            clock = new Clock();
            clock.initialize();
        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            clock.update(gameTime);
            if(playerScore != 0) { 
                if(skullSquare.Count <= 7 && playerScore%cSkull==0 ) {
                    skullSquare.Add(new Rectangle(
                                                rand.Next(0, 800 - 25),
                                                rand.Next(0, 480 - 25), SIZE, SIZE));
                    cSkull+= deltaSkull;
                }
                if(SIZE > 25 && playerScore%cSize == 0) {
                    SIZE -= 10;
                    cSize += deltaSize;
                }
            }
            if (timeRemaining == 0.0f)
            {
                currentSquare1 = new Rectangle(
                                      rand.Next(0, 800 - 25),
                                      rand.Next(0, 480 - 25), SIZE, SIZE);
                for(int i = 0;i < skullSquare.Count;i++) {
                    skullSquare[i]= new Rectangle(
                                      rand.Next(0, 800 - 25),
                                      rand.Next(0, 480 - 25), SIZE, SIZE);
                }

                timeRemaining = TimePerSquare;
            }


            MouseState mouse = Mouse.GetState();

            if ((mouse.LeftButton == ButtonState.Pressed) && (currentSquare1.Contains(mouse.X, mouse.Y)))
            {
                playerScore += 25;
                timeRemaining = 0.0f;
            }

            for(int i = 0;i < skullSquare.Count;i++) {
                if((mouse.LeftButton == ButtonState.Pressed) && (skullSquare[i].Contains(mouse.X, mouse.Y))) {
                    return State.FAIL;
                }
            }

            if (playerScore>=500)
            {
                return State.CLEAR;
            }

            timeRemaining = MathHelper.Max(0, timeRemaining - (float)gameTime.ElapsedGameTime.TotalSeconds);
            



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
            return State.PLAY;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin){
            ;
        }
        public void draw(SpriteBatch spriteBatch, Texture2D mouse1, Texture2D skull, SpriteFont font, Vector2 origin)
        {
            spriteBatch.Draw(mouse1, currentSquare1, Color.White);
            for(int i = 0;i < skullSquare.Count;i++) {
                spriteBatch.Draw(skull, skullSquare[i], Color.White);
            }
            spriteBatch.DrawString(font, "Score : "+playerScore, new Vector2(670,440), Color.White);
        }
        
    }
}