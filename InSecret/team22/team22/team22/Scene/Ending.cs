using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Scene
{
    class Ending : IScene
    {
        #region Fields
        private InputState inputstate;
        private NameMotion starMotion;  //星モーション
        private NameMotion heriMotion;  //ヘリコプターモーション

        private float clearAlpha;
        private float buttonAlpha;

        private bool isButtonUp;     //buttonのtextの透過度が上がっているのか？
        private bool endFlag;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public Ending(GameDevice gameDevice)
        {
            inputstate = gameDevice.InputState;
            starMotion = new NameMotion();
            heriMotion = new NameMotion();
            clearAlpha = 0;
            buttonAlpha = 0;
            isButtonUp = true;
            endFlag = false;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("gameclear", Vector2.Zero);
            renderer.DrawTexture(starMotion.DrawName(), Vector2.Zero);
            renderer.DrawTexture(heriMotion.DrawName(), Vector2.Zero);
            renderer.DrawTexture("gameclearname", Vector2.Zero, clearAlpha);
            renderer.DrawTexture("gameclearbutton", Vector2.Zero, buttonAlpha);


            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //星モーション登録
            starMotion.Add(0, "star_gameclear1");
            starMotion.Add(1, "star_gameclear2");
            starMotion.Initialize(new Range(0, 1), new CountDownTimer(0.5f));
            //ヘリコプターモーション登録
            heriMotion.Add(0, "gameclear_heri1");
            heriMotion.Add(1, "gameclear_heri2");
            heriMotion.Initialize(new Range(0, 1), new CountDownTimer(0.1f));
            clearAlpha = 0;
            buttonAlpha = 0;
            isButtonUp = true;
            endFlag = false;
        }

        /// <summary>
        /// シーンが終わったか？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return endFlag;
        }

        /// <summary>
        /// 次のシーンは？
        /// </summary>
        /// <returns></returns>
        public SceneType Next()
        {
            return SceneType.Title;
        }

        public void Shutdown()
        {

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (inputstate.GetKeyTrigger(Keys.X) || inputstate.GetKeyTrigger(Buttons.A))
            {
                endFlag = true;
            }

            //clearのtextの透過度
            clearAlpha += 0.03f;
            if (clearAlpha >= 1) clearAlpha = 1;

            //buttonの透過度の更新
            if (isButtonUp)
            {
                buttonAlpha += 0.03f;
                if (buttonAlpha >= 1) isButtonUp = false;
            }
            else
            {
                buttonAlpha -= 0.03f;
                if (buttonAlpha <= 0) isButtonUp = true;
            }
            //モーションアップデート
            starMotion.Update(gameTime);
            heriMotion.Update(gameTime);
        }
    }
}
