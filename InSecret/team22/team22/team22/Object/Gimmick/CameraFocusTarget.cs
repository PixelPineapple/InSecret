using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class CameraFocusTarget : GameObject
    {
        private Vector2 firstPosition;
        private bool isMove;
        public bool IsMove
        {
            get { return isMove; }
            set { isMove = value; }
        }

        public CameraFocusTarget(Vector2 worldPosition, GameDevice gameDevice)
            : base("block2", worldPosition, 0, 0, 64, 64, gameDevice)
        {
            firstPosition = worldPosition;
            isMove = false;
        }

        public CameraFocusTarget(CameraFocusTarget other)
            : this(other.worldPosition, other.gameDevice)
        { }

        public void Initialize()
        {
            worldPosition = firstPosition;
            isMove = false;
        }

        public override object Clone()
        {
            return new CameraFocusTarget(this);
        }

        public override void Hit(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
