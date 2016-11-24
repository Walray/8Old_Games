using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using _8Old_Games.Games.MineSweeper.Sequence;

namespace _8Old_Games.Games.MineSweeper {

    public enum State { START, SELECTION, PLAY, MENU, EXIT, NOTHING };

    public class MineSweeper {

        State state;
        Start start;
        Selection selection;
        Play play;
        Menu menu;
        int width;
        int height;
        int mines;


        public static SpriteFont font; // 폰트
        public static Texture2D selectSize;
        public static Texture2D Back;
        public static Texture2D clicked; // 클릭"함"
        public static Texture2D clicked1; // 1
        public static Texture2D clicked2; // 2
        public static Texture2D clicked3; // 3
        public static Texture2D clicked4; // 4
        public static Texture2D clicked5; // 5
        public static Texture2D clicked6; // 6
        public static Texture2D mine; // 폭탄
        public static Texture2D unclicked; // 클릭"전"
        public static Texture2D flag; // 깃발
        public static Texture2D startImage; // 시작화면
        public static Texture2D menuImage; // 메뉴화면

        public MineSweeper() {; }
        public void initialize() {
            start = new Start();
            selection = new Selection();
            menu = new Menu();
            state = State.START;
            
        }
        



        public Selector update(GameTime gameTime) {

            switch (state) {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.SELECTION:
                    state = selection.update(gameTime, Keyboard.GetState(), out width, out height, out mines);
                    if (state == State.PLAY) {
                        play = new Play();

                        play.initialize(width, height, mines);
                    }
                    break;
                case State.PLAY:
                    state = play.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU:
                    state = menu.update(gameTime, Keyboard.GetState());
                    break;
                case State.EXIT:
                    return Selector.MAIN_SELECTOR;

            }
            return Selector.MINE_SWEEPER;
        }


        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            

            switch (state) {
                case State.START:
                    start.draw(spriteBatch, startImage, new Vector2(0, 0));
                    break;
                case State.SELECTION:
                    selection.draw(spriteBatch, selectSize, font);
                    break;
                case State.PLAY:
                    play.draw(spriteBatch, Back, clicked, flag, unclicked, mine, clicked1, clicked2, clicked3, clicked4, clicked5, clicked6, font, new Vector2(0, 0));
                    break;
                case State.MENU:
                    menu.draw(spriteBatch, menuImage, Vector2.Zero);
                    break;
            }

            
            
        }
    }
}