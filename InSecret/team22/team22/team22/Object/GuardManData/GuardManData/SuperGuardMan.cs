using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Utility;
using team22.Device;
using team22.Object;
using team22.Def;

namespace team22.Object
{
    class SuperGuardMan : GuardMan
    {
        private Vector2 leftPosition;//左端
        private Vector2 rightPosition;//右端
        private Vector2 velocity;//移動量

        private Vector2 direction;//向き用

        private float speed;//移動速度

        private bool discover;//プレイヤー発見フラッグ

        private Timer timer;//立ち止まる処理用
        private Timer escape;//逃げ伸びるまでの時間
        private bool stop;//立ち止まる処理用フラッグ
        private bool eventFlag;
        private bool moveFlag;
        public bool drawFlag;
        private int num;
        private Sound sound;

        private int nowFloor;

        private InputState input;

        //モーション用
        private bool changeMotion;//左右の歩きモーション判別用flag
        private bool discoverFlag;//追跡時の歩きモーション移行用flag

        private Conditions.CharacterMovements charMovement;

        private Motion motion;

        private CountDownTimer eventTimer;

        public SuperGuardMan(Vector2 leftPosition, Vector2 rightPosition, int floorWidth, int floorHeight, GameDevice gameDevice, Light light)
            : base(leftPosition, rightPosition, floorWidth, floorHeight, gameDevice, light)
        {
            name = "Super_Guardman";
            //左端から移動開始
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;
            velocity = new Vector2(1.0f, 0.0f);//最初は右移動
            guardDirection = false;

            this.light = light;

            input = gameDevice.InputState;
            sound = gameDevice.Sound;

            speed = 5f;

            nowFloor = 3;

            timer = new Timer(2.0f);//立ち止まる処理用
            escape = new Timer(5f);//逃げ延びるまでの時間

            //フラッグは最初false
            stop = false;
            discover = false;
            changeMotion = false;
            discoverFlag = false;
            eventFlag = false;
            moveFlag = false;
            drawFlag = false;

            // モション生成
            motion = new Motion();
            int counter = 0;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    motion.Add(counter, new Rectangle(54 * x, 80 * y, 54, 80));
                    counter++;
                }
            }
            motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
            charMovement = Conditions.CharacterMovements.Walk;

