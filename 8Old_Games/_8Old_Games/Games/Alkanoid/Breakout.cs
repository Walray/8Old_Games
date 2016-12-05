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
            Active,
            Dead
        }
        
        private State state;
        
        private Paddle paddle;

        private Wall wall;
        
        private int screenWidth, screenHeight;

        private const int ballWidth = 32;
        private const int ballHeight = 32;


        private Vector2 initialPosition;

        private Vector2 lastPosition;
        private Vector2 position;

        private Vector2 initialDirection = new Vector2(0, 1);
        private Vector2 direction = new Vector2(0, 1);
        
        private const int initialSpeed = 150;
        private const int speedIncrement = 75;
        private const int maxSpeed = 750;
        private int speed = initialSpeed;
        
        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, ballWidth,ballHeight); }
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
            if (state == State.Active)
            {
                UpdatePosition(gameTime);

                if (position.Y > screenHeight)
                {
                    state = State.Dead;
                }
                else
                {
                    HandleCollisions();
                }
            }
            else if (state == State.Dead && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                LaunchBall();
            }
        }
        
        private void LaunchBall()
        {
            state = State.Active;
            speed = initialSpeed;
            direction = initialDirection;
            position = initialPosition - new Vector2(ballWidth / 2, ballHeight / 2);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            lastPosition = position;
            position = position + direction * (float)(speed * gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void HandleCollisions()
        {
            HandleBoardCollisions();
            HandlePaddleCollision();
            HandlWallCollision();
        }

        private void HandleBoardCollisions()
        {
            if (position.Y < 0)
            {
                position.Y = 0;
                direction.Y = -direction.Y;
            }

            if (position.X < 0)
            {
                position.X = 0;
                direction.X = -direction.X;
            }
            else if (position.X + ballWidth > screenWidth)
            {
                position.X = screenWidth - ballHeight;
                direction.X = -direction.X;
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
                
                speed += speedIncrement;
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
                    Console.WriteLine("left");
                    changed = true;
                }
                else if (direction.X < 0 && LineIntersects(lastPosition + centerLeft, position + centerLeft, topRight, bottomRight))
                {
                    newDirection.X *= -1;
                    Console.WriteLine("right");
                    changed = true;
                }

                if (direction.Y > 0 && LineIntersects(lastPosition + centerBottom, position + centerBottom, topLeft, topRight))
                {
                    newDirection.Y *= -1;
                    Console.WriteLine("top");
                    changed = true;
                }
                else if (direction.Y < 0 && LineIntersects(lastPosition + centerTop, position + centerTop, bottomLeft, bottomRight))
                {
                    newDirection.Y *= -1;
                    Console.WriteLine("bottom");
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

        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime, Texture2D obj)
        {
            if (state == State.Active)
            {
                spriteBatch.Draw(obj, position, Color.White);
            }
        }
    }

    class Paddle
    {
        const int INITIAL_SPEED = 300;
        Vector2 ACCELERATION = new Vector2(2000, 0);
        const int PADDLE_OFFSET = 10;

        private const int paddleWidth = 150;
        private const int paddleHeight = 21;

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
        private const int bricksPerRow = 20;
        private const double wallHeightRatio = 0.20;
        private const int numRows = 7;
        private static readonly Color[] colors = {
            Color.Red, Color.Orange, Color.Yellow, Color.Green,
            Color.Blue, Color.Indigo, Color.Violet
        };

        private readonly int screenWidth;
        private readonly int screenHeight;
        private float brickHeight;
        private float brickWidth;
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
            wallBounds = new Rectangle(0, (int)(wallHeight / 2),
                        screenWidth, (int)(wallHeight + wallHeight / 2));
            brickHeight = wallBounds.Height / numRows;
            brickWidth = screenWidth / bricksPerRow;
            for (int i = 0; i < numRows * bricksPerRow; i++)
            {
                int row = i / bricksPerRow;
                int column = i % bricksPerRow;
                bricks.Add(new Brick(
                        row, column,
                        column * brickWidth,
                        wallBounds.Top + (row * brickHeight),
                        brickWidth,
                        brickHeight));
            }
        }
        


        internal void Draw(SpriteBatch spriteBatch, GameTime gameTime,Texture2D obj, SpriteFont font)
        {
            foreach (Brick brick in bricks)
            {
                if (brick.State == Brick.BrickState.ALIVE)
                {
                    spriteBatch.Draw(obj, brick.Bounds, colors[colors.Length - brick.Row - 1]);
                }
            }
            spriteBatch.DrawString(font, "Alkanoid", new Vector2(550, 10), Color.Beige);
        }

        internal List<Rectangle> DestroyBricksAt(Rectangle r)
        {
            List<Rectangle> destroyedRectangles = new List<Rectangle>();

            if (wallBounds.Intersects(r))
            {
                foreach (Brick brick in bricks)
                {
                    if (brick.State == Brick.BrickState.ALIVE && brick.Bounds.Intersects(r))
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
        public enum BrickState { ALIVE, BROKEN };
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

        public Brick(int row, int column, float x, float y, float w, float h)
        {
            this.state = BrickState.ALIVE;
            this.row = row;
            this.column = column;
            this.bounds = new Rectangle((int)x, (int)y, (int)w, (int)h);
        }
    }
}

