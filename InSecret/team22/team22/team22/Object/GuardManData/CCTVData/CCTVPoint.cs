using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Utility;

namespace team22.Object
{
    class CCTVPoint : GameObject
    {
        public Vector2 leftPosition;//左端
        public Vector2 rightPosition;//右端
        public Vector2 middlePosition;//CCTV用Position
        private Vector2 velocity;//移動量

        public static Vector2 playerPos;


        private float speed;//移動速度

        private Floor floor;

        private CCTV cctv;


        private Timer timer;//立ち止まる処理用
        private Timer wait;
        private bool stop;//立ち止まる処理用フラッグ

        public CCTVPoint(Vector2 leftPosition, Vector2 rightPosition, int floorWidth, int floorHeight, GameDevice gameDevice, Floor floor) : base("CCTVpoint", leftPosition, floorWidth, floorHeight, 32, 64, gameDevice)
        {
            //左端から移動開始
            this.leftPosition = leftPosition;
            this.rightPosition = rightPosition;

            this.floor = floor;

            middlePosition.X = (rightPosition.X + leftPosition.X) / 2;
            middlePosition.Y = leftPosition.Y - 64.0f;

            velocity = new Vector2(1.0f, 0.0f);//最初は右移動

            cctv = new CCTV(middlePosition + new Vector2(0, -128), floorWidth, floorHeight, gameDevice, this);
            floor.AddObject(cctv);

            speed = 2f;

            timer = new Timer(3.0f);//立ち止まる処理用
            wait = new Timer(0.2f);

            //フラッグは最初false
            stop = false;
        }

        public CCTVPoint(CCTVPoint other) : this(other.leftPosition, other.rightPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice, other.floor)
        {

        }

        public override object Clone()
        {
            return new CCTVPoint(this);
        }

        public override void Hit(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);//どの向きで当たっているか

            //Hitしたら発見フラッグをTrueにし、PlayerのPositionを拾う
            if (gameObject is Player && PlayerInfo.IsVisible)
            {
                EnemyManager.CCTVFlag = true;
                playerPos = gameObject.GetWorldPosition();
                wait.Initialize();
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
                }

                else if (dir == Direction.Left)//左
                {
                    worldPosition.X = gameObject.GetRectangle().Left - this.width;
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
                #region カメラの動き
                //折り返し処理
                if (worldPosition.X > rightPosition.X && !stop)
                {
                    worldPosition.X = rightPosition.X;
                    timer.Initialize();
                    stop = true;
                }
                if (worldPosition.X < leftPosition.X && !stop)
                {
                    worldPosition.X = leftPosition.X;
                    timer.Initialize();
                    stop = true;
                }

                //立ち止まっていればTimer起動
                if (stop)
                {
                    timer.Update();
                    Wait();
                }
                //それ以外ならば進む
                else if (!stop)
                {
                    worldPosition += velocity * speed;//移動
                    drawingPosition += velocity * speed;
                }
                #endregion

                wait.Update();
                if (wait.IsTime())
                {
                    EnemyManager.CCTVFlag = false;
                }

                cctv.Update(gameTime);
            }
        }

        /// <summary>
        /// タイマー処理
        /// </summary>
        private void Wait()
        {
            //時間になったら逆を向き、歩き始める
            if (timer.IsTime())
            {
                velocity.X = -velocity.X;//向き反転
                stop = false;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
