using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    // まだ使っていない。
    class Hint : GameObject
    {
        #region Fields
        private Dialogue _dialogue;
        private Font _fonts;
        private string[] _texts;
        private bool _oneTimeOnly = false;
        private GameObject _basedOn;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="gameDevice"></param>
        public Hint (string name, Vector2 worldPosition, int floorWidth, int floorHeight, int drawHeight, int drawWidth, bool oneTimeOnly, GameDevice gameDevice)
            : base (name, worldPosition, floorWidth, floorHeight, drawHeight, drawWidth, gameDevice)
        {
            _fonts = gameDevice.Font;
            _oneTimeOnly = oneTimeOnly;
            _basedOn = null;
        }

        public Hint(string name, Vector2 worldPosition, int floorWidth, int floorHeight, int drawHeight, int drawWidth, bool oneTimeOnly, GameObject basedOn, GameDevice gameDevice)
            : base(name, worldPosition, floorWidth, floorHeight, drawHeight, drawWidth, gameDevice)
        {
            _fonts = gameDevice.Font;
            _oneTimeOnly = oneTimeOnly;
            _basedOn = basedOn;
        }

        public Hint (Hint other)
            : this (other.name, other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.height, other.width, other._oneTimeOnly, other.gameDevice)
        { }

        public override object Clone()
        {
            return new Hint (this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && _oneTimeOnly)
            {
                isDead = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_basedOn is UpElevator && !((UpElevator)_basedOn).GetIsLock())
            {
                isDead = true;
            }
        }
    }
}
