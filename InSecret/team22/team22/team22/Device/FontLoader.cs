using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    class FontLoader : Loader
    {
        #region Fields
        private Font _font;
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="font"></param>
        /// <param name="resources"></param>
        public FontLoader(Font font, string[,] resources) :
            base (resources)
        {
            _font = font;
        }

        public override void Update()
        {
            endFlag = true;

            if (counter < maxNum)
            {
                _font.LoadFont(
                    resources[counter, 0],
                    resources[counter, 1]);

                counter += 1;

                endFlag = false;
            }
        }
    }
}
