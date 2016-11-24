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

namespace _8Old_Games.Games.MineSweeper.Sequence {
    public class Selection : Sequence {

        public Selection() : base() {; }
        public override void initialize() {

        }
        public override State update(GameTime gameTime, KeyboardState ks) { return State.NOTHING; }
        public State update(GameTime gameTime, KeyboardState ks, out int width, out int height, out int mines) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                    int mouseX = Mouse.GetState().X;
                    int mouseY = Mouse.GetState().Y;
                    int mouseXRelease = Mouse.GetState().X;
                    int mMouseYRelease = Mouse.GetState().Y;

                    if ((mouseXRelease > 42 && mouseXRelease < 195) && (mMouseYRelease > 253 && mMouseYRelease < 338)) // 6x6버튼 
                    {
                        width = 6; // 가로로 타일 총 갯수
                        height = 6; // 세로로 타일 총 갯수
                        mines = 8; // 폭탄 갯수
                        return State.PLAY;
                    }
                    else if ((mouseXRelease > 227 && mouseXRelease < 379) && (mMouseYRelease > 253 && mMouseYRelease < 338)) // 7x7버튼 
                    {
                        width = 7; // 가로로 타일 총 갯수
                        height = 7; // 세로로 타일 총 갯수
                        mines = 9; // 폭탄 갯수
                        return State.PLAY;
                    }
                    else if ((mouseXRelease > 411 && mouseXRelease < 564) && (mMouseYRelease > 253 && mMouseYRelease < 338)) // 8x8버튼 
                    {
                        width = 8; // 가로로 타일 총 갯수
                        height = 8; // 세로로 타일 총 갯수
                        mines = 10; // 폭탄 갯수
                        return State.PLAY;
                    }
                    else if ((mouseXRelease > 607 && mouseXRelease < 759) && (mMouseYRelease > 253 && mMouseYRelease < 338)) // 9x9버튼 
                    {
                        width = 9; // 가로로 타일 총 갯수
                        height = 9; // 세로로 타일 총 갯수
                        mines = 11; // 폭탄 갯수
                        return State.PLAY;
                    }
                }
            }
            width = 0;
            height = 0;
            mines = 0;
            return State.SELECTION;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }

        public void draw(SpriteBatch spriteBatch, Texture2D sprite, SpriteFont sf) {
            spriteBatch.Draw(sprite, new Rectangle(0, 0, 800, 480), Color.White);
        }

    }
}
