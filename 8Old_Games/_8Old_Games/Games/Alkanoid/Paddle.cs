using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    public class Paddle : Objects
    {

        // 막대기가 다른 공과 충돌할 때 처리
        public Collision collideDirectionPad = Collision.NO;

        // 패드의 위 아래 움직임
        public bool moveUp = true;
        public bool moveDown = true;
        public bool moveLeft = true;
        public bool moveRight = true;

        public Paddle(Vector2 position, Vector2 size, Vector2 speed) : base(position, size, speed)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            // 위치 업데이트 부분 
            // 막대기 조정은 화살표로 조작한다.
            if (keystate.IsKeyDown(Keys.Up) && moveUp)
                position.Y = position.Y - speed.Y;
            if (keystate.IsKeyDown(Keys.Down) && moveDown)
                position.Y = position.Y + speed.Y;
            if (keystate.IsKeyDown(Keys.Left) && moveLeft)
                position.X = position.X - speed.Y;
            if (keystate.IsKeyDown(Keys.Right) && moveRight)
                position.X = position.X + speed.Y;

            // 경계부분 체크, 막대기가 윈도우 벗어나면 안된다.
            float windowWidth = GlobalData.windowSize.X;
            float windowHeight = GlobalData.windowSize.Y;
            if (position.X < 0)
                position.X = 0;
            if (position.X > windowWidth - size.X)
                position.X = windowWidth - size.X;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > windowHeight - size.Y)
                position.Y = windowHeight - size.Y;

            // 다시 원래대로 변경
            moveUp = true;
            moveDown = true;
            moveLeft = true;
            moveRight = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(Alkanoid.pad, position, Color.White);
        }

        // 충돌을 탐색하기 위한 작업
        public override Collision CollideType(Objects objOther)
        {
            float xLow = this.position.X;
            float xHigh = this.position.X + size.X;
            float yLow = this.position.Y;
            float yHigh = this.position.Y + size.Y;
            float xlLow = objOther.position.X;
            float xlHigh = objOther.position.X + objOther.size.X;
            float ylLow = objOther.position.Y;
            float ylHigh = objOther.position.Y + objOther.size.Y;

            Collision coll = Collision.NO; // 충돌 체크 변수

            // 왼쪽 충돌 탐색
            if (xLow - xlHigh >= -err && xLow - xlHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveLeft = false;
                coll = Collision.yes;
            }

            // 오른쪽 충돌 탐색
            if (xlLow - xHigh >= -err && xlLow - xHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveRight = false;
                coll = Collision.yes;
            }

            // 위쪽 충돌 탐색
            if ((xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && yLow - ylHigh >= -err && yLow - ylHigh <= 0)
            {
                moveUp = false;
                coll = Collision.yes;
            }

            // 아래쪽 충돌 탐색
            if ((xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && ylLow - yHigh >= -err && ylLow - yHigh <= 0)
            {
                moveDown = false;
                coll = Collision.yes;
            }

            return coll;
        }
    }
}
