using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Def
{
    /// <summary>
    /// 画面サイズ用定数
    /// </summary>
    static class Screen
    {
        public static readonly int Width = 48 * 22;
        public static readonly int Height = 48 * 14;
        public static readonly int WidthHalf = Width / 2;
        public static readonly int HeightHalf = Height / 2;
        public static readonly int tileSize = 32;
        public static readonly int widthMax = 22;
        public static readonly int heightMax = 14;
        //16:9サイズ（1280x720, 1600x900, 1920x1080）
        //4:3サイズ（800x600, 1024x768, 1280x960）
    }
}
