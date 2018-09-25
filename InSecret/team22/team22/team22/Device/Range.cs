using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    class Range
    {
        private int first;  //最初
        private int end;    //最後

        public Range(int first, int end)
        {
            this.first = first;
            this.end = end;
        }

        //最初の番号の取得
        public int First()
        {
            return first;
        }

        //最後の番号の取得
        public int End()
        {
            return end;
        }

        //範囲内か？
        public bool IsWithin(int num)
        {
            if (num < first || end < num) return false;
            return true;
        }

        //（設定した最初・最後が）範囲外か？
        public bool IsOutOfRange()
        {
            return first >= end;
        }

        //指定された番号が範囲外か？
        public bool IsOutOfRange(int num)
        {
            return !IsWithin(num);
        }
    }
}
