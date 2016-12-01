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
using _8Old_Games.Games.CatchMouse.Sequence;
//시퀀스 만들고 풀어주세요

/*
 * 초기화(initialize), 논리(update), 그리고 그리기(draw) 코드 반드시 분리
 * 필요하면 함수 원형 자유롭게 변경
*/
namespace _8Old_Games.Games.CatchMouse {
    public enum State { START, SELECTION, PLAY, MENU, EXIT, NOTHING }; //자유롭게 추가
    public class CatchMouse {
        State state;

        /*
        모든 리소스를 담는 변수는 public static으로 선언 ex) 
        public static Texture2D selectSize;
        */
        public CatchMouse() {; }
        public void initialize() {; }
        
        public Selector update(GameTime gameTime) { return Selector.MAIN_SELECTOR; }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime) {; }

    }
}
