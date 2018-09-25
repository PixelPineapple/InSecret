using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Scene
{
    /// <summary>
    /// 各シーンのインタフェース
    /// </summary>
    interface IScene
    {
        void Initialize(); //初期化
        void Update(GameTime gameTime); //更新
        void Draw(Renderer renderer); //描画
        void Shutdown(); //終了

        //シーン管理
        bool IsEnd(); //シーンが終了か？
        SceneType Next(); //次のシーン名の取得
    }
}
