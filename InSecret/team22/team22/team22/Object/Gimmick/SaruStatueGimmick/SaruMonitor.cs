using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;
using team22.Object.Effect;

namespace team22.Object.Gimmick.SaruStatue
{
    class SaruMonitor : GameObject
    {
        private enum SelectedLimbs
        {
            RIGHTHAND,
            LEFTHAND,
            TAIL,
        }

        #region Fields
        private int _rightHand, _leftHand, _tail;
        private InputState _input;
        //private int _counter;
        private List<Key> _saruKeys;
        private SelectedLimbs _selectedLimbs;
        private SelectedLimbs _previousSelectedLimbs;
        private ParticleManager _particleManager;
        private Random _rand;
        private Sound _sound;
        #endregion

        public SaruMonitor(int floorWidth, int floorHeight, List<Key> saruKeys, GameDevice gameDevice)
            : base ("Saru_Button", GimmickFront.frontDisplayPosi, floorWidth, floorHeight, GimmickFront.frontY, GimmickFront.frontX, gameDevice)
        {
            _input = gameDevice.InputState;
            _sound = gameDevice.Sound;
            _rightHand = 0;
            _leftHand = 0;
            _tail = 0;
            //_counter = 0;
            _selectedLimbs = SelectedLimbs.RIGHTHAND;
            _previousSelectedLimbs = SelectedLimbs.RIGHTHAND;
            _saruKeys = saruKeys;
            // パーティクル
            _particleManager = new ParticleManager();
            _rand = gameDevice.Random;
        }

        #region Unused Method / 使わないメソッド
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            MoveMonkey();

