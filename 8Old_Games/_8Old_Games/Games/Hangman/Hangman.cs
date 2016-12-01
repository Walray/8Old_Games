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
using _8Old_Games.Games.Hangman.Sequence;
//시퀀스 만들고 풀어주세요


namespace _8Old_Games.Games.Hangman {
    public enum State { START, SELECTION, PLAY, MENU, EXIT, NOTHING }; //자유롭게 추가

    public class Hangman {
        State state;



        public static SpriteFont font; // 폰트
        public static Texture2D a;
        public static Texture2D b;
        public static Texture2D c; 
        public static Texture2D d; // 1
        public static Texture2D e; // 2
        public static Texture2D f; // 3
        public static Texture2D g; // 4
        public static Texture2D h; // 5
        public static Texture2D i; // 6
        public static Texture2D j; // 폭탄
        public static Texture2D k; // 클릭"전"
        public static Texture2D l; // 깃발
        public static Texture2D n; // 시작화면
        public static Texture2D m; // 메뉴화면
        public static Texture2D o; // 메뉴화면
        public static Texture2D p; // 메뉴화면
        public static Texture2D q; // 클릭"전"
        public static Texture2D r; // 깃발
        public static Texture2D s; // 시작화면
        public static Texture2D t; // 메뉴화면
        public static Texture2D u; // 메뉴화면
        public static Texture2D v; // 메뉴화면
        public static Texture2D w; // 클릭"전"
        public static Texture2D x; // 깃발
        public static Texture2D y; // 시작화면
        public static Texture2D z; // 메뉴화면



        public Hangman() {; }
        public void initialize() {; }

        public Selector update(GameTime gameTime) { return Selector.MAIN_SELECTOR; }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {; }
    }
}
