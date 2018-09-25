using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class CameraMoveEvent : GameObject
    {
        #region Fields
        private Camera2D camera;
        private CountDownTimer timer;
        private GameObject focus;
        private GameObject player;
        private InputState inputState;
        private bool countDown;
        private bool isHit;
        private int levelCounter;
        private float cameraSpeed;
        #endregion

        #region Getter メソッド

        #endregion

        public CameraMoveEvent(Vector2 position, GameDevice gameDevice, GameObject focus)
            : base("block2", position, 32, 64, gameDevice)
        {
            inputState = gameDevice.InputState;
            this.gameDevice = gameDevice;
            camera = gameDevice.Camera;
            timer = new CountDownTimer(3);
            this.focus = focus;
            countDown = false;
            isHit = false;
            isDead = false;
            //レベル1
            levelCounter = 1;
            cameraSpeed = 10;
        }

        public CameraMoveEvent(CameraMoveEvent other)
            : this(other.worldPosition,other.gameDevice, other.focus)
        { }

        public override object Clone()
        {
            return new CameraMoveEvent(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && !isHit && levelCounter < 2)
            {
                player = gameObject;
                camera.SetFocus(focus, cameraSpeed);
                isHit = true;
                //レベル2
                levelCounter++;
                PlayerInfo.IsMove = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (levelCounter >= 4 && !camera.IsMoving)
            {
                isDead = true;
                PlayerInfo.IsMove = true;
            }

            //プレイヤーと衝突している＆カメラが止まっているとき
            if (isHit && !camera.IsMoving && levelCounter < 3 ) // && inputState.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.X))
            {
                //時間を計る
                countDown = true;
                //レベル3
                levelCounter++;
            }

            if (countDown)
            { timer.Update(); }

            // Console.WriteLine("countDown = {0}", countDown);

            if (timer.IsTime() && player != null)
            {
                camera.SetFocus(player, cameraSpeed);
                timer.Initialize();
                countDown = false;
                //レベル4
                levelCounter++;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition, 0);
        }
    }
}
