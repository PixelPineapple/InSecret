using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Utility;

namespace team22.Object
{
    class Laser : GameObject
    {
        public Vector2 leftworldPosition;//左端
        public Vector2 rightworldPosition;//右端
        private Vector2 velocity;//移動量

        public static Vector2 playerPos;

        private bool side;
        private bool leftDiagonal;
        private bool rightDiagonal;

        private Timer timer;
        private Timer wait;

        private float speed;//移動速度

        /// <summary>
        /// 横移動するレーザー
        /// </summary>
        /// <param name="leftPosition">左端</param>
        /// <param name="rightPosition">右端</param>
        /// <param name="gameDevice"></param>
        public Laser(Vector2 leftPosition, Vector2 rightPosition, int floorWidth, int floorHeight, GameDevice gameDevice,Vector2 velocity) : base("laser", leftPosition, floorWidth, floorHeight, 8, 8, gameDevice)
        {
            this.leftworldPosition = leftPosition;
            this.rightworldPosition = rightPosition;

            this.velocity = velocity;

            speed = 1f;

            side = true;

            timer = new Timer(0.3f);
            wait = new Timer(0.2f);
            timer.Initialize();
        }

        /// <summary>
        /// 斜め移動するレーザー
        /// </summary>
        /// <param name="leftworldPosition">左端</param>
        /// <param name="rightworldPosition">右端</param>
        /// <param name="gameDevice"></param>
        /// <param name="speed">速度</param>
        public Laser(Vector2 leftworldPosition, Vector2 rightworldPosition, int floorWidth, int floorHeight, GameDevice gameDevice, float speed) : base("laser", leftworldPosition, floorWidth, floorHeight, 8, 8, gameDevice)
        {
            this.leftworldPosition = leftworldPosition;
            this.rightworldPosition = rightworldPosition;


            if (leftworldPosition.X < rightworldPosition.X)
            {
                velocity = new Vector2(1.0f, -1.0f);
                rightDiagonal = true;
            }
            else if (leftworldPosition.X > rightworldPosition.X)
            {
                velocity = new Vector2(-1.0f, -1.0f);
                leftDiagonal = true;
            }

            this.speed = speed;

            timer = new Timer(0.3f);
            wait = new Timer(0.2f);
            timer.Initialize();
        }

        ///// <summary>
        ///// 縦移動するレーザー
        ///// </summary>
        ///// <param name="worldPosition"></param>
        ///// <param name="gameDevice"></param>
        //public Laser(Vector2 topPosition, Vector2 bottomPosition, int floorWidth, int floorHeight, GameDevice gameDevice) : base("laser", leftPosition, floorWidth, floorHeight, 8, 8, gameDevice)
        //{
        //    worldPosition += new Vector2(16f, 16f);
        //    drawingPosition += new Vector2(16f, 16f);
        //    velocity = new Vector2(0f, 1.0f);//最初は下に移動
        //    speed = 5f;

        //    side = false;

        //    timer = new Timer(0.3f);
        //    wait = new Timer(0.2f);
        //    timer.Initialize();
        //}

        public Laser(Laser other) : this(other.leftworldPosition, other.rightworldPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice,other.velocity)
        {

        }

        public override object Clone()
        {
            return new Laser(this);
        }

        public override void Hit(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);//どの向きで当たっているか

            //Hitしたら発見フラッグをTrueにし、PlayerのworldPositionを拾う
            if (gameObject is Player && PlayerInfo.IsVisible)
            {
                EnemyManager.LaserFlag = true;
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
                        drawingPosition.Y = gameObject.GetRectangle().Top - this.height;
                        velocity.Y = -velocity.Y;
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
                    drawingPosition.Y = gameObject.GetRectangle().Bottom;
                    velocity.Y = -velocity.Y;
                }
            }
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            if (PlayerInfo.IsMove || !PlayerInfo.IsVisible)
            {
                //跳ね返り処理
                if (side)
                {
                    if (worldPosition.X > rightworldPosition.X || worldPosition.X < leftworldPosition.X
                        || worldPosition.Y > rightworldPosition.Y || worldPosition.Y < leftworldPosition.Y )
                    {
                        velocity = -velocity;
                    }
                }
                if (rightDiagonal)
                {
                    if (worldPosition.X > rightworldPosition.X || worldPosition.X < leftworldPosition.X)
                    {
                        velocity.X = -velocity.X;
                        velocity.Y = -velocity.Y;
                    }
                }
                if (leftDiagonal)
                {
                    if (worldPosition.X < rightworldPosition.X || worldPosition.X > leftworldPosition.X)
                    {
                        velocity.X = -velocity.X;
                        velocity.Y = -velocity.Y;
                    }
                }

                worldPosition += velocity * speed;
                drawingPosition += velocity * speed;

                wait.Update();
                if (wait.IsTime())
                {
                    EnemyManager.LaserFlag = false;
                }
            }

        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
