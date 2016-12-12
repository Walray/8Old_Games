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
using _8Old_Games.Games.Hangman.Hangman;


/// <summary>
/// 이미지 선언 
/// </summary>

namespace _8Old_Games.Games.Hangman {

    public enum State { START, PLAY, MENU, EXIT, NOTHING }; //자유롭게 추가

    public class HangMan {


        State state;
        Start start;
        Play play;
        Menu menu;

        int inti = 0; //override 함수랑 함수명이랑 변수 같으면 안되서 

        #region 시작및메뉴
        public static Texture2D startImage; // 시작화면
        public static Texture2D menuImage; // 메뉴화면
        #endregion

        #region 알파벳 이미지
        public static SpriteFont font; // 폰트
        public static Texture2D a;
        public static Texture2D b;
        public static Texture2D c; 
        public static Texture2D d; 
        public static Texture2D e; 
        public static Texture2D f; 
        public static Texture2D g; 
        public static Texture2D h; 
        public static Texture2D i; 
        public static Texture2D j;
        public static Texture2D k; 
        public static Texture2D l;
        public static Texture2D n;
        public static Texture2D m; 
        public static Texture2D o;
        public static Texture2D p; 
        public static Texture2D q; 
        public static Texture2D r; 
        public static Texture2D s; 
        public static Texture2D t; 
        public static Texture2D u; 
        public static Texture2D v; 
        public static Texture2D w; 
        public static Texture2D x; 
        public static Texture2D y; 
        public static Texture2D z;
        public static Texture2D clicked;
        #endregion

        #region 매달린남자

        public static Texture2D scaffold;
        public static Texture2D head;
        public static Texture2D rigth_hand;
        public static Texture2D left_hand;
        public static Texture2D body;
        public static Texture2D rigth_foot;
        public static Texture2D left_foot;

        #endregion

        public HangMan() {; }

        public void initialize()
        {
            start = new Start();
            play = new Play();
            menu = new Menu();
            state = State.START;

        }

        public Selector update(GameTime gameTime)
        {

            switch (state)
            {
                case State.START:
                    state = start.update(gameTime, Keyboard.GetState());
                    play.init();
                    play.initialize(inti);
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

            return Selector.HANGMAN; }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            switch (state)
            {
                case State.START:
                    start.draw(spriteBatch, startImage, new Vector2(0, 0));
                    play.init();
                    play.initialize(inti);
                    break;
                case State.PLAY:
                       play.draw(spriteBatch, scaffold, a,b,c,d,e,f,g,h,i,j,k,l,n,m,o,p,q,r,s,t,u,v,w,x,y,z,clicked, font, new Vector2(0, 0)); // error
                    play.draw2(spriteBatch, scaffold,  head,  rigth_hand,  left_hand,  body,  rigth_foot,  left_foot);
                    break;
                case State.MENU:
                    menu.draw(spriteBatch, menuImage, Vector2.Zero);
                    break;
            }



        }




    }
}
