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
using _8Old_Games.Games.Frogger;
using _8Old_Games.Games.TicTacToe;
using _8Old_Games.Games.Sudoku;
using _8Old_Games.Games.MineSweeper;

/*
 * 
 * deded
 * 
 * 
 * class MainSelector
 * 
 * 여덟 개의 게임 상태 관리
*/
namespace _8Old_Games {
    public enum Selector { MAIN_SELECTOR, MINE_SWEEPER, RESERVE1, SUDOKU, RESERVE2, TICTACTOE, CATCH_MOUSE, FROGGER, RESERVE4, END };
    //Tetris, Packman(?), Digda, (?) 

    public class MainSelector : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont test;

        Texture2D button_Frogger;
        Texture2D button_Minesweeper;
        Texture2D button_Sudoku;
        Texture2D button_Tictactoe;

        Rectangle rect_Frogger;
        Rectangle rect_Minesweeper;
        Rectangle rect_Sudoku;
        Rectangle rect_Tictactoe;



        Selector selector;
        Frogger frogger;
        TicTacToe tictactoe;
        Sudoku sudoku;
        MineSweeper mineSweeper;

        const int WIDTH = 150;
        const int HEIGHT = 70;


        double mTimeSinceLastInput;
        const double MIN_TIME = 0.11;

        public MainSelector() {
            graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            mTimeSinceLastInput = 0.0f;
            selector = Selector.MAIN_SELECTOR;
            rect_Frogger = new Rectangle(50, 200, WIDTH, HEIGHT);
            rect_Minesweeper = new Rectangle(235, 200, WIDTH, HEIGHT);
            rect_Sudoku = new Rectangle(420, 200, WIDTH, HEIGHT);
            rect_Tictactoe = new Rectangle(605, 200, WIDTH, HEIGHT);
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //for button images(start)
            button_Frogger= Content.Load<Texture2D>("Common\\Image\\Button_Frogger"); ;
            button_Minesweeper = Content.Load<Texture2D>("Common\\Image\\Button_Minesweeper");
            button_Sudoku = Content.Load<Texture2D>("Common\\Image\\Button_Sudoku"); 
            button_Tictactoe = Content.Load<Texture2D>("Common\\Image\\Button_Tictactoe"); 
            //for button images(end)

            //for Frogger(start)
            Frogger.mapImages = Content.Load<Texture2D>("Games\\Frogger\\Image\\Map");
            Frogger.frogImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\Frog");
            Frogger.objectImages = Content.Load<Texture2D>("Games\\Frogger\\Image\\Object");
            Frogger.deadImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\Dead");
            Frogger.backgroundImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\Background");
            Frogger.clearImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\Clear");
            Frogger.sStart = Content.Load<Texture2D>("Games\\Frogger\\Image\\sStart");
            Frogger.sLoad = Content.Load<Texture2D>("Games\\Frogger\\Image\\sLoad");
            Frogger.sMenu = Content.Load<Texture2D>("Games\\Frogger\\Image\\sMenu");
            Frogger.sClear = Content.Load<Texture2D>("Games\\Frogger\\Image\\sClear");
            Frogger.sFail = Content.Load<Texture2D>("Games\\Frogger\\Image\\sFail");
            Frogger.dummyImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\dummy");
            Frogger.lastCrocImage = Content.Load<Texture2D>("Games\\Frogger\\Image\\last_croc");
            Frogger.menuFont = Content.Load<SpriteFont>("Games\\Frogger\\Font\\sf_menu");
            Frogger.font = Content.Load<SpriteFont>("Games\\Frogger\\Font\\Spritefont");
            //for Frogger(end)

            //for TicTacToe(start)
            TicTacToe.main = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\메인화면-틱택토");
            TicTacToe.menuImage = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\메뉴화면-틱택토");
            TicTacToe.loading = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\로딩화면");
            TicTacToe.square = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\square");
            TicTacToe.spriteFont = Content.Load<SpriteFont>("Games\\TicTacToe\\Font\\SpriteFont1");
            TicTacToe.spriteFont2 = Content.Load<SpriteFont>("Games\\TicTacToe\\Font\\SpriteFont2");
            TicTacToe.cross = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\cross");
            TicTacToe.zero = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\zero");
            TicTacToe.selectmode = Content.Load<Texture2D>("Games\\TicTacToe\\Image\\틱택토모드");
            //for TicTacToe(end)

            //for Sudoku(start)
            Sudoku.cell = Content.Load<Texture2D>("Games\\Sudoku\\Image\\cell");
            Sudoku.line = Content.Load<Texture2D>("Games\\Sudoku\\Image\\line");
            Sudoku.sf = Content.Load<SpriteFont>("Games\\Sudoku\\Font\\sf");
            Sudoku.sf2 = Content.Load<SpriteFont>("Games\\Sudoku\\Font\\sf2");
            Sudoku.sf_bold = Content.Load<SpriteFont>("Games\\Sudoku\\Font\\sf_bold");
            Sudoku.sudoku_main = Content.Load<Texture2D>("Games\\Sudoku\\Image\\sudoku_main");
            Sudoku.sudoku_menu = Content.Load<Texture2D>("Games\\Sudoku\\Image\\sudoku_menu");
            Sudoku.sudoku_loading = Content.Load<Texture2D>("Games\\Sudoku\\Image\\sudoku_loading");
            Sudoku.button_easy = Content.Load<Texture2D>("Games\\Sudoku\\Image\\button_easy");
            Sudoku.button_normal = Content.Load<Texture2D>("Games\\Sudoku\\Image\\button_normal");
            Sudoku.button_hard = Content.Load<Texture2D>("Games\\Sudoku\\Image\\button_hard");
            Sudoku.button_extreme = Content.Load<Texture2D>("Games\\Sudoku\\Image\\button_extreme");
            //for Sudoku(end)

            //for MineSweeper(start)
            MineSweeper.font = Content.Load<SpriteFont>("Games\\MineSweeper\\Font\\SpriteFont1"); // 폰트
            MineSweeper.clicked = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\grid-0"); // 선택됨
            MineSweeper.selectSize = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\게임사이즈");
            MineSweeper.Back = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\Back"); // 뒷배경
            MineSweeper.clicked1 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\1"); // 1
            MineSweeper.clicked2 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\2"); // 2
            MineSweeper.clicked3 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\3"); // 3
            MineSweeper.clicked4 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\4"); // 4
            MineSweeper.clicked5 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\5"); // 5
            MineSweeper.clicked6 = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\6"); // 6
            MineSweeper.mine = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\지뢰"); // 폭탄
            MineSweeper.unclicked = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\grid-unknown"); // 선택전
            MineSweeper.flag = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\grid-flag"); // 깃발
            MineSweeper.startImage = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\start");
            MineSweeper.menuImage = Content.Load<Texture2D>("Games\\MineSweeper\\Image\\menu");
            //for MineSweeper(end)

            test = Content.Load<SpriteFont>("Common\\Font\\MainFont");
        }

