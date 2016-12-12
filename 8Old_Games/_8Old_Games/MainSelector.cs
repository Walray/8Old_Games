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
using _8Old_Games.Games.Bomberman;
using _8Old_Games.Games.Alkanoid;
using _8Old_Games.Games.CatchMouse;
using _8Old_Games.Games.Hangman;

/*
 * class MainSelector
 * 여덟 개의 게임 상태 관리
*/
namespace _8Old_Games {
    public enum Selector { MAIN_SELECTOR, MINE_SWEEPER, HANGMAN, SUDOKU, BOMBERMAN, TICTACTOE, CATCH_MOUSE, FROGGER, ALKANOID, END };

    public class MainSelector : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont han;

        #region 버튼용
        Texture2D button_Frogger;
        Texture2D button_Minesweeper;
        Texture2D button_Sudoku;
        Texture2D button_Tictactoe;
        Texture2D button_Hangman;
        Texture2D button_CatchMouse;
        Texture2D button_Alkanoid;
        Texture2D button_Bomberman;
        Texture2D background;

        Rectangle rect_Frogger;
        Rectangle rect_Minesweeper;
        Rectangle rect_Sudoku;
        Rectangle rect_Tictactoe;
        Rectangle rect_Hangman;
        Rectangle rect_CatchMouse;
        Rectangle rect_Alkanoid;
        Rectangle rect_Bomberman;

        #endregion

        Selector selector;
        Frogger frogger;
        TicTacToe tictactoe;
        Sudoku sudoku;
        MineSweeper mineSweeper;
        Alkanoid alkanoid;
        Bomberman bomberman;
        CatchMouse catchMouse;
        HangMan hangman;


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
            rect_Frogger = new Rectangle(50, 150, WIDTH, HEIGHT);
            rect_Minesweeper = new Rectangle(235, 150, WIDTH, HEIGHT);
            rect_Sudoku = new Rectangle(420, 150, WIDTH, HEIGHT);
            rect_Tictactoe = new Rectangle(605, 150, WIDTH, HEIGHT);
            rect_Alkanoid = new Rectangle(50, 250, WIDTH, HEIGHT);
            rect_Bomberman = new Rectangle(235, 250, WIDTH, HEIGHT);
            rect_CatchMouse = new Rectangle(420, 250, WIDTH, HEIGHT);
            rect_Hangman = new Rectangle(605, 250, WIDTH, HEIGHT);
            base.Initialize();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region 게임별 리소스 로드
            //for button images(start)
            button_Frogger = Content.Load<Texture2D>("Common\\Image\\Button_Frogger"); ;
            button_Minesweeper = Content.Load<Texture2D>("Common\\Image\\Button_Minesweeper");
            button_Sudoku = Content.Load<Texture2D>("Common\\Image\\Button_Sudoku"); 
            button_Tictactoe = Content.Load<Texture2D>("Common\\Image\\Button_Tictactoe");
            button_Alkanoid = Content.Load<Texture2D>("Common\\Image\\Button_Alkanoid");
            button_Bomberman = Content.Load<Texture2D>("Common\\Image\\Button_Bomberman");
            button_CatchMouse = Content.Load<Texture2D>("Common\\Image\\Button_CatchMouse");
            button_Hangman = Content.Load<Texture2D>("Common\\Image\\Button_Hangman");
            background = Content.Load<Texture2D>("Common\\Image\\background");

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
            // ddd
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

            //for Alkanoid(start)
            Alkanoid.sMenu = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\메뉴화면");
            Alkanoid.sStart = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\메인");
            Alkanoid.sLoad = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\로딩");
            Alkanoid.pad = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\paddle");
            Alkanoid.ball = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\ball");
            Alkanoid.brick = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\brick");
            Alkanoid.font = Content.Load<SpriteFont>("Games\\Alkanoid\\Font\\Arial");
            Alkanoid.heart= Content.Load<Texture2D>("Games\\Alkanoid\\Image\\heart");
            Alkanoid.font2 = Content.Load<SpriteFont>("Games\\Alkanoid\\Font\\Arial2");
            Alkanoid.font3 = Content.Load<SpriteFont>("Games\\Alkanoid\\Font\\font3");
            Alkanoid.background = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\back");
            Alkanoid.heart_b = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\heart_b");
            Alkanoid.cross = Content.Load<Texture2D>("Games\\Alkanoid\\Image\\cross");
            //for Alkanoid(end)

