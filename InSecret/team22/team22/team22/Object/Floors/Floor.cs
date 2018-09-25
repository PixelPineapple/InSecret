#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Def;
using team22.Device;
using team22.Object.Gimmick.DiamondRoseGimmick;
#endregion

namespace team22.Object
{
    abstract class Floor 
    {
        #region Fields
        private List<GameObject> gameObjects;
        private List<GameObject> addGameObjects;
        private Rectangle floorRange;
        protected GameDevice gameDevice;
        protected MapLighting mapLighting;
        protected List<GameObject> _lightObjects;
        protected CameraFocusTarget eleatorTarget;
        #endregion

        #region Getter Setter メソッド
        public Rectangle FloorRange { get { return floorRange; } }

        public GameObject GetGameObjects(int index)
        {
            return gameObjects[index];
        }

        public List<GameObject> GetGameObjectsList()
        {
            return gameObjects;
        }

        //public void TransferGameObject (GameObject gameObject, Floor transferTo)
        //{
        //    foreach (GameObject c in gameObjects)
        //    {
        //        transferTo.AddObject(gameObject);
        //    }
        //}
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Floor(int topleftX, int topleftY, int width, int height, GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            // フロアーの範囲
            floorRange = new Rectangle(Screen.tileSize * topleftX, Screen.tileSize * topleftY, Screen.tileSize * width, Screen.tileSize * height);
            Initialize();

            eleatorTarget = new CameraFocusTarget(Vector2.Zero, gameDevice);
        }

        public void AddLights(GameObject light)
        {
            _lightObjects.Add(light);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            if (gameObjects != null)
            {
                gameObjects.Clear();
            }
            else
            {
                gameObjects = new List<GameObject>();
            }
            if (addGameObjects != null)
            {
                addGameObjects.Clear();
            }
            else
            {
                addGameObjects = new List<GameObject>();
            }

            // 光の位置を設定
            if (_lightObjects != null)
                _lightObjects.Clear();
            else
                _lightObjects = new List<GameObject>();
        }
        
        protected abstract void InsertObjects();

        /// <summary>
        /// オブジェクトを追加
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddObject(GameObject gameObject)
        {
            if (gameObject is DiamondRose)
                gameObjects.Add(gameObject);

            if (gameObjects == null) return;
            addGameObjects.Add(gameObject);
        }

        public void AddLight(GameObject light)
        {
            _lightObjects.Add(light);
        }

        /// <summary>
        /// オブジェクトどうしの衝突
        /// </summary>
        private void HitGameObject()
        {
            foreach (var c1 in gameObjects)
            {
                foreach (var c2 in gameObjects)
                {
                    if (c1.Equals(c2) || c1.IsDead() || c2.IsDead()) continue;
                    if (c1.Collision(c2))
                    {
                        c1.Hit(c2);
                        c2.Hit(c1);
                    }
                }
            }
        }

        /// <summary>
        /// 不要なオブジェクトを削除
        /// </summary>
        private void RemoveDeadGameObject()
        {
            gameObjects.RemoveAll(c => c.IsDead());
        }

        public abstract void SetLight();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (var c in _lightObjects)
            {
                c.Update(gameTime);
            }

            // 全体オブジェクトを更新
            foreach (var c in gameObjects)
            {
                c.Update(gameTime);
            }
            // ゲームオブジェクトを追加
            foreach (var c in addGameObjects)
            {
                gameObjects.Add(c);
            }
            addGameObjects.Clear();

            // 当たり判定
            HitGameObject();

            RemoveDeadGameObject();
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void Draw(Renderer renderer, List<Player> player)
        {
            //foreach (var c in gameObjects)
            //{
            //    c.Draw(renderer);
            //}

            mapLighting.Draw(renderer, player);
        }

        /// <summary>
        /// 前に描画
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void FrontDraw(Renderer renderer, Player player)
        {
            foreach (var c in gameObjects)
            {
                c.FrontDraw(renderer);
            }
        }
    }
}