        protected override void UnloadContent() {
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch(selector){
                case Selector.MAIN_SELECTOR:

                    mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
                    if (mTimeSinceLastInput >= MIN_TIME) {
                        MouseState ms = Mouse.GetState();
                        if (rect_Frogger.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.FROGGER;
                            frogger = new Frogger();
                            frogger.initialize();
                        }
                        else if (rect_Tictactoe.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.TICTACTOE;
                            tictactoe = new TicTacToe();
                            tictactoe.initialize();
                        }
                        else if (rect_Sudoku.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.SUDOKU;
                            sudoku = new Sudoku();
                            sudoku.initialize();
                        }
                        else if (rect_Minesweeper.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.MINE_SWEEPER;
                            mineSweeper = new MineSweeper();
                            mineSweeper.initialize();
                        }
                        mTimeSinceLastInput = 0.0f;
                    }
                    break;
                case Selector.FROGGER:
                    selector = frogger.update(gameTime);
                    break;
                case Selector.MINE_SWEEPER:
                    Mouse.WindowHandle = Window.Handle;
                    selector = mineSweeper.update(gameTime);
                    break;
                case Selector.SUDOKU:
                    Mouse.WindowHandle = Window.Handle;
                    selector = sudoku.update(gameTime);
                    break;
                case Selector.TICTACTOE:
                    Mouse.WindowHandle = Window.Handle;
                    selector = tictactoe.update(gameTime);
                    break;
                case Selector.RESERVE1:
                    break;
                case Selector.RESERVE2:
                    break;
                case Selector.CATCH_MOUSE:
                    break;
                case Selector.RESERVE4:
                    break;
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            switch (selector) {
                case Selector.MAIN_SELECTOR:
                    spriteBatch.Draw(button_Frogger, rect_Frogger, Color.LightCyan);
                    spriteBatch.Draw(button_Minesweeper, rect_Minesweeper, Color.Honeydew);
                    spriteBatch.Draw(button_Sudoku, rect_Sudoku, Color.Lavender);
                    spriteBatch.Draw(button_Tictactoe, rect_Tictactoe, Color.LemonChiffon);
                    break;
                case Selector.FROGGER:
                    frogger.draw(spriteBatch, gameTime);
                    break;
                case Selector.MINE_SWEEPER:
                    GraphicsDevice.Clear(Color.Black);

                    mineSweeper.draw(spriteBatch, gameTime);
                    break;
                case Selector.SUDOKU:
                    GraphicsDevice.Clear(Color.Pink);

                    sudoku.draw(spriteBatch, gameTime);
                    break;
                case Selector.TICTACTOE:
                    tictactoe.draw(spriteBatch, gameTime);
                    break;
                case Selector.RESERVE1:
                    break;
                case Selector.RESERVE2:
                    break;
                case Selector.CATCH_MOUSE:
                    break;
                case Selector.RESERVE4:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
