using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class Firewall : GameObject
    {
        #region Fields
        private float _finalPosition;
        private float _moveSpeed;
        private float _initialPosition;
        private bool _isMoveDown;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="gameDevice"></param>
        public Firewall(Vector2 worldPosition, float finalPosition, int height, int width, float moveSpeed, GameDevice gameDevice)
            : base ("Firewall", worldPosition, height, width, gameDevice)
        {
            _initialPosition = worldPosition.Y;
            _isMoveDown = true;
            _finalPosition = finalPosition;
            _moveSpeed = moveSpeed;
        }


        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (worldPosition.Y + height >= _finalPosition)
            {
                _isMoveDown = false;
            }
            else if(worldPosition.Y <= _initialPosition)
            {
                _isMoveDown = true;
            }

            if (_isMoveDown)
            {
                worldPosition.Y += _moveSpeed;
            }
            else
            {
                worldPosition.Y -= _moveSpeed;
            }
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
