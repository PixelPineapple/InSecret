using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class BlueFile : GameObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="charHeight"></param>
        /// <param name="charWidth"></param>
        /// <param name="gameDevice"></param>
        public BlueFile (Vector2 worldPosition, int charHeight, int charWidth, GameDevice gameDevice)
            : base ("LockedFolder", worldPosition, charHeight, charWidth, gameDevice)
        {

        }

        public override void Hit(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void FrontDraw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition);
        }
        #region 使わないメソッド
        public override object Clone()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
