using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;

namespace team22.Object.Effect
{
    class OutliningParticle : Particle
    {
        #region Fields
        private Vector2 _scale;
        #endregion

        public OutliningParticle (Vector2 initialPosition)
            : base ("OutliningParticle", initialPosition, Vector2.Zero)
        {
            _scale = new Vector2(1, 1);
        }

        public override bool IsDead()
        {
            return _scale.X > 1.8f;
        }

        public override void Update()
        {
            _scale.X += 0.1f;
            _scale.Y += 0.1f;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(_name, _initialPosition, _scale, Color.Yellow, 0.3f);
        }
    }
}