            if (_selectedLimbs == SelectedLimbs.LEFTHAND && _leftHand == 0)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 2.3f + 3.1f), (int)(worldPosition.X * 2.3f + 3.1f + (9.0f * 4))), 
                    _rand.Next((int)(worldPosition.Y * 2f - 8.1f), (int)(worldPosition.Y * 2f - 8.1f + (18.0f * 4)))
                    )));
            }
            else if (_selectedLimbs == SelectedLimbs.LEFTHAND && _leftHand == 1)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 2.3f + 3.1f), (int)(worldPosition.X * 2.3f + 3.1f + (9.0f * 4))),
                    _rand.Next((int)(worldPosition.Y * 2.3f + 12f), (int)(worldPosition.Y * 2.3f + 12f + (18.0f * 4)))
                    )));
            }

            if (_selectedLimbs == SelectedLimbs.RIGHTHAND && _rightHand == 0)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 1.5f + 10.5f), (int)(worldPosition.X * 1.5f + 10.5f + (9.0f * 4))),
                    _rand.Next((int)(worldPosition.Y * 2f - 8.1f), (int)(worldPosition.Y * 2f - 8.1f + (18.0f * 4)))
                    )));
            }
            else if (_selectedLimbs == SelectedLimbs.RIGHTHAND && _rightHand == 1)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 1.5f + 10.5f), (int)(worldPosition.X * 1.5f + 10.5f + (9.0f * 4))),
                    _rand.Next((int)(worldPosition.Y * 2.3f + 12f), (int)(worldPosition.Y * 2.3f + 12f + (18.0f * 4)))
                    )));
            }

            if (_selectedLimbs == SelectedLimbs.TAIL && _tail == 0)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 1.7f), (int)(worldPosition.X * 1.7f + (18.0f * 4))),
                    _rand.Next((int)(worldPosition.Y * 2.7f), (int)(worldPosition.Y * 2.7f + (6.0f * 4)))
                    )));
            }
            else if (_selectedLimbs == SelectedLimbs.TAIL && _tail == 1)
            {
                _particleManager.Add(new OutliningParticle(new Vector2(
                    _rand.Next((int)(worldPosition.X * 2f), (int)(worldPosition.X * 2f + (18.0f * 4))),
                    _rand.Next((int)(worldPosition.Y * 2.7f), (int)(worldPosition.Y * 2.7f + (6.0f * 4)))
                    )));
            }

            _particleManager.Update();
            
                if (_input.GetKeyTrigger(Keys.Z))
                {
                    if (_rightHand == 1 && _leftHand == 0 && _tail == 1)
                        _saruKeys[0].IsOpen = true;
                    else if (_rightHand == 0 && _leftHand == 1 && _tail == 0)
                        _saruKeys[1].IsOpen = true;

                    isDead = true;
                    PlayerInfo.IsMove = true;
                }
        }

        public override void FrontDraw(Renderer renderer)
        {
            //renderer.DrawTexture("pc", worldPosition);
            
            if (_tail == 0)
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 1.7f, worldPosition.Y * 2.7f), new Rectangle(0, 36, 18, 6), new Vector2(4, 4));
            else
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 2f, worldPosition.Y * 2.7f), new Rectangle(0, 42, 18, 6), new Vector2(4, 4));

            renderer.DrawTexture(name, new Vector2(worldPosition.X * 2 - 82, worldPosition.Y * 2), new Vector2(4, 4), Color.White);
            renderer.DrawTexture("Saru_Setsumei", new Vector2(worldPosition.X * 2 - (89.5f * 2), Def.Screen.HeightHalf + 100), new Vector2(2, 2), Color.White);

            if (_rightHand == 0)
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 1.5f + 15.5f, worldPosition.Y * 2f - 8.1f), new Rectangle(9, 0, 9, 18), new Vector2(4, 4));
            else
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 1.5f + 15.5f, worldPosition.Y * 2.3f + 12f), new Rectangle(0, 18, 9, 18), new Vector2(4, 4));

            if (_leftHand == 0)
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 2.3f + 3.1f, worldPosition.Y * 2f - 8.1f), new Rectangle(0, 0, 9, 18), new Vector2(4, 4));
            else
                renderer.DrawTexture("Saru_Limbs", new Vector2(worldPosition.X * 2.3f + 3.1f, worldPosition.Y * 2.3f + 12f), new Rectangle(9, 18, 9, 18), new Vector2(4, 4));

            _particleManager.Draw(renderer);
        }

        private void MoveMonkey()
        {
            if(_input.GetKeyTrigger(Keys.Right) && _selectedLimbs != SelectedLimbs.LEFTHAND)
            {
                _previousSelectedLimbs = _selectedLimbs;
                _selectedLimbs = SelectedLimbs.LEFTHAND;
            } 
            else if(_input.GetKeyTrigger(Keys.Left) && _selectedLimbs != SelectedLimbs.RIGHTHAND)
            {
                _previousSelectedLimbs = _selectedLimbs;
                _selectedLimbs = SelectedLimbs.RIGHTHAND;
            }
            else if(_input.GetKeyTrigger(Keys.Down) && _selectedLimbs != SelectedLimbs.TAIL)
            {
                _previousSelectedLimbs = _selectedLimbs;
                _selectedLimbs = SelectedLimbs.TAIL;
            }
            else if(_input.GetKeyTrigger(Keys.Up) && _selectedLimbs == SelectedLimbs.TAIL)
            {
                SelectedLimbs x = _selectedLimbs;
                _selectedLimbs = _previousSelectedLimbs;
                _previousSelectedLimbs = x;
            }

            if (_input.GetKeyTrigger(Keys.X) && _selectedLimbs == SelectedLimbs.LEFTHAND)
            {
                _sound.PlaySE("metal");
                if (_leftHand == 0) _leftHand = 1;
                else _leftHand = 0;
            }
            else if(_input.GetKeyTrigger(Keys.X) && _selectedLimbs == SelectedLimbs.RIGHTHAND)
            {
                _sound.PlaySE("metal");
                if (_rightHand == 0) _rightHand = 1;
                else _rightHand = 0;
            }
            else if(_input.GetKeyTrigger(Keys.X) && _selectedLimbs == SelectedLimbs.TAIL)
            {
                _sound.PlaySE("metal");
                if (_tail == 0) _tail = 1;
                else _tail = 0;
            }
        }
    }
}
