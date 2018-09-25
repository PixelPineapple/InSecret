using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;

namespace team22.Object
{
    /// <summary>
    /// 鍵とエレベーターのリスト
    /// </summary>
    static class GimmickInfo
    { 
        public static List<GameObject> lockList = new List<GameObject>();
        public static List<GameObject> List
            {
                get { return lockList; }
            }

        public static List<GameObject> superList = new List<GameObject>();
        public static List<GameObject> SuperList
        {
            get { return superList; }
        }

        public static void SuperDraw(Renderer renderer)
        {
            foreach (GameObject g in superList)
            {
                g.Draw(renderer);
            }
        }

        public static bool eventFlag;

        public static bool EventFlag
        {
            get { return eventFlag; }
        }
    }
}
