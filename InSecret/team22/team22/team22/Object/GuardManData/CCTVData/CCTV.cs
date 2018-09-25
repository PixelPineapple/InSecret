using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    /// <summary>
    /// ライト
    /// </summary>
    class CCTV : GameObject
    {

        private CCTVPoint cctvPoint;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position">位置</param>
        /// <param name="gameDevice">ゲームデバイス</param>
        public CCTV(Vector2 position, int floorWidth, int floorHeight, GameDevice gameDevice, CCTVPoint cctvPoint) : base("CCTVL", position, floorWidth, floorHeight, 32, 32, gameDevice)
        {
            this.cctvPoint = cctvPoint;
        }
        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public CCTV(CCTV other) : this(other.worldPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice, other.cctvPoint)
        {

        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CCTV(this);
        }
        /// <summary>
        /// 衝突
        /// </summary>
        /// <param name="gameObject"></param>
        public override void Hit(GameObject gameObject)
        {
            
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public override void Update(GameTime gameTime)
        {
            float length = Vector2.Distance(cctvPoint.middlePosition,cctvPoint.GetWorldPosition());

            if (cctvPoint.GetWorldPosition().X >= (cctvPoint.leftPosition.X + cctvPoint.middlePosition.X) / 2
                && cctvPoint.GetWorldPosition().X <= (cctvPoint.rightPosition.X + cctvPoint.middlePosition.X) / 2)
            {
                name = "CCTVM";
            }
            else if (cctvPoint.GetWorldPosition().X <= (cctvPoint.leftPosition.X + cctvPoint.middlePosition.X) / 2)
            {
                name = "CCTVL";
            }
            else if (cctvPoint.GetWorldPosition().X >= (cctvPoint.rightPosition.X + cctvPoint.middlePosition.X) / 2)
            {
                name = "CCTVR";
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
