#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
#endregion

namespace team22.Object
{
    class BackGroundLight : GameObject
    {
        #region Fields

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="floorWidth"></param>
        /// <param name="floorHeight"></param>
        /// <param name="gameDevice"></param>
        public BackGroundLight (string name, Vector2 worldPosition, int floorWidth, int floorHeight, int height, int width, GameDevice gameDevice)
            : base (name, worldPosition, floorWidth, floorHeight, height, width, gameDevice)
        { }

        public BackGroundLight (BackGroundLight other)
            : this (other.name, other.worldPosition, (int)other.GetDrawingPosition().Y, (int)other.GetDrawingPosition().X, other.height, other.width, other.gameDevice)
        { }

        public override object Clone()
        {
            return new BackGroundLight(this);
        }

        public override void Hit(GameObject gameObject)
        { }

        public override void Update(GameTime gameTime)
        { }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition, Color.White);
        }
    }
}
