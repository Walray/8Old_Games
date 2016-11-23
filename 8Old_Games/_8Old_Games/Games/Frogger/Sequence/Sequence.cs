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

namespace _8Old_Games.Games.Frogger.Sequence {
    //시퀀스의 뼈대가 되는 abstract 클래스
    abstract public class Sequence {
        //입력 타이밍 제어용 변수
        protected double mTimeSinceLastInput;
        protected const float MIN_TIME = 0.12f;

        protected Sequence() {; }

        abstract public void initialize();
        abstract public State update(GameTime gameTime, KeyboardState ks);
        abstract public void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin);

    }
}
