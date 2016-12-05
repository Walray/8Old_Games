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
using _8Old_Games.Games.Bomberman.Sequence;

namespace _8Old_Games.Games.Bomberman {
    public class Stage {
        const int WIDTH = 19;
        const int HEIGHT = 15;
        const int STAGE_ID_2PLAYERS = 0;
        const int EXPLOSION_TIME = 180;
        const int EXPLOSION_LIFE = 60;

        struct StageData {
            public int mEnemyNumber;
            public int mBrickRate;
            public int mItemPowerNumber;
            public int mItemBombNumber;
            public StageData(int enemyNumber, int brickRate, int itemPowerNumber, int itemBombNumber) {
                mEnemyNumber = enemyNumber;
                mBrickRate = brickRate;
                mItemBombNumber = itemBombNumber;
                mItemPowerNumber = itemPowerNumber;
            }
        };
        
        private StageData[] mStageData;

        private StaticObject[,] mStaticObjects;

        private DynamicObject[] mDynamicObjects;
        private int mDynamicObjectNumber;
        private Random mRand;
        private int mStageID;

        public Stage() {; }

        public void initialize(int stageID) {
            mStageID = stageID;
            mStageData = new StageData[3];
            mStageData[0] = new StageData(2,90,4,6);
            mStageData[1] = new StageData(3, 80, 1, 0);
            mStageData[2] = new StageData(6, 50, 3, 2);
            StageData stageTest = mStageData[0];
            mStaticObjects = new StaticObject[HEIGHT, WIDTH];
            Point[] brickList = new Point[HEIGHT* WIDTH];
            Point[] floorList = new Point[HEIGHT * WIDTH];
            int brickNumber = 0;
            int floorNumber = 0;
            mRand = new Random();

            for (int y=0; y<HEIGHT; y++) {
                for(int x=0; x<WIDTH; x++) {
                    mStaticObjects[y, x] = new StaticObject();
                    StaticObject o = mStaticObjects[y, x];
                    if (x==0 || y==0 || x==WIDTH-1 || y == HEIGHT - 1) {
                        o.setFlag(StaticObject.Flag.FLAG_WALL);
                    }
                    else if ( (x%2==0) && (y%2==0)) {
                        o.setFlag(StaticObject.Flag.FLAG_WALL);
                    }
                    else if (y + x < 4) {
                        ;
                    }
                    else if(mStageID==0 && (y + x) > (WIDTH + HEIGHT - 6)) {
                        ;//2인용 나중에 손보기
                    }
                    else {
                        if (mRand.Next(100) < stageTest.mBrickRate) {
                            o.setFlag(StaticObject.Flag.FLAG_BRICK);

                            brickList[brickNumber].X = x;
                            brickList[brickNumber].Y = y;

                            
                            brickNumber++;
                        }
                        else {
                            floorList[floorNumber].X = x;
                            floorList[floorNumber].Y = y;
                            floorNumber++;

                        }
                    }
                }
            }
            //아이템 설치
            int powerNumber = stageTest.mItemPowerNumber;
            int bombNumber = stageTest.mItemBombNumber;
            
            for (int i=0; i<powerNumber+bombNumber; i++) {
                int swapped = mRand.Next(brickNumber - 1 - i) + i;
                Point t = brickList[i];
                brickList[i] = brickList[swapped];
                brickList[swapped] = t;

                int x = brickList[i].X;
                int y = brickList[i].Y;
                StaticObject o = mStaticObjects[y, x];
                if (i < powerNumber) {
                    o.setFlag(StaticObject.Flag.FLAG_ITEM_POWER);
                }
                else {
                    o.setFlag(StaticObject.Flag.FLAG_ITEM_BOMB);

                }
            }

            int playerNumber = (mStageID == 0) ? 2 : 1;
            int enemyNumber = stageTest.mEnemyNumber;
            mDynamicObjectNumber = playerNumber + enemyNumber;
            mDynamicObjects = new DynamicObject[mDynamicObjectNumber];
            mDynamicObjects[0] = new DynamicObject();
            mDynamicObjects[0].set(1, 1, DynamicObject.Type.TYPE_PLAYER);
            mDynamicObjects[0].PlayerID = 0;

            if (mStageID==0) {
                mDynamicObjects[1] = new DynamicObject();
                mDynamicObjects[1].set(WIDTH - 2, HEIGHT - 2, DynamicObject.Type.TYPE_PLAYER); ;
                mDynamicObjects[1].PlayerID = 1;
            }
            for(int i = 0; i < enemyNumber; i++ ) {
                int swapped = mRand.Next(floorNumber - 1 - i) + i;
                Point t = floorList[i];
                floorList[i] = floorList[swapped];
                floorList[swapped] = t;

                int x = floorList[i].X;
                int y = floorList[i].Y;
                mDynamicObjects[playerNumber + i] = new DynamicObject();
                mDynamicObjects[playerNumber + i].set(x,y, DynamicObject.Type.TYPE_ENEMY);
            }

        }
        public void draw(SpriteBatch spriteBatch, Texture2D obj) {
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    o.draw(spriteBatch, x, y, obj);
                }
            }
            for (int i = 0; i < mDynamicObjectNumber; ++i) {
                mDynamicObjects[i].draw(spriteBatch, obj);
            }
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    o.drawExplosion(spriteBatch, x, y, obj);
                }
            }
        }
        public void update(GameTime gametime, KeyboardState ks) {
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    if (o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                       o.Count++;

                        if (o.checkFlag(StaticObject.Flag.FLAG_EXPLODING)) { 
                            if (o.Count == EXPLOSION_LIFE) {
                                o.resetFlag(StaticObject.Flag.FLAG_EXPLODING | StaticObject.Flag.FLAG_BOMB);
                                o.Count = 0;
                            }
                        } else { 
                            if (o.Count == EXPLOSION_TIME) {
                                o.setFlag(StaticObject.Flag.FLAG_EXPLODING);
                                o.Count = 0;
                            } else if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) { 
                                o.setFlag(StaticObject.Flag.FLAG_EXPLODING);
                                o.Count = 0;
                            }
                        }
                    } else if (o.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) {
                            o.Count++;
                            if (o.Count == EXPLOSION_LIFE) {
                                o.Count = 0;
                                o.resetFlag(StaticObject.Flag.FLAG_BRICK);
                            }
                        }
                    }

                    o.resetFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y);
                }
            }

            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    if (o.checkFlag(StaticObject.Flag.FLAG_EXPLODING)) {
                        setFire(x, y);
                    }
                }
            }
            
     
            int[] bombNumber= { 0, 0 };
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    if (o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                        bombNumber[o.BombOwner.PlayerID]++;
                    }
                }
            }
            for (int i = 0; i < mDynamicObjectNumber; i++) {
                if (mDynamicObjects[i].isDead()) { 
                    continue;
                }

                for (int j = 0; j < 2; ++j) {
                    if (mDynamicObjects[i].LastBombX[j] >= 0) { 
                        if (!mDynamicObjects[i].isIntersectWall(mDynamicObjects[i].LastBombX[j], mDynamicObjects[i].LastBombY[j])) {
                            mDynamicObjects[i].LastBombX[j] = mDynamicObjects[i].LastBombY[j] = -1;
                        }
                    }
                }

                int x, y;
                mDynamicObjects[i].getCell(out x, out y);

                int[] wallsX=new int[9];
                int[] wallsY=new int[9];
                int wallNumber = 0;

                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        int tx = x + j - 1;
                        int ty = y + k - 1;

                        StaticObject o = mStaticObjects[ty, tx];

                        if (o.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                            bool myBomb0 = (mDynamicObjects[i].LastBombX[0] == tx) && (mDynamicObjects[i].LastBombY[0] == ty);
                            bool myBomb1 = (mDynamicObjects[i].LastBombX[1] == tx) && (mDynamicObjects[i].LastBombY[1] == ty);
                            if (!myBomb0 && !myBomb1) { 
                                wallsX[wallNumber] = x + j - 1;
                                wallsY[wallNumber] = y + k - 1;
                                ++wallNumber;
                            }
                        }
                    }
                }
                mDynamicObjects[i].update(wallsX, wallsY, wallNumber, ks);

                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        int tx= x + j - 1;
                        int ty =y +k - 1;
                        StaticObject o = mStaticObjects[ty, tx];
                        if (mDynamicObjects[i].isIntersectWall(tx,ty)) {
                            if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) {
                                mDynamicObjects[i].die(); 
                            } else if (!o.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                                if (o.checkFlag(StaticObject.Flag.FLAG_ITEM_POWER)) {
                                    o.resetFlag(StaticObject.Flag.FLAG_ITEM_POWER);
                                    mDynamicObjects[i].BombPower++;
                                } else if (o.checkFlag(StaticObject.Flag.FLAG_ITEM_BOMB)) {
                                    o.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB);
                                    mDynamicObjects[i].BombNumber++;
                                }
                            }
                        }
                    }
                }

                mDynamicObjects[i].getCell(out x, out y);

                if (mDynamicObjects[i].hasBombButtonPressed(ks)) {
                    if (bombNumber[mDynamicObjects[i].PlayerID] < mDynamicObjects[i].BombNumber) {
                      
                        StaticObject o = mStaticObjects[y, x];
                        if (!o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                            o.setFlag(StaticObject.Flag.FLAG_BOMB);
                            o.BombOwner = mDynamicObjects[i];
                            o.Count = 0;
                            
                            if (mDynamicObjects[i].LastBombX[0] < 0) {
                                mDynamicObjects[i].LastBombX[0] = x;
                                mDynamicObjects[i].LastBombY[0] = y;
                            } else {
                                mDynamicObjects[i].LastBombX[1] = x;
                                mDynamicObjects[i].LastBombY[1] = y;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < mDynamicObjectNumber; ++i) {
                for (int j = i + 1; j < mDynamicObjectNumber; ++j) {
                    mDynamicObjects[i].doCollisionReactionToDynamic(mDynamicObjects[j]);
                }
            }

        }


        void setFire(int x, int y) {
            StaticObject o = mStaticObjects[y, x];
            int power = o.BombOwner.BombPower;
            int end;
            
            //좌
            end = (x - power < 0) ? 0 : (x - power);
            for (int i = x - 1; i >= end; --i) {
                StaticObject to = mStaticObjects[y, i];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_X);
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            //우
            end = (x + power >= WIDTH) ? (WIDTH - 1) : (x + power);
            for (int i = x + 1; i <= end; ++i) {
                StaticObject to = mStaticObjects[y, i];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_X);
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            //상
            end = (y - power < 0) ? 0 : (y - power);
            for (int i = y - 1; i >= end; --i) {
                StaticObject to = mStaticObjects[i, x];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_Y);
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            //하
            end = (y + power >= HEIGHT) ? (HEIGHT - 1) : (y + power);
            for (int i = y + 1; i <= end; ++i) {
                StaticObject to = mStaticObjects[i, x];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_Y);
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            

            //연쇄 폭발 처리

            //좌
            end = (x - power < 0) ? 0 : (x - power);
            for (int i = x - 1; i >= end; --i) {
                StaticObject to = mStaticObjects[y, i];
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) { 
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) {
                        to.Count = 0;
                    }
                    break;
                }
            }
            //우
            end = (x + power >= WIDTH) ? (WIDTH - 1) : (x + power);
            for (int i = x + 1; i <= end; ++i) {
                StaticObject to = mStaticObjects[y, i];
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) {
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        to.Count = 0;
                    }
                    break;
                }
            }
            //상
            end = (y - power < 0) ? 0 : (y - power);
            for (int i = y - 1; i >= end; --i) {
                StaticObject to = mStaticObjects[i, x];
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) {
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        to.Count = 0;
                    }
                    break;
                }
            }
            //하
            end = (y + power >= HEIGHT) ? (HEIGHT - 1) : (y + power);
            for (int i = y + 1; i <= end; ++i) {
                StaticObject to = mStaticObjects[i, x];
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) {
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        to.Count = 0;
                    }
                    break;
                }
            }
        }

        bool hasCleared() {
	        for ( int i = 0; i<mDynamicObjectNumber; ++i ){
		        if ( mDynamicObjects[i].isEnemy() ){
			        return false;
		        }
	        }
	        return true;
        }

        bool isAlive(int playerID)  {
	        for ( int i = 0; i<mDynamicObjectNumber; ++i ){
		        if ( mDynamicObjects[i].getType == DynamicObject.Type.TYPE_PLAYER ){
			        if ( mDynamicObjects[i].PlayerID == playerID ){
				        return true;
			        }
		        }
	        }
	        return false;
        }

    }
}
