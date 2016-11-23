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
    public class Play : Sequence {
        private int[,] answer;
        private bool[,] visible;
        private int Time;
        private Clock time;

        private int score;

        int checkFlagCount;
        bool clear;
        private float timeSinceLastInput = 0.0f; // 타임컨트롤 변수1
        private const float MinTimeSinceLastInput = 0.10f; //  타임컨트롤 변수2

        Cell[,] map;

        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        public Play() : base() {

        }
        public override void initialize() {
            ;

        }
        public void initialize(int[,] mapData, bool[,] visibleData) {
            map = new Cell[9, 9];
            answer = mapData;
            visible = visibleData;
            Cell temp;
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++) {
                    temp = new Cell();
                    temp.initialize(x, y, 0, answer[y, x], visible[y, x]);
                    map[y, x] = temp;
                }
            time = new Clock();
            time.initialize();

            checkFlagCount = 0;
            Time = 0;
            clear = false;

        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU;
                }
                mTimeSinceLastInput = 0.0f;
            }
            MouseState mouse = Mouse.GetState();

            time.update(gameTime);

            timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeSinceLastInput >= MinTimeSinceLastInput) {

                for (int y = 0; y < 9; y++)
                    for (int x = 0; x < 9; x++) {
                        Rectangle currentSquare = new Rectangle(x * 53, y * 53, 53, 53);

                        if ((mouse.RightButton == ButtonState.Pressed)) {
                            map[y, x].update(mouse.X-45, mouse.Y-45, true);
                        } else if ((mouse.LeftButton == ButtonState.Pressed)) {
                            map[y, x].update(mouse.X-45, mouse.Y-45, false);
                        }

                    }


                timeSinceLastInput = 0.0f;
            }
            for (int y = 0; y < 9; y++) {
                for (int x = 0; x < 9; x++) {
                    if (map[y, x].checkclear()) checkFlagCount++;

                }

            }

            if (checkFlagCount == 81) clear = true;
            else checkFlagCount = 0;

            if (clear) {
                return State.SELECTION;
            }

            return State.PLAY;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }

        public void draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D line, Texture2D cell, SpriteFont sf, SpriteFont sf2, SpriteFont sf_bold, Vector2 origin) {

            int Time=0;
            Time = (int)gameTime.TotalGameTime.TotalSeconds;

            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++) {
                    map[y, x].draw(spriteBatch, cell, sf_bold, sf);

                }
            spriteBatch.Draw(line, new Rectangle(171, 0, 3, 450), Color.Pink);
            spriteBatch.Draw(line, new Rectangle(306, 0, 3, 450), Color.Pink);
            spriteBatch.Draw(line, new Rectangle(0, 171, 450, 3), Color.Pink);
            spriteBatch.Draw(line, new Rectangle(0, 306, 450, 3), Color.Pink);
            spriteBatch.DrawString(sf_bold, "Chorok's Sudoku", new Vector2(520, 80), Color.White);
            time.draw(spriteBatch, sf2, new Vector2(500, 140), Color.Black);
            //spriteBatch.DrawString(sf2, ""+ Time, new Vector2(650, 200), Color.Black);

            if (clear) {
                score = Time;
                spriteBatch.DrawString(sf2, "Clear" + score, new Vector2(180, 180), Color.DeepPink);
                clear = false;
                map = null;
            }
        }

    }
}
