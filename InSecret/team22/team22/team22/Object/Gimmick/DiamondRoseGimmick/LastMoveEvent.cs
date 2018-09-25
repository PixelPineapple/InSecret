using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class LastMoveEvent : GameObject
    {
        private InputState input;
        private Camera2D camera;
        private Vector2 previousPosition;

        private CountDownTimer intervalTimer;

        private bool isActive;

        private int targetNum;      //今何番目のtargetなのか？
        private int actionLevel;    //行動制限用のナンバー

        private Vector2[] targets = new Vector2[3];   //これからカメラ移動する

        private bool isEventStart;  //イベントを始めるのか？
        private bool isMovingCamera;    //カメラが動いた後か？

        public LastMoveEvent(Vector2 position, int floorWidth, int floorHeight, GameDevice gameDevice)
            : base("block2", position, floorWidth, floorHeight, 32, 32, gameDevice)
        {
            input = gameDevice.InputState;
            camera = gameDevice.Camera;

            intervalTimer = new CountDownTimer(2.0f);
            targetNum = 1;
            actionLevel = 1;

            targets[0] = new Vector2(32 * 20, 32 * 32);
            targets[1] = new Vector2(32 * 20, 32 * 21);
            targets[2] = new Vector2(32 * 20, 32 * 10);

            isMovingCamera = false;
            PlayerInfo.IsVisible = false;
            Initialize();
        }

        public LastMoveEvent(LastMoveEvent other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X / 32, (int)other.GetDrawingPosition().Y / 32, other.gameDevice)
        { }

        public override object Clone()
        {
            return new LastMoveEvent(this);
        }

        public void Initialize()
        {
            GimmickInfo.eventFlag = true;
            //元のカメラの位置を記憶しておく
            previousPosition = camera.Position;
            //カメラの位置を変更する
            camera.SetFixedPosition(targets[0], true);
            //camera.SetFocus(targets[0]);
            isEventStart = true;
        }

        public override void Hit(GameObject gameObject)
        {
            // 必要ない
            //if (gameObject is Player && !isEventStart &&
            //    input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A))
            //{
            //    Lock.eventFlag = true;
            //    //元のカメラの位置を記憶しておく
            //    previousPosition = camera.Position;
            //    //カメラの位置を変更する
            //    camera.SetFixedPosition(targets[0], true);
            //    //camera.SetFocus(targets[0]);
            //    isEventStart = true;
            //}
        }

        public override void Update(GameTime gameTime)
        {
            Interval();

            //Console.WriteLine("カメラの位置 {0}", camera.Position);

            //死んだら元に戻す
            if (isDead)
            {
                camera.SetFixedPosition(previousPosition, true);
            }
        }

        private void Interval()
        {
            if (!isEventStart) return;
            FloorNumInfo.IsDraw = false;

            if (targetNum < 4) intervalTimer.Update();

            if (intervalTimer.IsTime())
            {

                if (targetNum == 3)
                {
                    PlayerInfo.IsVisible = true;
                    isDead = true;
                    targetNum++;
                    FloorNumInfo.IsDraw = true;
                    return;
                }
                //カメラを動かす
                camera.SetFixedPosition(targets[targetNum], true);
                //targetNumを次へ変更
                targetNum++;
                intervalTimer.Initialize();
                actionLevel++;
            }
        }
    }
}
