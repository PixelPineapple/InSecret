using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class Virus : GameObject
    {
        #region Fields
        private Motion _motion;
        private Vector2 _velocity;
        private InputState _input;
        #endregion

        public Virus (Vector2 worldPosition, int charHeight, int charWidth, GameDevice gameDevice)
            : base ("Virus", worldPosition, charHeight, charWidth, gameDevice)
        {
            _input = gameDevice.InputState;
            _motion = new Motion();
            int counter = 0;
            for (int y = 0; y < 1; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    _motion.Add(counter, new Rectangle(charWidth * x, charHeight * y, charWidth, charHeight));
                    counter++;
                }
            }
            _motion.Initialize(new Range(0, 3), new CountDownTimer(0.08f));
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Firewall)
            {
                isDead = true;
            }
            if (gameObject is BlueFile)
            {
                gameObject.SetIsDead(true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float speed = 1.3f;

            _velocity.X = _input.Velocity().X * speed;
            _velocity.Y = _input.Velocity().Y * speed;

            worldPosition = worldPosition + _velocity * speed;

            _motion.Update(gameTime);


            Console.WriteLine(isDead);
        }

        public override void FrontDraw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition, _motion.DrawingRange());
        }

        public void ClampingPosition (Vector2 min, Vector2 max)
        {
            worldPosition = Vector2.Clamp(worldPosition, min, max);
        }

        #region 使わないメソッド
        public override object Clone()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
