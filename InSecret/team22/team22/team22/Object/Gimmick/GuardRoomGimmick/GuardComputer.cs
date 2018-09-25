using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class GuardComputer : GameObject
    {
        #region Fields
        private Floor _floor;
        private InputState _input;
        private HackingCom _hackingCom;
        private bool isHacked = false;
        private LastMoveEvent _lastMoveEvent;
        #endregion

        #region Getter Setter メソッド
        public bool IsHacked
        {
            get { return isHacked; }
            set { isHacked = value; }
        }
        #endregion

        public GuardComputer(Vector2 worldPosition, int floorWidth, int floorHeight, Floor floor, GameDevice gameDevice) 
            : base ("block2", worldPosition, floorWidth, floorHeight, 33, 41, gameDevice)
        {
            _floor = floor;
            _input = gameDevice.InputState;
            isHacked = false;
        }
        
        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && 
                (_input.GetKeyTrigger(Keys.X) || _input.GetKeyTrigger(Buttons.A)) && PlayerInfo.IsMove && !isHacked)
            {
                _hackingCom = new HackingCom(_floor.FloorRange.Width, _floor.FloorRange.Height, gameDevice, this);
                _floor.AddObject(_hackingCom);
                PlayerInfo.IsMove = false;
            }

            if (gameObject is Player && isHacked && _lastMoveEvent == null)
            {
                _lastMoveEvent = new LastMoveEvent(GetWorldPosition(), floorWidth, floorHeight, gameDevice);
            }
        }

        
        public override void Update(GameTime gameTime)
        {
            if (_lastMoveEvent != null && !_lastMoveEvent.IsDead())
            {
                _lastMoveEvent.Update(gameTime);
            }
        }

        #region 使わないメソッド
        public override object Clone()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
