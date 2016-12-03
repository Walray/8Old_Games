using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace _8Old_Games.Games.Alkanoid.Sequence
{
    public class Play : Sequence
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Paddle pad;
        Ball ball;
        const int rows = GlobalData.rows;
        const int cols = GlobalData.cols;
        Brick[,] Wall;

        int bricksLeft = GameMap.brickNum;
        int timeLeft = GlobalData.maxTime;
        int timer = 0;
        bool newGame = true;

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {;}
        public void draw(GameTime gameTime, SpriteFont Arial, SpriteFont Arial2, Texture2D bricks)
        {
            spriteBatch.Begin();
            pad.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    // 블록 그릴 자리
                    if (Wall[i, j].appear == 1)
                        spriteBatch.Draw(Alkanoid.bricks, Wall[i, j].position, Color.White);
                }
            // 화면에 남은 시간이랑 남은 블록 수 출력
            spriteBatch.DrawString(Arial, "Time count", new Vector2(200, 50), Color.Gold);
            spriteBatch.DrawString(Arial, "Block count", new Vector2(400, 50), Color.Gold);
            spriteBatch.DrawString(Arial, ": " + timeLeft, new Vector2(320, 50), Color.OrangeRed);
            spriteBatch.DrawString(Arial, ": " + bricksLeft, new Vector2(525, 50), Color.OrangeRed);

            // 블럭수 다 깨면 게임결과 win
            if (bricksLeft == 0)
            {
                spriteBatch.DrawString(Arial, "You won!", new Vector2(250, 50), Color.Gold, 0F,
                    new Vector2(0, 0), 5F, SpriteEffects.None, 0);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }

            // 제한시간 1분 다 썼는데 벽돌 못깼으면 game over
            if (timeLeft == 0)
            {
                spriteBatch.DrawString(Arial2, "Game Over", new Vector2(600, 120), Color.Gold);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }

        public override void initialize(){;}
        public override State update(GameTime gameTime, KeyboardState ks)
        {

            // ball이랑 pad 변수 초기화
            ball = new Ball(GlobalData.ballPosition, GlobalData.ballSize, GlobalData.ballSpeed);
            pad = new Paddle(GlobalData.padPosition, GlobalData.padSize, GlobalData.padSpeed);

            // 벽돌 배열 저장
            Wall = new Brick[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    // 벽돌 위치 조정
                    Wall[i, j] = new Brick(new Vector2(90 + GlobalData.brickSize.X * j, 120 + GlobalData.brickSize.Y * i),
                        GlobalData.brickSize);
                    // GameMap.cs에서 가져온 걸로 저장함 (1이면 보이고, 0이면 안보임)
                    Wall[i, j].appear = GameMap.gameMap[i, j];
                }

            //새로운 게임 시작하려면 스페이스바 
            if (newGame)
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    newGame = false;
            }

            // 새로운 게임이 아닐경우 
            if (!newGame && timeLeft > 0)
            {
                ball.CollideType(pad); // 공이랑 막대기 충돌 탐지 

                foreach (Brick br in Wall) // 공이 블럭 깼는지 탐색
                {
                    if (br.appear == 0) // 안보이는 벽돌은 충돌체크 안해도 됨
                        continue;
                    else // 보이는 벽돌만 충돌처리
                    {
                        pad.CollideType(br);
                        if (ball.CollideType(br) != Collision.NO) // 충돌일어났으면
                        {
                            br.appear = 0; // 벽돌 안보이게 설정
                            bricksLeft--; // 벽돌 남은수 하나씩 감소
                            break;
                        }
                    }
                }

                // 시간 1씩 감소 부분
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 1000)
                {
                    timeLeft--;
                    timer -= 1000;
                }

                ball.Update(gameTime);
                pad.Update(gameTime);
                //base.Update(gameTime);
          
            }
   
            if (timeLeft == 0 || bricksLeft == 0) // 시간다됐고, 벽돌도 다 깬경우
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    timeLeft = GlobalData.maxTime;

                for (int i = 0; i < rows; i++) // 벽돌 남은수 reset 
                    for (int j = 0; j < cols; j++)
                    {
                        Wall[i, j].appear = GameMap.gameMap[i, j];
                    }
                bricksLeft = GameMap.brickNum;
            }

            return State.PLAY;
        }

    }
}
