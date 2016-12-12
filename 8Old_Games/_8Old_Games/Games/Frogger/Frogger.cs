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
using _8Old_Games.Games.Frogger.Sequence;
/*
 * class Frogger
 * ���ΰ� ������ ��ü �� ���
 * ������ : ���Ͽ�
 */
namespace _8Old_Games.Games.Frogger {
    public enum State { START, LOAD, PLAY, MENU, CLEAR, FAIL, EXIT ,NOTHING };

    public class Frogger : Microsoft.Xna.Framework.Game {

        //������ ������ ����
        Start start;
        Load load;
        Play play;
        Menu menu;
        Clear clear;
        Fail fail;
        State state;

        //�������� �̵� ������ ����
        int outStage;
        int currentStage;

        //���ҽ�
        public static Texture2D sStart;
        public static Texture2D sLoad;
        public static Texture2D sMenu;
        public static Texture2D sClear;
        public static Texture2D sFail;
        public static Texture2D mapImages;
        public static Texture2D frogImage;
        public static Texture2D objectImages;
        public static Texture2D deadImage;
        public static Texture2D backgroundImage;
        public static Texture2D clearImage;
        public static Texture2D lastCrocImage;
        public static Texture2D dummyImage;

        public static SpriteFont font;
        public static SpriteFont menuFont;

        //���÷��� ����
        Vector2 displayOrigin = new Vector2(0, 32);


        public void initialize() {
            start = new Start();
            load = new Load();
            menu = new Menu();
            clear = new Clear();
            outStage = currentStage = 1;
            fail = new Fail();
        }


        public Selector update(GameTime gameTime) {
            switch (state) {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.LOAD:
                    state = load.update(gameTime, Keyboard.GetState());
                    if (state == State.PLAY) {
                        play = new Play();
                        play.initialize(currentStage);
                    }
                    break;
                case State.PLAY:
                    state = play.update(gameTime, Keyboard.GetState(), out currentStage);
                    break;
                case State.MENU:
                    state = menu.update(gameTime, Keyboard.GetState(), currentStage, out outStage);
                    currentStage = outStage;
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
            return Selector.FROGGER;
        }
        
        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            switch (state) {
                case State.START:
                    start.draw(spriteBatch, sStart, new Vector2(0, 0));
                    break;
                case State.LOAD:
                    load.draw(spriteBatch, sLoad, new Vector2(0, 0));
                    break;
                case State.PLAY:
                    play.draw(spriteBatch, font, frogImage, deadImage, mapImages, objectImages, backgroundImage, lastCrocImage, dummyImage, displayOrigin, gameTime);
                    break;
                case State.MENU:
                    menu.draw(spriteBatch, menuFont, sMenu, new Vector2(0, 0));
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
