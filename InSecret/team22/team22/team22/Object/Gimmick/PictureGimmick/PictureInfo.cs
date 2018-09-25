using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace team22.Object
{
    static class PictureInfo
    {
        //絵画のギミック
        //文字の順番に絵画の名前と、絵画の大きさが(0=width,1=height)で入ってる
        private static List<string> ansName = new List<string>();
        public static List<string> AnsName
        {
            get { return ansName; }
            set { ansName = value; }
        }
        private static List<Vector2> ansSize = new List<Vector2>();
        public static List<Vector2> AnsSize
        {
            get { return ansSize; }
            set { ansSize = value; }
        }

        //アルファベットはリストの何番目なのか
        public static int alphabetNum = 0;
        public static int AlphabetNum
        {
            get { return alphabetNum; }
            set { alphabetNum = value; }
        }
    }
}
