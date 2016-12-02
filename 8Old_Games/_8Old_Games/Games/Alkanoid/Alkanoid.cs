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
using _8Old_Games.Games.Alkanoid.Sequence;

/*
 * 초기화(initialize), 논리(update), 그리고 그리기(draw) 코드 반드시 분리
 * 필요하면 함수 원형 자유롭게 변경
 *
*/
namespace _8Old_Games.Games.Alkanoid {
    public enum State { START, SELECTION, PLAY, MENU, EXIT, NOTHING, LOAD }; //자유롭게 추가
    public class Alkanoid {
        // 시퀀스 변수들
        State state;
        Start start;
        Play play;
        Menu menu;
        Load load;

        // 게임에서 사용할 리소스
        public static Texture2D pad; // 막대기
        public static Texture2D ball; // 공
        public static Texture2D bricks; // 벽돌
        // 메뉴 리소스
        public static Texture2D sStart; // 시작화면
        public static Texture2D sLoad; // 로딩화면 
        public static Texture2D sMenu; // 메뉴화면
        public static SpriteFont Arial;
        public static SpriteFont Arial2;

        /*
        모든 리소스를 담는 변수는 public static으로 선언 ex) 
        public static Texture2D selectSize;
        */
        public Alkanoid() {; }
        public void initialize() {
            start = new Start();
            menu = new Menu();
            state = State.START;
            load = new Load();
        }

        public Selector update(GameTime gameTime) {
            switch (state){
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    break;
                case State.PLAY:
                    state = play.update(gameTime, Keyboard.GetState());
                    break;
                case State.MENU:
                    state = menu.update(gameTime, Keyboard.GetState());
                    break;
                case State.EXIT:
                    return Selector.MAIN_SELECTOR;
                case State.LOAD:
                    play = new Play();
                    play.initialize();
                    state = load.update(gameTime, Keyboard.GetState());
                    break;

            }
            return Selector.MAIN_SELECTOR;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {
            switch (state)
            {
                case State.START:
                    start.draw(spriteBatch, sStart, new Vector2(0, 0));
                    break;
                case State.LOAD:
                    load.draw(spriteBatch, sLoad, new Vector2(0, 0));
                    break;
                case State.PLAY:
                    play.draw(gameTime, Arial, Arial2, bricks);
                    break;
                case State.MENU:
                    menu.draw(spriteBatch, sMenu, new Vector2(0, 0));
                    break;
            }
       }
    }
}
