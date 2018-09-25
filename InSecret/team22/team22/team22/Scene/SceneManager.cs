using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Scene
{
    class SceneManager
    {
        //複数のシーン管理
        private Dictionary<SceneType, IScene> scenes = new Dictionary<SceneType, IScene>();
        //現在のシーン
        private IScene currentScene = null; //一番最初はシーンなしに設定

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        { }

        /// <summary>
        /// シーン追加
        /// </summary>
        /// <param name="name">シーン名</param>
        /// <param name="scene">シーンオブジェクト</param>
        public void Add(SceneType name, IScene scene)
        {
            if (scenes.ContainsKey(name))
            {
                //すでに同じ名前でシーンが追加されてれば終了
                return;
            }

            //シーンを追加
            scenes.Add(name, scene);
        }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="name">次のシーン名</param>
        public void Change(SceneType name)
        {
            //すでにシーンが登録されていれば終了処理
            if (currentScene != null)
            {
                currentScene.Shutdown();
            }

            //シーン管理から指定された次のシーンを取り出し、
            //現在のシーンに設定
            currentScene = scenes[name];

            //現在のシーンを初期化
            currentScene.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //シーンがなければ終了
            if (currentScene == null)
            {
                return;
            }

            //現在のシーンの更新
            currentScene.Update(gameTime);

            //更新処理後、シーンが終了したか？
            if (currentScene.IsEnd())
            {
                //次のシーンへ
                Change(currentScene.Next());
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //シーンがなければ終了
            if (currentScene == null)
            {
                return;
            }
            //現在のシーンの描画
            currentScene.Draw(renderer);
        }
    }
}
