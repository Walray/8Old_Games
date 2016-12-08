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

/*
 * 각 시퀀스는 얘로부터 상속을 받습니다.
 * abstact 메서드는 반드시 오버라이드 해주어야합니다.
 * 오버라이드할 메서드가 구현할 게임에 맞지 않으면 게임에 맞는 메서드를 서브클래스에서 새로 오버로드 해주세요
*/
namespace _8Old_Games.Games.Alkanoid.Sequence {
    //시퀀스의 뼈대가 되는 abstract 클래스
    abstract public class Sequence
    {
        protected double mTimeSinceLastInput;

        protected const float MIN_TIME = 0.15f;

        protected Sequence() {; }

        abstract public void initialize();

        abstract public State update(GameTime gameTime, KeyboardState ks);
        abstract public void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin);

    }

    /*
    abstract public class Sequence {

        //입력 타이밍 제어용 변수
        protected double mTimeSinceLastInput;
        protected const float MIN_TIME = 0.12f;

        protected Sequence() {; }

        abstract public void initialize();
        abstract public State update(GameTime gameTime, KeyboardState ks);
        abstract public void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin);

    }
    */
}
