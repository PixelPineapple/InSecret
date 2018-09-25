using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Utility;

namespace team22.Scene
{
    /// <summary>
    /// フェードシーン
    /// </summary>
    class SceneFader : IScene //インターフェースを実装
    {
        //フェードシーン状態の列挙
        private enum SceneFadeState
        {
            In,
            Out,
            None
        };

        private Timer timer; //フェード時間
        private static float FADE_TIMER = 1.0f; //2秒で
        private SceneFadeState state;
        private IScene scene; //現在のシーン
        private bool isEnd = false; //終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scene"></param>
        public SceneFader(IScene scene)
        {
            this.scene = scene;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            scene.Initialize(); //現在のシーンを初期化
            state = SceneFadeState.In; //フェードインに設定
            timer = new Timer(FADE_TIMER);
            isEnd = false;
        }

        public void Update(GameTime gameTime)
        {
            //状態に応じて更新するシーンを選択
            switch (state)
            {
                case SceneFadeState.In:
                    UpdateFadeIn(gameTime);
                    break;
                case SceneFadeState.Out:
                    UpdateFadeOut(gameTime);
                    break;
                case SceneFadeState.None:
                    UpdateFadeNone(gameTime);
                    break;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //状態に応じて描画するシーンを選択
            switch (state)
            {
                case SceneFadeState.In:
                    DrawFadeIn(renderer);
                    break;
                case SceneFadeState.Out:
                    DrawFadeOut(renderer);
                    break;
                case SceneFadeState.None:
                    DrawFadeNone(renderer);
                    break;
            }
        }

        /// <summary>
        /// フェードインの更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeIn(GameTime gameTime)
        {
            //シーンの更新
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;
            }

            //時間の更新
            timer.Update();
            if (timer.IsTime())
            {
                state = SceneFadeState.None;
            }
        }

        /// <summary>
        /// フェードアウトの更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeOut(GameTime gameTime)
        {
            //シーンの更新
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;
            }

            //時間の更新
            timer.Update();
            if (timer.IsTime())
            {
                isEnd = true;
            }
        }
        /// <summary>
        /// フェードなし状態の更新
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateFadeNone(GameTime gameTime)
        {
            //シーンの更新
            scene.Update(gameTime);
            if (scene.IsEnd())
            {
                state = SceneFadeState.Out;
                //フェードアウト用の時間のために初期化
                timer.Initialize();
            }

        }

        /// <summary>
        /// フェードイン状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeIn(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, timer.Rate());
        }

        /// <summary>
        /// フェードアウト状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeOut(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, 1.0f - timer.Rate());
        }

        /// <summary>
        /// フェードなし状態の描画
        /// </summary>
        /// <param name="renderer"></param>
        private void DrawFadeNone(Renderer renderer)
        {
            scene.Draw(renderer);
        }

        /// <summary>
        /// エフェクト描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        /// <param name="alpha">透明値</param>
        private void DrawEffect(Renderer renderer, float alpha)
        {
            renderer.Begin();
            renderer.DrawTexture(
                "fade",
                Vector2.Zero,
                new Vector2(48 * 22, 48 * 14),
                alpha);
            renderer.End();
        }

        /// <summary>
        /// シーンが終了したか？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEnd;
        }

        /// <summary>
        /// 次のシーンの取得
        /// </summary>
        /// <returns></returns>
        public SceneType Next()
        {
            return scene.Next();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
            scene.Shutdown();
        }
    }

}

