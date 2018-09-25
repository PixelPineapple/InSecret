using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class Map
    {
        //ListのListで縦横（行列）を表現
        private List<List<GameObject>> mapList;

        private GameDevice gameDevice;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="gameDevice"></param>
        public Map(GameDevice gameDevice)
        {
            mapList = new List<List<GameObject>>();
            this.gameDevice = gameDevice;
        }

        /// <summary>
        /// (1行単位で)ブロックを追加
        /// </summary>
        /// <param name="lineCnt"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private List<GameObject> AddBlock(int lineCnt, string[] line)
        {
            //ブロック情報を登録
            Dictionary<string, GameObject> objectList = new
                Dictionary<string, GameObject>();
            objectList.Add("0", new Space(Vector2.Zero, gameDevice));
            objectList.Add("1", new Block(Vector2.Zero, gameDevice));
            objectList.Add("2", new TransparentBlock(Vector2.Zero, gameDevice));

            List<GameObject> workList = new List<GameObject>();

            int colCnt = 0;
            foreach (var s in line)
            {
                try
                {
                    //Dictionaryから情報を取り出し、複製して登録
                    GameObject work = (GameObject)objectList[s].Clone();
                    work.SetWorldPosition(new Vector2(colCnt * 32, lineCnt * 32));
                    workList.Add(work);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                colCnt++;
            }

            return workList;
        } 

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="filename">CSVデータファイル名</param>
        public void Load(string filename)
        {
            //拡張子忘れ対策
            if (!filename.Contains(".csv"))
            {
                filename = filename + ".csv";
            }

            CSVReader csv = new CSVReader();
            csv.Read(filename);

            //読み込んだデータを取得
            var data = csv.GetData(); //List<string[]>

            //読み込んだ情報から、GameObjectのListに変換し、登録
            for (int lineCnt = 0; lineCnt < data.Count(); lineCnt++)
            {
                mapList.Add(AddBlock(lineCnt, data[lineCnt]));
            }
        }

        /// <summary>
        /// リストの解放
        /// </summary>
        public void Unload()
        {
            mapList.Clear();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //行の繰り返し
            foreach (List<GameObject> list in mapList)
            {
                //列の繰り返し
                foreach (GameObject obj in list)
                {
                    //スペースだったら次へ
                    if (obj is Space)
                    {
                        continue;
                    }

                    //オブジェクトの更新
                    obj.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// 衝突
        /// </summary>
        /// <param name="gameObject"></param>
        public void Hit(GameObject gameObject)
        {
            //中心座標を取得
            Point work = gameObject.GetRectangle().Center;
            int x = work.X / 32;
            int y = work.Y / 32;

            //移動で食い込んでるときの修正
            if (x < 1) x = 1;
            if (y < 1) y = 1;

            for (int i = y - 1; i <= (y + 1); i++)
            {
                for (int j = x - 1; j <= (x + 1); j++)
                {
                    GameObject obj = mapList[i][j];

                    //空白なら何もしない
                    if (obj is Space)
                    {
                        continue;
                    }

                    //相手に衝突を通知
                    if (obj.Collision(gameObject))
                    {
                        gameObject.Hit(obj);
                    }
                }
            }
        }

        public void Draw(Renderer renderer)
        {
            foreach (var list in mapList)
            {
                foreach (var obj in list)
                {
                    obj.Draw(renderer);
                }
            }
        }

    }
}
