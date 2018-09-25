using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility
{
    public class Range
    {
        private int first;//最初  
        private int end;  //終端

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="first"></param>
        /// <param name="end"></param>
        public Range(int first, int end)
        {
            this.first = first;
            this.end = end;
        }

        /// <summary>
        /// 最初の番号の取得
        /// </summary>
        /// <returns></returns>
        public int First()
        {
            return first;
        }

        /// <summary>
        /// 最後の番号の取得
        /// </summary>
        /// <returns></returns>
        public int End()
        {
            return end;
        }

        /// <summary>
        /// 範囲内か？
        /// </summary>
        /// <param name="num">調べたい番号</param>
        /// <returns>範囲内だったらtrue</returns>
        public bool IsWithin( int num)
        {
            //範囲外
            //最初の番号より小さいか？
            if( num < first)
            {
                return false;
            }
            //最後の番号より大きいか？
            if( num > end)
            {
                return false;
            }

            //範囲内
            return true;
        }

        /// <summary>
        /// 指定した開始、終端が範囲外か？
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfRange()
        {
            return first >= end;
        }

        /// <summary>
        /// 範囲外か？
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool IsOutOfRange( int num )
        {
            return !IsWithin(num);
        }
    }
}
