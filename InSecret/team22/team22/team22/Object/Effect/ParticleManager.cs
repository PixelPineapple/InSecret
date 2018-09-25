using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;

namespace team22.Object.Effect
{
    class ParticleManager
    {
        #region Fields
        private List<Particle> _particles;
        #endregion

        public ParticleManager()
        {
            _particles = new List<Particle>();
        }

        public void Add(Particle particle)
        {
            _particles.Add(particle);
        }

        public void Update()
        {
            if (_particles.Count > 0)
            {
                _particles.ForEach(particle => particle.Update());
                _particles.RemoveAll(particle => particle.IsDead());
            }
        }

        public void Draw(Renderer renderer)
        {
            if(_particles.Count > 0)
            _particles.ForEach(particle => particle.Draw(renderer));
        }
    }
}
