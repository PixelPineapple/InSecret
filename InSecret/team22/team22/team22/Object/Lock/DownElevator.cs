using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class DownElevator : GameObject
    {
        #region Files
        private InputState input;
        private Camera2D camera;
        private Motion openMotion;
        private Motion closeMotion;
        private Player player;
        private GameObject key;
        private GameObject target;
        private CountDownTimer downTimer;
        private Sound sound;

        private int actionLevelCounter;
        private string playerName;      //エレベーターの中に表示するプレイヤーの名前
        private Rectangle playerRect;   //　　　　　　　　"　"　　　　　　　　の矩形情報　　　　　
        private int motionNum;

        private bool isMovePlayer;       //プレイヤーを移動させるか？
        private bool isPlayerHit;       //プレイヤーとあたっているのか？
        private bool isOpen;            //開くのか？
        private bool isClose;           //閉じるのか？
        private bool isMoveCamera;      //カメラは動くのか？
        private bool isLock;            //鍵がかかっているのか？
        #endregion

        public int GetMotionNum
        {
            get { return motionNum; }
        }

        public DownElevator(Vector2 worldPosition, int floorWidth, int floorHeight, GameDevice gameDevice, GameObject target)
            : base("Elevator_Open", worldPosition, floorWidth, floorHeight, 118, 107, gameDevice)
        {
            input = gameDevice.InputState;
            camera = gameDevice.Camera;
            sound = gameDevice.Sound;
            //開く時のモーション
            openMotion = new Motion();
            for (int i = 0; i <= 3; i++)
            { openMotion.Add(i, new Rectangle(107 * i, 0, 107, 118)); }
            openMotion.Initialize(new Range(0, 3), new CountDownTimer(0.5f));
            //閉まる時のモーション
            closeMotion = new Motion();
            for (int i = 0; i <= 3; i++)
            { closeMotion.Add(i, new Rectangle(107 * i, 0, 107, 118)); }
            closeMotion.Initialize(new Range(0, 3), new CountDownTimer(0.5f));

            downTimer = new CountDownTimer(0.8f);
            this.target = target;

            actionLevelCounter = 1; //行動レベル1にする
            motionNum = 0;

            isPlayerHit = false;
            isOpen = false;
            isClose = false;
            isMoveCamera = false;
            isLock = false;
        }

        private void Initialize()
        {
            openMotion.Initialize(new Range(0, 3), new CountDownTimer(0.5f));
            closeMotion.Initialize(new Range(0, 3), new CountDownTimer(0.5f));
            downTimer.Initialize();

            actionLevelCounter = 1;
            isMovePlayer = false;
            playerName = "Man_Model";
            playerRect = new Rectangle(0, 0, 32, 76);
        }

        public DownElevator(DownElevator other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice, other.target)
        { }

        public override object Clone()
        {
            return new DownElevator(this);
        }

        public override void Hit(GameObject gameObject)
        {
            //鍵と当たっているか？
            if (gameObject is Key && key == null)
            { key = gameObject; }

            if (key != null && !key.IsDead())
            { isLock = true; }
            if (key != null && key.IsDead())
            { isLock = false; }

            //プレイヤーと当たっているか？
            if (gameObject is Player && !isPlayerHit && actionLevelCounter < 2
                && (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)))
            {
                sound.PlaySE("autodoor");
                if (isLock) return;
                player = (Player)gameObject;
                if (player.IsJump()) return;
                isOpen = true;
                //行動レベルを2にする
                actionLevelCounter++;
                PlayerInfo.IsMove = false;
                PlayerInfo.InElevator = true;
            }
            else
            { isPlayerHit = false; }
        }

        public override void Update(GameTime gameTime)
        {
            OpenUpdate(gameTime);
            CloseUpdate(gameTime);
            MoveCamera();
            if (isMovePlayer && player != null)
            {
                Initialize();
                player.SetWorldPosition(new Vector2(GetRectangle().Center.X - player.GetRectangle().Width / 2, player.GetWorldPosition().Y + 32 * 12));
                player.SetDrawingPosition(new Vector2((GetRectangle().Center.X - player.GetRectangle().Width / 2) - 32, player.GetDrawingPosition().Y));
                camera.SetFocus(player);
                PlayerInfo.IsMove = true;
                PlayerInfo.IsVisible = true;
            }

            if (PlayerInfo.IsMan)
            {
                playerName = "Man_Model";
                playerRect = new Rectangle(0, 0, 32, 76);
            }
            else
            {
                playerName = "Woman_Model";
                playerRect = new Rectangle(0, 0, 25, 69);
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition, openMotion.DrawingRange());
            if (isClose)
            {
                renderer.DrawTexture(playerName, new Vector2(drawingPosition.X + 32 + 3, drawingPosition.Y + 24), playerRect);
                renderer.DrawTexture("Elevator_Close", drawingPosition, closeMotion.DrawingRange());
            }
        }

        private void OpenUpdate(GameTime gameTime)
        {
            if (!isOpen) return;
            openMotion.Update(gameTime);
            motionNum = openMotion.MotionNum;

            if (openMotion.MotionNum == 3)
            {
                openMotion.MotionNum = 3;
                isOpen = false;
                isClose = true;
            }
        }

        private void CloseUpdate(GameTime gameTime)
        {
            if (!isClose) return;
            PlayerInfo.IsVisible = false;

            downTimer.Update();
            if (downTimer.IsTime())
            {
                closeMotion.Update(gameTime);

                if (closeMotion.MotionNum == 0) motionNum = 4;
                else if (closeMotion.MotionNum == 1) motionNum = 5;
                else if (closeMotion.MotionNum == 2) motionNum = 6;
                else if (closeMotion.MotionNum == 3) motionNum = 7;

                if (closeMotion.MotionNum == 3)
                {
                    closeMotion.MotionNum = 3;
                    isClose = false;
                    isMoveCamera = true;

                    openMotion.MotionNum = 0;
                    closeMotion.MotionNum = 0;
                }
            }
        }

        private void MoveCamera()
        {
            if (!isMoveCamera) return;
            PlayerInfo.InElevator = false;

            if (camera.Position.Y >= GetWorldPosition().Y + 32 * 14.5f)
            {
                isMovePlayer = true;
                isMoveCamera = false;
            }
            camera.SetPositionY(GetWorldPosition().Y + 32 * 14.5f);
            camera.SetFocus(target, 3);
        }
    }
}
