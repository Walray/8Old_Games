using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _8Old_Games.Games.Hangman.Hangman
{
    public class Play : Sequence
    {
        String mousePos = "0,0";// 마우스위치디버깅
        
        String[] words = new string[227];
        String temp = "";
        String underbar = "_";
        string winovertext = ""; // 게임 승리여부 txt
        Color winTextColor = Color.Red;
      
        bool leftmouse = false;
        bool wordDone = false;

        string []arysel = new string [24];
        int index = 0;
        int txtcnt = 0;

        #region 잡동사니 변수
        bool gameOver = false;  // gameover
        int deadcnt = 0;

        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        #endregion


        int PmouseX = 0; // 게임중
        int PmouseY = 0; // 게임중

        bool[] isClicked=new bool[26]; //버튼이 클릭되었는가
        public override void initialize() {  ;  }

        public void initialize(int a)
        {

            readWord();

            if (wordDone == false)
            {
                Random rnd = new Random();
                temp = words[rnd.Next(words.Length)];
                wordDone = true;
            }
            for(int i = 0;i < 26;i++) isClicked[i] = false;

         
        }

        public void init()
        {
            winovertext = "";
            wordDone = false;
            leftmouse = false;
            deadcnt = 0;
            txtcnt = 0;
            index = 0;
        }

        public override State update(GameTime gameTime, KeyboardState ks)
        {
            #region 
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;

            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0)
            {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Space))
                {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU;
                }
                mTimeSinceLastInput = 0.0f;
            }
            #endregion

            if (gameOver) // 게임 끝났을 경우
            {                 
                // 스페이스 누르면 게임 재시작 
                if (ks.IsKeyDown(Keys.S))
                {
                    return State.START;
                }

            }


            if(deadcnt == 6)
            {
                Lose();
            }
           
            if(txtcnt == temp.Length && temp.Length!=0)
            {
                Win();
            }

            mousePos = string.Format("X : {0}, Y : {1}", Mouse.GetState().X, Mouse.GetState().Y); // 위치 디버깅
            
            for(int i=0; i<26;i++) {
                if(isClicked[i])
                Console.WriteLine("{0}", i);
            }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    leftmouse = true;
                    PmouseX = (Mouse.GetState().X);
                    PmouseY = (Mouse.GetState().Y);
                }

                else if (Mouse.GetState().LeftButton == ButtonState.Released && leftmouse == true)
                {

                    int PmouseXRelease = (Mouse.GetState().X);
                    int PmMouseYRelease = (Mouse.GetState().Y);
                    int idx = 0;
                    String cmp;

                if(PmouseX == PmouseXRelease && PmouseY == PmMouseYRelease) {
                    cmp = clickPosition(PmouseX, PmouseY,out idx);
                    if(cmp != "not" ) {
                        int pp = 1;
                        arysel[index] = cmp;
                        Console.WriteLine("{0}", isClicked[idx]);
                        if(!isClicked[idx]) {
                            if(idx < 26) isClicked[idx] = true;
                            index = index + 1;
                            deadcnt = deadcnt + 1;

                            for(int T = 0;T < temp.Length;T++) {

                                if(temp[T].ToString() == cmp) {
                                    txtcnt++;
                                    if(pp == 1)
                                        deadcnt = deadcnt - 1;
                                    pp = 0;
                                }

                            }
                        }
                    }


                    
                }

                leftmouse = false;
            }
            else if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                leftmouse = false;
            }

            

            return State.PLAY;

        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }

        public void draw(SpriteBatch spriteBatch, Texture2D scaffold, Texture2D a, Texture2D b, Texture2D c, Texture2D d, Texture2D e, Texture2D f, Texture2D g, Texture2D h, Texture2D i, Texture2D j, Texture2D k, Texture2D l, Texture2D n, Texture2D m, Texture2D o, Texture2D p, Texture2D q, Texture2D r, Texture2D s, Texture2D t, Texture2D u, Texture2D v, Texture2D w, Texture2D x, Texture2D y, Texture2D z, Texture2D clicked, SpriteFont font, Vector2 origin)
        {
            #region 알파벳 그리기

       
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

            #endregion

            #region  클릭함
            for (int K = 0; K < index; K++)
            {
                if (arysel[K] == "a")
                    spriteBatch.Draw(clicked, new Vector2(150, 300), Color.White);
                if (arysel[K] == "b")
                    spriteBatch.Draw(clicked, new Vector2(200, 300), Color.White);
                if (arysel[K] == "c")
                    spriteBatch.Draw(clicked, new Vector2(250, 300), Color.White);
                if (arysel[K] == "d")
                    spriteBatch.Draw(clicked, new Vector2(300, 300), Color.White);
                if (arysel[K] == "e")
                    spriteBatch.Draw(clicked, new Vector2(350, 300), Color.White);
                if (arysel[K] == "f")
                    spriteBatch.Draw(clicked, new Vector2(400, 300), Color.White);
                if (arysel[K] == "g")
                    spriteBatch.Draw(clicked, new Vector2(450, 300), Color.White);
                if (arysel[K] == "h")
                    spriteBatch.Draw(clicked, new Vector2(500, 300), Color.White);
                if (arysel[K] == "i")
                    spriteBatch.Draw(clicked, new Vector2(550, 300), Color.White);
                if (arysel[K] == "j")
                    spriteBatch.Draw(clicked, new Vector2(600, 300), Color.White);
                if (arysel[K] == "k")
                    spriteBatch.Draw(clicked, new Vector2(150, 350), Color.White);
                if (arysel[K] == "l")
                    spriteBatch.Draw(clicked, new Vector2(200, 350), Color.White);
                if (arysel[K] == "n")
                    spriteBatch.Draw(clicked, new Vector2(250, 350), Color.White);
                if (arysel[K] == "m")
                    spriteBatch.Draw(clicked, new Vector2(300, 350), Color.White);
                if (arysel[K] == "o")
                    spriteBatch.Draw(clicked, new Vector2(350, 350), Color.White);
                if (arysel[K] == "p")
                    spriteBatch.Draw(clicked, new Vector2(400, 350), Color.White);
                if (arysel[K] == "q")
                    spriteBatch.Draw(clicked, new Vector2(450, 350), Color.White);
                if (arysel[K] == "r")
                    spriteBatch.Draw(clicked, new Vector2(500, 350), Color.White);
                if (arysel[K] == "s")
                    spriteBatch.Draw(clicked, new Vector2(550, 350), Color.White);
                if (arysel[K] == "t")
                    spriteBatch.Draw(clicked, new Vector2(600, 350), Color.White);
                if (arysel[K] == "u")
                    spriteBatch.Draw(clicked, new Vector2(250, 400), Color.White);
                if (arysel[K] == "v")
                    spriteBatch.Draw(clicked, new Vector2(300, 400), Color.White);
                if (arysel[K] == "w")
                    spriteBatch.Draw(clicked, new Vector2(350, 400), Color.White);
                if (arysel[K] == "x")
                    spriteBatch.Draw(clicked, new Vector2(400, 400), Color.White);
                if (arysel[K] == "y")
                    spriteBatch.Draw(clicked, new Vector2(450, 400), Color.White);
                if (arysel[K] == "z")
                    spriteBatch.Draw(clicked, new Vector2(500, 400), Color.White);

            }
            #endregion

            #region
            underBar(spriteBatch, font); 

       //     spriteBatch.DrawString(font, temp, new Vector2(30, 30), Color.Yellow);

     //       spriteBatch.DrawString(font, mousePos, new Vector2(400, 400), Color.Yellow);

            #endregion

    
            correct(spriteBatch, font);
           
        //    spriteBatch.DrawString(font, txtcnt.ToString(), new Vector2(300, 400), Color.Yellow);
        //    spriteBatch.DrawString(font, deadcnt.ToString(), new Vector2(320, 400), Color.Yellow);


            #region 승리판별
            if (winovertext != "")
            {
                spriteBatch.DrawString(font, winovertext, new Vector2(500, 250), winTextColor, 0, font.MeasureString(winovertext), 1.5f, SpriteEffects.None, 1.0f);
                spriteBatch.DrawString(font, "    press 'S' to restart", new Vector2(300, 280), winTextColor, 0, font.MeasureString(winovertext), 1.5f, SpriteEffects.None, 1.0f);

            }
            #endregion

        }

        #region 핵맨
        public void draw2(SpriteBatch spriteBatch, Texture2D scaffold, Texture2D head, Texture2D rigth_hand, Texture2D left_hand, Texture2D body, Texture2D rigth_foot, Texture2D left_foot)
        {             
             
           
             spriteBatch.Draw(scaffold, new Vector2(450+50, 5), Color.White);     
            
            if( deadcnt == 1)
            spriteBatch.Draw(head, new Vector2(515+50, 60), Color.White); //1

            if (deadcnt == 2)
            {
                spriteBatch.Draw(head, new Vector2(515 + 50, 60), Color.White); //1
                spriteBatch.Draw(body, new Vector2(577 + 50, 149), Color.White); //2
            }
            if (deadcnt == 3)
            {
                spriteBatch.Draw(head, new Vector2(515 + 50, 60), Color.White); //1
                spriteBatch.Draw(body, new Vector2(577 + 50, 149), Color.White); //2
                spriteBatch.Draw(rigth_hand, new Vector2(589 + 50, 136), Color.White); //3
            }
            if (deadcnt == 4)
            {
                spriteBatch.Draw(head, new Vector2(515 + 50, 60), Color.White); //1
                spriteBatch.Draw(body, new Vector2(577 + 50, 149), Color.White); //2
                spriteBatch.Draw(rigth_hand, new Vector2(589 + 50, 136), Color.White); //3
                spriteBatch.Draw(left_hand, new Vector2(552 + 50, 163), Color.White); //4
            }

            if (deadcnt == 5)
            {
                spriteBatch.Draw(head, new Vector2(515 + 50, 60), Color.White); //1
                spriteBatch.Draw(body, new Vector2(577 + 50, 149), Color.White); //2
                spriteBatch.Draw(rigth_hand, new Vector2(589 + 50, 136), Color.White); //3
                spriteBatch.Draw(left_hand, new Vector2(552 + 50, 163), Color.White); //4
                spriteBatch.Draw(rigth_foot, new Vector2(603 + 50, 203), Color.White); //5

            }

            if (deadcnt == 6)
            {
                spriteBatch.Draw(head, new Vector2(515 + 50, 60), Color.White); //1
                spriteBatch.Draw(body, new Vector2(577 + 50, 149), Color.White); //2
                spriteBatch.Draw(rigth_hand, new Vector2(589 + 50, 136), Color.White); //3
                spriteBatch.Draw(left_hand, new Vector2(552 + 50, 163), Color.White); //4
                spriteBatch.Draw(rigth_foot, new Vector2(603 + 50, 203), Color.White); //5
                spriteBatch.Draw(left_foot, new Vector2(577 + 50, 207), Color.White); //6
            }


        }
        #endregion

        void readWord()
        {
            int counter = 0;
            string line;

            System.IO.StreamReader file =
                new System.IO.StreamReader(".\\영어단어.txt");

            while ((line = file.ReadLine()) != null)
            {
                words[counter] =line;
                counter++;
            }

            file.Close();


        }

        void underBar(SpriteBatch spriteBatch, SpriteFont font)
        {
            int cnt = 0;
            int x = 100;
          while ( temp.Length!=cnt)
          {
                spriteBatch.DrawString(font, underbar, new Vector2(x, 230), Color.Black, 0, font.MeasureString(underbar), 3.0f, SpriteEffects.None, 1.0f);
                cnt++;
                x += 50;
             }


        }
 
        void correct(SpriteBatch spriteBatch, SpriteFont font)
        {
            int g;
            if (clickPosition(PmouseX, PmouseY,out g) != "not")
            {
                for (int T = 0; T < temp.Length; T++)
                {
                    if (index > 0)
                    {
                        for (int K = 0; K < index; K++)
                        {
                            if (temp[T].ToString() == arysel[K])
                            {

                                spriteBatch.DrawString(font, temp[T].ToString(), new Vector2((75 + (50 * T)), 175), Color.Black);
                                
                            }
                        }
                    }
                 
                }

            
            }
        }
        
        
        string clickPosition(int PmouseX, int PmouseY, out int idx)
        {

            string[] abc = {  "a", "b" ,"c","d","e","f","g","h","i","j", "k", "l", "n", "m", "o", "p", "q", "r", "s", "t" , "u", "v", "w", "x", "y", "z" };
            int x = 150;
            int y = 300;
            int cnt=0;
            
            for(cnt=0; cnt<26;cnt++) {
             
                if(cnt == 0) {
                    x = 150; y = 300;
                }
                else if(cnt == 10) {
                    x = 150; y = 350;
                }
                else if(cnt == 20) {
                    x = 250; y = 400;
                }

                if(x < PmouseX && PmouseX < (x + 40) && y < PmouseY && PmouseY < (y + 40)) {
                    idx = cnt;
                    return abc[cnt]; 
                }
                x += 50;
            }
            idx = cnt;
            return "not";
        }


        // 이겻을때
        private void Win()
        {
            winTextColor = Color.Blue;
            winovertext = "You win!";
            gameOver = true;

        }

        // 졌을 때
        private void Lose()
        {
            winTextColor = Color.Red;
            winovertext = "Game Over";
            gameOver = true;

        }



    }
}