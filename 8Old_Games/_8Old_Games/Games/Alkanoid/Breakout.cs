using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace _8Old_Games.Games.Alkanoid
{
    class Ball
    {

        enum State
        {
            Active, // 활성상태
            Dead,  // 죽은 상태
            Over  // 게임 종료
        }

        private State state; // 상태변수
        private Paddle paddle; // 막대기 변수
        private Wall wall; // 벽돌들이 모인 wall 변수
        private int screenWidth, screenHeight; // 스크린 너비와 높이 

        private const int ballWidth = 32; // 볼사이즈 너비
        private const int ballHeight = 32; // 볼사이즈 높이

        private Vector2 initialPosition; // 공 초기위치
        private Vector2 lastPosition; // 공 마지막 위치
        private Vector2 position; // 공이 변할때 마다 쓰일  위치

        private Vector2 initialDirection = new Vector2(0, 1); //  처음 위치 
        private Vector2 direction = new Vector2(0, 1); // 
        
        private const int initialSpeed = 150; // 초기 스피드 
        private const int speedIncrement = 75; // 스피드 증가 간격 
        private const int maxSpeed = 750; // 최대스피드 
        private int speed = initialSpeed; // speed는 초기 스피드로 먼저 설정

        private bool isHeart = false; // 목숨 제어를 위해 필요한 bool 변수
        private static int heart_cnt = 5; // 목숨 카운트 변수
        
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, ballWidth,ballHeight); } // 공 position get
        }


        public Ball(Paddle paddle, Wall wall, int screenWidth, int screenHeight)
        {
            heart_cnt = 5;
            this.state = State.Active;
            this.paddle = paddle;
            this.wall = wall;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            initialPosition = new Vector2(screenWidth / 2, screenHeight / 2);
            position = initialPosition - new Vector2(ballWidth / 2, ballHeight / 2);
        }
            
        internal void Update(GameTime gameTime)
        {
            if (state == State.Active) // 상태가 활성상태면 
            {
                UpdatePosition(gameTime); // 위치 업데이트 
                if (position.Y > screenHeight) // 공이 바닥으로 떨어진 경우
                {
                    heart_cnt = heart_cnt - 1; // 여기서 목숨 줄어든다 한개씩 
                    isHeart = true; 
                    state = State.Dead; // 죽은 상태로 설정
                }
                else
                {
                    HandleCollisions(); // 안죽었으면 계속 충돌 처리
                }
            }
            else if (state == State.Dead && Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right)) // 상태 죽었고, 왼쪽키나 오른쪽키 다시 누를경우
            {
                if (heart_cnt != 0 && heart_cnt > 0)
                {
                    LaunchBall(); // 다시 게임 진행(다만 목숨 줄어든 상태)
                }
            }
            else if(state == State.Dead && heart_cnt <= 0) // 죽은 상태에다가 목숨 0이면
            {
                state = State.Over; // 게임 오버
               
            }
        }
        
        private void LaunchBall() // 초기 설정(젤 처음이나 공 떨어질때마다 실행)
        {
            state = State.Active; // 상태는 활성상태
            speed = initialSpeed; // 스피드는 초기 스피드부터 설정
            direction = initialDirection; // 위치도 초기화
            position = initialPosition - new Vector2(ballWidth / 2, ballHeight / 2); // 위치는 초기위치에서 공의 너비/2, 높이/2 만큼 뺀것으로 설정
        }

        private void UpdatePosition(GameTime gameTime) // 위치 업데이트 함수
        {
            lastPosition = position; // 마지막 위치는 젤 마지막으로 업데이트된 위치 변수 
            position = position + direction * (float)(speed * gameTime.ElapsedGameTime.TotalSeconds); // 위치 설정 (스피드 고려)
        }

        private void HandleCollisions() // 충돌 처리 핸들러
        {
            HandleBoardCollisions(); // 화면 충돌처리
            HandlePaddleCollision(); // 막대기 충돌처리
            HandlWallCollision(); // 벽돌 충돌 처리
        }

        private void HandleBoardCollisions()
        {
            if (position.Y < 0) // 현재 위치가 높이 0보다 작을 경우 
            {
                position.Y = 0; // 높이 위치 0으로 설정
                direction.Y = -direction.Y; // 너비 방향 음수값으로 설정
            }

            if (position.X < 0) // 현재 위치가 너비 0보다 작을 경우
            {
                position.X = 0; // 너비 위치 0으로 설정
                direction.X = -direction.X; // 방향 음수값으로 설정
            }
            else if (position.X + ballWidth > screenWidth) // 너비 위치와 공이 너비만큼 더한 값이 현재 스크린 너비보다 크면
            {
                position.X = screenWidth - ballHeight; // 너비 위치는 스크린 너비에서 공 높이 뺀것으로 위치 조정
                direction.X = -direction.X; // 방향 음수값으로 설정  
            }
        }
        /* 공이 막대기에 충돌했을 때 설정 */
        private void HandlePaddleCollision()
        {
            if (direction.Y > 0 && paddle.Bounds.Intersects(this.Bounds)) // 공 안떨어지고, 막대기에 맞았을 경우
            {
                direction.Y = -direction.Y;
                position.Y = paddle.Bounds.Y - ballHeight;

                // 막대기로 공을 정확히 맞출때마다 X 방향 업데이트 해준다
                direction.X = ((float)Bounds.Center.X - paddle.Bounds.Center.X) / (paddle.Bounds.Width / 2);
                direction = Vector2.Normalize(direction);
                
                /* 공이 어딘가에 hit할 때 마다 스피드를 증가한다. */
                speed += speedIncrement; // 스피드 75간격 만큼씩 증가
                speed = Math.Min(speed, maxSpeed); // 최종 speed에는 maxSpeed와 비교해서 제일 작은 값으로 저장된다.
            }
        }
 
        private bool LineIntersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            float ua = (p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p4.Y) * (p1.X - p3.X);
            float ub = (p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X);
            float de = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);
            bool intersects = false;

            if (Math.Abs(de) >= 0.00001f)
            {
                ua /= de;
                ub /= de;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    intersects = true;
                }
            }

            return intersects;
        }

        /* 공이 벽에 충돌했을 때 설정 */
        private void HandlWallCollision()
        {
            /* 충돌 위치에 따라 지정된 변수 생성 */
            Vector2 middleTop = new Vector2(Bounds.Width / 2, 0);
            Vector2 middleLeft = new Vector2(0, Bounds.Height / 2);
            Vector2 middleRight = new Vector2(Bounds.Width, Bounds.Height / 2);
            Vector2 middleBottom = new Vector2(Bounds.Width / 2, Bounds.Height);
            Vector2 newDirection = new Vector2(direction.X, direction.Y); // 충돌했을 때 새로운 방향을 처리해주어야 함
            List<Rectangle> destroyed = wall.DesBrick(this.Bounds); // 깨트린 벽 리스트를 생성한다.

            foreach (Rectangle brick in destroyed)
            {
                bool changed = false;
                Vector2 left_top = new Vector2(brick.Left, brick.Top);
                Vector2 right_top = new Vector2(brick.Right, brick.Top);
                Vector2 left_bottom = new Vector2(brick.Left, brick.Bottom);
                Vector2 right_bottom = new Vector2(brick.Right, brick.Bottom);

                if (direction.X > 0 && LineIntersects(lastPosition + middleRight, position + middleRight, left_top, left_bottom))
                {
                    newDirection.X *= -1; // X 방향 다시 돌아오도록 설정, 변할때마다 change 변수 true로 설정
                    changed = true; 
                }
                else if (direction.X < 0 && LineIntersects(lastPosition + middleLeft, position + middleLeft, right_top, right_bottom))
                {
                    newDirection.X *= -1; // X 방향 다시 돌아오도록 설정, 변할때마다 change 변수 true로 설정
                    changed = true;
                }

                if (direction.Y > 0 && LineIntersects(lastPosition + middleBottom, position + middleBottom, left_top, right_top))
                {
                    newDirection.Y *= -1; // Y 방향 다시 돌아오도록 설정, 변할때마다 change 변수 true로 설정
                    changed = true;
                }
                else if (direction.Y < 0 && LineIntersects(lastPosition + middleTop, position + middleTop, left_bottom, right_bottom))
                {
                    newDirection.Y *= -1; // Y 방향 다시 돌아오도록 설정, 변할때마다 change 변수 true로 설정
                    changed = true;
                }

                if (!changed) // 코너에 맞은경우에도 다시 돌아오도록 설정
                {
                    newDirection.X *= -1; 
                    newDirection.Y *= -1;
                }

                break;
            }

            direction = newDirection; // 방향은 새로운 방향으로 지정
        }

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D obj, SpriteFont font2, Texture2D cross, Texture2D background, Texture2D heart, SpriteFont font3)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White); // 배경 그린다.
            if (state == State.Active) // 살아있는 상태
            {
                spriteBatch.Draw(obj, position, Color.White); // 공 그린다.
                /* 목숨 5개를 그린다. */
                spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White); 
                spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);

                /* 목숨 줄어드는 것을 체크 및 반영 */
                if (heart_cnt == 4) // 목숨 4개 남았을 때
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 3) // 목숨 3개 남았을 때
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(620, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 2) // 목숨 2개 남았을 때
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(660, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 1) // 목숨 하나 남았을 때
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(700, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 0) // 목숨이 하나도 남지 않았을 때
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(740, 10, 25, 25), Color.White);
                }

            }
            else if (state == State.Over) // 게임 오버 상태면(목숨 5개 다 쓴 상태)
            {
                spriteBatch.DrawString(font2, "Game Over!", new Vector2(180, 250), Color.Yellow); // 게임 종료 문자 출력
            }
            else if (state != State.Over && state == State.Dead) // 죽었는데 오버된 상태가 아니면
            {
                spriteBatch.DrawString(font3, "press <- or -> key", new Vector2(250, 250), Color.Yellow); // 게임 이어서하기 문자 출력
            }

            // 게임 승리는 밑에 Wall 클래스에서 처리해준다.
        }
    }

    class Paddle
    {
        /* 막대기 속도 조정 상수 */
        const int initialSpeed = 300;
        Vector2 ACCELE = new Vector2(2000, 0);
        const int padFixedPosition = 10; // 막대기 고정 위치

        /* 막대기 길이 조정 변수 */
        private const int paddleWidth = 150;
        private const int paddleHeight = 20;
        /* 막대기 위치 조정 변수 */

        private Vector2 padSpeed = Vector2.Zero;
        private Vector2 position;
        private Vector2 minPosition;
        private Vector2 maxPosition;
        private Rectangle source;

        private int screenHeight;
        private int screenWidth;
        private KeyboardState keyState;

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Velocity
        {
            get { return padSpeed; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, paddleWidth, paddleHeight); }
        }

         public Paddle(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.keyState = Keyboard.GetState();
            int x = screenWidth - (screenWidth / 2) - (paddleHeight / 2);
            int y = screenHeight - padFixedPosition - paddleHeight;
            position = new Vector2(x, y);
            minPosition = new Vector2(0, position.Y);
            maxPosition = new Vector2(screenWidth - 1 - paddleWidth, position.Y);
            source = new Rectangle(0, 0, paddleWidth, paddleHeight);
        }

        internal void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboard = Keyboard.GetState();

            /* 막대기 이동과 속도 조절  */
            if (keyboard.IsKeyDown(Keys.Left) && !keyState.IsKeyDown(Keys.Left))
            {
                padSpeed = new Vector2(-initialSpeed, 0); 
            }
            else if (keyboard.IsKeyDown(Keys.Right) && !keyState.IsKeyDown(Keys.Right))
            {
                padSpeed = new Vector2(initialSpeed, 0); 
            }
            else if (keyboard.IsKeyDown(Keys.Left) && keyState.IsKeyDown(Keys.Left))
            {
                padSpeed = padSpeed + (-ACCELE * time); // 왼쪽 방향키 눌렸을 때 속도 조정
            }
            else if (keyboard.IsKeyDown(Keys.Right) && keyState.IsKeyDown(Keys.Right))
            {
                padSpeed = padSpeed + (ACCELE * time); // 오른쪽 방향키 눌렸을 때 속도 조정
            }
            else // 방향키 왼쪽 오른쪽 아니면 안움직이게 한다.
            {
                padSpeed = Vector2.Zero;
            }

            position = position + (padSpeed * time);
            position = Vector2.Clamp(position, minPosition, maxPosition);

            keyState = keyboard;
        }


        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D obj)
        {
            spriteBatch.Draw(obj, position, source, Color.White); // 막대기 그린다.
        }
    }
    
    class Wall
    {
        private const int bricksPerRow = 20; // 줄하나 당 벽돌 갯수
        private const double heightRatio = 0.20; // 벽돌 굵기
        private const int numRows = 7; // 벽돌 line수
        private static readonly Color[] colors = { // 벽돌 색깔 설정
            Color.Red, Color.Orange, Color.Yellow, Color.Green,
            Color.Blue, Color.Navy, Color.Violet
    };

        private int desCnt = 0; // 깨진 블럭수가 담길 변수
        private readonly int screenWidth;
        private readonly int screenHeight;
        private float brickHeight; // 벽돌 높이
        private float brickWidth; // 벽돌 너비
        private Rectangle wallBounds;
        private List<Brick> bricks = new List<Brick>();

        public Rectangle Bounds
        {
            get { return wallBounds; }
        }

        public Wall(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            double wallHeight = screenHeight * heightRatio;
            wallBounds = new Rectangle(0, (int)(wallHeight / 2), screenWidth, (int)(wallHeight + wallHeight/2));
            brickHeight = wallBounds.Height / numRows; // 총 벽돌 높이는 높이에서 벽돌 라인수로 나눈 것으로 구할 수 있다.
            brickWidth = screenWidth / bricksPerRow; // 총 벽돌 너비는 스크린 너비 나누기 줄하나당 벽돌 개수로 구할 수 있다.

            /* 벽 생성 부분 */
            for (int i = 0; i < numRows * bricksPerRow; i++) // 총 벽돌 수는 벽 한줄 당 벽돌 개수 x 벽의 라인수 
            {
                int row = i / bricksPerRow; // 열 생성
                int column = i % bricksPerRow; // 행 생성
                bricks.Add(new Brick(row, column, column * brickWidth, wallBounds.Top + (row * brickHeight), brickWidth, brickHeight)); // 리스트에 추가한다.
            }
        }

        public List<Rectangle> DesBrick(Rectangle r)
        {
            List<Rectangle> desBrick = new List<Rectangle>();  // 깨트린 벽돌들 담을 list 생성
            {
                foreach (Brick brick in bricks) // 벽돌 list돌면서 그린다.
                {
                    if (brick.State == Brick.BrickState.NOT_BROKEN && brick.Bounds.Intersects(r))
                    {
                        brick.State = Brick.BrickState.BROKEN; // 벽돌 깨진 상태면
                        desBrick.Add(brick.Bounds); // 깨진 리스트에 추가한다.
                        desCnt++; // 깨진 벽돌수 카운트
                        break;
                    }
                }
            }

            return desBrick;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime,Texture2D obj, SpriteFont font, Texture2D heart, SpriteFont font2, Texture2D background)
        {
            int unBrokenBrick = (numRows * bricksPerRow)-desCnt; // 남은 벽돌수는 전체 벽돌수에서 깨트린 벽돌수 뺀 값

            foreach (Brick brick in bricks)
            {
                if (brick.State == Brick.BrickState.NOT_BROKEN)
                {
                    spriteBatch.Draw(obj, brick.Bounds, colors[colors.Length - brick.Row - 1]);
                }
            }
            if(unBrokenBrick == 0) // 블록 다 깼으면
            {
               spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
               spriteBatch.DrawString(font2, "YOU  WIN!", new Vector2(200, 150), Color.Yellow); // 게임 승리 문자 출력
               spriteBatch.DrawString(font2, "return to menu", new Vector2(100, 250), Color.Yellow); // 게임 승리 문자 출력
             
            }
            spriteBatch.DrawString(font, "BrickBreak  by YunMi", new Vector2(30, 10), Color.Beige); // 상단에 개발자 정보 출력
            spriteBatch.DrawString(font, "unbroken brick : " + unBrokenBrick, new Vector2(320, 10), Color.YellowGreen); // 남은 벽돌수 출력

        }

    }

    /* Wall 클래스를 위한 벽돌 객체 생성 및 초기화 */
    class Brick 
    {
        public enum BrickState { NOT_BROKEN, BROKEN }; // 벽돌 상태는 깨지거나 안깨지거나 
        private readonly int column; // 행
        private readonly int row; // 열 
        private Rectangle bounds;
        private BrickState state;

        public int Row
        {
            get { return row; }
        }

        public Rectangle Bounds
        {
            get { return bounds; }
        }

        public BrickState State { get; set; }

        public Brick(int row, int column, float x, float y, float width, float height)
        {
            // 초기화 설정
            this.state = BrickState.NOT_BROKEN; // 안깨진 블록으로 초기화
            this.row = row;
            this.column = column;
            this.bounds = new Rectangle((int)x, (int)y, (int)width, (int)height);
        }
    }
}

