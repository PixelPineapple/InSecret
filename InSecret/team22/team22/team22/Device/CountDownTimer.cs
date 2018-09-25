using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    class CountDownTimer
    {
        private float currentTime;
        private float limitTimer;
        private bool isInitialized = false;

        #region Getter Setter メソッド
        public bool GetIsInitialized()
        {
            return isInitialized;
        }

        public float Now()
        {
            return currentTime / 60.0f;
        }

        public bool IsTime()
        {
            isInitialized = false;
            return currentTime <= 0.0f;
        }
        #endregion

        public CountDownTimer( float second)
        {
            limitTimer = 60.0f * second;
            Initialize();
        }

        public void Initialize()
        {
            currentTime = limitTimer;
            isInitialized = true;
        }

        public void Update()
        {
            currentTime = (currentTime <= 0.0f) ? (0.0f) : (currentTime -= 1.0f);
        }

        public void Change(float second)
        {
            limitTimer = 60.0f * second;
            Initialize();
        }
    }
}
