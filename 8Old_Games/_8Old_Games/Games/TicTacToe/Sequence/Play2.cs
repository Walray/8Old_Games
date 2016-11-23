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

namespace _8Old_Games.Games.TicTacToe.Sequence {
    public class Play2 : Sequence {

        MouseState mouseState;
        KeyboardState keyboardState;
        Point mousePosition;
        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;


        Rectangle[] cell; // 밑에 foreach문에서 돌리기위해 사용. Rectangle정보를 배열에 담음
        List<Rectangle> p1List; // player1의 List 여기에 클릭한 정보를 다 담는다.
        List<Rectangle> comList; // computerPlayer의 List 여기에 컴퓨터가 랜덤으로 선택한 정보를 담는다.
        Rectangle[][] winStates; // 승리여부를 담는 변수

        bool playerOneTurn = true; // 턴을 변경할 때 사용.
        bool drawCells = false; // 그리냐 마냐 판별할때 사용
        bool gameEnded = false; // 게임 끝났는지 판별할때 사용
        bool playerOneWon = false; // player가 이겼을 때 
        bool playerComWon = false; // computer가 이겼을 때
        public const int blocksize = 160; // 하나의 square 가로 130, 세로 130
        Random rand = new Random(); // 컴퓨터 차례일때 랜덤으로 놓기 위해 선언
        int num = 0;
        int comCount = 1;
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);


