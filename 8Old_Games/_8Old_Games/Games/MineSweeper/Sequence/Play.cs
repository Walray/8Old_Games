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

namespace _8Old_Games.Games.MineSweeper.Sequence {
    public class Play :Sequence {
        #region Texture2D 공백 (픽셀 단위)
        #endregion

        private const double WAIT_MENU = 0.15;
        private double mTimeAfterMenu = WAIT_MENU;

        #region Texture2D 이미지 변수 선언
        const int tileSize=40; // 타일 사이즈
        #endregion

        #region 잡동사니변수

        Color winTextColor = Color.Red;
        Color generalTextColor = Color.Black;

        string winovertext = ""; // 게임 승리여부 txt
        

        bool gameOver = false;  // gameover
        int minesLeft;


        // 마우스 왼쪽 오른쪽 클릭 사용 여부 ㅇㅇ
        bool leftmouse = false;
        bool rightmouse = false;
        
        int PmouseX = 0; // 게임중
        int PmouseY = 0; // 게임중
        

        #endregion


        #region 게임 배열(타일 총 갯수, 폭탄 갯수)

        Grid[,] grid;


        int width = 0; // 가로로 타일 총 갯수
        int height = 0; // 세로로 타일 총 갯수
        int mines = 0; // 폭탄 갯수

        #endregion

        public Play() {; }

        public override void initialize() {; }
        public void initialize(int w, int h, int m) {
            width = w;
            height = h;
            mines = m;
            InitializeGrid();
            
        }
        public override State update(GameTime gameTime, KeyboardState ks) {
            mTimeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu <= 0) {
                KeyboardState newState = Keyboard.GetState();
                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU;
                }
                mTimeSinceLastInput = 0.0f;
            }

            if (gameOver) // 게임 끝났을 경우
            {
                // 오른쪽 버튼 누르면 게임 재시작 (! 문제.. 타일 안에서만 가능함 ..)
                if (Mouse.GetState().RightButton == ButtonState.Pressed && leftmouse == false) {
                    return State.SELECTION;
                }

            }
            else {
                #region 왼쪽버튼
                // 마우스 클릭 위치랑 클릭 뒤 위치 같을 때
                //1. 마우스 클릭했을 때
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && leftmouse == false) {
                    leftmouse = true; // 왼쪽 마우스 사용함
                    PmouseX = (Mouse.GetState().X ) / tileSize; // -uwith
                    PmouseY = (Mouse.GetState().Y ) / tileSize;

                }
                //2. 마우스 클릭 뒤에
                else if (Mouse.GetState().LeftButton == ButtonState.Released && leftmouse == true) {
                    int PmouseXRelease = (Mouse.GetState().X ) / tileSize;  // -uwith
                    int PmMouseYRelease = (Mouse.GetState().Y ) / tileSize;


                    // 마우스 클릭 위치랑 클릭 뒤 위치 같을 때(왜냐면 클릭해놓고 마우스 움직인 후에 클릭 놓는 경우가 있어서)
                    if (PmouseX == PmouseXRelease && PmouseY == PmMouseYRelease) {
                        OpenClickedTile(); // 선택한 타일 오픈

                        IsWin(); // 승리여부
                    }
                    leftmouse = false;
                }

                // 3. 그 외의 경우
                else if (Mouse.GetState().LeftButton == ButtonState.Released) {
                    leftmouse = false;
                }
                #endregion


                #region 오른쪽 버튼(깃발)

                //마우스 클릭했을 때
                if (Mouse.GetState().RightButton == ButtonState.Pressed && rightmouse == false) {
                    rightmouse = true; // 오른쪽 마우스 사용함
                    PmouseX = (Mouse.GetState().X) / tileSize; // -uwith
                    PmouseY = (Mouse.GetState().Y) / tileSize;
                }

                //마우스 클릭 뒤에
                else if (Mouse.GetState().RightButton == ButtonState.Released && rightmouse == true) {
                    int PmouseXRelease = (Mouse.GetState().X) / tileSize; // -uwith
                    int PmouseYRelease = (Mouse.GetState().Y ) / tileSize;

                    // 마우스 클릭 위치랑 클릭 뒤 위치 같을 때
                    if (PmouseX == PmouseXRelease && PmouseY == PmouseYRelease) {
                        // 깃발 함수
                        FlagTile();
                    }
                    rightmouse = false;
                }

                else if (Mouse.GetState().RightButton == ButtonState.Released) {
                    rightmouse = false;
                }
                #endregion

