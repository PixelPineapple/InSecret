using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object.Gimmick.GuardRoomGimmick
{
    class HackingCom : GameObject
    {
        #region Fields
        private InputState _input;
        private bool _isHacked = false;
        private Virus _virus;
        private List<GameObject> gameObjects;
        private GuardComputer _guardCom;
        #endregion

        #region Getter Setter メソッド

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="floorWidth"></param>
        /// <param name="floorHeight"></param>
        /// <param name="gameDevice"></param>
        public HackingCom (int floorWidth, int floorHeight, GameDevice gameDevice, GuardComputer guardCom)
        : base ("Hacking_Terminal", GimmickFront.frontDisplayPosi, floorWidth, floorHeight, GimmickFront.frontY, GimmickFront.frontX, gameDevice)
        {
            if (!_isHacked)
            {
                _isHacked = false;
            }

            _guardCom = guardCom;

            // Virus を作る。
            _virus = new Virus(new Vector2(GimmickFront.frontDisplayPosi.X + 47, 48 * 7), 45, 42, gameDevice);

            #region Virus 以外のオブジェクト を作る。
            gameObjects = new List<GameObject>()
            {
                new Firewall(new Vector2(GimmickFront.frontX + 48 * (-2), GimmickFront.frontDisplayPosi.Y),
                GimmickFront.frontY + GimmickFront.frontDisplayPosi.Y,
                92,
                13,
                2f,
                gameDevice),
                new Firewall(new Vector2(GimmickFront.frontX + 48 * (-1), GimmickFront.frontDisplayPosi.Y),
                GimmickFront.frontY + GimmickFront.frontDisplayPosi.Y,
                92,
                13,
                4f,
                gameDevice),
                new Firewall(new Vector2(GimmickFront.frontX + 48 * 0, GimmickFront.frontDisplayPosi.Y),
                GimmickFront.frontDisplayPosi.Y * 2.5f,
                92,
                13,
                1.5f,
                gameDevice),
                new Firewall(new Vector2(GimmickFront.frontX + 48 * 0, GimmickFront.frontDisplayPosi.Y * 2.5f),
                GimmickFront.frontY + GimmickFront.frontDisplayPosi.Y,
                92,
                13,
                1.5f,
                gameDevice),
                new Firewall(new Vector2(GimmickFront.frontX + 48 * 1, GimmickFront.frontDisplayPosi.Y),
                GimmickFront.frontY + GimmickFront.frontDisplayPosi.Y,
                92,
                13,
                2.5f,
                gameDevice),
                new Firewall(new Vector2(GimmickFront.frontX + 48 * 2, GimmickFront.frontDisplayPosi.Y + 48),
                GimmickFront.frontY + GimmickFront.frontDisplayPosi.Y,
                92,
                13,
                3f,
                gameDevice),
                new BlueFile(new Vector2(GimmickFront.frontX + 48 * 3, GimmickFront.frontDisplayPosi.Y + 48 * 4),
                38,
                40,
                gameDevice),
            };
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            _virus.Update(gameTime);
            _virus.ClampingPosition(GimmickFront.frontDisplayPosi + new Vector2(_virus.GetPicWidth(), _virus.GetPicHeight()), 
                new Vector2(
                    GimmickFront.frontDisplayPosi.X + GimmickFront.frontX - _virus.GetPicWidth() * 2, 
                    GimmickFront.frontDisplayPosi.Y + GimmickFront.frontY - _virus.GetPicHeight() * 2
                    ));
           
            foreach (var o in gameObjects)
            {
                o.Update(gameTime);
                if (o.Collision(_virus))
                {
                    _virus.Hit(o);
                }
                if(o is BlueFile && o.IsDead())
                {
                    _isHacked = true;
                    isDead = true;
                    PlayerInfo.IsMove = true;
                    _guardCom.IsHacked = true;
                }
            }

            if (_virus.IsDead())
            {
                isDead = true;
                PlayerInfo.IsMove = true;
            }
        }

        public override void FrontDraw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition);
            _virus.FrontDraw(renderer);
            foreach(var o in gameObjects)
            {
                o.FrontDraw(renderer);
            }
            renderer.DrawTexture("Computer_Tutorial", new Vector2(worldPosition.X * 2 - (89.5f * 2), Def.Screen.HeightHalf + 150), new Vector2(2, 2), Color.White);
        }

        #region 使わないメソッド
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {

        }
        #endregion
    }
}
