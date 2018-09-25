using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class Light : GameObject
    {
        #region Fields
        public static Vector2 playerPos;
        private bool flag;
        public GameObject _backgroundLight;

        private GuardMan guardMan;
        private bool lightMovement;
        private CountDownTimer movementTimer;
        #endregion

        #region Getter Setter メソッド
        public GameObject BackgroundLight
        {
            get { return _backgroundLight; }
        }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition">位置</param>
        /// <param name="gameDevice">ゲームデバイス</param>
        public Light(Vector2 position, int floorWidth, int floorHeight, GameDevice gameDevice, GuardMan guardMan)
            : base("light2", position, floorWidth, floorHeight, 80, 126, gameDevice)
        {
            this.guardMan = guardMan;
            position = new Vector2(guardMan.GetWorldPosition().X - width, guardMan.GetWorldPosition().Y - (height / 2) + 27);
            flag = false;

            _backgroundLight = new BackGroundLight(name, worldPosition, floorWidth, floorHeight, height, width, gameDevice);
            movementTimer = new CountDownTimer(0.3f);
            lightMovement = true;
        }
        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Light(Light other) : this(other.worldPosition, (int)other.drawingPosition.X, (int)other.drawingPosition.Y, other.gameDevice, other.guardMan)
        {

        }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Light(this);
        }
        /// <summary>
        /// 衝突
        /// </summary>
        /// <param name="gameObject"></param>
        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && PlayerInfo.IsVisible)
            {
                flag = true;
                playerPos = gameObject.GetWorldPosition();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public override void Update(GameTime gameTime)
        {
            if (PlayerInfo.IsMove || !PlayerInfo.IsVisible)
            {
                if (!guardMan.guardDirection)
                {
                    worldPosition = new Vector2(guardMan.GetWorldPosition().X + guardMan.GetPicWidth(),
                        guardMan.GetWorldPosition().Y - (height / 2) + 27);
                    drawingPosition = new Vector2(guardMan.GetDrawingPosition().X + guardMan.GetPicWidth(),
                        guardMan.GetDrawingPosition().Y - (height / 2) + 27);
                    name = "light2";

                    _backgroundLight.SetWorldPosition(worldPosition);
                    _backgroundLight.SetDrawingPosition(drawingPosition);
                    _backgroundLight.Name = name + "_mask";
                }
                else if (guardMan.guardDirection)
                {
                    worldPosition = new Vector2(guardMan.GetWorldPosition().X - width, guardMan.GetWorldPosition().Y - (height / 2) + 27);
                    drawingPosition = new Vector2(guardMan.GetDrawingPosition().X - width, guardMan.GetDrawingPosition().Y - (height / 2) + 27);
                    name = "light1";

                    _backgroundLight.SetWorldPosition(worldPosition);
                    _backgroundLight.SetDrawingPosition(drawingPosition);
                    _backgroundLight.Name = name + "_mask";
                }
            }
            movementTimer.Update();
            if (movementTimer.IsTime())
            {
                lightMovement = !lightMovement;
                movementTimer.Initialize();
            }
        }


        public Vector2 PlayerPos()
        {
            return playerPos;
        }

        public bool LightFlag()
        {
            return flag;
        }

        public void FalseFlag()
        {
            flag = false;
        }

        public override void Draw(Renderer renderer)
        {
            if (guardMan is SuperGuardMan)
            {
                if (lightMovement)
                    renderer.DrawTexture(name, worldPosition);
                else
                    renderer.DrawTexture(name, worldPosition + new Vector2(0, 1));
            }
            else
            {
                if (lightMovement)
                    renderer.DrawTexture(name, drawingPosition);
                else
                    renderer.DrawTexture(name, drawingPosition + new Vector2(0, 1));
            }
        }
    }
}
