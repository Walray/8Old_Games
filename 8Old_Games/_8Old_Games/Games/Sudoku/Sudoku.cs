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
using _8Old_Games.Games.Sudoku.Sequence;

namespace _8Old_Games.Games.Sudoku {
    public enum State { START, SELECTION, LOAD, PLAY, MENU, EXIT, NOTHING };

public class Sudoku {
        public static Texture2D cell;
        public static Texture2D line;
        public static Texture2D sudoku_main;
        public static Texture2D sudoku_menu;
        public static Texture2D sudoku_loading;
        public static Texture2D button_easy;
        public static Texture2D button_normal;
        public static Texture2D button_hard;
        public static Texture2D button_extreme;
        public static SpriteFont sf;
        public static SpriteFont sf2;
        public static SpriteFont sf_bold;


        int stage;
        int num;

        public int Stage { get { return stage; } set { stage = value; } }
        public int Num { get { return num; } set { num = value; } }

        State state;

        Start start;
        Selection selection;
        Load load;
        Play play;
        Menu menu;

        MapData mp;

        public Sudoku() {; }

        public void initialize() {
            state = State.START;
            start = new Start();
            selection = new Selection();
            load = new Load();
            menu = new Menu();
            mp = new MapData();

        }


        public Selector update(GameTime gameTime) { 
            switch (state) {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.SELECTION:
                    state = selection.update(gameTime, Keyboard.GetState(), out stage, out num);
                    break;
                case State.LOAD:
                    play = new Play();
                    play.initialize(mp.getMap(stage, num), mp.getVisible(stage, num));
                    state = load.update(gameTime, Keyboard.GetState());
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
            return Selector.SUDOKU;

        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {

            switch (state) {
                case State.START:
                    start.draw(spriteBatch, sudoku_main, new Vector2(0, 0));
                    break;
                case State.SELECTION:
                    selection.draw(spriteBatch, button_easy, button_normal, button_hard, button_extreme, new Vector2(0, 0));
                    break;
                case State.LOAD:
                    load.draw(spriteBatch, sudoku_loading, new Vector2(0, 0));
                    break;
                case State.PLAY:
                    play.draw(gameTime, spriteBatch, line, cell, sf, sf2, sf_bold, new Vector2(0, 0));
                    break;
                case State.MENU:
                    menu.draw(spriteBatch, sudoku_menu, new Vector2(0, 0));
                    break;
            }
        }

    }
}
