using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class TestGimmick : GameObject
    {
        private InputState input;
        private Key key;

        private bool isUnLock;
        public TestGimmick(Vector2 worldPosition, int floorHeight, int floorWidth, GameDevice gameDevice, Key key)
            : base("block2", worldPosition, floorHeight, floorWidth, 64, 64, gameDevice)
        {
            input = gameDevice.InputState;
            this.key = key;
            isUnLock = false;
        }

        public TestGimmick(TestGimmick other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice, other.key)
        { }

        public override object Clone()
        {
            return new TestGimmick(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && input.GetKeyTrigger(Keys.X))
            {
                isUnLock = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isUnLock)
            {
                key.IsOpen = true;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
