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

namespace _8Old_Games.Games.Frogger {
    public class Map {
        //맵 크기 및 리소스 크기 정보에 대한 상수들
        public const int WIDTH = 32;
        public const int HEIGHT = 32;
        public const int ROW = 13;
        public const int COL = 14;

        //맵 타일 정보
        enum Type { NOTHING, SAFE_1, ROAD, SAFE_2, WATER, END, GOAL, KING};

        //맵 (13*14)
        private Type[] mMap;
        //이동가능 여부
        private bool[] mWalk;

        public Map() {; }

        public void initialize() {
            int index = 0;
            mMap = new Type[ROW * COL];
            mWalk = new bool[ROW * COL];
            for (int y = 0; y < ROW; y++) {
                for (int x = 0; x < COL; x++) {
                    index = x + y * COL;
                    if (y == ROW - 1) { mMap[index] = Type.SAFE_1; mWalk[index]=true;} 
                    else if (y > 6 && y < ROW - 1) { mMap[index] = Type.ROAD; mWalk[index] = true; } 
                    else if (y == 6) { mMap[index] = Type.SAFE_2; mWalk[index] = true; } 
                    else if (y > 0 && y < 6) { mMap[index] = Type.WATER; mWalk[index] = false; } 
                    else if (y == 0) {
                        if (x % 3 == 0) {
                            mMap[index] = Type.GOAL; 
                            mWalk[index] = true;
                        } 
                        else { 
                        mMap[index] = Type.END;
                        mWalk[index] = false;
                        }
                    }
                }//for (int x = 0; x < COL; x++)
            }//for (int y = 0; y < ROW; y++)
        }

        //해당 위치의 GOAL이 채워졌는지(KING) 확인
        public bool isKing(int x, int y) {
            if (mMap[y * COL + x] == Type.KING) return true;
            else return false; 
        }

        //GOAL에 개구리가 도달 했는지 검사하고 도달했으면 해당 GOAL을 KING으로 변경
        public bool checkClear(int frogX, int frogY) {
            if (frogY != 0) return false;
            if (frogX % 3 == 0) {
                resetWalk(frogX, frogY);
                mMap[frogX + frogY * COL] = Type.KING;
                return true;
             };
            return false;
        }

        //게임이 끝났는지 검사(모든 GOAL이 KING으로 바뀌었는지)
        public bool checkEnd() {
            for(int x=0; x<ROW; x++) {
                if (getWalk(x, 0)) return false;
            }
            return true;
        }

        //해당 위치로 이동할 수 있는지 검사
        public bool getWalk(int x, int y) {
            if (!(x>=0 && x<COL && y>=0 && y<ROW)) return false;
            int index = x + y * COL;
            return mWalk[index];
        }
        
        //해당 위치를 걷기 가능으로 set 
        public void setWalk(int x, int y) {
            if (!(x >= 0 && x < COL && y >= 0 && y < ROW)) return;
            int index = x + y * COL;
            mWalk[index]=true;
        }

        //해당 위치를 걷기 불가능으로 set
        public void resetWalk(int x, int y) {
            if (!(x >= 0 && x < COL && y >= 0 && y < ROW)) return;
            int index = x + y * COL;
            mWalk[index] = false;
        }

        //해당 오브젝트의 리소스 파일 내 위치 계산
        public Rectangle getSourceRect(int x, int y) {
            int index = x + y * COL;
            Type type = mMap[index];
            return new Rectangle(((int)type - 1) * WIDTH, 0, WIDTH, HEIGHT);
        }

        //그리기 함수
        public void draw(Texture2D map, SpriteBatch spriteBatch, Vector2 origin) {
            int index = 0;
            for (int y = 0; y < ROW; y++) {
                for (int x = 0; x < COL; x++) {
                    index = x + y * COL;
                    spriteBatch.Draw(map, new Rectangle(x * Map.WIDTH+(int)origin.X, y * HEIGHT + (int)origin.Y, WIDTH, HEIGHT), getSourceRect(x,y), Color.White);
                }
            }

        }
    }
}
