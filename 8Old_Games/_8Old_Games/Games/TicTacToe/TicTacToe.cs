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
using _8Old_Games.Games.TicTacToe.Sequence;

namespace _8Old_Games.Games.TicTacToe {
    public enum State { START, SELECTION, PLAY1, PLAY2, MENU1, MENU2, EXIT, NOTHING };
    public class TicTacToe : Microsoft.Xna.Framework.Game { 
        Start start;
        Selection selection;
        Play1 play1;
        Play2 play2;

        Menu1 menu1;
        Menu2 menu2;
        State state;

        public static Texture2D main;
        public static Texture2D menuImage;
        public static Texture2D loading;
        public static Texture2D cross;
        public static Texture2D zero;
        public static Texture2D square;
        public static Texture2D selectmode;
        public static SpriteFont spriteFont;
        public static SpriteFont spriteFont2;

        public TicTacToe() {; }

        public void initialize() {
            start = new Start();
            selection = new Selection();
            menu1 = new Menu1();
            menu2 = new Menu2();
            state = State.START;
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
                case State.PLAY2:
                    state = play2.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU1:
                    state = menu1.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU2:
                    state = menu2.update(gameTime, Keyboard.GetState());
                    break;
                case State.EXIT:
                    return Selector.MAIN_SELECTOR;
            }
            return Selector.TICTACTOE;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            
            switch (state) {
                case State.START:
                    start.draw(spriteBatch, main);
                    break;
                case State.SELECTION:
                    selection.draw(spriteBatch, selectmode, spriteFont);
                    break;
                case State.PLAY1:
                    play1.draw(spriteBatch, square, cross, zero, spriteFont, spriteFont2);
                    break;
                case State.PLAY2:
                    play2.draw(spriteBatch, square, cross, zero, spriteFont, spriteFont2);
                    break;
                case State.MENU1:
                    menu1.draw(spriteBatch, menuImage);
                    break;
                case State.MENU2:
                    menu2.draw(spriteBatch, menuImage);
                    break;
            }
            
        }

    }
}