                return State.PLAY;
            }
            return State.PLAY;
        }
        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {;  }

        public void draw(SpriteBatch spriteBatch, Texture2D Back, Texture2D clicked, Texture2D flag, Texture2D unclicked,  Texture2D mine, Texture2D clicked1 , Texture2D clicked2, Texture2D clicked3, Texture2D clicked4, Texture2D clicked5, Texture2D clicked6, SpriteFont font, Vector2 origin) {
            spriteBatch.Draw(Back, new Vector2(0, 0), Color.White);

            //  spriteBatch.DrawString(font, test, new Vector2(400, 450), Color.Yellow); 전역변수 디버깅
            
            #region Draw grid

            // 이미지 출력 
            for (int j = 0; j < height; j++) {
                for (int i = 0; i < width; i++) {
                    // grid 배열에 해당하는 각 배열의 위치 (즉, grid[0,0]의 위치는 20 20)
                    Vector2 Position = new Vector2(i * clicked.Height , j * clicked.Width); // +uwith
                   
                    if (grid[i, j].isFlagged) // 깃발
                    {
                        spriteBatch.Draw(flag,Position, Color.White);
                    }
                    else if (!grid[i, j].isClicked) // 선택함
                    {
                        spriteBatch.Draw(unclicked, Position, Color.White);
                    }
                    else if (grid[i, j].isMine) // 폭탄
                    {
                        spriteBatch.Draw(mine, Position, Color.White);
                    }
                    else {
                        switch (grid[i, j].SurroundingMines) // 주변 폭탄 갯수
                        {
                            case 1:
                                spriteBatch.Draw(clicked1, Position, Color.White);
                                break;
                            case 2:
                                spriteBatch.Draw(clicked2, Position, Color.White);
                                break;
                            case 3:
                                spriteBatch.Draw(clicked3, Position, Color.White);
                                break;
                            case 4:
                                spriteBatch.Draw(clicked4, Position, Color.White);
                                break;
                            case 5:
                                spriteBatch.Draw(clicked5, Position, Color.White);
                                break;
                            case 6:
                                spriteBatch.Draw(clicked6, Position, Color.White);
                                break;
                            default:
                                spriteBatch.Draw(clicked, Position, Color.White);
                                break;
                        }
                    }
                }
            }
            #endregion

            #region 게임승패여부

            if (winovertext != "") {
                spriteBatch.DrawString(font, winovertext, new Vector2(500,300), winTextColor, 0, font.MeasureString(winovertext), 1.5f, SpriteEffects.None, 1.0f);
            }
            


            #endregion
        }

        void InitializeGrid() {

            winovertext = "";
            gameOver = false;
            CreateGrid();
            RandomizeMines(mines);
            minesLeft = mines;
        }

        // 게임 배열 생성
        void CreateGrid() {
            //grid 객체 생성
            grid = new Grid[width, height];

            // 배열 각각 객체 생성
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    grid[i, j] = new Grid();
                }
            }
        }

        // 폭탄 랜덤하게 생성
        void RandomizeMines(int munMine) {
            Random rnd = new Random();


            for (int i = 0; i < munMine; i++) {
                int x = rnd.Next(0, width);
                int y = rnd.Next(0, height);

                //선택한 곳이 이미 폭탄이 생성된 곳이라면
                if (grid[x, y].isMine) {
                    i--;
                }
                else {
                    // 폭탄으로 지정 
                    grid[x, y].AddMine();
                    // 그리고 주변 타일에 폭탄 몇개인지 갯수 갱신
                    SurroundingMinesNum(x, y);
                }
            }
        }

        // 주변에 폭탄 몇개인지 갱신
        void SurroundingMinesNum(int x, int y) {

            if (y > 0) {
                grid[x, y - 1].SurroundingMines++;
                if (x > 0) {
                    grid[x - 1, y - 1].SurroundingMines++;
                }
                if (x < width - 1) {
                    grid[x + 1, y - 1].SurroundingMines++;
                }
            }

            if (y < height - 1) {
                grid[x, y + 1].SurroundingMines++;
                if (x > 0) {
                    grid[x - 1, y + 1].SurroundingMines++;
                }
                if (x < width - 1) {
                    grid[x + 1, y + 1].SurroundingMines++;
                }
            }
            if (x > 0) {
                grid[x - 1, y].SurroundingMines++;
            }
            if (x < height - 1) {
                grid[x + 1, y].SurroundingMines++;
            }
        }
        

        #region 타일처리
        // 깃발 
        private void FlagTile() {
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y ;

            if (mouseY <= 0 && mouseX <= 0) {
                return;
            }
            Console.WriteLine("{0:D},{0:D}", mouseX, mouseY);
            if (grid[mouseX / tileSize, mouseY / tileSize].isFlagged) {
                minesLeft++;
            }
            else {
                minesLeft--;
            }

            grid[mouseX / tileSize, mouseY / tileSize].Flag();
        }

        // 클릭된 타일 열기
        private void OpenClickedTile() {
            int mouseX = (Mouse.GetState().X ) / tileSize; // -uwith
            int mouseY = (Mouse.GetState().Y) / tileSize;


            // 타일 범위를 넘어서면(게임 영역 아니면)
            if (mouseX >= width || mouseY >= height || mouseX < 0 || mouseY < 0) {
                return;
            }

            // 깃발 표시 있거나 이미 선택했던 타일 이면
            else if (grid[mouseX, mouseY].isClicked || grid[mouseX, mouseY].isFlagged) {
                return;
            }

            // 타일 열어주고, 게임 끝내기
            else if (grid[mouseX, mouseY].isMine) {
                // 타일 열어주기
                grid[mouseX, mouseY].Open();
                // 졌음
                Lose();
                return;
            }

            //타일 열어주기
            grid[mouseX, mouseY].Open();

        }

        // 모든 폭탄 타일 열기
        void AllOpneMine() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (grid[x, y].isMine) {
                        grid[x, y].Open();
                    }
                }
            }
        }
        #endregion

        #region 승리판별

        // 승리여부판단
        private void IsWin() {
            int cnt = 0;

            // 만약 한번이라도 폭탄 클릭 했으면,
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (!grid[x, y].isClicked && !grid[x, y].isMine) {
                        return;
                    }
                }
            };

            //승리함
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (grid[x, y].isClicked)
                        cnt++;

                    if (cnt == mines) // 클릭하지 않는 숫자와 폭탄 갯수가 같으면
                        Win();
                }
            }

        }

        //  이겼을 때
        private void Win() {
            winTextColor = Color.Blue;
            winovertext = "You win!";
            gameOver = true;

        }

        // 졌을 때
        private void Lose() {
            winTextColor = Color.Yellow;
            winovertext = "Game Over";
            gameOver = true;
            //모든 폭탄 열어주기
            AllOpneMine();
        }

        #endregion



    }
}
