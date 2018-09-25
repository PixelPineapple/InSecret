using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class HintsPoster : GameObject
    {
        #region Fields
        private bool _isPressed;
        #endregion


        public HintsPoster (Vector2 worldPosition, int floorWidth, int floorHeight, GameDevice gameDevice)
            : base ("poster_small", worldPosition, floorWidth, floorHeight, 40, 64, gameDevice)
        {
            _isPressed = false;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && 
                (gameDevice.InputState.GetKeyTrigger(Keys.X) || gameDevice.InputState.GetKeyTrigger(Buttons.A)))
            {
                _isPressed = !_isPressed;
                if (_isPressed)
                    PlayerInfo.IsMove = false;
                else
                    PlayerInfo.IsMove = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void FrontDraw(Renderer renderer)
        {
            if (_isPressed)
            {
                renderer.DrawTexture("poster", GimmickFront.frontDisplayPosi);
            }
        }
    }
}
