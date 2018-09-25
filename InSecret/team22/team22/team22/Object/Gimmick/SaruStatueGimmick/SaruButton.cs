#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;
using team22.Object.Gimmick.SaruStatue;
#endregion

namespace team22.Object
{
    class SaruButton : GameObject
    {
        #region Fields
        private InputState input;
        private Floor _floor;
        private SaruMonitor _saruMonitor;
        private List<Key> _keys;
        #endregion

        public SaruButton(Vector2 position, int floorWidth, int floorHeight, Floor floor, List<Key> keys, GameDevice gameDevice)
            : base ("Saru", position, floorWidth, floorHeight, 72, 59, gameDevice)
        {
            _floor = floor;
            input = gameDevice.InputState;
            _keys = keys;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && 
                (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)) && PlayerInfo.IsMove)
            {
                _saruMonitor = new SaruMonitor(_floor.FloorRange.X / 32, _floor.FloorRange.Y / 32, _keys, gameDevice);
                _floor.AddObject(_saruMonitor);
                PlayerInfo.IsMove = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
