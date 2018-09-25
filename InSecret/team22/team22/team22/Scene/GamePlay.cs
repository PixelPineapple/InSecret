using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;
using team22.Def;
using team22.Object;
using team22.Object.Gimmick.DiamondRoseGimmick;
using team22.Device.EventDialogue;

namespace team22.Scene
{
    class GamePlay : IScene
    {
        #region Fields
        private GameDevice gameDevice;
        private InputState inputState;
        private bool endFlag;
        private Map map;
        private Camera2D camera;
        private GameObjectManager gameObjectManager;
        private Sound sound;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public GamePlay(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.InputState;
            endFlag = false;
            map = new Map(gameDevice);
            camera = gameDevice.Camera;
            sound = gameDevice.Sound;

            gameObjectManager = new GameObjectManager(gameDevice);
            
            PlayerInfo.IsFinish = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            endFlag = false;
            map.Load("stage");

            camera.Zoom(1.5f);
            gameObjectManager.Initialize();

            // プレイヤーを生成する
            Player man = new Player("Man_Model", new Vector2(32 * 2, 32 * 53), 1, 47, 76, 32, gameDevice);
            Player woman = new Player("Woman_Model", new Vector2(32 * 52f, 32 * 9), 1, 3, 69, 25, gameDevice);
            //男から登録
            gameObjectManager.AddPlayer(man);
            gameObjectManager.AddPlayer(woman);
            gameObjectManager.AddMap(map);
            camera.Initialize(man);
            PlayerInfo.IsFinish = false;
        }

        /// <summary>
        /// 終わったか？
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
            if (!PlayerInfo.IsFinish)
            {
                return SceneType.GameOver;
            }
            else
            {
                return SceneType.Ending;
            }
        }

        public void Shutdown()
        {
            map.Unload();
            sound.StopBGM();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (inputState.GetKeyTrigger(Keys.D1) || inputState.GetKeyTrigger(Buttons.Start) || EnemyManager.playerHit ) // コメントしないと、敵にぶつかったら、ゲームオーバーになる。
            {
                endFlag = true;
            }

            map.Update(gameTime);

           gameObjectManager.Update(gameTime);

            if (gameObjectManager.GetFloor(2).GetGameObjects(0).IsDead())
            {
                endFlag = true;
            }

            sound.PlayBGM("gameplay_bgm");
        }
    
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            gameObjectManager.Draw(renderer);

            // カメラとレンダー
            renderer.Begin(camera.GetMatrix());
            
            map.Draw(renderer);
            GimmickInfo.SuperDraw(renderer);

            renderer.End();

            // ずっと画面の中にある「GUI」
            renderer.Begin();

            renderer.DrawTexture("Boundaries_Ue", Vector2.Zero);
            renderer.DrawTexture("Boundaries_Shita", new Vector2(0, 64 * 8.5f));

            // ここで対話を書かなきゃ
            gameObjectManager.FrontDraw(renderer);

            renderer.End();
        }
    }
}
