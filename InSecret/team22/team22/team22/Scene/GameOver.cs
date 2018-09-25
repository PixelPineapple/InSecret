using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Scene
{
    class GameOver : IScene
    {
        #region Fields
        private InputState inputstate;
        private Vector2 latticePos;
        private float nameAlpha;
        private float buttonAlpha;
        private float alpha;
        private bool endFlag;
        private Sound sound;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public GameOver(GameDevice gameDevice)
        {
            inputstate = gameDevice.InputState;
            endFlag = false;
            latticePos = new Vector2(0, -672);
            nameAlpha = 0f;
            buttonAlpha = 0f;
            alpha = 0.03f;
            sound = gameDevice.Sound;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();

            renderer.DrawTexture("gameover", Vector2.Zero);
            renderer.DrawTexture("gameover_lattice", latticePos);
            renderer.DrawTexture("gameovername", Vector2.Zero, nameAlpha);
            renderer.DrawTexture("gameoverbutton", Vector2.Zero, buttonAlpha);

            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            endFlag = false;
            latticePos = new Vector2(0, -672);
            nameAlpha = 0f;
            buttonAlpha = 0f;
            alpha = 0.03f;
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
            sound.StopBGM();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if ((inputstate.GetKeyTrigger(Keys.X) || inputstate.GetKeyTrigger(Buttons.A)) && nameAlpha >= 1)
            {
                endFlag = true;
            }

            //檻を下げる処理
            if (latticePos.Y < 0)
            {
                latticePos.Y += 8f;
            }
            else if (latticePos.Y > 0)
            {
                latticePos.Y = 0;
            }

            //GameOverの文字の表示
            if (latticePos.Y == 0 && nameAlpha < 1)
            {
                nameAlpha += 0.03f;
            }

            //「ボタン押して！」の文字の表示
            if (nameAlpha >= 1)
            {
                buttonAlpha += alpha;
                if ((buttonAlpha < 0 && alpha < 0) || (buttonAlpha > 1 && alpha > 0))
                {
                    alpha = -alpha;
                }
            }

            sound.PlayBGM("gameover");
        }
    }
}
