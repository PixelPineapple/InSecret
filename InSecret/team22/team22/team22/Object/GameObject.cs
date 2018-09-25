using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using team22.Device;
using team22.Def;

namespace team22.Object
{

    enum Direction
    {
        //上、下、左、右
        Top, Botton, Left, Right
    };

    abstract class GameObject : ICloneable // コピー機能を追加
    {
        #region Fields
        protected string name;
        protected Vector2 worldPosition;
        protected Vector2 drawingPosition;
        protected int height; //高さ
        protected int width; //幅
        protected bool isDead = false; //死亡フラグ
        protected GameDevice gameDevice;
        protected int floorWidth;
        protected int floorHeight;
        protected string keyDialogue;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="worldPosition"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="gameDevice"></param>
        public GameObject(string name, Vector2 worldPosition, int height, int
            width, GameDevice gameDevice)
        {
            this.name = name;
            this.worldPosition = worldPosition;
            this.height = height;
            this.width = width;
            this.gameDevice = gameDevice;
        }

        public GameObject(string name, Vector2 worldPosition, int floorWidth, int floorHeight, int height, int
            width, GameDevice gameDevice)
        {
            this.floorHeight = floorHeight;
            this.floorWidth = floorWidth;
            this.name = name;
            this.worldPosition = worldPosition;
            drawingPosition = worldPosition - new Vector2(floorWidth * 32, floorHeight * 32);
            this.height = height;
            this.width = width;
            this.gameDevice = gameDevice;
        }

        #region Getter Setter メソッド

        /// <summary>
        /// 位置の設定
        /// </summary>
        /// <param name="position"></param>
        public void SetWorldPosition(Vector2 position)
        {
            worldPosition = position;
        }

        public void SetDrawingPosition (Vector2 position)
        {
            drawingPosition = position;
        }

        /// <summary>
        /// プレイヤーが瞬間移動する用
        /// </summary>
        public void SetDrawingPosition ()
        {
            drawingPosition = worldPosition - (new Vector2(floorWidth * 32, floorHeight * 32));
        }

        /// <summary>
        /// 位置の取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetWorldPosition()
        { return worldPosition; }

        public Vector2 GetDrawingPosition()
        { return drawingPosition; }

        public int GetPicHeight()
        { return height; }

        public int GetPicWidth()
        { return width; }

        public string KeyDialogue
        {
            get { return keyDialogue; }
            set { keyDialogue = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        //抽象メソッド
        public abstract object Clone(); //Icloneableで必ず必要
        public abstract void Update(GameTime gameTime);
        public abstract void Hit(GameObject gameObject);

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer)
        { renderer.DrawTexture(name, drawingPosition);
        }

        /// <summary>
        /// 前に描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void FrontDraw(Renderer renderer)
        {
        }

        /// <summary>
        /// 死亡しているか？
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        { return isDead; }

        public void SetIsDead(bool value)
        {
            isDead = value;
        }

        /// <summary>
        /// 矩形情報の取得
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRectangle()
        {
            //矩形情報の作成
            Rectangle area = new Rectangle();

            area.X = (int)worldPosition.X;
            area.Y = (int)worldPosition.Y;
            area.Height = height;
            area.Width = width;

            return area;
        }

        /// <summary>
        /// 衝突判定
        /// </summary>
        /// <param name="otherObj"></param>
        /// <returns></returns>
        public bool Collision(GameObject otherObj)
        {
            return this.GetRectangle().Intersects(otherObj.GetRectangle());
        }


        public Direction CheckDirection(GameObject otherObj)
        {
            //中心位置の取得
            Point thisCenter = this.GetRectangle().Center;
            Point otherCenter = otherObj.GetRectangle().Center;
            Vector2 dir =
                new Vector2(thisCenter.X, thisCenter.Y) -
                new Vector2(otherCenter.X, otherCenter.Y);

            //差分からどの方向で当たっているかを調べ、値を返す
            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                { return Direction.Right; }
                else
                { return Direction.Left; }

            }
                if (dir.Y > 0)
                { return Direction.Botton; }

                //プレイヤーがブロックに乗った
                return Direction.Top;
            
        }
    }
}
