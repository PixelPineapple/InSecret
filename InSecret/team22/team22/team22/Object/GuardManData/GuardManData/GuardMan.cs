using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Object;
using team22.Def;
using team22.Utility;

namespace team22.Object
{
    class GuardMan : GameObject
    {
        private Vector2 leftPosition;//左端
        private Vector2 rightPosition;//右端
        private Vector2 velocity;//移動量

        public bool guardDirection;//Lightに渡す用Direction(falseが右向き、trueが左向き)

        private Vector2 direction;//向き用

        private float speed;//移動速度

        private bool discover;//プレイヤー発見フラッグ

        private Timer timer;//立ち止まる処理用
        private Timer escape;//逃げ伸びるまでの時間
        private bool stop;//立ち止まる処理用フラッグ

        //モーション用
        private bool changeMotion;//左右の歩きモーション判別用flag
        private bool discoverFlag;//追跡時の歩きモーション移行用flag

        private Conditions.CharacterMovements charMovement;

        private Motion motion;
        private Sound sound;
        public Light light;

        public GuardMan(Vector2 leftPosition, Vector2 rightPosition, int floorWidth, int floorHeight, GameDevice gameDevice, Light light)
            : base("GuardMan", leftPosition, floorWidth, floorHeight, 80, 54, gameDevice)
        {
            //左端から移動開始
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;
            velocity = new Vector2(1.0f, 0.0f);//最初は右移動
            guardDirection = false;
            sound = gameDevice.Sound;

            this.light = light;

            speed = 2f;

            timer = new Timer(2.0f);//立ち止まる処理用
            escape = new Timer(5f);//逃げ延びるまでの時間

            //フラッグは最初false
            stop = false;
            discover = false;
            changeMotion = false;
            discoverFlag = false;

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
            charMovement = Conditions.CharacterMovements.Idle;
        }

        public GuardMan(GuardMan other) : this(other.leftPosition, other.rightPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice, other.light)
        {

        }

        public override object Clone()
        {
            return new GuardMan(this);
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
                        drawingPosition.Y = gameObject.GetRectangle().Top - this.height;
                        velocity.Y = 0.0f;
                    }
                }

                else if (dir == Direction.Right)//右
                {
                    worldPosition.X = gameObject.GetRectangle().Right;
                    drawingPosition.X = gameObject.GetRectangle().Right;
                }

                else if (dir == Direction.Left)//左
                {
                    worldPosition.X = gameObject.GetRectangle().Left - this.width;
                    drawingPosition.X = gameObject.GetRectangle().Left - this.width;
                }

                else if (dir == Direction.Botton)//下
                {
                    worldPosition.Y = gameObject.GetRectangle().Bottom;
                    drawingPosition.Y = gameObject.GetRectangle().Bottom;
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
                if (worldPosition.X > rightPosition.X && !discover && !stop)
                {
                    worldPosition.X = rightPosition.X;
                    drawingPosition.X = rightPosition.X - 32;
                    timer.Initialize();
                    stop = true;
                    velocity.X = 0;
                }
                if (worldPosition.X < leftPosition.X && !discover && !stop)
                {
                    worldPosition.X = leftPosition.X;
                    drawingPosition.X = leftPosition.X - 32;
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
                else if (!stop && !discover)
                {
                    worldPosition += velocity * speed;//移動
                    drawingPosition += velocity * speed;
                }

                //Hitしたら追跡
                if (discover)
                {
                    worldPosition.X += velocity.X * speed;
                    drawingPosition.X += velocity.X * speed;
                    if (!PlayerInfo.IsVisible)
                    {
                        escape.Update();
                    }


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
                        drawingPosition.X += velocity.X * speed;

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
                        drawingPosition.X = leftPosition.X - 32;
                        timer.Initialize();
                        stop = true;
                        Wait();
                        discover = false;
                    }

                }

                //画面端にいくと反転
                if (worldPosition.X <= Screen.widthMax || worldPosition.X >= 32 * 54)
                {
                    velocity.X = -velocity.X;
                }
                #endregion



                motion.Update(gameTime);
                UpdateMotion(velocity);
                //Console.WriteLine(velocity.Length());
                //Console.WriteLine(escape.Now());
                //Console.WriteLine(light.LightFlag());
            }
            Discover();
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

        /// <summary>
        /// 実体生成用
        /// </summary>
        /// <param name="light"></param>
        public void GuardLight(Light light)
        {
            this.light = light;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition, motion.DrawingRange());
            if (discover)
            {
                if (escape.IsTime())
                {
                    renderer.DrawTexture("questionMark", drawingPosition + new Vector2(10, -32));
                }
                else
                {
                    renderer.DrawTexture("exclamationMark", drawingPosition + new Vector2(10, -32));
                }
            }

        }

        /// <summary>
        /// モーション管理
        /// </summary>
        /// <param name="velocity"></param>
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
