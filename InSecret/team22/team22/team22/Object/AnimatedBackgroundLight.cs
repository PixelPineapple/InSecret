using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class AnimatedBackgroundLight : GameObject
    {
        #region Fields
        private Motion motion;
        #endregion

        public AnimatedBackgroundLight(string name, Vector2 worldPosition, int floorWidth, int floorHeight, int height, int width, GameDevice gameDevice)
            : base (name, worldPosition, floorWidth, floorHeight, height, width, gameDevice)
        {
            motion = new Motion(2);
            int counter = 5;
            for (int i = 0; i < counter; i++)
            {
                motion.Add(i, new Rectangle(width * i, 0, width, height));
            }
            motion.Initialize(new Range(0, 4), new CountDownTimer(0.15f));
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            motion.Update(gameTime);
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition, motion.DrawingRange(), Color.White);
        }
    }
}
