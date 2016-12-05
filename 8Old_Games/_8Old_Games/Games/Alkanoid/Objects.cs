using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Alkanoid
{
    // 벽돌이 왼쪽, 오른쪽, 아래, 위로 부딪힐 때 처리
    public enum Collision { NO, LEFT, RIGHT, yes, DOWN };

    public class Objects
    {
        // 공이나 막대기의 위치 사이즈 속도 조절
        public Vector2 position;
        public Vector2 size;
        public Vector2 speed = new Vector2(0F, 0F);

        public static float err = GlobalData.err;

        public Objects(Vector2 position, Vector2 size, Vector2 speed)
        {
            this.position = position;
            this.size = size;
            this.speed = speed;
        }

        public Objects(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        public virtual Collision CollideType(Objects objOther)
        {
            return Collision.NO;
        }
        
        public virtual void Load(ContentManager content)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        
    }
}
