using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Object
{
    static class FloorNumInfo
    {
        //表示する階の番号（0から1階と始まっていく）
        public static int nannkai;
        public static int floorNum
        {
            get { return nannkai; }
            set { nannkai = value; }
        }
        //階を表示するのか？
        public static bool isDraw;
        public static bool IsDraw
        {
            get { return isDraw; }
            set { isDraw = value; }
        }
    }
}
