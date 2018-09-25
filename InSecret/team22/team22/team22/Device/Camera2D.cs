using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Def;
using team22.Object;

namespace team22.Device
{
    class Camera2D
    {
        #region Fields
        public Vector2 m_position = Vector2.Zero;
        public Vector2 m_zoom = Vector2.One;
        public Rectangle m_visibleArea;
        public float m_rotation = 0.0f;

        public Vector2 m_screenPosition = Vector2.Zero;
        private Vector2 cameraMin = new Vector2(32 * 12, 32 * 7);
        private Vector2 cameraMax = new Vector2(32 * 55 - 32 * 11, 32 * 58 - 32 * 4);
        private Vector2 c_velo;
        private Vector2 targetPosi;
        private Vector2 fixedPosition;      //固定された視点

        private GameObject currentFocus;
        private GameObject newFocus;

        private bool isMoving;
        private bool isGoRight;
        private bool isGoUp;
        private bool isTeleportation;
        private bool[] newFocusLimitPosi = new bool[13];    //newFocusが画面端にいるかどうか

        private float speed;
        private float c_positionY;
        private float count = 0;

        private enum Direction
        { UpRight, UpLeft, DownRight, DownLeft, Left, Right, Up, Down, Object, Elevator, PlayerRight, PlayerLeft, NONE };
        private Direction direction;
        #endregion

        #region Get メソッド
        public bool IsMoving
        {
            get { return isMoving; }
        }
        public bool IsTeleportation
        {
            get { return isTeleportation; }
            set { isTeleportation = value; }
        }
        public Vector2 TargetPosition
        {
            get { return targetPosi; }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Camera2D()
        {
            m_visibleArea = new Rectangle(0, 0, Screen.Width, Screen.Height);
            m_position = new Vector2(Screen.Width / 2, Screen.Height / 2);
            Position = m_position;
            m_screenPosition = new Vector2(Screen.Width / 2, Screen.Height / 1.5f);
            c_velo = Vector2.Zero;
            targetPosi = Vector2.Zero;

            isMoving = false;
            isGoRight = false;
            isGoUp = false;
            isTeleportation = false;

            speed = 15;
            c_positionY = 0;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="obj"></param>
        internal void Initialize(GameObject obj)
        {
            currentFocus = obj;
            newFocus = obj;

            for (int i = 0; i <= 12; i++)
            {
                newFocusLimitPosi[i] = false;
            }
            newFocusLimitPosi[12] = true;

        }

        public Vector2 Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
                m_visibleArea.X = (int)(m_position.X - m_visibleArea.Width / 2);
                m_visibleArea.Y = (int)(m_position.Y - m_visibleArea.Height / 2);
            }
        }

        public void Move(float x, float y)
        {
            Position = new Vector2(m_position.X + x, m_position.Y + y);
        }

        public void Zoom(float z)
        {
            m_zoom = new Vector2(z, z);
        }

        public virtual Matrix GetMatrix()
        {
            Vector3 pos = new Vector3(m_position, 0);
            Vector3 screenPos = new Vector3(m_screenPosition, 0);

            return Matrix.CreateTranslation(-pos) *
                Matrix.CreateScale(m_zoom.X, m_zoom.Y, 1.0f) *
                Matrix.CreateRotationZ(m_rotation) *
                Matrix.CreateTranslation(screenPos);
        }

        public void Update()
        {
            if (currentFocus == null || newFocus == null) return;

            if (!isTeleportation)
            {
                if (currentFocus == newFocus)
                {
                    Vector2 currentFocusCen = new Vector2(currentFocus.GetRectangle().Center.X, currentFocus.GetRectangle().Center.Y);
                    Position = new Vector2(currentFocusCen.X, c_positionY);
                    currentFocus = newFocus;
                    isMoving = false;
                    count = 0;
                }
                else
                {
                    FocusChangeMove();
                }
                RangeLimit();
            }
            else
            {
                m_position = fixedPosition;
            }
        }

