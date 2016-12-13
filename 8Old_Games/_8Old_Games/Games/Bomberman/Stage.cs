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
        const int WIDTH = 19; // 맵 너비
        const int HEIGHT = 15; // 맵 높이
        const int EXPLOSION_TIME = 180; // 폭탄 터지는데 걸리는 시간
        const int EXPLOSION_LIFE = 60; // 폭발 수명
        const int MAX_STAGE = 5; // 최대 스테이지 수

        //스테이지 데이터 클래스
        struct StageData {
            public int mEnemyNumber; // 적 수
            public int mBrickRate; // 벽돌 비율
            public int mItemPowerNumber; // 화력 아이템 수
            public int mItemBombNumber; // 폭탄 아이템 수
            public int mSpeedEnemey; // 적 스피드 
            public StageData(int enemyNumber, int brickRate, int itemPowerNumber, int itemBombNumber, int speedEnemey) {
                mEnemyNumber = enemyNumber;
                mBrickRate = brickRate;
                mItemBombNumber = itemBombNumber;
                mItemPowerNumber = itemPowerNumber;
                mSpeedEnemey = speedEnemey;
            }
        };
        private StageData m2PStageData;
        private StageData[] mStageData; // 스테이지 데이터
        private int mCurrentStage; // 현재 스테이지 데이터
        private StaticObject[,] mStaticObjects; // 정적 오브젝트 배열(StaticObejct.Flag 참조)

        private DynamicObject[] mDynamicObjects; // 동적 오브젝트 배열(DynamicObject.Type 참조)
        private int mDynamicObjectNumber; // 동적 오브젝트 수
        private Random mRand; // 랜덤 클래스
        public static  int mStageID; // 1p=1, 2p=0 구분
        private int mLife; // 목(기본 3)
        private double mTimeSinceLastInput;
        // -2 아무도, -1 : 1p, 1: 2p, 0 : 비김
        private int mWhoWin = -2; 
        public Stage() {; }
        

        public void reinitialize(int stageID, int stageNum) {
            mStageID = stageID;
            mWhoWin =- 2;
            StageData stage = mStageData[mCurrentStage];
            if(mStageID == 0) stage = m2PStageData;
            mStaticObjects = new StaticObject[HEIGHT, WIDTH];
            Point[] brickList = new Point[HEIGHT * WIDTH];
            Point[] floorList = new Point[HEIGHT * WIDTH];
            int brickNumber = 0;
            int floorNumber = 0;

            //정적 오브젝트 초기화
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    mStaticObjects[y, x] = new StaticObject();
                    StaticObject o = mStaticObjects[y, x];
                    //외부 벽 설치
                    if (x == 0 || y == 0 || x == WIDTH - 1 || y == HEIGHT - 1) {
                        o.setFlag(StaticObject.Flag.FLAG_WALL);
                    } 
                    //중간 벽 설치
                    else if ((x % 2 == 0) && (y % 2 == 0)) {
                        o.setFlag(StaticObject.Flag.FLAG_WALL);
                    }// 좌상단 ┌는 비워둬야함
                    else if (y + x < 4) {
                        ;
                    }// 우하단 ┘는 비워둬야함
                    else if (mStageID == 0 && (y + x) > (WIDTH + HEIGHT - 6)) {
                        ;
                    }//  나머지는 정해진 비율만큼 벽돌이나 바닥으로 초기화
                    else {
                        if (mRand.Next(100) < stage.mBrickRate) {
                            o.setFlag(StaticObject.Flag.FLAG_BRICK);
                            brickList[brickNumber].X = x;
                            brickList[brickNumber].Y = y;
                            brickNumber++;
                        } else {
                            floorList[floorNumber].X = x;
                            floorList[floorNumber].Y = y;
                            floorNumber++;
                        }
                    }
                }// for (int x = 0; x < WIDTH; x++)
            }// for (int y = 0; y < HEIGHT; y++)


            //아이템 설치
            int powerNumber = stage.mItemPowerNumber;
            int bombNumber = stage.mItemBombNumber;

            for (int i = 0; i < powerNumber + bombNumber; i++) {
                int swapped = mRand.Next(brickNumber - 1 - i) + i;
                Point t = brickList[i];
                brickList[i] = brickList[swapped];
                brickList[swapped] = t;

                int x = brickList[i].X;
                int y = brickList[i].Y;
                StaticObject o = mStaticObjects[y, x];
                if (i < powerNumber) {
                    o.setFlag(StaticObject.Flag.FLAG_ITEM_POWER);
                } else {
                    o.setFlag(StaticObject.Flag.FLAG_ITEM_BOMB);

                }
            }

            //mStageID가 0이면 2p, 1이면 1p
            int playerNumber = (mStageID == 0) ? 2 : 1;
            int enemyNumber = stage.mEnemyNumber/*+1*/;
            //동적 오브젝트 초기화
            mDynamicObjectNumber = playerNumber + enemyNumber;
            mDynamicObjects = new DynamicObject[mDynamicObjectNumber];
            //1P초기화
            mDynamicObjects[0] = new DynamicObject();
            mDynamicObjects[0].set(1, 1, DynamicObject.Type.TYPE_PLAYER);
            mDynamicObjects[0].PlayerID = 0;
            //2P초기화
            if (mStageID == 0) {
                mDynamicObjects[1] = new DynamicObject();
                mDynamicObjects[1].set(WIDTH - 2, HEIGHT - 2, DynamicObject.Type.TYPE_PLAYER); ;
                mDynamicObjects[1].PlayerID = 1;
            }
            //적 초기화
            int b=0;
            for (int i = 0; i < enemyNumber/*-1*/; i++) {
                int swapped = mRand.Next(floorNumber - 1 - i) + i;
                Point t = floorList[i];
                floorList[i] = floorList[swapped];
                floorList[swapped] = t;

                int x = floorList[i].X;
                int y = floorList[i].Y;
                b = i;
                mDynamicObjects[playerNumber + i] = new DynamicObject();
                mDynamicObjects[playerNumber + i].set(x, y, DynamicObject.Type.TYPE_ENEMY,mStageData[mCurrentStage].mSpeedEnemey);
            }

        }
        public void initialize(int stageID) {
            mStageID = stageID; 
            mRand = new Random();
            mCurrentStage = 0;
            mStageData = new StageData[MAX_STAGE];
            mStageData[0] = new StageData(3, 80, 2, 2,1000);
            mStageData[1] = new StageData(4, 60, 2, 1,1200);
            mStageData[2] = new StageData(5, 40, 1, 2, 1300);
            mStageData[3] = new StageData(6, 40, 2, 1,1500);
            mStageData[4] = new StageData(6, 10, 1, 1,1500);
            m2PStageData = new StageData(5, 40, 7, 7, 1200);
            mTimeSinceLastInput = 0.0;
            reinitialize(stageID, mCurrentStage);

        }
        public void draw(SpriteBatch spriteBatch, Texture2D obj, SpriteFont font) {

            //정적 오브젝트 그리기
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    o.draw(spriteBatch, x, y, obj);
                }
            }
            //동적 오브젝트 그리기
            for (int i = 0; i < mDynamicObjectNumber; i++) {
                mDynamicObjects[i].draw(spriteBatch, obj);
            }
            //화염 오브젝트 그리기
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    o.drawExplosion(spriteBatch, x, y, obj);
                }
            }
            spriteBatch.DrawString(font, "Bomberman!" , new Vector2(650, 10), Color.Yellow);
            if (mStageID == 1) {
                spriteBatch.DrawString(font, "Stage : " + (mCurrentStage + 1), new Vector2(650, 200), Color.Yellow);
                spriteBatch.DrawString(font, "Life : " + mLife, new Vector2(650, 300), Color.Yellow);
                spriteBatch.DrawString(font, "Move: arrow key", new Vector2(620, 350), Color.Yellow);
                spriteBatch.DrawString(font, "bomb: L", new Vector2(620, 400), Color.Yellow);
            }
            else {
                spriteBatch.DrawString(font, "2P Play! ", new Vector2(650, 100), Color.Yellow);

                spriteBatch.DrawString(font, "1P key", new Vector2(620, 150), Color.Yellow);
                spriteBatch.DrawString(font, "Move: arrow key", new Vector2(620, 200), Color.Yellow);
                spriteBatch.DrawString(font, "bomb: L", new Vector2(620, 250), Color.Yellow);

                spriteBatch.DrawString(font, "2P key", new Vector2(620, 300), Color.Yellow);
                spriteBatch.DrawString(font, "Move: a,w,s,d", new Vector2(620, 350), Color.Yellow);
                spriteBatch.DrawString(font, "bomb: g", new Vector2(620, 400), Color.Yellow);
            }

            if (hasCleared()) {
                spriteBatch.DrawString(font, "Clear!", new Vector2(650, 150), Color.YellowGreen);
            }
            if(mWhoWin != -2) {
                if(mWhoWin == 0) {
                    spriteBatch.DrawString(font, "Draw!", new Vector2(650, 50), Color.YellowGreen);

                }
                else if(mWhoWin == -1) {
                    spriteBatch.DrawString(font, "1P Win!", new Vector2(650, 50), Color.YellowGreen);

                }
                else if(mWhoWin == 1) {
                    spriteBatch.DrawString(font, "2P Win!", new Vector2(650, 50), Color.YellowGreen);

                }
            }
        }
        public int update(GameTime gameTime, KeyboardState ks) {
            for(int i=0; i<mDynamicObjectNumber;i++) {
                if(mDynamicObjects[i].getType == DynamicObject.Type.TYPE_PLAYER) mLife = mDynamicObjects[i].Life;
            }
            if ( mStageID==0 ) {
                if (!isAlive(0) && !isAlive(1)) {
                    mWhoWin = 0;
                }
                else if (isAlive(0) && !isAlive(1)) {
                    mWhoWin = -1;
                }
                else if (!isAlive(0) && isAlive(1)) {
                    mWhoWin = 1;
                }
            }
            if(mWhoWin != -2) {
                mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
                if(mTimeSinceLastInput >= 2.0) {
                    return -1;
                }
                return 0;
            }
            else {
                if(!isAlive(0)) return -1;

            }

             if (mStageID==1 && hasCleared()) {
                mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
                if (mTimeSinceLastInput >= 1.0) {
                    if (mCurrentStage >= MAX_STAGE - 1) {
                        return 1;
                    }
                    reinitialize(mStageID, ++mCurrentStage);
                }
                return 0;
            }
            //폭탄 처리
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    if (o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                       o.Count++;//폭발 대기시간 증가
                        //폭발 시작, 종료 판정
                        if (o.checkFlag(StaticObject.Flag.FLAG_EXPLODING)) { 
                            //폭발 시간이 다 되었으면
                            if (o.Count == EXPLOSION_LIFE) {
                                //폭발관련 플래그를 다 지워준다.
                                o.resetFlag(StaticObject.Flag.FLAG_EXPLODING | StaticObject.Flag.FLAG_BOMB);
                                o.Count = 0;
                            }
                        } 
                        //폭발 판정
                        else { 
                            //폭발 시간에 달하면 폭발 플래그를 세워주고 count를 초기화
                            if (o.Count == EXPLOSION_TIME) {
                                o.setFlag(StaticObject.Flag.FLAG_EXPLODING);
                                o.Count = 0;
                            } 
                            //연쇄 폭발 시작
                            else if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) { 
                                o.setFlag(StaticObject.Flag.FLAG_EXPLODING);
                                o.Count = 0;
                            }
                        }
                    }// if (o.checkFlag(StaticObject.Flag.FLAG_BOMB))
                    //벽돌 처리
                    else if (o.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        //만일 벽돌이 폭발에 휘말렸으면
                        if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) {
                            o.Count++;
                            //폭발 시간이 다되었으면
                            if (o.Count == EXPLOSION_LIFE) {
                                o.Count = 0;
                                //벽돌을 리셋하고 바닥으로 초기화
                                o.resetFlag(StaticObject.Flag.FLAG_BRICK);
                            }
                        }
                    }//else if (o.checkFlag(StaticObject.Flag.FLAG_BRICK)) 
                    
                    //매 프레임마다 폭발처리를 하므로 다른 처리에 방해 안되게 일단 제거
                    o.resetFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y);
                }//for (int x = 0; x < WIDTH; x++)
            }// for (int y = 0; y < HEIGHT; y++) 


            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    //폭발 플래그가 세워져있으면
                    if (o.checkFlag(StaticObject.Flag.FLAG_EXPLODING)) {
                        //불꽃 설치
                        setFire(x, y);
                    }
                }
            }
            
     
            //폭탄 갯수 세기
            int[] bombNumber= { 0, 0 };
            for (int y = 0; y < HEIGHT; y++) {
                for (int x = 0; x < WIDTH; x++) {
                    StaticObject o = mStaticObjects[y, x];
                    if (o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                        bombNumber[o.BombOwner.PlayerID]++;
                    }
                }
            }

            //동적 오브젝트 처리
            for (int i = 0; i < mDynamicObjectNumber; i++) {
                //동적 오브젝트가 죽었으면 그냥 무시한다.
                if (mDynamicObjects[i].isDead()) { 
                    continue;
                }


                //동적 오브젝트가 놓은 폭탄과 교차하고 있는지 검사
                /*
                 * 폭탄을 설치하는 순간엔 PLAYER 오브젝트와 폭탄은 겹칠 수 밖에 없다.
                 * 그 순간 충돌처리 함수에 의해 오브젝트는 움직일 수 없게 되는데
                 * 이 문제를 해결하기 위해 매 프레임마다 LastBombX(Y) 값을 이용해
                 * 겹칠 시 -1 값을 주어 아래 동적오브젝트 처리에서
                 * 오브젝트가 폭탄을 벗어날 때까지 충돌 처리를 무시하게 한다.
                 * LastBomb~이 길이 2의 배열인 이유는 최대로 연속될 수 있는 폭탄의
                 * 갯수가 2 개이기 때문이다.
                 */
                for (int j = 0; j < 2; j++) {
                    if (mDynamicObjects[i].LastBombX[j] >= 0) { 
                        if (!mDynamicObjects[i].isIntersectWall(mDynamicObjects[i].LastBombX[j], mDynamicObjects[i].LastBombY[j])) {
                            mDynamicObjects[i].LastBombX[j] = mDynamicObjects[i].LastBombY[j] = -1;
                        }
                    }
                }

                //현재 셀좌표 계산 
                int x, y;
                mDynamicObjects[i].getCell(out x, out y);

                //동적 오브젝트(o) 중심으로 벽이 있는지 계산
                /*
                 *  ***
                 *  *o*
                 *  ***
                 */
                int[] wallsX=new int[9];
                int[] wallsY=new int[9];
                int wallNumber = 0;


                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        int tx = x + j - 1;
                        int ty = y + k - 1;

                        StaticObject o = mStaticObjects[ty, tx];
                        //주변에 벽, 벽돌,폭탄이 있으면
                        if (o.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                            //오브젝트가 폭탄 설치 위치도 벗어났으면
                            bool myBomb0 = (mDynamicObjects[i].LastBombX[0] == tx) && (mDynamicObjects[i].LastBombY[0] == ty);
                            bool myBomb1 = (mDynamicObjects[i].LastBombX[1] == tx) && (mDynamicObjects[i].LastBombY[1] == ty);
                            if (!myBomb0 && !myBomb1) { 
                                //해당 부분을 벽 배열에 넣는다.
                                wallsX[wallNumber] = x + j - 1;
                                wallsY[wallNumber] = y + k - 1;
                                ++wallNumber;
                            }
                        }
                    }
                }
                //벽 배열 정보를 동적 오브젝트에게 넘겨주고 update(이동처리)를 진행한다.
                mDynamicObjects[i].update(wallsX, wallsY, wallNumber, ks);

                //이동 후 주위 9개의 오브젝트와 충돌판정 진행
                for (int j = 0; j < 3; j++) {
                    for (int k = 0; k < 3; k++) {
                        int tx= x + j - 1;
                        int ty =y +k - 1;
                        StaticObject o = mStaticObjects[ty, tx];
                        //걸리는 게 있으면
                        if (mDynamicObjects[i].isIntersectWall(tx,ty)) {
                            //그게 불이면 죽고
                            if (o.checkFlag(StaticObject.Flag.FLAG_FIRE_X | StaticObject.Flag.FLAG_FIRE_Y)) {
                                mDynamicObjects[i].die(gameTime);
                                
                            }
                            //아이템이 있으면 그에 맞는 처리를 한다.
                            else if (!o.checkFlag(StaticObject.Flag.FLAG_BRICK)) {
                                if (o.checkFlag(StaticObject.Flag.FLAG_ITEM_POWER)) {
                                    o.resetFlag(StaticObject.Flag.FLAG_ITEM_POWER);
                                    mDynamicObjects[i].BombPower++;
                                } else if (o.checkFlag(StaticObject.Flag.FLAG_ITEM_BOMB)) {
                                    o.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB);
                                    mDynamicObjects[i].BombNumber++;
                                }
                            }
                            //벽이나 벽돌이면 아무 처리도 하지 않는다. 
                            else {; }
                        }
                    }
                }

                //이동 후 셀좌표를 취득한다.
                mDynamicObjects[i].getCell(out x, out y);

                //폭탄 설치 버튼이 눌려졌다면
                if (mDynamicObjects[i].hasBombButtonPressed(ks)) {
                    //최대 폭탄 수 미만이라면
                    if (bombNumber[mDynamicObjects[i].PlayerID] < mDynamicObjects[i].BombNumber) {
                      
                        StaticObject o = mStaticObjects[y, x];
                        //해당 위치에 폭탄이 안놓여있다면 
                        if (!o.checkFlag(StaticObject.Flag.FLAG_BOMB)) {
                            o.setFlag(StaticObject.Flag.FLAG_BOMB);//설치한다.
                            o.BombOwner = mDynamicObjects[i];//폭탄의 주인도 설정한다.
                            o.Count = 0; // 카운트도 초기화하고
                            
                            //폭탄의 위치를 갱신한다.
                            if (mDynamicObjects[i].LastBombX[0] < 0) {
                                mDynamicObjects[i].LastBombX[0] = x;
                                mDynamicObjects[i].LastBombY[0] = y;
                            } else {
                                mDynamicObjects[i].LastBombX[1] = x;
                                mDynamicObjects[i].LastBombY[1] = y;
                            }
                        }
                    }//f (bombNumber[mDynamicObjects[i].PlayerID] < mDynamicObjects[i].BombNumber) 
                }//if (mDynamicObjects[i].hasBombButtonPressed(ks))
            } //for (int i = 0; i < mDynamicObjectNumber; i++) 

            //적과 충돌판정을 한다.
            for (int i = 0; i < mDynamicObjectNumber; i++) {
                for (int j = i + 1; j < mDynamicObjectNumber; j++) {
                    mDynamicObjects[i].doCollisionReactionToDynamic(mDynamicObjects[j],gameTime);
                }
            }
            return 0;
        }

        //폭발처리 함수
        void setFire(int x, int y) {
            StaticObject o = mStaticObjects[y, x];
            int power = o.BombOwner.BombPower;
            int end;
            
            /*
             * 상하좌우 처리가 비슷하므로 좌측만 설명한다.
             */
            //좌 
            end = (x - power < 0) ? 0 : (x - power);
            for (int i = x - 1; i >= end; i--) {
                StaticObject to = mStaticObjects[y, i];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_X);
                //폭발 선상에 무언가 있으면 폭발을 멈춘다.
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    //아이템이 폭발에 휘말리면 제거
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            //우
            end = (x + power >= WIDTH) ? (WIDTH - 1) : (x + power);
            for (int i = x + 1; i <= end; i++) {
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
            for (int i = y - 1; i >= end; i--) {
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
            for (int i = y + 1; i <= end; i++) {
                StaticObject to = mStaticObjects[i, x];
                to.setFlag(StaticObject.Flag.FLAG_FIRE_Y);
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK | StaticObject.Flag.FLAG_BOMB)) { 
                    break;
                } else {
                    to.resetFlag(StaticObject.Flag.FLAG_ITEM_BOMB | StaticObject.Flag.FLAG_ITEM_POWER);
                }
            }
            

            //연쇄 폭발 처리
            /*
             * ㅇ = 폭탄, ㅁ= 벽
             * 가령 ㅇㅇㅁㅁ 같은 환경이 있다고 가정하자.
             *  ㅇ'ㅇㅁ'ㅁ  ''안 에 폭탄이 해당 프레임에 폭발하면 해당 처리에서 ''안 벽돌이 사라져서
             *  다음 순간 ㅇㅁ가 되었을 때 ㅁ도 사라져버린다.
             *  이는 바람직한 처리가 아니므로 폭발에 휘말린 벽돌은 유폭중 가장 마지막 폭탄에 맞추어 사라져야한다.
             */

            //좌
            end = (x - power < 0) ? 0 : (x - power);
            for (int i = x - 1; i >= end; i--) {
                StaticObject to = mStaticObjects[y, i];
                //폭탄은 어차피 연쇄적으로 사라지니 무시한다.
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) { 
                    //벽돌이면 카운트 시작, 가장 느리게 터지는 폭탄에 카운트를 맞춘다.
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) {
                        to.Count = 0;
                    }
                    break;
                }
            }
            //우
            end = (x + power >= WIDTH) ? (WIDTH - 1) : (x + power);
            for (int i = x + 1; i <= end; i++) {
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
            for (int i = y - 1; i >= end; i--) {
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
            for (int i = y + 1; i <= end; i++) {
                StaticObject to = mStaticObjects[i, x];
                if (to.checkFlag(StaticObject.Flag.FLAG_WALL | StaticObject.Flag.FLAG_BRICK)) {
                    if ((o.Count == 0) && to.checkFlag(StaticObject.Flag.FLAG_BRICK)) { 
                        to.Count = 0;
                    }
                    break;
                }
            }
        }

        //클리어 처리
        bool hasCleared() {
	        for ( int i = 0; i<mDynamicObjectNumber; i++ ){
		        if ( mDynamicObjects[i].isEnemy() ){
			        return false;
		        }
	        }
	        return true;
        }

        //생존여부 확인
        bool isAlive(int playerID)  {
	        for ( int i = 0; i<mDynamicObjectNumber; i++ ){
		        if ( mDynamicObjects[i].getType == DynamicObject.Type.TYPE_PLAYER ){
                    if (mDynamicObjects[i].PlayerID == playerID) {
                        return true;
                    }
                }
	        }
	        return false;
        }

    }
}
