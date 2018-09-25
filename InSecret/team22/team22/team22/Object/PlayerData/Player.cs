using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;
using team22.Def;
using team22.Device.EventDialogue;

namespace team22.Object
{
    class Player : GameObject
    {
        #region Fields
        private InputState input;   // 入力処理オブジェクト
        private Direction dir;
        private CountDownTimer timer;

        private Conditions.CharacterMovements charMovement;
        private Conditions.FaceDirection faceDirection;

        private Vector2 velocity;   //移動量
        private bool isJump;        //ジャンプの状態管理
        private bool isDash;        //走っているのか？
        private bool isPad;
        private Sound sound;
        private CountDownTimer walkTimer;
        private bool shouldBeDrawn; // 描画されるのか？

        private Motion motion;

        private int pushCnt;

        private Buttons currentButton;
        private Keys currentKey;
        #endregion


        #region GetterSetter メソッド
        public bool ShouldBeDrawn
        {
            get { return shouldBeDrawn; }
            set { shouldBeDrawn = value;}
        }

        public Motion GetMotion()
        {
            return motion;
        }

        public Conditions.FaceDirection FaceDirection
        {
            get { return faceDirection; }
            set { faceDirection = value; }
        }
        #endregion
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="gameDevice"></param>
        public Player(string name, Vector2 worldPosition, int floorWidth, int floorHeight, int charHeight, int charWidth, GameDevice gameDevice)
            : base(name, worldPosition, floorWidth, floorHeight, charHeight, charWidth, gameDevice)
        {
            this.input = gameDevice.InputState;
            velocity = Vector2.Zero;
            timer = new CountDownTimer(0.5f);
            walkTimer = new CountDownTimer(0.5f);
            isJump = false;
            isDash = false;
            isPad = false;
            pushCnt = 0;

            motion = new Motion();
            int counter = 0;
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    motion.Add(counter, new Rectangle(charWidth * x, charHeight * y, charWidth, charHeight));
                    counter++;
                }
            }
            motion.Initialize(new Range(1, 5), new CountDownTimer(0.1f));
            charMovement = Conditions.CharacterMovements.Idle;
            faceDirection = Conditions.FaceDirection.Right;

