using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;
using team22.Def;

namespace team22.Scene
{
    /// <summary>
    /// タイトルクラス
    /// </summary>
    class Title : IScene
    {
        private GameDevice gameDevice;
        private InputState input;//入力処理用オブジェクト
        private Sound sound;
        private Motion entranceMotion;  //入口のモーション
        private Motion playerMotion;    //プレイヤーのモーション
        private NameMotion starMotion;  //星のモーション
        private CountDownTimer entranceLastTimer;   //入口が開いた後に少し時間を置く
        private Vector2 selectPosi;         //選択画像の位置
        private Vector2[] cursorPositions;   //カーソルの各位置配列
        private Vector2 playerPosi;         //プレイヤーの位置
        private int selectNum;  //選択されている配列番号
        private float ruleButtonAlpha;      //ルール説明のボタンの透明度
        private bool isEntrance;    //入口のモーションを動かすかどうか？
        private bool isPlayerWalk;  //プレイヤーが歩き始めるかどうか？
        private bool isRule;        //ルールを表示するのか？
        private bool isButtonAlphaUp;   //ボタンの透明度が上がっているのか？
        private bool endFlag; //終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public Title(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            sound = gameDevice.Sound;
            //入口のモーション登録
            entranceMotion = new Motion();
            for (int i = 0; i <= 2; i++)
            { entranceMotion.Add(i, new Rectangle(128 * i, 0, 128, 498)); }
            //入口が開いた後の待ち時間
            entranceLastTimer = new CountDownTimer(0.2f);
            //プレイヤーのモーション登録
            playerMotion = new Motion();
            for (int i = 0; i < 12; i++)
            { playerMotion.Add(i, new Rectangle(32 * i, 0, 32, 76)); }
            //カーソルの位置登録
            cursorPositions = new Vector2[3];
            for (int i = 0; i < 3; i++)
            { cursorPositions[i] = new Vector2(-45, 64 * i); }
            //星のモーション登録
            starMotion = new NameMotion();
            starMotion.Add(0, "star1");
            starMotion.Add(1, "star2");
            //その他初期化
            selectNum = 0;
            isEntrance = false;
            isPlayerWalk = false;
            isRule = false;
            isButtonAlphaUp = false;
            endFlag = false;
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //描画開始
            renderer.Begin();

            //この間に描画処理を書く
            renderer.DrawTexture("title", Vector2.Zero);
            renderer.DrawTexture("entrance", new Vector2(Screen.Width - 128, Screen.Height - 498 - 16), entranceMotion.DrawingRange());
            renderer.DrawTexture("prov_titlename", new Vector2(Screen.WidthHalf - 250, 48 * 3));

            renderer.DrawTexture(starMotion.DrawName(), Vector2.Zero);
            //プレイヤー描画
            if (isPlayerWalk)
            {
                renderer.DrawTexture("Man_Model", playerPosi, playerMotion.DrawingRange(), new Vector2(3, 3)); }
            else
            {
                renderer.DrawTexture("Man_Model", playerPosi, playerMotion.DrawingRange(), new Vector2(3, 3));
            }
            //選択肢描画
            selectPosi = new Vector2(Screen.WidthHalf - 130, Screen.HeightHalf);
            renderer.DrawTexture("select", selectPosi);
            renderer.DrawTexture("select_cursor", selectPosi + cursorPositions[selectNum]);

            //ルール描画
            if (isRule)
            {
                renderer.DrawTexture("rule", Vector2.Zero);
                renderer.DrawTexture("rulebutton", Vector2.Zero, ruleButtonAlpha);
            }

            //描画終了
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            entranceMotion.Initialize(new Range(0, 2), new CountDownTimer(0.1f));
            //プレイヤーの初期化
            playerMotion.Initialize(new Range(1, 5), new CountDownTimer(0.1f));
            playerPosi = new Vector2(48 * 5, Screen.Height - (76 * 3) - 48);
            starMotion.Initialize(new Range(0, 1), new CountDownTimer(0.5f));
            isEntrance = false;
            isPlayerWalk = false;
            isRule = false;
            isButtonAlphaUp = false;
            endFlag = false;
        }

        /// <summary>
        /// タイトルシーン終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return endFlag;
        }

        /// <summary>
        /// タイトルシーンの次のシーン名を取得
        /// </summary>
        /// <returns></returns>
        public SceneType Next()
        {
            return SceneType.GamePlay;
        }

        /// <summary>
        /// シーン終了処理
        /// </summary>
        public void Shutdown()
        {
            entranceMotion.MotionNum = 0;
            sound.StopBGM();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Select();
            Entrance(gameTime);
            PlayerWalk(gameTime);
            starMotion.Update(gameTime);
            Rule();
            sound.PlayBGM("title_bgm");
        }

        /// <summary>
        /// 選択の処理
        /// </summary>
        private void Select()
        {
            if (isRule) return;

            if (isEntrance) return;
            if (input.GetKeyTrigger(Keys.Up) || input.GetKeyTrigger(Buttons.DPadUp))
            {
                selectNum--;
                if (selectNum < 0) selectNum = 2;
                sound.PlaySE("click");
            }
            if (input.GetKeyTrigger(Keys.Down) || input.GetKeyTrigger(Buttons.DPadDown))
            {
                selectNum++;
                if (selectNum > 2) selectNum = 0;
                sound.PlaySE("click");
            }
            //選択結果
            if (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A))
            {
                if (selectNum == 0)
                {
                    isPlayerWalk = true;
                    sound.PlaySE("cat");
                    playerMotion.Initialize(new Range(6, 11), new CountDownTimer(0.1f));
                }
                else if (selectNum == 1) sound.PlaySE("page");
                else if (selectNum == 2) gameDevice.GameEnd = true;

            }
        }

        /// <summary>
        /// 入口の処理
        /// </summary>
        /// <param name="gameTime"></param>
        private void Entrance(GameTime gameTime)
        {
            //入口が開かないなら、何もしない
            if (!isEntrance) return;

            //入口が開ききったら
            if (entranceMotion.MotionNum == 2)
            {
                entranceMotion.MotionNum = 2;
            }
            else
            {
                entranceMotion.Update(gameTime);
            }
        }

        /// <summary>
        /// プレイヤーの歩く処理
        /// </summary>
        private void PlayerWalk(GameTime gameTime)
        {
            playerMotion.Update(gameTime);

            //プレイヤーが歩かないなら、何もしない
            if (!isPlayerWalk) return;

            playerPosi.X += 5.0f;

            if (playerPosi.X >= 48 * 16)
            { isEntrance = true; }

            if (playerPosi.X >= Screen.Width)
            {
                endFlag = true;
            }
        }

        private void Rule()
        {
            if ((input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)) && selectNum == 1)
            {
                isRule = !isRule;

            }

            if (!isRule) return;
            if (isButtonAlphaUp)
            {
                ruleButtonAlpha += 0.03f;
                if (ruleButtonAlpha >= 1) isButtonAlphaUp = false;
            }
            else
            {
                ruleButtonAlpha -= 0.03f;
                if (ruleButtonAlpha <= 0) isButtonAlphaUp = true;
            }
        }
    }
}
