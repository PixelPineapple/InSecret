using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    /// <summary>
    /// テクスチャ読み込み
    /// </summary>
    public class TextureLoader : Loader
    {
        #region Fields
        //描画オブジェクト
        private Renderer renderer;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="resources"></param>
        public TextureLoader(Renderer renderer, string[,] resources) :
            base(resources)//親クラスで初期化
        {
            this.renderer = renderer;
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
                //画像読み込み
                renderer.LoadTexture(
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
