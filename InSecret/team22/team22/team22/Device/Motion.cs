using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace team22.Device
{
    class Motion
    {
        private Range range;                        //範囲
        private CountDownTimer countDowntimer;      //モーション時間
        private int motionNumber;                   //モーション番号
        private bool isDead;                        //削除フラグ
        private bool isLoopingAction;
        private bool hasInterval;
        private CountDownTimer _interval;

        public int MotionNum
        {
            get { return motionNumber; }
            set { motionNumber = value; }
        }

        public bool IsLoopingAction
        {
            get { return isLoopingAction; }
            set { isLoopingAction = value; }
        }

        //表示位置を番号で管理
        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();

        public Motion() {
            isLoopingAction = true;
            hasInterval = false;
        }

        public Motion(float interval)
        {
            isLoopingAction = true;
            hasInterval = true;
            _interval = new CountDownTimer(interval);
        }


        public void Initialize(Range range, CountDownTimer timer)
        {
            this.range = range;
            this.countDowntimer = timer;
            motionNumber = range.First();
            isDead = false;
        }


        //モーション矩形情報の追加
        public void Add(int index, Rectangle rect)
        {
            if (rectangles.ContainsKey(index)) return;
            rectangles.Add(index, rect);
        }


        //モーションの更新
        private void MotionUpdate()
        {
            motionNumber += 1;
            if (range.IsOutOfRange(motionNumber) && isLoopingAction)
            {
                motionNumber = range.First();
                isDead = true;
                if (hasInterval)
                    _interval.Initialize();
            }
        }


        //更新
        public void Update(GameTime gameTime)
        {
            if (range.IsOutOfRange()) return;

            if (hasInterval)
            {
                if (_interval.IsTime())
                    countDowntimer.Update();
                else
                    _interval.Update();
            }
            else
            {
                countDowntimer.Update();
            }

            if (countDowntimer.IsTime())
            {
                countDowntimer.Initialize();
                MotionUpdate();
            }
        }


        //描画範囲の取得
        public Rectangle DrawingRange()
        {
            return rectangles[motionNumber];
        }


        //１回モーションしたらtrueを返す
        public bool OnceMotion()
        {
            return isDead;
        }

        public void Initialize(Range range, object countDownTimer)
        {
            throw new NotImplementedException();
        }
    }
}
