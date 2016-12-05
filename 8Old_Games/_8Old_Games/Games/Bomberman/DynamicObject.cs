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
    public class DynamicObject {
        public enum Type {
            TYPE_PLAYER,
            TYPE_ENEMY,
            TYPE_NONE,
        };
        private const int PLAYER_SPEED = 2000;
        private const int ENEMY_SPEED = 1000;
        private const int HALF_SIZE = 12000; // 3/4크기

        private Type mType;
        private int mX;
        private int mY;
        private int mDirectionX;
        private int mDirectionY;
        private int mPlayerID;
        private int mBombPower;
        private int mBombNumber;
        private int[] mLastBombX=new int[2];
        private int[] mLastBombY=new int[2];

        public int PlayerID { get { return mPlayerID; } set { mPlayerID = value; } }

        public int BombPower { get { return mBombPower; } set { mBombPower = value; } }
        public int BombNumber { get { return mBombNumber; } set { mBombNumber = value; } }
        public int[] LastBombX { get { return mLastBombX; } set { mLastBombX = value; } }
        public int[] LastBombY { get { return mLastBombY; } set { mLastBombY = value; } }
        public Type getType { get { return mType; } set { mType = value; } }


        private Random mRand;

        public DynamicObject() {
            mType = Type.TYPE_NONE;
            mRand = new Random();
            mPlayerID = -1;
            mLastBombX = new int[2];
            mLastBombY = new int[2];
            mLastBombX[0] = mLastBombX[1] = -1;
            mLastBombY[0] = mLastBombY[1] = -1;
            mBombPower=10;
            mBombNumber=10;
            mX = mY = -1;
            mDirectionX = mDirectionY = 0;

    }
        //셀 좌표를 내부좌표로 치환
        public int convertCelltoInner(int x) {
            return x * 32000 + 16000;
        }
        //내부좌표를 셀좌표로
        public int convertInnertoCell(int x) {
            return (x - 16000 + 500) / 1000; //500 반올림
        }
        public void set(int x, int y, Type type) {
            mX = convertCelltoInner(x);
            mY = convertCelltoInner(y);
            mType = type;
            if (mType == Type.TYPE_ENEMY) {
                mDirectionX = mDirectionY = 0;
                switch (mRand.Next(0,4)) {
                    case 0: mDirectionX = 1; break;
                    case 1: mDirectionX = -1; break;
                    case 2: mDirectionY = 1; break;
                    case 3: mDirectionY = -1; break;
                }
            }
        }
        private void getDirection(out int dx, out int dy, KeyboardState ks) {
            dx = dy = 0;
            if (mType == Type.TYPE_PLAYER) {
                if (mPlayerID == 0) {
                    if (ks.IsKeyDown(Keys.Up)) {
                        dy = -1;
                    } else if (ks.IsKeyDown(Keys.Down)) {
                        dy = 1;
                    }
                    if (ks.IsKeyDown(Keys.Left)) {
                        dx = -1;
                    } else if (ks.IsKeyDown(Keys.Right)) {
                        dx = 1;
                    }
                } else if (mPlayerID == 1) {
                    if (ks.IsKeyDown(Keys.W)) {
                        dy = -1;
                    } else if (ks.IsKeyDown(Keys.S)) {
                        dy = 1;
                    }
                    if (ks.IsKeyDown(Keys.A)) {
                        dx = -1;
                    } else if (ks.IsKeyDown(Keys.D)) {
                        dx = 1;
                    }

                }

            } 
            else if (mType == Type.TYPE_ENEMY) {
                dx = mDirectionX;
                dy = mDirectionY;
            }
        }

        private void getVelocity(out int dx, out int dy, KeyboardState ks) {
            int speedX, speedY;
            if (mType == Type.TYPE_ENEMY) {
                speedX = ENEMY_SPEED;
                speedY = ENEMY_SPEED;
            } 
            else {
                speedX = PLAYER_SPEED;
                speedY = PLAYER_SPEED;
            }
            getDirection(out dx, out dy, ks);
            dx *= speedX;
            dy *= speedY;
        }

        public void draw(SpriteBatch spriteBatch, Texture2D obj) {
            if (isDead()) return;
            int destX = convertInnertoCell(mX);
            int destY = convertInnertoCell(mY);
            int srcX=0, srcY=0;
            switch (mType) {
                case Type.TYPE_PLAYER:
                    switch (mPlayerID) {
                        case 0: srcX = 0; srcY = 0; break;
                        case 1: srcX = 16; srcY = 0; break;
                    }
                break;
                case Type.TYPE_ENEMY: srcX = 32; srcY = 16; break;
            }
            spriteBatch.Draw(obj, new Rectangle(destX, destY, 32, 32), new Rectangle(srcX, srcY, 16, 16), Color.White);
        }

        private bool isIntersectWall(int x, int y, int wallX, int wallY) {
            int wx = convertCelltoInner(wallX);
            int wy = convertCelltoInner(wallY);

            int al = x - HALF_SIZE; //left A
            int ar = x + HALF_SIZE; //right A
            int bl = wx - 16000; //left B
            int br = wx + 16000; //right B
            if ((al < br) && (ar > bl)) {
                int at = y - HALF_SIZE; //top A
                int ab = y + HALF_SIZE; //bottom A
                int bt = wy - 16000; //top B
                int bb = wy + 16000; //bottom B
                if ((at < bb) && (ab > bt)) {
                    return true;
                }
            }
            return false;
        }

        public bool isIntersectWall(int wallX, int wallY) {
            return isIntersectWall(mX, mY, wallX, wallY);
        }

        public void update(int[] wallsX, int[] wallsY, int wallNumber, KeyboardState ks) {
            int dx, dy;
            getVelocity(out dx, out dy, ks);
            int movedX = mX + dx;
            int movedY = mY + dy;
            bool hitX = false;
            bool hitY = false;
            bool hit = false;

            //벽충돌검사
            for (int i = 0; i < wallNumber; ++i) {
                if (isIntersectWall(movedX, mY, wallsX[i], wallsY[i])) {
                    hitX = hit = true;
                }
                if (isIntersectWall(mX, movedY, wallsX[i], wallsY[i])) {
                    hitY = hit = true;
                }
            }
            if (hitX && !hitY) {
                mY = movedY; 
            } else if (!hitX && hitY) {
                mX = movedX; 
            } else { 
                for (int i = 0; i < wallNumber; ++i) {
                    if (isIntersectWall(movedX, movedY, wallsX[i], wallsY[i])) {
                        hit = true;
                    }
                }
                if (!hit) {
                    mX = movedX;
                    mY = movedY;
                }
            }


            if (hit && mType == Type.TYPE_ENEMY) {
                mDirectionX = mDirectionY = 0;
                switch (mRand.Next(0, 4)) {
                case 0: mDirectionX = 1; break;
                case 1: mDirectionX = -1; break;
                case 2: mDirectionY = 1; break;
                case 3: mDirectionY = -1; break;
                }
            }
        }

        public void doCollisionReactionToDynamic(DynamicObject another) {
            if (another.isDead())  return;
            
            if (isIntersect(another)) { 
                                      
                if (isPlayer() && another.isEnemy()) {
                    this.die();
                } else if (isEnemy() && another.isPlayer()) {
                    another.die();
                }
            }
        }

        public bool isIntersect( DynamicObject o )  {

            int al = mX - HALF_SIZE; //left A
                int ar = mX + HALF_SIZE; //right A
                int bl = o.mX - HALF_SIZE; //left B
                int br = o.mX + HALF_SIZE; //right B
	        if ( ( al<br ) && ( ar > bl ) ){
		        int at = mY - HALF_SIZE; //top A
                int ab = mY + HALF_SIZE; //bottom A
                int bt = o.mY - HALF_SIZE; //top B
                int bb = o.mY + HALF_SIZE; //bottom B
		        if ( ( at<bb ) && ( ab > bt ) ){
			        return true;
		        }
	        }
	        return false;
        }


        public bool hasBombButtonPressed(KeyboardState ks) {
            
	        if ( mType == Type.TYPE_PLAYER ){
		        if ( mPlayerID == 0 ){
                    return ks.IsKeyDown(Keys.L);

                } else if ( mPlayerID == 1 ){
                    return ks.IsKeyDown(Keys.G);

                }
	        }
	        return false;
        }



        public void getCell(out int x, out int y) {
	        x = mX / 32000;
	        y = mY / 32000;
        }

        public bool isPlayer() {
            return ( mType == Type.TYPE_PLAYER );
        }

        public bool isEnemy() {
            return ( mType == Type.TYPE_ENEMY );
        }

        public void die() {
            mType = Type.TYPE_NONE;
        }


        public bool isDead(){
	        return ( mType == Type.TYPE_NONE );
        }

    }
}
