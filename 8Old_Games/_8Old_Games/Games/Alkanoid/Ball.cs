using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    public class Ball : Objects
    {
        public bool moveUp = false;
        public bool moveLeft = false;
        public static float gravity = GlobalData.gravity; // 중력? 설정

        // 충돌처리에서 필요한 조건들
        public bool verticalMoveChange = false;
        public bool horizontalMoveChange = false;

        public Ball(Vector2 position, Vector2 size, Vector2 speed) : base(position, size, speed)
        {

        }

        public override void Update(GameTime gameTime)
        {
            // 공이 위로 닿으면 아래로 가도록 처리
            if (speed.Y <= 0)
            {
                moveUp = false;
                speed.Y = 0;
            }

            // 윈도우 창 벽면에 닿을 경우 방향바꿔줌
            if (position.X < err)
                moveLeft = false;
            if (position.X + size.X + err > GlobalData.windowSize.X)
                moveLeft = true;
            if (position.Y < err)
                moveUp = false;
            if (position.Y + size.Y + err > GlobalData.windowSize.Y)
                moveUp = true;

            // 위치를 업데이트 한다.
            if (moveLeft)
                position.X = position.X - speed.X;
            else
                position.X = position.X + speed.X;
            if (moveUp)
                position.Y = position.Y - speed.Y;
            else
                position.Y = position.Y + speed.Y;

            // 스피드 업뎃
            if (moveUp)
                speed.Y = speed.Y - gravity;
            else
                speed.Y = speed.Y + gravity;

            verticalMoveChange = false;
            horizontalMoveChange = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Alkanoid.ball, position, Color.White);
        }

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

            // 충돌된 것을 기록할 변수
            Collision coll = Collision.NO; // no로 초기화

            // 왼쪽에서 발생하는 충돌처리
            if (!horizontalMoveChange && moveLeft && xLow - xlHigh >= -err && xLow - xlHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveLeft = !moveLeft;
                horizontalMoveChange = true;
                coll = Collision.yes;
            }

            // 오른쪽에서 발생하는 충돌처리
            if (!horizontalMoveChange && !moveLeft && xlLow - xHigh >= -err && xlLow - xHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveLeft = !moveLeft;
                horizontalMoveChange = true;
                coll = Collision.yes;
            }

            // 윗부분에서 일어나는 충돌처리
            if (!verticalMoveChange && moveUp && (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && yLow - ylHigh >= -err && yLow - ylHigh <= 0)
            {
                moveUp = !moveUp;
                verticalMoveChange = true;
                coll = Collision.yes;
            }

            // 밑부분에서 일어나는 충돌처리
            if (!verticalMoveChange && !moveUp && (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && ylLow - yHigh >= -err && ylLow - yHigh <= 0)
            {
                moveUp = !moveUp;
                verticalMoveChange = true;
                coll = Collision.yes; // 충돌일어난 걸로 변경
            }

            return coll;
        }
    }
}
