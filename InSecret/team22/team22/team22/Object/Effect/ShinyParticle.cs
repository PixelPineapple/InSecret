using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;

namespace team22.Object.Effect
{
    class ShinyParticle : Particle
    {
        #region Fields
        private float _alpha;
        private bool _isGlowing;
        private Color _color;
        #endregion

        public ShinyParticle (Vector2 position, Color color)
            : base("ShiningParticle", position, Vector2.Zero)
        {
            _alpha = 0.1f;
            _isGlowing = true;
            _color = color;
        }

        /// <summary>
        /// パーティクルを消すか？
        /// </summary>
        public override bool IsDead()
        {
            return _alpha <= 0;
        }

        /// <summary>
        /// 更新メソッド
        /// </summary>
        public override void Update()
        {
            if (_isGlowing)
            {
                _alpha += 0.1f;
                if (_alpha >= 1.0f)
                {
                    _isGlowing = false;
                }
            }
            else
            {
                _alpha -= 0.1f;
            }
        }

        /// <summary>
        /// 描画メソッド
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(_name, _initialPosition, _color, _alpha);
        }
    }
}
