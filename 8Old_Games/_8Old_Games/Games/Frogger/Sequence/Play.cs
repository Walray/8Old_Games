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
using _8Old_Games.Games.Frogger.Object;


namespace _8Old_Games.Games.Frogger.Sequence {
    public class Play : Sequence {

        private const int LEN_ROAD = 5; //도로의 행 수
        private const int LEN_TURTLE = 2; // 거북이가 있는 행의 수
        private const int LEN_WATER = 5; // 물의 행 수
        private const int MIN_CAR = 2; //최소 출현 차 갯수

        private const int SRC_HEIGHT_CAR = 0; //리소스내 차 리소스 행 위치(Object.png) 참고
        private const int SRC_HEIGHT_TURTLE = 1; //리소스내 거북이 리소스 행 위치
        private const int SRC_HEIGHT_LOG = 2;//리소스내 통나무 리소스 행 위치

        //각 오브젝트 별 점수 상수
        private const int SCORE_ROAD = 30;
        private const int SCORE_WATER = 50;
        private const int SCORE_GOAL = 200;
        private const int SCORE_PLUS = 7000;
        private readonly int[] SCORE_INTERVAL = { 0, 20, 40, 60 }; // 구간 별 가산 점수

        //최고 스테이지
        private const int MAX_STAGE = 23;
        //입력 타이밍 컨트롤 및 지연 처리 컨트롤 용 변수
        private const double WAIT_TIME = 2.0;
        private const double WAIT_MENU = 0.15;
        //구간별 속도 변화량
        private const int deltaSpeed = 2;

        //구간별 흭득 점수
        private int deltaScore = 0;
        //보너스 라이프 계산용 변수
        private int deltaLife = 0;
        //타이밍 컨트롤 용
        private double timeCount = 0.0;
        private double mWaitNext = 0.0;
        private double mTimeAfterMenu = WAIT_MENU;

        //점수
        private int score = 0;
        //라이프
        private int life = 5;
        //현재 스테이지
        private int stage = 1;


        //오브젝트 선언
        private Random rand;
        private Map map;
        private Frog frog;
        private List<Car>[] cars;
        private List<Turtle> turtle1;
        private List<Turtle> turtle2;
        private List<Log> log1;
        private List<Log> log2;
        private List<Log> log3;
        private LastCroc lastCroc;

        public Play() : base() {; }

