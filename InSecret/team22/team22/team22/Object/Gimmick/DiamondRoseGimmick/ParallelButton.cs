using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;

namespace team22.Object.Gimmick.DiamondRoseGimmick
{
    class ParallelButton : GameObject
    {
        #region Fields
        private InputState _input;
        private bool isPressed;
        private Player player;
        #endregion

        #region Getter Setter メソッド
        /// <summary>
        /// プレイやーに触られるか
        /// </summary>
        /// <returns></returns>
        public bool GetIsPressed()
        {
            return isPressed;
        }
        #endregion

        public ParallelButton (Vector2 worldPosition, int floorWidth, int floorHeight, int height, int width, GameDevice gameDevice)
            : base ("PinkRoseButton", worldPosition, floorWidth, floorHeight, height, width, gameDevice)
        {
            _input = gameDevice.InputState;
            isPressed = false;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && _input.GetKeyTrigger(Keys.X) && !isPressed)
            {
                isPressed = true;
                PlayerInfo.ForceSwitch = true;
            }

        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