            sound = gameDevice.Sound;
            shouldBeDrawn = false;
        }

        public Player(Player other)
            : this(other.name, other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.height, other.width, other.gameDevice)
        { }

        public override object Clone()
        {
            return new Player(this);
        }

        /// <summary>
        /// 衝突
        /// </summary>
        /// <param name="gameObject">確認したいオブジェクト</param>
        public override void Hit(GameObject gameObject)
        {
            //どの向きで当たっているか取得
            dir = this.CheckDirection(gameObject);

            //ブロックと当たっているとき
            if (gameObject is Block || (gameObject is TransparentBlock && !((TransparentBlock)gameObject).Switch))
            {
                if (dir == Direction.Top)//上
                {
                    //プレイヤーがブロックに乗った
                    if (worldPosition.Y > 0.0f)// 降下中の時、ジャンプ状態終了
                    {
                        worldPosition.Y = gameObject.GetRectangle().Top - height;
                        velocity.Y = 0.0f;
                        isJump = false;
                    }
                }
                else if (dir == Direction.Right) //右
                {
                    worldPosition.X = gameObject.GetRectangle().Right;
                    //drawingPosition.X = gameObject.GetDrawingPosition().X;
                    //drawingPosition.X = worldPosition.X - (floorWidth * 32);
                }
                else if (dir == Direction.Left) //左
                {
                    worldPosition.X = gameObject.GetRectangle().Left - this.width;
                    //drawingPosition.X = drawingPosition.X;
                    //drawingPosition.X = worldPosition.X - (floorWidth * 32) - width;
                }
                else if (dir == Direction.Botton) //下
                {
                    worldPosition.Y = gameObject.GetRectangle().Bottom;
                }
            }

            if (gameObject is TransparentBlock && !((TransparentBlock)gameObject).Switch)
            {
                if (dir == Direction.Top)//上
                {
                    //プレイヤーがブロックに乗った
                    if (worldPosition.Y > 0.0f)// 降下中の時、ジャンプ状態終了
                    {
                        worldPosition.Y = gameObject.GetRectangle().Top - height;
                        velocity.Y = 0.0f;
                        isJump = false;
                    }
                }
                else if (dir == Direction.Right) //右
                {
                    worldPosition.X = gameObject.GetRectangle().Right;
                    //drawingPosition.X = gameObject.GetDrawingPosition().X;
                    drawingPosition.X = worldPosition.X - (floorWidth * 32);
                }
                else if (dir == Direction.Left) //左
                {
                    worldPosition.X = gameObject.GetRectangle().Left - this.width;
                    //drawingPosition.X = drawingPosition.X;
                    drawingPosition.X = worldPosition.X - (floorWidth * 32) - width;
                }
                else if (dir == Direction.Botton) //下
                {
                    worldPosition.Y = gameObject.GetRectangle().Bottom;
                }
            }
        }

        public void UpdateMotion(Vector2 velocity)
        {
            if (faceDirection == Conditions.FaceDirection.Right && PlayerInfo.IsMove)
            {
                if (velocity.Length() == 0 && charMovement != Conditions.CharacterMovements.Idle)
                {
                    motion.Initialize(new Range(1, 5), new CountDownTimer(0.1f));
                    charMovement = Conditions.CharacterMovements.Idle;
                }
                else if (velocity.Length() != 0 && charMovement != Conditions.CharacterMovements.Walk)
                {
                    motion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                    charMovement = Conditions.CharacterMovements.Walk;
                }
            }
            else if (faceDirection == Conditions.FaceDirection.Left && PlayerInfo.IsMove)
            {
                if (velocity.Length() == 0 && charMovement != Conditions.CharacterMovements.Idle)
                {
                    motion.Initialize(new Range(13, 17), new CountDownTimer(0.1f));
                    charMovement = Conditions.CharacterMovements.Idle;
                }
                else if (velocity.Length() != 0 && charMovement != Conditions.CharacterMovements.Walk)
                {
                    motion.Initialize(new Range(18, 23), new CountDownTimer(0.1f));
                    charMovement = Conditions.CharacterMovements.Walk;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Jump();
            Dash();

            //左右移動処理
            float speed = 2f; //通常時の速さ
            if (isDash)
            {
                //ダッシュ時の速さ
                speed = 3f;
            }
            if (velocity.X < 0) faceDirection = Conditions.FaceDirection.Left;
            else if (velocity.X > 0) faceDirection = Conditions.FaceDirection.Right;

            UpdateMotion(velocity);

            //左右の移動量計算
            if (!PlayerInfo.AutoMove)
            velocity.X = input.Velocity().X * speed;// 左右だけ
            
            //位置の計算
            worldPosition = worldPosition + velocity * speed;
            drawingPosition = drawingPosition + velocity * speed;

            motion.Update(gameTime);

            walkTimer.Update();
            if (walkTimer.IsTime() && charMovement == Conditions.CharacterMovements.Walk && !isJump)
            {
                sound.PlaySE("man_walk");
                walkTimer.Initialize();
            }
        }

        private void Jump()
        {
            if (drawingPosition.X <= 0 || drawingPosition.X >= PlayerInfo.RightWall - width)
            {
                if (drawingPosition.Y < 32 * 6)
                { velocity.Y += 0.5f; }
            }
            else if (drawingPosition.Y < 32 * 6 && !isJump)
            { velocity.Y += 2.5f; }
            //ジャンプ処理（上下の移動量の計算）
            //ジャンプしてないときに、Bボタンまたはスペースキーが押されたらジャンプ開始
            else if ((isJump == false) &&
                (input.GetKeyTrigger(Buttons.X) ||
                input.GetKeyTrigger(Keys.Space)))
            {
                velocity.Y = -4.0f; //移動量を上に
                isJump = true; //ジャンプ中に
            }
            else if (isJump)
            {
                //ジャンプ中だけ降下
                velocity.Y = velocity.Y + 0.2f; //ちょっとずつ下へ
                //落下速度制限（画像大きさの半分を超えて落下させない）
                velocity.Y = (velocity.Y > 16f) ? (16.0f) : (velocity.Y);
            }
        }

        /// <summary>
        /// ジャンプするか？
        /// </summary>
        /// <returns></returns>
        public bool IsJump()
        {
            return isJump;
        }

        public override void Draw(Renderer renderer)
        {
            float alpha;
            //if (PlayerInfo.IsVisible)
            if (PlayerInfo.IsVisible)
            { alpha = 1; }
            else
            { alpha = 0; }
            
            renderer.DrawTexture(name, drawingPosition, motion.DrawingRange(), alpha);
        }

        /// <summary>
        /// 走るのかどうか？
        /// </summary>
        public void Dash()
        {
            //パッド右の入力確認
            if (input.GetKeyTrigger(Buttons.DPadRight))
            {
                pushCnt++;
                currentButton = Buttons.DPadRight;
                isPad = true;
            }//パッド左の入力確認
            else if (input.GetKeyTrigger(Buttons.DPadLeft))
            {
                pushCnt++;
                currentButton = Buttons.DPadLeft;
                isPad = true;
            }//キーボード右の入力確認
            else if (input.GetKeyTrigger(Keys.Right))
            {
                pushCnt++;
                currentKey = Keys.Right;
                isPad = false;
            }//キーボード左の入力確認
            else if (input.GetKeyTrigger(Keys.Left))
            {
                pushCnt++;
                currentKey = Keys.Left;
                isPad = false;
            }
            // ゲームパッド用
            if (isPad)
            {
                //走っていてパッドの入力がないとき、入力回数が3の時、入力回数が0のとき
                if ((isDash && !input.GetKeyState(currentButton)) || pushCnt >= 3 || pushCnt <= 0)
                {
                    //初期化
                    timer.Initialize();
                    pushCnt = 0;
                    isDash = false;
                }
                //入力回数が0より多く、2より少ない時
                else if (pushCnt > 0 && pushCnt <= 2)
                {
                    if (!isDash) timer.Update();    //走ってなければ時間の更新
                    if (!timer.IsTime() && pushCnt >= 2) isDash = true; //時間が0ではなくて、入力回数が2になったら走る
                    else if (timer.IsTime())    //時間が0になったら
                    {
                        //初期化
                        timer.Initialize();
                        pushCnt = 0;
                    }
                }
            }
            else
            {   //キーボード用
                //走っていてパッドの入力がないとき、入力回数が3の時、入力回数が0のとき
                if ((isDash && !input.GetKeyState(currentKey)) || pushCnt >= 3 || pushCnt <= 0)
                {
                    //初期化
                    timer.Initialize();
                    pushCnt = 0;
                    isDash = false;
                }
                //入力回数が0より多く、2より少ない時
                else if (pushCnt > 0 && pushCnt <= 2)
                {
                    if (!isDash) timer.Update();    //走ってなければ時間の更新
                    if (!timer.IsTime() && pushCnt >= 2) isDash = true; //時間が0ではなくて、入力回数が2になったら走る
                    else if (timer.IsTime())    //時間が0になったら

                    {
                        //初期化
                        timer.Initialize();
                        pushCnt = 0;
                    }
                }
            }
        } 
    }
}