        public Play2() : base() {; }
        public override void initialize() {
            // rectangle containing all the visible game elements            
            cell = new Rectangle[] { new Rectangle(0, 0, blocksize, blocksize),     //0,0
                                     new Rectangle(blocksize, 0, blocksize, blocksize),    //1,0
                                     new Rectangle(blocksize*2, 0, blocksize, blocksize),    //2,0
                                     new Rectangle(0, blocksize, blocksize, blocksize),    //0,1
                                     new Rectangle(blocksize, blocksize, blocksize, blocksize),  //1,1
                                     new Rectangle(blocksize*2, blocksize, blocksize, blocksize),  //2,1
                                     new Rectangle(0, blocksize*2, blocksize, blocksize),    //0,2
                                     new Rectangle(blocksize, blocksize*2, blocksize, blocksize),  //1,2
                                     new Rectangle(blocksize*2, blocksize*2, blocksize, blocksize)}; //2,2

            winStates = new Rectangle[][] { new Rectangle[] { cell[0], cell[1], cell[2], },   //top row
                                            new Rectangle[] { cell[3], cell[4], cell[5], },   //middle row
                                            new Rectangle[] { cell[6], cell[7], cell[8], },   //bottom row
                                            new Rectangle[] { cell[0], cell[3], cell[6], },   //left column
                                            new Rectangle[] { cell[1], cell[4], cell[7], },   //middle column
                                            new Rectangle[] { cell[2], cell[5], cell[8], },   //right column
                                            new Rectangle[] { cell[0], cell[4], cell[8], },   //diagonal from top left to bottom right
                                            new Rectangle[] { cell[2], cell[4], cell[6]} }; //diagonal from top right to bottom left



            Rectangle center = new Rectangle(blocksize, blocksize, blocksize, blocksize);
            Rectangle edgeCase1 = new Rectangle(0, 0, blocksize, blocksize);
            Rectangle edgeCase2 = new Rectangle(blocksize * 2, 0, blocksize, blocksize);
            Rectangle edgeCase3 = new Rectangle(0, blocksize * 2, blocksize, blocksize);
            Rectangle edgeCase4 = new Rectangle(blocksize * 2, blocksize * 2, blocksize, blocksize);
            Rectangle[] randRect = { edgeCase1, edgeCase2, edgeCase3, edgeCase4, };

            Rectangle case0 = new Rectangle(0, 0, blocksize, blocksize);     //0,0
            Rectangle case1 = new Rectangle(blocksize, 0, blocksize, blocksize);    //1,0
            Rectangle case2 = new Rectangle(blocksize * 2, 0, blocksize, blocksize);   //2,0
            Rectangle case3 = new Rectangle(0, blocksize, blocksize, blocksize);   //0,1
            Rectangle case4 = new Rectangle(blocksize, blocksize, blocksize, blocksize);  //1,1
            Rectangle case5 = new Rectangle(blocksize * 2, blocksize, blocksize, blocksize); //2,1
            Rectangle case6 = new Rectangle(0, blocksize * 2, blocksize, blocksize);    //0,2
            Rectangle case7 = new Rectangle(blocksize, blocksize * 2, blocksize, blocksize);  //1,2
            Rectangle case8 = new Rectangle(blocksize * 2, blocksize * 2, blocksize, blocksize);

            // 각각의 플레이어가 이동하는 각 선수의 리스트(셀)
            p1List = new List<Rectangle>();
            comList = new List<Rectangle>();

            playerOneTurn = true;
            playerOneWon = false;
            playerComWon = false;
            gameEnded = false;
            comCount = 1;

        }

        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU2;
                }
                mTimeSinceLastInput = 0.0f;
            }

            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            mousePosition = new Point(mouseState.X, mouseState.Y);

            if (playerOneTurn) // 플레이어1 차례일때 
            {
                if (mouseState.LeftButton == ButtonState.Pressed && !gameEnded) {
                    while (mouseState.LeftButton == ButtonState.Pressed) {
                        mouseState = Mouse.GetState();
                    }

                    foreach (Rectangle r in cell) {
                        if (r.Intersects(new Rectangle(mouseState.X, mouseState.Y, 0, 0))) {
                            if (!p1List.Contains(r) && !comList.Contains(r)) {
                                p1List.Add(r);
                                playerOneTurn = !playerOneTurn;
                            }
                        }
                    }

                }
            } else if (!playerOneTurn) // 컴퓨터 차례일 때
              {
                Rectangle center = new Rectangle(blocksize, blocksize, blocksize, blocksize);
                Rectangle edgeCase1 = new Rectangle(0, 0, blocksize, blocksize);
                Rectangle edgeCase2 = new Rectangle(blocksize * 2, 0, blocksize, blocksize);
                Rectangle edgeCase3 = new Rectangle(0, blocksize * 2, blocksize, blocksize);
                Rectangle edgeCase4 = new Rectangle(blocksize * 2, blocksize * 2, blocksize, blocksize);
                Rectangle[] randRect = { edgeCase1, edgeCase2, edgeCase3, edgeCase4, };
                int i = rand.Next(3);
                int j = rand.Next(3);
                int[] rand_num = { 0, blocksize * 1, blocksize * 2 };
                Rectangle test = new Rectangle(rand_num[i], rand_num[j], blocksize, blocksize);

                if (!p1List.Contains(center) && !comList.Contains(center)) // 가장 가운데 부터 먼저 차지하자!
                {
                    comCount++;
                    comList.Add(center);
                    playerOneTurn = !playerOneTurn;
                }
                else // 가운데 이미 상대방이 점령했다면 모서리를 노리자!
                {
                    if (!p1List.Contains(randRect[i]) && !comList.Contains(randRect[i]) && comCount == 1)
                    {
                        comCount++; //  카운트 증가
                        comList.Add(randRect[i]);
                        playerOneTurn = !playerOneTurn;
                    }
                    else if (comCount == 2 || comCount == 3) // 2번 이상 카운트 된 경우
                    {
                        comCount++;
                        comStateCheck();
                        playerOneTurn = !playerOneTurn;
                    }
                    else
                    {
                        if (!comList.Contains(test) && !p1List.Contains(test))
                        {
                            comCount++;
                            comList.Add(test);
                            playerOneTurn = !playerOneTurn;
                        }
                    }
                }
            }             

            if (p1List.Count != 0 || comList.Count != 0) // 하나씩 리스트에 생기면 그리기 시작한다.
                drawCells = true;

            if (p1List.Count + comList.Count == 9) // 9칸 모두 다 차면 게임종료
                gameEnded = true;

            CheckWinState(); // 승자 상태 체크

            return State.PLAY2;
        }
        /* winner 체크 함수 */
        private void CheckWinState() {
            int n = 0;

            for (int i = 0; i < 8; i++) {
                foreach (Rectangle r in winStates[i]) {
                    if (p1List.Contains(r)) {
                        n++;
                        if (n == 3) // 한줄 완성되면 게임 끝!
                        {
                            n = 0;
                            playerOneWon = true;
                            gameEnded = true;
                            break;
                        }
                    }
                }
                n = 0;
            }

            for (int i = 0; i < 8; i++) {
                foreach (Rectangle r in winStates[i]) {
                    if (comList.Contains(r)) {
                        n++;
                        if (n == 3) // 한줄 완성되면 게임 끝!
                        {
                            n = 0;
                            playerComWon = true;
                            gameEnded = true;
                            break;
                        }
                    }
                }
                n = 0;
            }
        }

        // 컴퓨터 체크 함수        
        private void comStateCheck()
        {

            Rectangle case0 = new Rectangle(0, 0, blocksize, blocksize);     //0,0
            Rectangle case1 = new Rectangle(blocksize, 0, blocksize, blocksize);    //1,0
            Rectangle case2 = new Rectangle(blocksize * 2, 0, blocksize, blocksize);   //2,0
            Rectangle case3 = new Rectangle(0, blocksize, blocksize, blocksize);   //0,1
            Rectangle case4 = new Rectangle(blocksize, blocksize, blocksize, blocksize);  //1,1
            Rectangle case5 = new Rectangle(blocksize * 2, blocksize, blocksize, blocksize); //2,1
            Rectangle case6 = new Rectangle(0, blocksize * 2, blocksize, blocksize);    //0,2
            Rectangle case7 = new Rectangle(blocksize, blocksize * 2, blocksize, blocksize);  //1,2
            Rectangle case8 = new Rectangle(blocksize * 2, blocksize * 2, blocksize, blocksize); // 2,2

            int[] rand_num = { 0, blocksize * 1, blocksize * 2 };
            int i = rand.Next(3);
            int j = rand.Next(3);

            Rectangle test = new Rectangle(rand_num[i], rand_num[j], blocksize, blocksize);

            // 행 공격  ex) 컴퓨터가 배열 0자리랑 1자리에 체크했는데 2자리가 비어있으면 2자리 체크한다.
            if (comList.Contains(case0) && comList.Contains(case1) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            else if (comList.Contains(case3) && comList.Contains(case4) && !comList.Contains(case5) && !p1List.Contains(case5))
                comList.Add(case5);
            else if (comList.Contains(case6) && comList.Contains(case7) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (comList.Contains(case0) && comList.Contains(case2) && !comList.Contains(case1) && !p1List.Contains(case1))
                comList.Add(case1);
            else if (comList.Contains(case3) && comList.Contains(case5) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (comList.Contains(case6) && comList.Contains(case8) && !comList.Contains(case7) && !p1List.Contains(case7))
                comList.Add(case7);
            else if (comList.Contains(case1) && comList.Contains(case2) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (comList.Contains(case4) && comList.Contains(case5) && !comList.Contains(case3) && !p1List.Contains(case3))
                comList.Add(case3);
            else if (comList.Contains(case7) && comList.Contains(case8) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            // 열 공격
            else if (comList.Contains(case0) && comList.Contains(case3) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            else if (comList.Contains(case1) && comList.Contains(case4) && !comList.Contains(case7) && !p1List.Contains(case7))
                comList.Add(case7);
            else if (comList.Contains(case2) && comList.Contains(case5) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (comList.Contains(case0) && comList.Contains(case6) && !comList.Contains(case3) && !p1List.Contains(case3))
                comList.Add(case3);
            else if (comList.Contains(case1) && comList.Contains(case7) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (comList.Contains(case2) && comList.Contains(case8) && !comList.Contains(case5) && !p1List.Contains(case5))
                comList.Add(case5);
            else if (comList.Contains(case3) && comList.Contains(case6) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (comList.Contains(case4) && comList.Contains(case7) && !comList.Contains(case1) && !p1List.Contains(case1))
                comList.Add(case1);
            else if (comList.Contains(case5) && comList.Contains(case8) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            // 대각선 공격
            else if (comList.Contains(case0) && comList.Contains(case4) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (comList.Contains(case2) && comList.Contains(case4) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            else if (comList.Contains(case2) && comList.Contains(case6) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (comList.Contains(case0) && comList.Contains(case8) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (comList.Contains(case4) && comList.Contains(case8) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (comList.Contains(case4) && comList.Contains(case6) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            // 행 수비 ex) 상대방이 배열 0자리랑 1자리에 놓았는데 2자리가 비어있으면 3개완성이 위험! 2자리에 체크해서 수비
            else if (p1List.Contains(case0) && p1List.Contains(case1) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            else if (p1List.Contains(case3) && p1List.Contains(case4) && !comList.Contains(case5) && !p1List.Contains(case5))
                comList.Add(case5);
            else if (p1List.Contains(case6) && p1List.Contains(case7) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (p1List.Contains(case0) && p1List.Contains(case2) && !comList.Contains(case1) && !p1List.Contains(case1))
                comList.Add(case1);
            else if (p1List.Contains(case3) && p1List.Contains(case5) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (p1List.Contains(case6) && p1List.Contains(case8) && !comList.Contains(case7) && !p1List.Contains(case7))
                comList.Add(case7);
            else if (p1List.Contains(case1) && p1List.Contains(case2) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (p1List.Contains(case4) && p1List.Contains(case5) && !comList.Contains(case3) && !p1List.Contains(case3))
                comList.Add(case3);
            else if (p1List.Contains(case7) && p1List.Contains(case8) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            // 열 수비
            else if (p1List.Contains(case0) && p1List.Contains(case3) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            else if (p1List.Contains(case1) && p1List.Contains(case4) && !comList.Contains(case7) && !p1List.Contains(case7))
                comList.Add(case7);
            else if (p1List.Contains(case2) && p1List.Contains(case5) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (p1List.Contains(case0) && p1List.Contains(case6) && !comList.Contains(case3) && !p1List.Contains(case3))
                comList.Add(case3);
            else if (p1List.Contains(case1) && p1List.Contains(case7) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (p1List.Contains(case2) && p1List.Contains(case8) && !comList.Contains(case5) && !p1List.Contains(case5))
                comList.Add(case5);
            else if (p1List.Contains(case3) && p1List.Contains(case6) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (p1List.Contains(case4) && p1List.Contains(case7) && !comList.Contains(case1) && !p1List.Contains(case1))
                comList.Add(case1);
            else if (p1List.Contains(case5) && p1List.Contains(case8) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            // 대각선 수비
            else if (p1List.Contains(case0) && p1List.Contains(case4) && !comList.Contains(case8) && !p1List.Contains(case8))
                comList.Add(case8);
            else if (p1List.Contains(case2) && p1List.Contains(case4) && !comList.Contains(case6) && !p1List.Contains(case6))
                comList.Add(case6);
            else if (p1List.Contains(case2) && p1List.Contains(case6) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (p1List.Contains(case0) && p1List.Contains(case8) && !comList.Contains(case4) && !p1List.Contains(case4))
                comList.Add(case4);
            else if (p1List.Contains(case4) && p1List.Contains(case8) && !comList.Contains(case0) && !p1List.Contains(case0))
                comList.Add(case0);
            else if (p1List.Contains(case4) && p1List.Contains(case6) && !comList.Contains(case2) && !p1List.Contains(case2))
                comList.Add(case2);
            else // 위에 모두 속하지 않을때 그냥 랜덤으로 놓는다.
            {
                if (!comList.Contains(test) && !p1List.Contains(test))
                {
                    comList.Add(test);
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite) {; }
        public void draw(SpriteBatch spriteBatch, Texture2D square, Texture2D cross, Texture2D zero, SpriteFont spriteFont, SpriteFont spriteFont2) {

            /* square로 3*3칸 그린다 */
            for (int y = 0; y < 3; y++) {
                for (int x = 0; x < 3; x++) {
                    if (x % 2 == 0 && y % 2 == 0)
                        spriteBatch.Draw(square, new Rectangle(x * blocksize, y * blocksize, blocksize, blocksize), Color.Khaki);
                    else if (x == y)
                        spriteBatch.Draw(square, new Rectangle(x * blocksize, y * blocksize, blocksize, blocksize), Color.Khaki);
                    else
                        spriteBatch.Draw(square, new Rectangle(x * blocksize, y * blocksize, blocksize, blocksize), Color.White);
                }
            }

            /* o와 x가 그려지는 부분 */
            if (drawCells) // drawCells이 true이면 그린다.
            {
                foreach (Rectangle r in p1List) {
                    spriteBatch.Draw(cross, r, Color.White); // player는 x로 그린다.
                }


                foreach (Rectangle r in comList) {
                        spriteBatch.Draw(zero, r, Color.White); // computer는 o로 그린다.
                    
                }
            }

            if(gameEnded) // 게임이 종료되면 종료메세지 출력
            spriteBatch.DrawString(spriteFont2, "Game over!", new Vector2(570, 400), Color.HotPink);
            spriteBatch.DrawString(spriteFont, "Winner is :) ", new Vector2(550, 100), Color.Yellow);
            /* 게임 결과 출력 부분 */
            if (playerOneWon && gameEnded) // 게임 종료되고, 플레이어1이 이겼으면
            {
                spriteBatch.DrawString(spriteFont, "\n  Player!!!", new Vector2(550, 100), Color.Yellow);
            }
            else if (playerComWon && gameEnded) // 게임 종료되고 컴퓨터가 이겼으면
            {
                spriteBatch.DrawString(spriteFont, "\n  Computer!!!", new Vector2(550, 100), Color.Yellow);
            }
            else if (gameEnded && !playerOneWon && !playerComWon) // 게임 종료되었는데 둘 다 안이겼으면 비김
            {
                spriteBatch.DrawString(spriteFont, "\n  Draw", new Vector2(550, 100), Color.Yellow);
            }               
        }

    }
}
