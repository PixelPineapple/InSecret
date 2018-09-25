using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    /// <summary>
    /// 読み込み抽象クラス
    /// </summary>
    public abstract class Loader
    {
        #region Fields
        //リソース：アセット名とファイルパスのセット
        protected string[,] resources;
        protected int counter;//現在何番目まで読み込んだか
        protected int maxNum;//リソース最大数
        protected bool endFlag;//終了フラグ
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resources"></param>
        public Loader(string[,] resources)
        {
            this.resources = resources;
            counter = 0;
            maxNum = 0;
            endFlag = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            counter = 0;
            endFlag = false;
            maxNum = 0;
            if (resources != null)
            {
                //配列から登録する個数を取得
                maxNum = resources.GetLength(0);
            }
        }

        /// <summary>
        /// 登録最大数を取得
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return maxNum;
        }

        /// <summary>
        /// 現在登録している番号を取得
        /// </summary>
        /// <returns></returns>
        public int CurrentCount()
        {
            return counter;
        }

        /// <summary>
        /// 終了フラグ
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return endFlag;
        }

        /// <summary>
        /// 抽象更新処理
        /// </summary>
        public abstract void Update();
    }
}
