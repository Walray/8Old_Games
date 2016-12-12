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
using _8Old_Games.Games.Bomberman.Sequence;

namespace _8Old_Games.Games.Bomberman {
    public enum State { START, SELECTION, PLAY1,PLAY2, MENU1, MENU2, CLEAR ,FAIL,EXIT, NOTHING }; 
    public class Bomberman {
        State state;

        //시퀀스 관리용 변수
        Start start;
        Selection selection;
        Play1 play1;
        Menu1 menu1;
        Play2 play2;
        Menu2 menu2;
        Clear clear;
        Fail fail;

        //리소스
        public static Texture2D sStart;
        public static Texture2D sSelection;
        public static Texture2D sMenu;
        public static Texture2D sPlay;
        public static Texture2D sClear;
        public static Texture2D sFail;
        public static SpriteFont font;
        public static Texture2D obj;

        //디스플레이 원점
        Vector2 displayOrigin = new Vector2(0,0);



        public Bomberman() {; }
        public void initialize() {
            state = State.START;
            start = new Start();
            selection = new Selection();
            menu1 = new Menu1();
            menu2 = new Menu2();
            clear = new Clear();
            fail = new Fail();

        }

        public Selector update(GameTime gameTime) {
            switch (state) {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.SELECTION:
                    state = selection.update(gameTime, Keyboard.GetState());
                    if (state == State.PLAY1) {
                        play1 = new Play1();
                        play1.initialize();
                    }
                    else if (state == State.PLAY2) {
                        play2 = new Play2();
                        play2.initialize();
                    }
                break;
                case State.PLAY1:
                    state = play1.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU1:
                    state = menu1.update(gameTime, Keyboard.GetState());
                    break;
                case State.PLAY2:
                    state = play2.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU2:
                    state = menu2.update(gameTime, Keyboard.GetState());
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
            return Selector.BOMBERMAN;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            switch (state) {
                case State.START:
                    start.draw(spriteBatch, sStart, new Vector2(0, 0));
                    break;
                case State.SELECTION:
                    selection.draw(spriteBatch, sSelection, new Vector2(0, 0));
                    break;
                case State.PLAY1:
                    play1.draw(spriteBatch, obj, font,displayOrigin);
                    break;
                case State.MENU1:
                    menu1.draw(spriteBatch, sMenu, new Vector2(0, 0));
                    break;
                case State.PLAY2:
                    play2.draw(spriteBatch, obj, font,displayOrigin);
                    break;
                case State.MENU2:
                    menu2.draw(spriteBatch, sMenu, new Vector2(0, 0));
                    break;
                case State.CLEAR:
                    clear.draw(spriteBatch, sClear, new Vector2(0, 0));
                break;
                case State.FAIL:
                    fail.draw(spriteBatch, sFail, new Vector2(0, 0));
                    break;
            }
        }
        
    }
}
