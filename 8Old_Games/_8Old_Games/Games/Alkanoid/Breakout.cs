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

        private bool isHeart = false;
        private static int heart_cnt = 5; // 목숨 카운트 변수
        
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, ballWidth,ballHeight); } // 공 position get
        }


        public Ball(Paddle paddle, Wall wall, int screenWidth, int screenHeight)
        {
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

        private void HandlePaddleCollision()
        {
            if (direction.Y > 0 && paddle.Bounds.Intersects(this.Bounds))
            {
                direction.Y = -direction.Y;
                position.Y = paddle.Bounds.Y - ballHeight;

                direction.X = ((float)Bounds.Center.X - paddle.Bounds.Center.X) / (paddle.Bounds.Width / 2);
                direction = Vector2.Normalize(direction);
                
                speed += speedIncrement; // 스피드 75간격 만큼씩 증가
                speed = Math.Min(speed, maxSpeed);
            }
        }

        private void HandlWallCollision()
        {
            Vector2 centerTop = new Vector2(Bounds.Width / 2, 0);
            Vector2 centerLeft = new Vector2(0, Bounds.Height / 2);
            Vector2 centerRight = new Vector2(Bounds.Width, Bounds.Height / 2);
            Vector2 centerBottom = new Vector2(Bounds.Width / 2, Bounds.Height);
            Vector2 newDirection = new Vector2(direction.X, direction.Y);
            List<Rectangle> destroyed = wall.DestroyBricksAt(this.Bounds);

            foreach (Rectangle brick in destroyed)
            {
                bool changed = false;
                Vector2 topLeft = new Vector2(brick.Left, brick.Top);
                Vector2 topRight = new Vector2(brick.Right, brick.Top);
                Vector2 bottomLeft = new Vector2(brick.Left, brick.Bottom);
                Vector2 bottomRight = new Vector2(brick.Right, brick.Bottom);

                if (direction.X > 0 && LineIntersects(lastPosition + centerRight, position + centerRight, topLeft, bottomLeft))
                {
                    newDirection.X *= -1;
                    Console.WriteLine("left"); // 위치 출력 
                    changed = true;
                }
                else if (direction.X < 0 && LineIntersects(lastPosition + centerLeft, position + centerLeft, topRight, bottomRight))
                {
                    newDirection.X *= -1;
                    Console.WriteLine("right"); // 위치 출력 
                    changed = true;
                }

                if (direction.Y > 0 && LineIntersects(lastPosition + centerBottom, position + centerBottom, topLeft, topRight))
                {
                    newDirection.Y *= -1;
                    Console.WriteLine("top"); // 위치 출력 
                    changed = true;
                }
                else if (direction.Y < 0 && LineIntersects(lastPosition + centerTop, position + centerTop, bottomLeft, bottomRight))
                {
                    newDirection.Y *= -1;
                    Console.WriteLine("bottom"); // 위치 출력 
                    changed = true;
                }
                
                if (!changed)
                {
                    newDirection.X *= -1;
                    newDirection.Y *= -1;
                }

                Console.WriteLine(direction);
                Console.WriteLine(newDirection);
                break;
            }

            direction = newDirection;
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

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D obj, SpriteFont font2, Texture2D cross, Texture2D background, Texture2D heart)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White); // 배경 그린다.

            if (state == State.Active)
            {
                spriteBatch.Draw(obj, position, Color.White);
                spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);

                if (heart_cnt == 4)
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 3)
                {
                    spriteBatch.Draw(heart, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(620, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(660, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(700, 10, 25, 25), Color.White);
                    spriteBatch.Draw(heart, new Rectangle(740, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(580, 10, 25, 25), Color.White);
                    spriteBatch.Draw(cross, new Rectangle(620, 10, 25, 25), Color.White);
                }
                else if (heart_cnt == 2)
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
                else if (heart_cnt == 1)
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
                else if (heart_cnt == 0)
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
            else if (state == State.Over)
            {
                spriteBatch.DrawString(font2, "Game Over!", new Vector2(180, 250), Color.Yellow); // 게임 종료 문자 출력
            }
        }
    }

    class Paddle
    {
        const int INITIAL_SPEED = 300;
        Vector2 ACCELERATION = new Vector2(2000, 0);
        const int PADDLE_OFFSET = 10;

        private const int paddleWidth = 150;
        private const int paddleHeight = 20;

        private Vector2 velocity = Vector2.Zero;
        private Vector2 position;
        private Vector2 minPosition;
        private Vector2 maxPosition;
        private Rectangle source;

        private int screenHeight;
        private int screenWidth;
        private KeyboardState previousKeyboard;

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, paddleWidth, paddleHeight); }
        }

         public Paddle(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.previousKeyboard = Keyboard.GetState();
            int x = screenWidth - (screenWidth / 2) - (paddleHeight / 2);
            int y = screenHeight - PADDLE_OFFSET - paddleHeight;
            position = new Vector2(x, y);
            minPosition = new Vector2(0, position.Y);
            maxPosition = new Vector2(screenWidth - 1 - paddleWidth, position.Y);
            source = new Rectangle(0, 0, paddleWidth, paddleHeight);

        }


        internal void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Left) && !previousKeyboard.IsKeyDown(Keys.Left))
            {
                velocity = new Vector2(-INITIAL_SPEED, 0);
            }
            else if (keyboard.IsKeyDown(Keys.Right) && !previousKeyboard.IsKeyDown(Keys.Right))
            {
                velocity = new Vector2(INITIAL_SPEED, 0);
            }
            else if (keyboard.IsKeyDown(Keys.Left) && previousKeyboard.IsKeyDown(Keys.Left))
            {
                velocity = velocity + (-ACCELERATION * time);
            }
            else if (keyboard.IsKeyDown(Keys.Right) && previousKeyboard.IsKeyDown(Keys.Right))
            {
                velocity = velocity + (ACCELERATION * time);
            }
            else
            {
                velocity = Vector2.Zero;
            }

            position = position + (velocity * time);
            position = Vector2.Clamp(position, minPosition, maxPosition);

            previousKeyboard = keyboard;
        }


        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D obj)
        {
            spriteBatch.Draw(obj, position, source, Color.White);
        }
    }
    
    class Wall
    {
        private const int bricksPerRow = 20; // 줄하나 당 벽돌 갯수
        private const double wallHeightRatio = 0.20; // 벽돌 굵기
        private const int numRows = 7; // 벽돌 line수
        private static readonly Color[] colors = { // 벽돌 색깔 설정
            Color.Red, Color.Orange, Color.Yellow, Color.Green,
            Color.Blue, Color.Navy, Color.Violet
        };

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

            double wallHeight = screenHeight * wallHeightRatio;
            wallBounds = new Rectangle(0, (int)(wallHeight / 2), screenWidth, (int)(wallHeight + wallHeight/2));
            brickHeight = wallBounds.Height / numRows; // 총 벽돌 높이는 높이에서 벽돌 라인수로 나눈 것으로 구할 수 있다.
            brickWidth = screenWidth / bricksPerRow; // 총 벽돌 너비는 스크린 너비 나누기 줄하나당 벽돌 개수로 구할 수 있다.

            for (int i = 0; i < numRows * bricksPerRow; i++)
            {
                int row = i / bricksPerRow;
                int column = i % bricksPerRow;
                bricks.Add(new Brick(row, column, column * brickWidth, wallBounds.Top + (row * brickHeight), brickWidth, brickHeight));
            }
        }
        


        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime,Texture2D obj, SpriteFont font, Texture2D heart)
        {
            foreach (Brick brick in bricks)
            {
                if (brick.State == Brick.BrickState.NOT_BROKEN)
                {
                    spriteBatch.Draw(obj, brick.Bounds, colors[colors.Length - brick.Row - 1]);
                }
            }
            spriteBatch.DrawString(font, "Alkanoid  by YunMi", new Vector2(50, 10), Color.Beige);
        }

        internal List<Rectangle> DestroyBricksAt(Rectangle r)
        {
            List<Rectangle> destroyedRectangles = new List<Rectangle>(); 
            {
                foreach (Brick brick in bricks) // 벽돌 list돌면서 그린다.
                {
                    if (brick.State == Brick.BrickState.NOT_BROKEN && brick.Bounds.Intersects(r))
                    {
                        brick.State = Brick.BrickState.BROKEN;
                        destroyedRectangles.Add(brick.Bounds);
                        break;
                    }
                }
            }

            return destroyedRectangles;
        }
    }

    class Brick
    {
        public enum BrickState { NOT_BROKEN, BROKEN }; // 벽돌 상태는 깨지거나 안깨지거나 
        private readonly int column;
        private readonly int row;
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

