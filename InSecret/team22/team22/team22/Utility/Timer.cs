using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Utility
{
    public class Timer
    {
        private float currentTime;//現在の時間
        private float limitTime;//制限時間

        public Timer()
        {
            limitTime = 60;//1秒, 60fps
            Initialize();
        }

        public Timer(float second)
        {
            limitTime = 60 * second;
            Initialize();
        }

        public void Initialize()
        {
            currentTime = limitTime;
        }

        public void Update()
        {
            currentTime -= 1.0f;
            currentTime = Math.Max(currentTime, 0);
        }

        public bool IsTime()
        {
            return currentTime <= 0.0f;
        }

        public float Now()
        {
            return currentTime / 60;
        }

        public void Change(float second)
        {
            this.limitTime = 60 * second;
            Initialize();
        }

        /// <summary>
        /// 割合
        /// </summary>
        /// <returns></returns>
        public float Rate()
        {
            return currentTime / limitTime;
        }
    }
}
