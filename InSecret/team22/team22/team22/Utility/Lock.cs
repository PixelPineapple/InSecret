using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Object;

namespace team22.Utility
{
    /// <summary>
    /// 鍵とエレベーターのリスト
    /// </summary>
    static class Lock
    { 
        public static List<GameObject> lockList = new List<GameObject>();
        public static List<GameObject> List
            {
                get { return lockList; }
            }
    }
}