        //재초기화용 함수
        private void reInitialize() {
            int det = 0;
            int num = 0;
            double speed = 0.0;
            int len = 0;
            int idxX = 0;
            int temp;

            Car cTemp;
            Turtle tTemp;
            Log lTemp;

            Rectangle src;

            map = new Map();
            frog = new Frog();
            frog.initialize();
            map.initialize();
            cars = new List<Car>[LEN_ROAD];
            turtle1 = new List<Turtle>();
            turtle2 = new List<Turtle>();
            log1 = new List<Log>();
            log2 = new List<Log>();
            log3 = new List<Log>();
            lastCroc = new LastCroc();


            //lastCroc
            if (stage >= 16) {//16 스테이지부터 마지막 악어 등장
                do {
                    temp = rand.Next()%13;
                } while (temp%3!=0); //마지막 악어 위치 선정
                lastCroc.initialize(temp);
            }

            //cars
            for (int y = 0; y < LEN_ROAD; y++) {
                cars[y] = new List<Car>();

                /* 한 칸은 32*32 pixel^2
                 * 구간 1 : 32 pixel/sec ~ 40 pixel/sec
                 * 구간 2 : 40 pixel/sec ~ 53 pixel/sec
                 * 구간 3 : 53 pixel/sec ~ 80 pixel/sec
                 * 이하 오브젝트 전부 이 식 적용
                 */
                speed = (double)rand.Next(6 -(stage/8)*deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0;

                /* 구간별 차량 가산
                 * 구간 1 : 2+0
                 * 구간 2 : 2+0~1
                 * 구간 3 : 2+0~2
                 */
                num = MIN_CAR + rand.Next() %((stage+8)/8);
                for (int x = 0; x < num; x++) {
                    det = (y + 1) % 2; //홀수 줄은 오른쪽 방향, 짝수 줄은 왼쪽 방향
                    if (y == 4) len = 2; //마지막 줄의 차는 길이 2
                    else len = 1;
                    cTemp = new Car();

                    cTemp.initialize(
                        det == 1 ? true : false,
                        det == 1 ? x * 3 - y : Map.COL - 1 - x * 3 - y,
                        Map.ROW - 1 - 1 - y,
                        speed,
                        len,
                        new Rectangle(y * Map.WIDTH, SRC_HEIGHT_CAR * Map.HEIGHT, Map.WIDTH * len, Map.HEIGHT)
                    );
                    cars[y].Add(cTemp);
                }
            }
            
            //trutle1
            idxX = 0;
            speed = (double)rand.Next(6 - (stage / 8) * deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0; 
            for (int x = 0; x < 9; x++) {
                tTemp = new Turtle();
                //세 마리씩 짝 지음(간격 3)
                if (x != 0 && x % 3 == 0) idxX += 3;
                
                tTemp.initialize(
                    false,
                    idxX,
                    Map.ROW - 1 - LEN_ROAD - 2,
                    speed,
                    3,
                    new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_TURTLE * Map.HEIGHT, Map.WIDTH, Map.HEIGHT),
                    (stage>=10 && x<3) ? true : false //10 스테이지부터 잠기는 거북이 등장
                );
                turtle1.Add(tTemp);
                idxX++;
            }

            //turtle2
            idxX = 0;
            speed = (double)rand.Next(6 - (stage / 8) * deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0; 
            for (int x = 0; x < 6; x++) {
                tTemp = new Turtle();
                //두 마리씩 짝 지음(간격 3)
                if (x != 0 && x % 2 == 0) idxX += 3;
                tTemp.initialize(
                    false,
                    idxX,
                    Map.ROW - 1 - LEN_ROAD - 5,
                    speed,
                    2,
                    new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_TURTLE * Map.HEIGHT, Map.WIDTH, Map.HEIGHT),
                    (stage >= 15 && x < 2) ? true : false // 15 스테이지 부터 두 번째 잠기는 거북이 등장

                );
                turtle2.Add(tTemp);
                idxX++;
            }

            //log1
            idxX = 0;
            speed = (double)rand.Next(6 - (stage / 8) * deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0;
            for (int x = 0; x < 6; x++) {
                lTemp = new Log();
                //리소스 위치 계산(object.png 참고)
                if (x % 3 == 0) src = new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                else if ((x + 1) % 3 == 0) src = new Rectangle(2 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                else src = new Rectangle(1 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);

                if (x != 0 && x % 3 == 0) idxX += 3;
                lTemp.initialize(
                    true,
                    idxX,
                    Map.ROW - 1 - LEN_ROAD - 3,
                    speed,
                    3,
                    src,
                    true
                );
                log1.Add(lTemp);
                idxX++;
            }

            //log2
            idxX = 0;
            speed = (double)rand.Next(6 - (stage / 8) * deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0; 
            for (int x = 0; x < 6; x++) {
                lTemp = new Log();
                if (x == 0) src = new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                else if (x == 5) src = new Rectangle(2 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                else src = new Rectangle(1 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                lTemp.initialize(
                    true,
                    idxX,
                    Map.ROW - 1 - LEN_ROAD - 4,
                    speed,
                    6,
                    src,
                    true
                );
                log2.Add(lTemp);
                idxX++;
            }

            //log3
            
            idxX = 0;
            speed = (double)rand.Next(6 - (stage / 8) * deltaSpeed, 9 - (stage / 8) * deltaSpeed) / 10.0;
            for (int x = 0; x < 8; x++) {
                lTemp = new Log();
                if (x < 4) {
                    if (x % 4 == 0) src = new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                    else if ((x + 1) % 4 == 0) src = new Rectangle(2 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                    else src = new Rectangle(1 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);

                    if (x != 0 && x % 4 == 0) idxX += 4;
                    lTemp.initialize(
                        true,
                        idxX,
                        Map.ROW - 1 - LEN_ROAD - 6,
                        speed,
                        4,
                        src,
                        true
                    );
                } 
                else {
                    if (stage >= 5) {//스테이지 5 부터 악어 등장.
                        if (x == 7) break; // 악어의 길이는 3
                        //리소스 파일 내 악어 리소스 위치 계산 
                        if (x % 4 == 0) src = new Rectangle(3 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                        else if (x == 6) src = new Rectangle(5 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                        else src = new Rectangle(4 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);

                        if (x != 0 && x % 4 == 0) idxX += 4;
                        lTemp.initialize(
                            true,
                            idxX,
                            Map.ROW - 1 - LEN_ROAD - 6,
                            speed,
                            4,
                            src,
                            false,
                            (x == 6) ? true : false
                        );
                    } 
                    else {
                        if (x % 4 == 0) src = new Rectangle(0 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                        else if ((x + 1) % 4 == 0) src = new Rectangle(2 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);
                        else src = new Rectangle(1 * Map.WIDTH, SRC_HEIGHT_LOG * Map.HEIGHT, Map.WIDTH, Map.HEIGHT);

                        if (x != 0 && x % 4 == 0) idxX += 4;
                        lTemp.initialize(
                            true,
                            idxX,
                            Map.ROW - 1 - LEN_ROAD - 6,
                            speed,
                            4,
                            src,
                            true
                        );
                    }
                }
                log3.Add(lTemp);
                idxX++;
            }//for (int x = 0; x < 4 + 3; x++)

        }

        public override void initialize() {; }
        public void initialize(int stage) {
            rand = new Random();
            this.stage = stage; // 지정 스테이지로
            mWaitNext = 0.0;
            reInitialize();
        }

        public override State update(GameTime gameTime, KeyboardState ks) { return State.NOTHING; }

        //게임 플레이의 논리 부분
        public State update(GameTime gameTime, KeyboardState ks, out int outStage) {
            outStage = stage;
            //스테이지 23을 넘어가면 클리어
            if (stage > 23) {
                while (true) {
                    mWaitNext += gameTime.ElapsedGameTime.TotalSeconds;
                    if (mWaitNext >= WAIT_TIME) {
                        mWaitNext = 0.0;
                        outStage = 1;
                        return State.CLEAR;

                    }
                }
             }

            //메뉴로 이동 처리(입력 타이밍 제어)
            mTimeSinceLastInput += gameTime.ElapsedGameTime.TotalSeconds;
            mTimeAfterMenu -= gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastInput >= MIN_TIME && mTimeAfterMenu<=0) {
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Space)) {
                    mTimeAfterMenu = WAIT_MENU;
                    return State.MENU;
                }
                mTimeSinceLastInput = 0.0;
            }

            //스테이지가 클리어 되었는지 검사 및 처리
            if (map.checkEnd()) {
                mWaitNext += gameTime.ElapsedGameTime.TotalSeconds;
                if (mWaitNext >= WAIT_TIME) {
                    stage++;
                    reInitialize();
                    mWaitNext = 0.0;
                }

            }

            //라이프가 0이 되었을 때 처리(FAIL시퀀스로 이동)
            if (life == 0) {
                while (true) { 
                    mWaitNext += gameTime.ElapsedGameTime.TotalSeconds;
                    if (mWaitNext >= WAIT_TIME) {
                        mWaitNext = 0.0;
                        return State.FAIL;
                    }
                }
            }
            
        
            //개구리 update
            if (!frog.IsDead) {
                frog.update(gameTime, Keyboard.GetState(), map, out deltaScore);
                score += deltaScore;
                //구간 점수 가산
                if (deltaScore != 0) score += SCORE_INTERVAL[stage / 8];
                
                // 마지막 악어 update
                if (lastCroc.update(gameTime, frog, map) && stage >= 16) {; } 

                else {
                    //개구리가 GOAL에 들어갔으면 초기화 하고 해당 점수 가산
                    if (map.checkClear(frog.X, frog.Y)) {
                        frog.initialize();
                        score += 200;
                        if (deltaScore != 0) score += SCORE_INTERVAL[stage / 8];
                    }
                }
                //보너스 라이프 계산, 7000점 당 1 라이프 플러스
                if ((score - deltaLife) / SCORE_PLUS != 0) {
                    deltaLife += SCORE_PLUS;
                    life++;
                }
            } 
            else {//개구리가 죽었으면
                timeCount += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeCount >= WAIT_TIME) {
                    frog.update(gameTime, Keyboard.GetState(), map, out deltaScore);
                    life--;
                    frog.initialize();
                    timeCount = 0.0;
                }
            }

            //차 update
            for (int y = 0; y < cars.Length; y++) {
                for (int x = 0; x < cars[y].Count; x++) {
                    cars[y][x].update(gameTime);
                    frog.checkCollision(cars[y][x]);
                }
            }

            //첫 번 째거북이 update
            for (int x = 0; x < turtle1.Count; x++) {
                turtle1[x].update(gameTime, map, frog);
            }
            
            //두 번 째거북이 update
            for (int x = 0; x < turtle2.Count; x++) {
                turtle2[x].update(gameTime, map, frog);
            }
            
            //첫 번 째 통나무 update
            for (int x = log1.Count - 1; x >= 0; x--) {
                log1[x].update(gameTime, map, frog);
            }
            
            //두 번 째 통나무 update
            for (int x = log2.Count - 1; x >= 0; x--) {
                log2[x].update(gameTime, map, frog);
            }
            //세 번 째 통나무 update
            for (int x = log3.Count - 1; x >= 0; x--) {
                log3[x].update(gameTime, map, frog);
            }
            return State.PLAY;
        }

        public override void draw(SpriteBatch spriteBatch, Texture2D sprite, Vector2 origin) {; }

        //전경(오브젝트들) 그리기 함수
        public void drawFront(SpriteBatch spriteBatch, SpriteFont font, Texture2D frogImage, Texture2D deadImage, Texture2D mapImages, Texture2D objectImages, Texture2D backgroundImage, Vector2 origin, GameTime gameTime) {
            for (int x = 0; x < turtle1.Count; x++) {
                turtle1[x].draw(spriteBatch, objectImages, false, origin, gameTime);
            }
            for (int x = 0; x < turtle2.Count; x++) {
                turtle2[x].draw(spriteBatch, objectImages, false, origin, gameTime);
            }
            for (int x = 0; x < log1.Count; x++) {
                log1[x].draw(spriteBatch, objectImages, origin, gameTime);
            }
            for (int x = 0; x < log2.Count; x++) {
                log2[x].draw(spriteBatch, objectImages, origin, gameTime);
            }
            for (int x = 0; x < log3.Count; x++) {
                log3[x].draw(spriteBatch, objectImages, origin, gameTime);
            }
            
            if (!frog.IsDead) frog.draw(frogImage, spriteBatch, origin);
            else {
                frog.draw(deadImage, spriteBatch, origin);
            }
            
            for (int y = 0; y < cars.Length; y++) {
                for (int x = 0; x < cars[y].Count; x++) {
                    cars[y][x].draw(spriteBatch, objectImages, origin, gameTime);
                }
            }

        }

        //그리기 함수
        public void draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D frogImage, Texture2D deadImage, Texture2D mapImages, Texture2D objectImages,  Texture2D backgroundImage, Texture2D lastCrocImage, Texture2D dummy, Vector2 origin, GameTime gameTime) {
            map.draw(mapImages, spriteBatch, origin);
            spriteBatch.DrawString(font, "Score : " + score + "   Stage : " + stage + "       Life : " + life, new Vector2(0, 0), Color.White);

            drawFront(spriteBatch, font, frogImage, deadImage, mapImages, objectImages, backgroundImage, origin, gameTime);
            if(stage>=16) lastCroc.draw(lastCrocImage, dummy, spriteBatch, origin);
            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, 800,480), Color.White);
        }

    }
}
