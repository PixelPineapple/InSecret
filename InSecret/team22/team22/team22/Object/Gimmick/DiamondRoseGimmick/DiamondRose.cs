using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;
using team22.Object.Effect;

namespace team22.Object.Gimmick.DiamondRoseGimmick
{
    class DiamondRose : GameObject
    {
        #region Fields
        private List<ParallelButton> _keys;
        private InputState _input;
        private bool _isTouchable;
        private bool[] _condition;
        private ParticleManager _particleManager;
        private Floor _floor;
        #endregion

        #region Getter Setter メソッド
        #endregion

        public DiamondRose(Vector2 worldPosition, int floorWidth, int floorHeight, List<ParallelButton> keys, GameDevice gameDevice, Floor floor)
            : base ("treasure", worldPosition, floorWidth, floorHeight, 64, 32, gameDevice)
        {
            _particleManager = new ParticleManager();
            _keys = keys;
            _input = gameDevice.InputState;
            _condition = new bool[2];
            _isTouchable = false;
            _floor = floor;
        }

        public void Initialize()
        {
            _isTouchable = false;
        }

        /// <summary>
        /// 使わないメソッド
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && _isTouchable)
            {
                isDead = true;
                PlayerInfo.IsFinish = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isTouchable)
            {
                _condition[0] = _keys[0].GetIsPressed();
                _condition[1] = _keys[1].GetIsPressed();

                if(_condition[0] && _condition[1])
                {
                    _isTouchable = true; // DiamondRoseが取れる。
                    PlayerInfo.AutoMove = true;
                }
            }
            _particleManager.Add(new ShinyParticle(new Vector2(
                gameDevice.Random.Next((int)drawingPosition.X, (int)drawingPosition.X + width),
                gameDevice.Random.Next((int)drawingPosition.Y, (int)drawingPosition.Y + height)), Color.MistyRose));

            _particleManager.Update();
        }

        public override void Draw(Renderer renderer)
        {
            if (!_isTouchable)
            {
                renderer.DrawTexture("glass_back", drawingPosition);
                renderer.DrawTexture("treasure", drawingPosition);
                renderer.DrawTexture("glass_front", drawingPosition);
            }
            else
            {
                renderer.DrawTexture("treasure", drawingPosition);
            }

            _particleManager.Draw(renderer);
        }
    }
}