            eventTimer = new CountDownTimer(1.5f);
        }

        public SuperGuardMan(SuperGuardMan other) : this(other.leftPosition, other.rightPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice, other.light)
        {

        }

        public override object Clone()
        {
            return new SuperGuardMan(this);
        }

        public override void Hit(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);//どの向きで当たっているか

            if (gameObject is Player && PlayerInfo.IsVisible)
            {
                EnemyManager.playerHit = true;
            }

            #region ステージとのあたり判定
            if (gameObject is Block)
            {
                if (dir == Direction.Top)//上
                {
                    if (worldPosition.Y > 0.0f)
                    {
                        worldPosition.Y = gameObject.GetRectangle().Top - this.height;
                        velocity.Y = 0.0f;
                    }
                }

                else if (dir == Direction.Right)//右
                {
                    worldPosition.X = gameObject.GetRectangle().Right;
                    velocity.X = -velocity.X;
                }

                else if (dir == Direction.Left)//左
                {
                    worldPosition.X = gameObject.GetRectangle().Left - this.width;
                    velocity.X = -velocity.X;
                }

                else if (dir == Direction.Botton)//下
                {
                    worldPosition.Y = gameObject.GetRectangle().Bottom;
                }
            }
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            if (PlayerInfo.IsMove || !PlayerInfo.IsVisible)
            {
                #region 警備員の動き
                //折り返し処理
                if (worldPosition.X > rightPosition.X && !discover && !stop && !eventFlag && !moveFlag)
                {
                    worldPosition.X = rightPosition.X;
                    timer.Initialize();
                    stop = true;
                    velocity.X = 0;
                }
                if (worldPosition.X < leftPosition.X && !discover && !stop && !eventFlag && !moveFlag)
                {
                    worldPosition.X = leftPosition.X;
                    timer.Initialize();
                    stop = true;
                    velocity.X = 0;
                }


                //立ち止まっていればTimer起動
                if (stop)
                {
                    timer.Update();
                    Wait();
                }
                //それ以外ならば進む
                else if (!stop && !discover && !eventFlag && !moveFlag)
                {
                    worldPosition += velocity * speed;//移動
                }

                //Hitしたら追跡
                if (discover && !eventFlag && !moveFlag)
                {
                    worldPosition.X += velocity.X * speed;
                    escape.Update();


                    //Lightに渡す用の向き判定
                    if (velocity.X > 0)
                    {
                        velocity.X = 1f;
                        guardDirection = false;
                    }
                    if (velocity.X < 0)
                    {
                        velocity.X = -1f;
                        guardDirection = true;
                    }

                    //時間分逃げ切ったら元の場所に戻る
                    if (escape.IsTime()||PlayerInfo.InElevator)
                    {
                        direction = leftPosition - worldPosition;
                        direction.Normalize(); //方向
                        velocity = direction;
                        worldPosition.X += velocity.X * speed;

                        if (direction.X > 0)
                        {
                            guardDirection = false;
                        }
                        else if (direction.X < 0)
                        {
                            guardDirection = true;
                        }

                        //元の場所に戻ったら追跡状態解除
                        float length = Vector2.Distance(leftPosition, worldPosition);
                        if (length < speed && !stop)
                        {
                            velocity.X = 0;
                            guardDirection = true;
                            timer.Initialize();
                            stop = true;
                            worldPosition.X = leftPosition.X;
                            discover = false;
                            changeMotion = true;
                            escape.Initialize();
                        }

                    }
                    if (float.IsNaN(worldPosition.X))
                    {
                        worldPosition.X = leftPosition.X;
                        timer.Initialize();
                        stop = true;
                        Wait();
                        discover = false;
                    }
                }


                #endregion

                if (!eventFlag)
                {
                    Discover();
                }


                motion.Update(gameTime);
                UpdateMotion(velocity);

                //Bキーを押すとイベント開始
                if (GimmickInfo.eventFlag)
                {
                    eventFlag = true;
                }

                if (GimmickInfo.EventFlag)
                {
                    LetsGo(5);
                }
            }

            //画面端にいくと反転
            if ((worldPosition.X <= Screen.widthMax || worldPosition.X >= 32 * 54) && !eventFlag)
            {
                velocity.X = -velocity.X;
            }

            //Console.WriteLine("POS:" + worldPosition);
            //Console.WriteLine("VEL:" + velocity.X);
            ////Console.WriteLine("NOWFLOOR:" + nowFloor);
            //Console.WriteLine("EVENTFLAG" + eventFlag);
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        private void Wait()
        {
            //時間になったら逆を向き、歩き始める
            if (timer.IsTime())
            {
                if (worldPosition == leftPosition)
                {
                    guardDirection = false;
                    velocity.X = 1;
                }
                else if (worldPosition == rightPosition)
                {
                    guardDirection = true;
                    velocity.X = -1;
                }

                stop = false;
            }
        }

        private void LetsGo5F()
        {
            if (worldPosition.Y != leftPosition.Y)
            {
                worldPosition.X += velocity.X * speed;
                drawingPosition.X += velocity.X * speed;
            }
            if (worldPosition.X >= 32 * 37)
            {
                velocity.X = -1f;
                worldPosition.Y -= 32 * 11;
                drawingPosition.Y -= 32 * 11;
                worldPosition.X -= 32;
                drawingPosition.X -= 32;
                guardDirection = !guardDirection;
            }
            if (worldPosition.X <= 32 * 2)
            {
                velocity.X = 1f;
                worldPosition.Y -= 32 * 11;
                drawingPosition.Y -= 32 * 11;
                guardDirection = !guardDirection;
            }
            if (worldPosition.Y == leftPosition.Y)
            {
                direction = leftPosition - worldPosition;
                direction.Normalize(); //方向
                velocity = direction;
                worldPosition.X += velocity.X * speed;
                drawingPosition.X += velocity.X * speed;
                //元の場所に戻ったらイベント終了
                float length = Vector2.Distance(leftPosition, worldPosition);
                if (length < speed)
                {
                    eventFlag = false;
                    speed = 3f;
                }
            }
        }

        public void LetsGo(int floor)
        {
            if (PlayerInfo.IsMove)
            {
                if (!moveFlag)
                {
                    num = nowFloor - floor;
                    leftPosition.Y += 32 * (11 * num);
                    rightPosition.Y += 32 * (11 * num);
                    stop = false;
                    speed = 10f;

                    #region Switch
                    switch (nowFloor)
                    {
                        case 1:
                            motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                            charMovement = Conditions.CharacterMovements.Walk;
                            changeMotion = false;
                            discoverFlag = false;
                            guardDirection = false;
                            velocity.X = 1;
                            break;
                        case 2:
                            if (num > 0)
                            {
                                motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = false;
                                discoverFlag = false;
                                guardDirection = false;
                                velocity.X = 1;
                            }
                            else if (num < 0)
                            {
                                motion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = true;
                                discoverFlag = false;
                                guardDirection = true;
                                velocity.X = -1;
                            }
                            break;
                        case 3:
                            if (num > 0)
                            {
                                motion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = true;
                                discoverFlag = false;
                                guardDirection = true;
                                velocity.X = -1;
                            }
                            else if (num < 0)
                            {
                                motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = false;
                                discoverFlag = false;
                                guardDirection = false;
                                velocity.X = +1;
                            }
                            break;
                        case 4:
                            if (num > 0)
                            {
                                motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = false;
                                discoverFlag = false;
                                guardDirection = false;
                                velocity.X = +1;
                            }
                            else if (num < 0)
                            {
                                motion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                                charMovement = Conditions.CharacterMovements.Walk;
                                changeMotion = true;
                                discoverFlag = false;
                                guardDirection = true;
                                velocity.X = -1;
                            }
                            break;
                        case 5:
                            motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                            charMovement = Conditions.CharacterMovements.Walk;
                            changeMotion = true;
                            discoverFlag = false;
                            guardDirection = true;
                            velocity.X = -1;
                            break;
                    }
                    moveFlag = true;
                    #endregion
                }

                worldPosition.X += velocity.X * speed;
                drawingPosition.X += velocity.X * speed;

                if (worldPosition.X <= 32 * 2 || worldPosition.X >= 32 * 44)
                {
                    if (num > 0)
                    {
                        worldPosition.Y += 32 * 11;
                        velocity = -velocity;
                        guardDirection = !guardDirection;
                        num -= 1;
                        nowFloor -= 1;
                    }
                    else if (num < 0)
                    {
                        worldPosition.Y -= 32 * 11;
                        velocity = -velocity;
                        guardDirection = !guardDirection;
                        num += 1;
                        nowFloor += 1;
                    }
                }
                // ５階に到着
                if (num == 0)
                {
                    direction = leftPosition - worldPosition;
                    if (direction.X > 0) velocity.X = 1;
                    else velocity.X = -1;

                    worldPosition.X += velocity.X * speed;
                    eventTimer.Update();

                    if (direction.X > 0)
                    {
                        guardDirection = false;
                    }
                    else if (direction.X < 0)
                    {
                        guardDirection = true;
                    }

                    //元の場所に戻ったら追跡状態解除
                    float length = Vector2.Distance(leftPosition, worldPosition);
                    if (length <= speed)
                    {
                        worldPosition = leftPosition;
                        guardDirection = true;
                        timer.Initialize();
                        discover = false;
                        moveFlag = false;
                        changeMotion = true;
                        
                        speed = 5f;
                    }
                    if (eventTimer.IsTime())
                    {
                        GimmickInfo.eventFlag = false;
                        eventFlag = false;
                    }
                }
                //Console.WriteLine(num);
            }
        }





        /// <summary>
        /// 追跡処理(Lightに当たった時用)
        /// </summary>
        private void Discover()
        {
            //自分のライトに当たった時の処理
            if (light.LightFlag() && PlayerInfo.IsVisible)
            {
                //Playerの方向を取得
                direction = light.PlayerPos() - worldPosition;
                direction.Normalize();
                velocity = direction;
                discoverFlag = true;

                stop = false;

                if (!discover)
                {
                    sound.PlaySE("warning");
                }

                discover = true;
                escape.Initialize();
                light.FalseFlag();
            }
            //監視カメラに当たった時の処理
            else if (EnemyManager.CCTVFlag && PlayerInfo.IsVisible)
            {
                //Playerの方向を取得
                direction = CCTVPoint.playerPos - worldPosition;
                direction.Normalize();
                velocity = direction;
                discoverFlag = true;

                stop = false;

                if (!discover)
                {
                    sound.PlaySE("warning");
                }

                discover = true;
                escape.Initialize();
            }
            //レーザーに当たった時の処理
            else if (EnemyManager.LaserFlag && PlayerInfo.IsVisible)
            {
                //Playerの方向を取得
                direction = Laser.playerPos - worldPosition;
                direction.Normalize();
                velocity = direction;
                discoverFlag = true;

                stop = false;

                if (!discover)
                {
                    sound.PlaySE("warning");
                }

                discover = true;
                escape.Initialize();
            }
        }

        public override void Draw(Renderer renderer)
        {
            if (FloorNumInfo.nannkai + 1 == nowFloor || eventFlag)
            {
                renderer.DrawTexture(name, worldPosition, motion.DrawingRange());
                if (discover)
                {
                    if (escape.IsTime())
                    {
                        renderer.DrawTexture("questionMark", worldPosition + new Vector2(10, -32));
                    }
                    else
                    {
                        renderer.DrawTexture("exclamationMark", worldPosition + new Vector2(10, -32));
                    }
                }
                drawFlag = true;
                return;
            }

            drawFlag = false;

        }

        private void UpdateMotion(Vector2 velocity)
        {
            if (velocity.X == 0 && !guardDirection && charMovement != Conditions.CharacterMovements.Idle && !discoverFlag)
            {
                motion.Initialize(new Range(12, 16), new CountDownTimer(0.1f));
                charMovement = Conditions.CharacterMovements.Idle;
            }
            else if (velocity.X == 0 && guardDirection && charMovement != Conditions.CharacterMovements.Idle && !discoverFlag)
            {
                motion.Initialize(new Range(0, 4), new CountDownTimer(0.1f));
                charMovement = Conditions.CharacterMovements.Idle;
            }
            else if ((velocity.X >= 0 && !guardDirection && changeMotion) || (discoverFlag && velocity.X >= 0 && guardDirection)
                || (charMovement == Conditions.CharacterMovements.Idle && discoverFlag && guardDirection))
            {
                motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                charMovement = Conditions.CharacterMovements.Walk;
                changeMotion = false;
                discoverFlag = false;
            }
            else if ((velocity.X <= 0 && guardDirection && !changeMotion) || (discoverFlag && velocity.X <= 0 && !guardDirection)
                || (charMovement == Conditions.CharacterMovements.Idle && discoverFlag && !guardDirection))
            {
                motion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                charMovement = Conditions.CharacterMovements.Walk;
                changeMotion = true;
                discoverFlag = false;
            }
        }
    }
}
