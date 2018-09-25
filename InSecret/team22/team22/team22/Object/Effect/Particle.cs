using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object.Effect
{
    abstract class Particle
    {
        #region Fields
        protected string _name;
        protected Vector2 _initialPosition;
        protected Vector2 _velocity;
        #endregion

        public Particle (string name, Vector2 initialPosition, Vector2 velocity)
        {
            _name = name;
            _initialPosition = initialPosition;
            _velocity = velocity;
        }

        public abstract void Update();
        public abstract void Draw(Renderer renderer);
        public abstract bool IsDead();
    }
}
