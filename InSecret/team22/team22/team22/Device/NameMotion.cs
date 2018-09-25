using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace team22.Device
{
    class NameMotion
    {
        private Range range;                        //範囲
        private CountDownTimer countDowntimer;      //モーション時間
        private int motionNumber;                   //モーション番号
        private bool isDead;                        //削除フラグ

        //表示位置を番号で管理
        private Dictionary<int, string> names = new Dictionary<int, string>();

        public int MotionNum
        {
            get { return motionNumber; }
        }

        public NameMotion() { }

        public void Initialize(Range range, CountDownTimer timer)
        {
            this.range = range;
            countDowntimer = timer;
            motionNumber = range.First();
            isDead = false;
        }

        //モーション名前情報の追加
        public void Add(int index, string name)
        {
            if (names.ContainsKey(index)) return;
            names.Add(index, name);
        }

        //モーションの更新
        private void MotionUpdate()
        {
            motionNumber += 1;
            if (range.IsOutOfRange(motionNumber))
            {
                motionNumber = range.First();
                isDead = true;
            }
        }

        //更新
        public void Update(GameTime gameTime)
        {
            if (range.IsOutOfRange()) return;

            countDowntimer.Update();
            if (countDowntimer.IsTime())
            {
                countDowntimer.Initialize();
                MotionUpdate();
            }
        }

        //描画名前の取得
        public string DrawName()
        { return names[motionNumber]; }

        //１回モーションしたらtrueを返す
        public bool OnceMoiton()
        { return isDead; }

        public void Initialize(Range range, object countDownTimer)
        {
            throw new NotImplementedException();
        }
    }
}
