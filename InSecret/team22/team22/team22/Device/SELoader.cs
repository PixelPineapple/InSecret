using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    public class SELoader : Loader
    {
        #region Fields
        private Sound sound;
        #endregion
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sound"></param>
        /// <param name="resources"></param>
        public SELoader(Sound sound, string[,] resources) :
            base(resources)//親クラスで初期化
        {
            this.sound = sound;
            Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update()
        {
            //まず終了フラグを有効にして
            endFlag = true;

            //カウンタが最大に達してないか？
            if (counter < maxNum)
            {
                //BGM読み込み
                sound.LoadSE(
                    resources[counter, 0], //アセット名
                    resources[counter, 1]);//ファイルパス
                //カウントアップ
                counter += 1;
                //まだ読み込むものがあったのでフラグを戻す
                endFlag = false;
            }
        }
    }
}
