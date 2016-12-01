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



        public Hangman()
        {;

        }

        public void initialize()
        {;




        }

        public Selector update(GameTime gameTime)
        {

            return Selector.MAIN_SELECTOR; }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Draw(a, new Vector2(150, 300), Color.White);
            spriteBatch.Draw(b, new Vector2(200, 300), Color.White);
            spriteBatch.Draw(c, new Vector2(250, 300), Color.White);
            spriteBatch.Draw(d, new Vector2(300, 300), Color.White);
            spriteBatch.Draw(e, new Vector2(350, 300), Color.White);
            spriteBatch.Draw(f, new Vector2(400, 300), Color.White);
            spriteBatch.Draw(g, new Vector2(450, 300), Color.White);
            spriteBatch.Draw(h, new Vector2(500, 300), Color.White);
            spriteBatch.Draw(i, new Vector2(550, 300), Color.White);
            spriteBatch.Draw(j, new Vector2(600, 300), Color.White);
            spriteBatch.Draw(k, new Vector2(150, 350), Color.White);
            spriteBatch.Draw(l, new Vector2(200, 350), Color.White);
            spriteBatch.Draw(n, new Vector2(250, 350), Color.White);
            spriteBatch.Draw(m, new Vector2(300, 350), Color.White);
            spriteBatch.Draw(o, new Vector2(350, 350), Color.White);
            spriteBatch.Draw(p, new Vector2(400, 350), Color.White);
            spriteBatch.Draw(q, new Vector2(450, 350), Color.White);
            spriteBatch.Draw(r, new Vector2(500, 350), Color.White);
            spriteBatch.Draw(s, new Vector2(550, 350), Color.White);
            spriteBatch.Draw(t, new Vector2(600, 350), Color.White);
            spriteBatch.Draw(u, new Vector2(250, 400), Color.White);
            spriteBatch.Draw(v, new Vector2(300, 400), Color.White);
            spriteBatch.Draw(w, new Vector2(350, 400), Color.White);
            spriteBatch.Draw(x, new Vector2(400, 400), Color.White);
            spriteBatch.Draw(y, new Vector2(450, 400), Color.White);
            spriteBatch.Draw(z, new Vector2(500, 400), Color.White);



        }




    }
}