            //for Bomberman(start)
            Bomberman.sStart = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sStart");
            Bomberman.sMenu = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sMenu");
            Bomberman.sSelection = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sLoad");
            Bomberman.sPlay = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sPlay");
            Bomberman.obj = Content.Load<Texture2D>("Games\\Bomberman\\Image\\object");
            Bomberman.font = Content.Load<SpriteFont>("Games\\Bomberman\\Font\\Spritefont");
            Bomberman.sClear = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sClear");
            Bomberman.sFail = Content.Load<Texture2D>("Games\\Bomberman\\Image\\sFail");
            //for Bomberman(end)


            //for CatchMouse(start)
            CatchMouse.cm_failed = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\cm_failed");
            CatchMouse.cm_load = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\cm_load");
            CatchMouse.cm_menu = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\cm_menu");
            CatchMouse.cm_start = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\cm_start");
            CatchMouse.mouse1 = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\mouse1");
            CatchMouse.skull = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\skull");
            CatchMouse.cm_clear = Content.Load<Texture2D>("Games\\CatchMouse\\Image\\cm_clear");
            CatchMouse.font = Content.Load<SpriteFont>("Games\\CatchMouse\\Font\\sf");
            //for CatchMouse(end)

            //for Hangman(start)
            HangMan.startImage = Content.Load<Texture2D>("Games\\Hangman\\Image\\행맨메인화면");
            HangMan.menuImage = Content.Load<Texture2D>("Games\\Hangman\\Image\\행맨메뉴선택");
            HangMan.font = Content.Load<SpriteFont>("Games\\MineSweeper\\Font\\SpriteFont1"); // 폰트
            HangMan.a = Content.Load<Texture2D>("Games\\Hangman\\Image\\a");
            HangMan.b = Content.Load<Texture2D>("Games\\Hangman\\Image\\b");
            HangMan.c = Content.Load<Texture2D>("Games\\Hangman\\Image\\c");
            HangMan.d = Content.Load<Texture2D>("Games\\Hangman\\Image\\d");
            HangMan.e = Content.Load<Texture2D>("Games\\Hangman\\Image\\e");
            HangMan.f = Content.Load<Texture2D>("Games\\Hangman\\Image\\f");
            HangMan.g = Content.Load<Texture2D>("Games\\Hangman\\Image\\g");
            HangMan.h = Content.Load<Texture2D>("Games\\Hangman\\Image\\h");
            HangMan.i = Content.Load<Texture2D>("Games\\Hangman\\Image\\i");
            HangMan.j = Content.Load<Texture2D>("Games\\Hangman\\Image\\j");
            HangMan.k = Content.Load<Texture2D>("Games\\Hangman\\Image\\k");
            HangMan.l = Content.Load<Texture2D>("Games\\Hangman\\Image\\l");
            HangMan.n = Content.Load<Texture2D>("Games\\Hangman\\Image\\n");
            HangMan.m = Content.Load<Texture2D>("Games\\Hangman\\Image\\m");
            HangMan.o = Content.Load<Texture2D>("Games\\Hangman\\Image\\o");
            HangMan.p = Content.Load<Texture2D>("Games\\Hangman\\Image\\p");
            HangMan.q = Content.Load<Texture2D>("Games\\Hangman\\Image\\q");
            HangMan.r = Content.Load<Texture2D>("Games\\Hangman\\Image\\r");
            HangMan.s = Content.Load<Texture2D>("Games\\Hangman\\Image\\s");
            HangMan.t = Content.Load<Texture2D>("Games\\Hangman\\Image\\t");
            HangMan.u = Content.Load<Texture2D>("Games\\Hangman\\Image\\u");
            HangMan.v = Content.Load<Texture2D>("Games\\Hangman\\Image\\v");
            HangMan.w = Content.Load<Texture2D>("Games\\Hangman\\Image\\w");
            HangMan.x = Content.Load<Texture2D>("Games\\Hangman\\Image\\x");
            HangMan.y = Content.Load<Texture2D>("Games\\Hangman\\Image\\y");
            HangMan.z = Content.Load<Texture2D>("Games\\Hangman\\Image\\z");
            HangMan.scaffold = Content.Load<Texture2D>("Games\\Hangman\\Image\\단두대");
            HangMan.head = Content.Load<Texture2D>("Games\\Hangman\\Image\\얼굴");
            HangMan.rigth_hand = Content.Load<Texture2D>("Games\\Hangman\\Image\\오른팔");
            HangMan.rigth_foot = Content.Load<Texture2D>("Games\\Hangman\\Image\\오른다리");
            HangMan.left_hand = Content.Load<Texture2D>("Games\\Hangman\\Image\\왼팔");
            HangMan.left_foot = Content.Load<Texture2D>("Games\\Hangman\\Image\\왼다리");
            HangMan.body = Content.Load<Texture2D>("Games\\Hangman\\Image\\몸통");
            HangMan.clicked = Content.Load<Texture2D>("Games\\Hangman\\Image\\선택함");
            //for Hangman(end)

