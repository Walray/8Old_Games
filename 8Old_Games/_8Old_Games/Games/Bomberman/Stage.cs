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
        struct StageData {
            public int mEnemyNumber;
            public int mBrickRate;
            public int mItemPowerNumber;
            public int mItemBombNumber;
            public int mMinBrick;
            public StageData(int enemyNumber, int brickRate, int itemPowerNumber, int itemBombNumber, int minBrickNumber) {
                mEnemyNumber = enemyNumber;
                mBrickRate = brickRate;
                mItemBombNumber = itemBombNumber;
                mItemPowerNumber = itemPowerNumber;
                mMinBrick = minBrickNumber;
            }
        };
        
        private StageData[] mStageData;
        private StaticObject[,] mStaticObjects;
        private Random mRand;

        public Stage() {; }

        public void initialize() {

            mStageData = new StageData[3];
            mStageData[0] = new StageData(2,90,4,6, 4+6);
            mStageData[1] = new StageData(3, 80, 1, 0, 1 + 0);
            mStageData[2] = new StageData(6, 50, 3, 2, 0 + 1);
            mStaticObjects = new StaticObject[WIDTH, HEIGHT];
            uint[] brickList = new uint[HEIGHT* WIDTH];
            StageData stageTest = mStageData[2];
            int brickNumber = 0;
            mRand = new Random();
            bool is2P = false;
            int playerNumber = 1;
            

            for(int y=0; y<HEIGHT; y++) {
                for(int x=0; x<WIDTH; x++) {
                    mStaticObjects[x, y] = new StaticObject();
                    if (x==0 || y==0 || x==WIDTH-1 || y == HEIGHT - 1) {
                        mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_WALL);
                    }
                    else if ( (x%2==0) && (y%2==0)) {
                        mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_WALL);
                    }
                    else if (y + x < 4) {
                        ;
                    }
                    else if(is2P && (y + x) > (WIDTH + HEIGHT - 6)) {
                        ;
                    }
                    else {
                        if (mRand.Next(100) > stageTest.mBrickRate) {
                            mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_BRICK);
                            
                            brickList[brickNumber] = ((uint)x<< 16) | ((uint)y);
                            brickNumber++;
                        }
                    }
                }
            }
            

            //아이템 설치
            int powerNumber = stageTest.mItemPowerNumber;
            int bombNumber = stageTest.mItemBombNumber;

            while (brickNumber < powerNumber + bombNumber) {
                int x, y;
                x = mRand.Next(0, WIDTH);
                y = mRand.Next(0, HEIGHT);
                if (mStaticObjects[x, y].checkFlag(StaticObject.Flag.FLAG_BRICK)) continue;
                mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_BRICK);
                brickList[brickNumber] = ((uint)x << 16) | ((uint)y);
                brickNumber++;
            }

            for (int i=0; i<powerNumber+bombNumber; i++) {
                int swapped = mRand.Next(brickNumber - 1 - i) + i;
                uint t = brickList[i];
                brickList[i] = brickList[swapped];
                brickList[swapped] = t;

                uint x = brickList[i] >> 16;
                uint y = brickList[i] & 0xffff;
                if (i < powerNumber) {
                    mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_ITEM_POWER);
                }
                else {
                    mStaticObjects[x, y].setFlag(StaticObject.Flag.FLAG_ITEM_BOMB);

                }
            }
            


        }
        public void update() {
            ;
        }
        public void draw(SpriteBatch spriteBatch, Texture2D obj) {

            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    mStaticObjects[x, y].draw(spriteBatch, x, y, obj);
                }

            }
        }

    }
}
