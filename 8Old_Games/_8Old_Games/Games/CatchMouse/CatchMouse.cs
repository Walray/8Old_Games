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
using _8Old_Games.Games.CatchMouse.Sequence;
//시퀀스 만들고 풀어주세요

/*
 * 초기화(initialize), 논리(update), 그리고 그리기(draw) 코드 반드시 분리
 * 필요하면 함수 원형 자유롭게 변경
*/
namespace _8Old_Games.Games.CatchMouse {
    public enum State { START, LOAD, PLAY, MENU, CLEAR,FAIL,EXIT, NOTHING }; //자유롭게 추가
    public class CatchMouse {
        State state;

        Start start;
        Load load;
        Play play;
        Menu menu;
        Clear clear;
        Fail fail;

        public static Texture2D mouse1;
        public static Texture2D cm_start;
        public static Texture2D cm_menu;
        public static Texture2D cm_load;
        public static Texture2D cm_failed;
        public static Texture2D skull;
        public static Texture2D cm_clear;
        public static SpriteFont font;

        public CatchMouse() {; }
        public void initialize() {
            state = State.START;
            start = new Start();
            load = new Load();
            clear = new Sequence.Clear();
            fail = new Sequence.Fail();
            menu = new Sequence.Menu();
        }

        public Selector update(GameTime gameTime) {
            switch (state)
            {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.LOAD:
                    state = load.update(gameTime, Keyboard.GetState());
                    play = new Play();
                    play.initialize();
                    break;
                case State.PLAY:
                    state = play.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU:
                    state = menu.update(gameTime, Keyboard.GetState());
                    break;
                case State.CLEAR:
                    state = clear.update(gameTime, Keyboard.GetState());
                    break;
                case State.FAIL:
                    state = fail.update(gameTime, Keyboard.GetState());
                    break;
                case State.EXIT:
                    return Selector.MAIN_SELECTOR;
            }
            return Selector.CATCH_MOUSE;

        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {

            switch (state)
            {
                case State.START:
                    start.draw(spriteBatch, cm_start, new Vector2(0, 0));
                    break;
                case State.LOAD:
                    load.draw(spriteBatch, cm_load, new Vector2(0, 0));
                    break;
                case State.PLAY:
                    play.draw(spriteBatch,mouse1,skull,font, new Vector2(0,0));
                    break;
                case State.MENU:
                        menu.draw(spriteBatch, cm_menu, new Vector2(0, 0));
                        break;
                    case State.CLEAR:
                    clear.draw(spriteBatch, cm_clear, new Vector2(0, 0));
                    break;
                case State.FAIL:
                    fail.draw(spriteBatch, cm_failed, new Vector2(0, 0));
                    break;
            }
        }

    }
}