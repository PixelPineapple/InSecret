using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class FloorNumber 
    {
        private Dictionary<int, string> floorNumbers = new Dictionary<int, string>(); 
        private Vector2 position;

        /// <summary>
        /// プレイヤーが何階にいるのか表示するクラス
        /// </summary>
        /// <param name="position"></param>
        public FloorNumber(Vector2 position)
        {
            this.position = position;
            floorNumbers.Add(0, "floor_first");
            floorNumbers.Add(1, "floor_second");
            floorNumbers.Add(2, "floor_third");
            floorNumbers.Add(3, "floor_fourth");
            floorNumbers.Add(4, "floor_fifth");
            floorNumbers.Add(5, "floor_security");
            floorNumbers.Add(6, "floor_saru");
            FloorNumInfo.floorNum = 0;
        }

        public void Draw(Renderer renderer)
        {
            //カメラが動いている時と、キャラ切り替えの時は表示されない
            //判定はGameObjectManagerのIsManでしている
            if (!FloorNumInfo.IsDraw) return;
            renderer.DrawTexture(floorNumbers[FloorNumInfo.floorNum], position);
        }
    }
}