        internal void SetFocus(GameObject obj)
        {
            newFocus = obj;
        }

        internal void SetFocus(GameObject obj, float speed)
        {
            newFocus = obj;
            this.speed = speed;
        }

        //focusが変わったときのカメラの動き
        private void FocusChangeMove()
        {
            isMoving = true;
            DirectionCalculation();

            //移動方向を決めるための処理
            IsNewFocusLimitPosi();
            SetMoveCameraVelo();
            //移動処理
            c_velo.Normalize();

            Move(c_velo.X * speed, c_velo.Y * speed);


            if (newFocus is Player && currentFocus is Player)
            {
                switch (direction)
                {
                    case Direction.UpRight:
                        if (m_position.X >= targetPosi.X ||
                             m_position.Y <= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.UpLeft:
                        if (m_position.X <= targetPosi.X ||
                             m_position.Y <= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.DownRight:
                        if (m_position.X >= targetPosi.X ||
                             m_position.Y >= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.DownLeft:
                        if (m_position.X <= targetPosi.X ||
                             m_position.Y >= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.Up:
                        if (m_position.Y <= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.Down:
                        if (m_position.Y >= c_positionY)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.PlayerRight:
                        if (Position.X >= targetPosi.X || Position.X >= cameraMax.X)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.PlayerLeft:
                        if (Position.X <= targetPosi.X || Position.X <= cameraMin.X)
                        { currentFocus = newFocus; }
                        break;
                    case Direction.Right:
                        if (isGoUp)
                        {
                            //上に向かっている時
                            if (Position.X >= cameraMax.X ||
                                m_position.Y <= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        else
                        {
                            //下に向かっている時
                            if (Position.X >= cameraMax.X ||
                                m_position.Y >= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        break;
                    case Direction.Left:
                        if (isGoUp)
                        {
                            //上に向かっている時
                            if (Position.X <= cameraMin.X ||
                                m_position.Y <= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        else
                        {
                            //下に向かっている時
                            if (Position.X <= cameraMin.X ||
                                m_position.Y >= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        break;
                    case Direction.NONE:
                        if (isGoUp)
                        {
                            if (m_position.Y <= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        else
                        {
                            if (m_position.Y >= c_positionY)
                            { currentFocus = newFocus; }
                        }
                        //右に進むとき
                        if (isGoRight)
                        {
                            if (Position.X >= targetPosi.X || Position.X >= cameraMax.X)
                            { currentFocus = newFocus; }
                        }
                        //左に進むとき
                        else
                        {
                            if (Position.X <= targetPosi.X || Position.X <= cameraMin.X)
                            { currentFocus = newFocus; }
                        }
                        break;
                }
            }
            else
            {   //プレイヤー以外のオブジェクトがnewFocusの場合
                if (newFocusLimitPosi[(int)Direction.Elevator])
                {
                    if (isGoUp)
                    {
                        if (m_position.Y <= c_positionY)
                        { currentFocus = newFocus; }
                    }
                    else
                    {
                        if (m_position.Y >= c_positionY)
                        { currentFocus = newFocus; }
                    }

                }
                else
                {
                    //右に進むとき
                    if (isGoRight)
                    {
                        if (Position.X >= targetPosi.X || Position.X >= cameraMax.X)
                        { currentFocus = newFocus; }
                    }
                    //左に進むとき
                    else
                    {
                        if (Position.X <= targetPosi.X || Position.X <= cameraMin.X)
                        { currentFocus = newFocus; }
                    }
                }
            }
        }

        private void DirectionCalculation()
        {
            //右に進むのか計算
            if (newFocus.GetWorldPosition().X - currentFocus.GetWorldPosition().X > 0) isGoRight = true;
            else if (newFocus.GetWorldPosition().X - currentFocus.GetWorldPosition().X < 0) isGoRight = false;
            //上に進むのか計算
            if (newFocus.GetWorldPosition().Y - currentFocus.GetWorldPosition().Y < 0) isGoUp = true;
            else if (newFocus.GetWorldPosition().Y - currentFocus.GetWorldPosition().Y > 0) isGoUp = false;

            float distanceX = newFocus.GetWorldPosition().X - currentFocus.GetWorldPosition().X;
            float distanceY = newFocus.GetWorldPosition().Y - currentFocus.GetWorldPosition().Y;

            if (isGoUp && (distanceX < 32 && distanceX > -32))
            { direction = Direction.Up; }
            else if (!isGoUp && (distanceX < 32 && distanceX > -32))
            { direction = Direction.Down; }
            else if (isGoRight && (distanceY < 32 && distanceY > -32) && currentFocus is Player && newFocus is Player)
            { direction = Direction.PlayerRight; }
            else if (!isGoRight && (distanceY < 32 && distanceY > -32) && currentFocus is Player && newFocus is Player)
            { direction = Direction.PlayerLeft; }
            else if (isGoUp && isGoRight)       //右上
            { direction = Direction.UpRight; }
            else if (isGoUp && !isGoRight)      //左上
            { direction = Direction.UpLeft; }
            else if (!isGoUp && isGoRight)      //右下
            { direction = Direction.DownRight; }
            else if (!isGoUp && !isGoRight)     //左下
            { direction = Direction.DownLeft; }
        }

        public void SetPositionY(float y)
        { c_positionY = y; }

        private void RangeLimit()
        {   //最小の位置
            if (m_position.X <= cameraMin.X) m_position.X = cameraMin.X;
            if (m_position.Y <= cameraMin.Y) m_position.Y = cameraMin.Y;
            //最大の位置
            if (m_position.X >= cameraMax.X) m_position.X = cameraMax.X;
            if (m_position.Y >= cameraMax.Y) m_position.Y = cameraMax.Y;
        }

        /// <summary>
        /// newFocusが画面端にいるかどうか？
        /// </summary>
        private void IsNewFocusLimitPosi()
        {
            //newFocusとcurrentFocusのセンター
            Vector2 newFocusCen = new Vector2(newFocus.GetRectangle().Center.X, newFocus.GetRectangle().Center.Y);
            Vector2 currentFocusCen = new Vector2(currentFocus.GetRectangle().Center.X,
                currentFocus.GetRectangle().Center.Y);

            //右上の角にカメラがいるか？
            if (newFocusCen.Y <= cameraMin.Y + 32 && newFocusCen.X >= cameraMax.X)
            {
                newFocusLimitPosi[(int)Direction.UpRight] = true;
            }
            else
            { newFocusLimitPosi[(int)Direction.UpRight] = false; }
            //左上の角にカメラがいるか？
            if (newFocusCen.Y <= cameraMin.Y + 32 && newFocusCen.X <= cameraMin.X)
            { newFocusLimitPosi[(int)Direction.UpLeft] = true; }
            else
            { newFocusLimitPosi[(int)Direction.UpLeft] = false; }
            //右下の角にカメラが来ているか？
            if (newFocusCen.Y >= cameraMax.Y && newFocusCen.X >= cameraMax.X)
            { newFocusLimitPosi[(int)Direction.DownRight] = true; }
            else
            { newFocusLimitPosi[(int)Direction.DownRight] = false; }
            //左下の角にカメラが来ているか？
            if (newFocusCen.Y >= cameraMax.Y && newFocusCen.X <= cameraMin.X)
            { newFocusLimitPosi[(int)Direction.DownLeft] = true; }
            else
            { newFocusLimitPosi[(int)Direction.DownLeft] = false; }
            //エレベーターの移動の時
            if (newFocus is CameraFocusTarget)
            {
                newFocusLimitPosi[(int)Direction.Elevator] = true;
                newFocusLimitPosi[(int)Direction.Left] = false;
                newFocusLimitPosi[(int)Direction.Right] = false;
            }
            else
            {
                newFocusLimitPosi[(int)Direction.Elevator] = false;
                //右端にいるか？
                if (newFocusCen.X >= cameraMax.X && !(direction == Direction.PlayerRight))
                {
                    newFocusLimitPosi[(int)Direction.Right] = true;
                    direction = Direction.Right;
                    //今のfocusが右下端にいるか？
                    if (currentFocusCen.X >= cameraMax.X && isGoUp)
                    {
                        direction = Direction.Up;
                        newFocusLimitPosi[(int)Direction.Up] = true;
                        newFocusLimitPosi[(int)Direction.Right] = false;
                    }
                    //今のfocusが右上端にいるか？
                    if (currentFocusCen.X >= cameraMax.X && !isGoUp)
                    {
                        direction = Direction.Down;
                        newFocusLimitPosi[(int)Direction.Down] = true;
                        newFocusLimitPosi[(int)Direction.Right] = false;
                    }
                }
                else
                {
                    newFocusLimitPosi[(int)Direction.Right] = false;
                    //プレイヤー同士同じ階にいるときの左に行くとき
                    if (direction == Direction.PlayerRight)
                    {
                        newFocusLimitPosi[(int)Direction.PlayerRight] = true;
                        direction = Direction.PlayerRight;
                    }
                    else
                    { newFocusLimitPosi[(int)Direction.PlayerRight] = false; }
                }
                //左端にいるか？
                if (newFocusCen.X <= cameraMin.X && !(direction == Direction.PlayerLeft))
                {
                    newFocusLimitPosi[(int)Direction.Left] = true;
                    direction = Direction.Left;
                    //今のfocusが左下にいるか？
                    if (currentFocusCen.X <= cameraMin.X && isGoUp)
                    {
                        direction = Direction.Up;
                        newFocusLimitPosi[(int)Direction.Up] = true;
                        newFocusLimitPosi[(int)Direction.Left] = false;
                    }
                    else
                    { newFocusLimitPosi[(int)Direction.Up] = false; }
                    //今のfocusが左上にいるか？
                    if (currentFocusCen.X <= cameraMin.X && !isGoUp)
                    {
                        direction = Direction.Down;
                        newFocusLimitPosi[(int)Direction.Down] = true;
                        newFocusLimitPosi[(int)Direction.Left] = false;
                    }
                    else
                    { newFocusLimitPosi[(int)Direction.Down] = false; }
                }
                else
                {
                    newFocusLimitPosi[(int)Direction.Left] = false;
                    //プレイヤー同士同じ階にいるときの左に行くとき
                    if (direction == Direction.PlayerLeft)
                    {
                        newFocusLimitPosi[(int)Direction.PlayerLeft] = true;
                        direction = Direction.PlayerLeft;
                    }
                    else
                    { newFocusLimitPosi[(int)Direction.PlayerLeft] = false; }
                }
            }

            //player以外とのカメラ移動の時
            if ((!(newFocus is Player) || !(currentFocus is Player)) && !(newFocus is CameraFocusTarget))
            {
                for (int i = 0; i < newFocusLimitPosi.Count(); i++)
                {
                    if (newFocusLimitPosi[i]) newFocusLimitPosi[i] = false;
                }
                newFocusLimitPosi[(int)Direction.Object] = true;
            }
            else
            { newFocusLimitPosi[(int)Direction.Object] = false; }
            //どれにも当てはまらない場合
            int count = 0;
            for (int i = 0; i < newFocusLimitPosi.Count() - 1; i++)
            {
                if (newFocusLimitPosi[i]) count++;
            }
            if (!newFocusLimitPosi.Contains(true) || newFocusLimitPosi[(int)Direction.NONE] && count == 0)
            {
                newFocusLimitPosi[(int)Direction.NONE] = true;
                direction = Direction.NONE;
            }
            else
            { newFocusLimitPosi[(int)Direction.NONE] = false; }
        }

        /// <summary>
        /// カメラの進む方向を設定
        /// </summary>
        private void SetMoveCameraVelo()
        {
            //newFocusが右上の角にいたら
            if (newFocusLimitPosi[(int)Direction.UpRight])
            {
                c_velo = new Vector2(cameraMax.X, cameraMin.Y) -
                    Position;
                targetPosi = new Vector2(cameraMax.X, cameraMin.Y);
            }
            //左上にいたら
            else if (newFocusLimitPosi[(int)Direction.UpLeft])
            {
                c_velo = new Vector2(cameraMin.X, cameraMin.Y) -
                   Position;
                targetPosi = new Vector2(cameraMin.X, cameraMin.Y);
            }
            //右下にいたら
            else if (newFocusLimitPosi[(int)Direction.DownRight])
            {
                c_velo = new Vector2(cameraMax.X, cameraMax.Y) -
                    Position;
                targetPosi = new Vector2(cameraMax.X, cameraMax.Y);
            }
            //左下にいたら
            else if (newFocusLimitPosi[(int)Direction.DownLeft])
            {
                c_velo = new Vector2(cameraMin.X, cameraMax.Y) -
                    Position;
                targetPosi = new Vector2(cameraMin.X, cameraMax.Y);
            }//右端にいたら
            else if (newFocusLimitPosi[(int)Direction.Right])
            {
                c_velo = new Vector2(cameraMax.X, newFocus.GetRectangle().Center.Y) -
                    Position;
                targetPosi = new Vector2(cameraMax.X, newFocus.GetRectangle().Center.Y);
            }//左端にいたら
            else if (newFocusLimitPosi[(int)Direction.Left])
            {
                c_velo = new Vector2(cameraMin.X, newFocus.GetRectangle().Center.Y) -
                    Position;
                targetPosi = new Vector2(cameraMin.X, newFocus.GetRectangle().Center.Y);
            }//エレベーターの移動の時
            else if (newFocusLimitPosi[(int)Direction.Elevator])
            {
                c_velo = new Vector2(Position.X, c_positionY) - Position;
                targetPosi = new Vector2(Position.X, c_positionY);
            }
            //player以外とのカメラ移動の時
            else if (newFocusLimitPosi[(int)Direction.Object])
            {
                c_velo = new Vector2(newFocus.GetRectangle().Center.X, c_positionY) -
                    Position;
                targetPosi = new Vector2(newFocus.GetRectangle().Center.X, c_positionY);
            }
            //player同士が同じ階にいるときに右に行く時
            else if (newFocusLimitPosi[(int)Direction.PlayerRight])
            {
                c_velo = new Vector2(newFocus.GetRectangle().Center.X - Position.X, 0);
                targetPosi = new Vector2(newFocus.GetRectangle().Center.X, c_positionY);
            }
            //player同士が同じ階にいるときに左に行く時
            else if (newFocusLimitPosi[(int)Direction.PlayerLeft])
            {
                c_velo = new Vector2(newFocus.GetRectangle().Center.X - Position.X, 0);
                targetPosi = new Vector2(newFocus.GetRectangle().Center.X, c_positionY);
            }
            //上下に行くとき
            else if (newFocusLimitPosi[(int)Direction.Up] || newFocusLimitPosi[(int)Direction.Down])
            {
                c_velo = new Vector2(Position.X, c_positionY) - Position;
                targetPosi = new Vector2(Position.X, c_positionY);
            }
            //何処の角にもいなかったら
            else if (newFocusLimitPosi[(int)Direction.NONE])
            {
                c_velo = new Vector2(newFocus.GetRectangle().Center.X, newFocus.GetRectangle().Center.Y) -
                                Position;
                targetPosi = new Vector2(newFocus.GetRectangle().Center.X, newFocus.GetRectangle().Center.Y);
            }
        }

        /// <summary>
        /// 固定カメラに行くときの設定
        /// </summary>
        /// <param name="position"></param>
        /// <param name="teleportation"></param>
        public void SetFixedPosition(Vector2 position, bool teleportation)
        {
            fixedPosition = position;
            isTeleportation = teleportation;
        }
    }
}
