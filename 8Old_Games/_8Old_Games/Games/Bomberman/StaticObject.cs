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


namespace _8Old_Games.Games.Bomberman {
    public class StaticObject {
        //정적 오브젝트의 플래그 정보, 비트 단위로 조작해 여러 상태를 겹칠 수 있다.
        public enum Flag {
            FLAG_NOTHING = (1 << 0), //초기화용
            FLAG_WALL =(1<<1), // 벽
            FLAG_BRICK=(1<<2), // 벽돌
            FLAG_BOMB = (1 << 3), // 폭탄
            FLAG_ITEM_BOMB =(1<<4), // 폭탄 아이템
            FLAG_ITEM_POWER=(1<<5), // 화력 아이템
            FLAG_FIRE_X = (1 << 6), // 가로 폭발
            FLAG_FIRE_Y = (1 << 7), // 세로 폭발
            FLAG_EXPLODING = (1 << 8), // 폭발 중심
        };

        private uint mFlags; //정적 오브젝트의 종류
        private int mCount; // 폭발 카운트

        private DynamicObject mBombOwner;
        public DynamicObject BombOwner { get { return mBombOwner; } set { mBombOwner = value; } }
        public int Count { get { return mCount; } set { mCount = value; } }

        public StaticObject() {
            setFlag(Flag.FLAG_NOTHING);
        }

 
        public void initialze() {; }

        //정적 오브젝트 그리기 함수
        public void draw(SpriteBatch spriteBatch, int x, int y, Texture2D obj) {
            int srcX = -1, srcY = -1;
            bool floor = false;
            //이미지 파일 내에서 좌표로 해당 이미지 추출
            if(checkFlag(Flag.FLAG_WALL)) {
                srcX = 48;
                srcY = 16;
            }
            else if (checkFlag(Flag.FLAG_BRICK)) {
                if (checkFlag(Flag.FLAG_FIRE_X | Flag.FLAG_FIRE_Y)) { 
                    srcX = 0;
                    srcY = 48;
                } else {
                    srcX = 0;
                    srcY = 32;
                }
            }
            else {
                srcX = 16;
                srcY = 32;
                floor = true;
            }
            //그리기
            spriteBatch.Draw(obj, new Rectangle(x * 32, y *32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);

            //아이템 그리기(주석은 디버깅용), 바닥일시에만 그린다.
            if( /*true||*/ floor) {
                srcX = -1;
                if (checkFlag(Flag.FLAG_BOMB) && !checkFlag(Flag.FLAG_EXPLODING)) {
                    srcX = 32;
                    srcY = 32;
                } else if (checkFlag(Flag.FLAG_ITEM_BOMB)) {
                    srcX = 32;
                    srcY = 0;
                } else if (checkFlag(Flag.FLAG_ITEM_POWER)) {
                    srcX = 48;
                    srcY = 0;
                }
                if (srcX != -1) {
                    spriteBatch.Draw(obj, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);
                }
            }
            
        }
        //폭발 그리기
        public void drawExplosion(SpriteBatch spriteBatch, int x, int y, Texture2D obj) {
            int srcX = -1;
            int srcY = -1;
            //이미지 파일 내에서 좌표로 해당 이미지 추출
            if ( !checkFlag(Flag.FLAG_WALL ) && !checkFlag(Flag.FLAG_BRICK ) ){ 
		        if (checkFlag(Flag.FLAG_EXPLODING) ){
			        srcX = 48;
			        srcY = 32;
		        }else if (checkFlag(Flag.FLAG_FIRE_X) ){
			        if (checkFlag(Flag.FLAG_FIRE_Y) ){
				        srcX = 48;
				        srcY = 32;
			        }else{
				        srcX = 0;
				        srcY = 16;
			        }
		        }else if (checkFlag(Flag.FLAG_FIRE_Y) ){
			        srcX = 16;
			        srcY = 16;
		        }
	        }
	        if ( srcX != -1 ) {
                spriteBatch.Draw(obj, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);

            }
        }

        //플래그를 확인하는 함수
        public bool checkFlag(Flag f) {
            return (mFlags &(uint)f) != 0;
        }

        //플래그를 SET하는 함수
        public void setFlag(Flag f) {
            mFlags |= (uint)f;
        }
        //플래그를 RESET하는 함수
        public void resetFlag(Flag f) {
            mFlags &= ~(uint)f;
        }


    }
}
