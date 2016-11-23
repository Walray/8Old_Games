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
    public class Play1 : Sequence {

        KeyboardState keyboardState;
        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        Rectangle[] cell; // 스퀘어 배열
        List<Rectangle> P1List; // 플레이어 1이 선택한 곳이 담길 리스트
        List<Rectangle> P2List; // 플레이어 2가 선택한 곳이 담길 리스크
        Rectangle[][] winStates; // 승리 조건이 담긴 배열

        /* 플래그 초기화 */
        bool playerOneTurn = true; // turn을 변경 하는 플래그
        bool drawCells = false; // draw를 할것인가?
        bool gameEnded = false; // 게임이 종료되었는가?
        bool playerOneWon = false; // player 1이 이겼는가?
        bool playerTwoWon = false; // player 2가 이겼는가?
        public const int blocksize = 160; // 하나의 스퀘어 사이즈 크기 지정

        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        public Play1() : base() {; }
        public override void initialize() {
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
                                            new Rectangle[] { cell[2], cell[4], cell[6], } }; //diagonal from top right to bottom left


            // lists conaining each players moves (clicked cells)
            P1List = new List<Rectangle>();
            P2List = new List<Rectangle>();

            playerOneTurn = true;
            playerOneWon = false;
            playerTwoWon = false;
            gameEnded = false;

        }

        public override State update(GameTime gameTime, KeyboardState ks) {

            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU1;
                }
                mTimeSinceLastInput = 0.0f;
            }

            MouseState mouseState= Mouse.GetState();
            keyboardState = Keyboard.GetState();
            
            
            if (mouseState.LeftButton == ButtonState.Pressed && !gameEnded) {
                while (mouseState.LeftButton == ButtonState.Pressed) {
                    mouseState = Mouse.GetState();
                }
                foreach (Rectangle r in cell)
                { // cell 전체를 돌면서
                    if (r.Intersects(new Rectangle(mouseState.X, mouseState.Y, 0, 0)))
                    { // 클릭한 곳을 담는다
                        if (playerOneTurn)
                        { // player 1 차례인 경우
                            if (!P1List.Contains(r) && !P2List.Contains(r))
                            { // 만약에 player 1이 누른 곳이 중복안되고, player2에도 포함안되었으면
                                P1List.Add(r); // player 1 리스트에 추가한다.
                                playerOneTurn = !playerOneTurn; // 차례를 상대방으로 변경
                            }
                        }
                        else
                        { // player 2 차례인 경우
                            if (!P1List.Contains(r) && !P2List.Contains(r))
                            { // 만약에 player 1이 누른 곳이 중복안되고, player2에도 포함안되었으면
                                P2List.Add(r); // player 2 리스트에 추가한다.
                                playerOneTurn = !playerOneTurn; // 차례를 상대방으로 변경
                            }
                        }
                    }
                }



                if (P1List.Count != 0 || P2List.Count != 0) // player 1과 2리스트 모두 하나라도 들어 있으면
                    drawCells = true; // 그리기 시작

                if (P1List.Count + P2List.Count == 9) // player 1과 player 2 리스트를 더했을 때 9가 되면 게임 종료
                    gameEnded = true;

                CheckWinState(); // 판에다 그렸고, 게임종료 상태도 아니면 게임 승리 조건 체크로 넘어간다.
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && gameEnded)
            { // 게임 종료된 후에 다시 클릭하면 재시작
                while (mouseState.LeftButton == ButtonState.Pressed) {
                    mouseState = Mouse.GetState();
                }

                P1List = new List<Rectangle>();
                P2List = new List<Rectangle>();
                playerOneTurn = true;
                playerOneWon = false;
                playerTwoWon = false;
                gameEnded = false;
            }


            return State.PLAY1;
        }
        /* 승리 조건 체크 함수 */
        private void CheckWinState() {
            int n = 0; // 카운트 할 변수 

            for (int i = 0; i < 8; i++) {
                foreach (Rectangle r in winStates[i])
                { // 위의 승리조건일 때 배열을 돌면서 속하는지 체크
                    if (P1List.Contains(r)) { // 만약 player 1 리스트에 포함되어 있으면
                        n++;// 하나씩 증가
                        if (n == 3) {// 그렇게 3개가 모두 속한다면
                            n = 0;
                            playerOneWon = true; // player 1이 승리하고,
                            gameEnded = true; // 게임 종료
                            break;
                        }
                    }
                }
                n = 0;
            }

            for (int i = 0; i < 8; i++) {
                foreach (Rectangle r in winStates[i])
                {  // 위의 승리조건일 때 배열을 돌면서 속하는지 체크
                    if (P2List.Contains(r)) {// 만약 player 2 리스트에 포함되어 있으면
                        n++; // 하나씩 증가
                        if (n == 3) { // 그렇게 3개가 모두 속한다면
                            n = 0;
                            playerTwoWon = true; // player 2가 승리
                            gameEnded = true; // 게임 종료
                            break;
                        }
                    }
                }
                n = 0;
            }
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite) {; }
        public void draw(SpriteBatch spriteBatch, Texture2D square, Texture2D cross, Texture2D zero, SpriteFont spriteFont, SpriteFont spriteFont2) {
            // 배경이 될 스퀘어를 그린다.
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

            // 각각의 player 1, 2 리스트에 하나씩 증가 될때마다 체크한 것을 그린다.
            if (drawCells) {
                foreach (Rectangle r in P1List) {
                    spriteBatch.Draw(cross, r, Color.White);
                }


                foreach (Rectangle r in P2List) {
                    spriteBatch.Draw(zero, r, Color.White);
                }
            } else {// 스퀘어 배열 초기화 
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
            }

            if (gameEnded) // 게임이 종료되면 한번더 클릭할 시 재시작 된다는 메세지 출력 
                spriteBatch.DrawString(spriteFont2, "Click again is restart", new Vector2(520, 400), Color.HotPink);

            spriteBatch.DrawString(spriteFont, "X is Player 1 \nO is Player 2", new Vector2(550, 30), Color.Beige);
            // 게임 결과를 나타낸다.
            spriteBatch.DrawString(spriteFont, "Winner is :) ", new Vector2(550, 150), Color.Yellow);
            if (playerOneWon && gameEnded) {
                spriteBatch.DrawString(spriteFont, "\n   Player 1", new Vector2(550, 150), Color.Yellow);
            }
            else if (playerTwoWon && gameEnded) {
                spriteBatch.DrawString(spriteFont, "\n   Player 2", new Vector2(550, 150), Color.Yellow);
            }
            else if (gameEnded && !playerOneWon && !playerTwoWon){
                spriteBatch.DrawString(spriteFont, " Draw", new Vector2(540, 180), Color.Yellow);
            }
        }

    }
}
