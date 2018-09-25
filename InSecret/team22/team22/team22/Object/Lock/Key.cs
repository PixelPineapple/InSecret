using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    /// <summary>
    /// 
    /// </summary>
    class Key : GameObject
    {
        public bool isOpen = false;
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public Key(Vector2 worldPosition, int floorHeight, int floorWidth, GameDevice gameDevice)
            : base("block2", worldPosition, floorHeight, floorWidth, 64, 64, gameDevice)
        {
        }

        public Key(Key other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice)
        { }

        public override object Clone()
        {
            return new Key(this);
        }

        public override void Hit(GameObject gameObject)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (isOpen)
            {
                isDead = true; 
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }

    }
}
