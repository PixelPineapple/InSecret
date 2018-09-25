using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class ElevatorLight : GameObject
    {
        #region Fileds
        private UpElevator upElevator;
        private DownElevator doElevator;
        private GameObject elevator;
        private Motion motion;

        #endregion


        public ElevatorLight(Vector2 worldPosition, int floorWidth, int floorHeight, GameDevice gameDevice, GameObject elevator)
            : base("elevator_Light", worldPosition, floorWidth, floorHeight, 0, 0, gameDevice)
        {
            if (elevator is UpElevator)
            { upElevator = (UpElevator)elevator; }
            else if (elevator is DownElevator)
            { doElevator = (DownElevator)elevator; }
            this.elevator = elevator;

            motion = new Motion();
            for (int i = 0; i <= 7; i++)
            { motion.Add(i, new Rectangle(107 * i, 0, 107, 118)); }
            motion.Initialize(new Range(0, 7), new CountDownTimer(0.2f));
        }

        public ElevatorLight(ElevatorLight other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().Y, (int)other.GetDrawingPosition().X, other.gameDevice, other.elevator)
        { }

        public override object Clone()
        {
            return new ElevatorLight(this);
        }

        public override void Hit(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (upElevator != null)
            {
                motion.MotionNum = upElevator.GetMotionNum;
            }
            if (doElevator != null)
            {
                motion.MotionNum = doElevator.GetMotionNum;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition, motion.DrawingRange());
        }
    }
}