            #endregion

            han = Content.Load<SpriteFont>("Common\\Font\\MainFont");
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
                        else if (rect_Alkanoid.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.ALKANOID;
                            alkanoid = new Alkanoid();
                            alkanoid.initialize();
                        }
                        else if (rect_Bomberman.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {

                            selector = Selector.BOMBERMAN;
                            bomberman = new Bomberman();
                            bomberman.initialize();
                        }
                        else if (rect_CatchMouse.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.CATCH_MOUSE;
                            catchMouse = new CatchMouse();
                            catchMouse.initialize();
                        }
                        else if (rect_Hangman.Contains(ms.X, ms.Y) && ms.LeftButton == ButtonState.Pressed) {
                            selector = Selector.HANGMAN;
                            hangman = new HangMan();
                            hangman.initialize();
                        }
                        mTimeSinceLastInput = 0.0f;
                    }
                    break;
                case Selector.FROGGER:
                    Mouse.WindowHandle = Window.Handle;
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
                case Selector.HANGMAN:
                    Mouse.WindowHandle = Window.Handle;
                    selector = hangman.update(gameTime);
                    break;
                case Selector.CATCH_MOUSE:
                    Mouse.WindowHandle = Window.Handle;
                    selector = catchMouse.update(gameTime);
                    break;
                case Selector.ALKANOID:
                    Mouse.WindowHandle = Window.Handle;
                    selector = alkanoid.update(gameTime);
                    //업데이트
                    break;
                case Selector.BOMBERMAN:
                    Mouse.WindowHandle = Window.Handle;
                    selector = bomberman.update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            switch (selector) {
                case Selector.MAIN_SELECTOR:
                    spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
                    spriteBatch.Draw(button_Frogger, rect_Frogger, Color.LightCyan);
                    spriteBatch.Draw(button_Minesweeper, rect_Minesweeper, Color.Honeydew);
                    spriteBatch.Draw(button_Sudoku, rect_Sudoku, Color.Lavender);
                    spriteBatch.Draw(button_Tictactoe, rect_Tictactoe, Color.White);
                    spriteBatch.Draw(button_Hangman, rect_Hangman, Color.LightCyan);
                    spriteBatch.Draw(button_CatchMouse, rect_CatchMouse, Color.Honeydew);
                    spriteBatch.Draw(button_Bomberman, rect_Bomberman, Color.Lavender);
                    spriteBatch.Draw(button_Alkanoid, rect_Alkanoid, Color.LightGoldenrodYellow);
                    break;
                case Selector.FROGGER:
                    GraphicsDevice.Clear(Color.Black);
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
                case Selector.HANGMAN:
                    hangman.draw(spriteBatch, gameTime);
                    break;
                case Selector.CATCH_MOUSE:
                    GraphicsDevice.Clear(Color.Black);
                    catchMouse.draw(spriteBatch, gameTime);
                    break;
                case Selector.ALKANOID:
                    GraphicsDevice.Clear(Color.Black);
                    alkanoid.draw(spriteBatch, gameTime);
                    break;
                case Selector.BOMBERMAN:
                    GraphicsDevice.Clear(Color.Black);
                    bomberman.draw(spriteBatch, gameTime);
                    break;
               }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
