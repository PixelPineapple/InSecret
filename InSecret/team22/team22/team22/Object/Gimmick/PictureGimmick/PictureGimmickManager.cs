using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class PictureGimmickManager
    {
        #region Fields
        private Floor floor;    //描画する階
        private GameDevice gameDevice;
        private Random rand;

        private List<string> alphavetAns;       //正答アルファベットのリスト
        private List<Picture> pictures;      //絵画のリスト
        private List<Vector2> picturesPosition; //絵画の位置リスト
        #endregion
        #region Getter Setter メソッド
        public List<Picture> GetPicturesList()
        {
            return pictures;
        }
        #endregion


        public PictureGimmickManager(Floor floor, GameDevice gameDevice)
        {
            this.floor = floor;
            this.gameDevice = gameDevice;
            rand = new Random();

            Initialize();
        }

        public void Initialize()
        {
            //正答アルファベットのリストの生成　
            if (alphavetAns == null)
            {
                alphavetAns = new List<string>();
                SetAlphabetAns();
            }
            else
            {
                alphavetAns.Clear();
                SetAlphabetAns();
            }
            //絵画の位置リスト生成
            if (picturesPosition == null)
            {
                picturesPosition = new List<Vector2>();
                SetPicturesPosi();
            }
            //絵画リスト生成
            if (pictures == null)
            {
                pictures = new List<Picture>();
                SetPictures();
                SetAns();
            }

            FloorAdd();
        }

        /// <summary>
        /// 絵画の位置リスト設定
        /// </summary>
        public void SetPicturesPosi()
        {
            for (int i = 0; i < 8; i++)
            {
                picturesPosition.Add(new Vector2(floor.FloorRange.Width / 8 * i + 1, floor.FloorRange.Bottom - 64));
            }

            //位置をシャッフル
            for (int i = 6; i >= 1; i--)
            {
                int r = rand.Next(1, i + 1);
                var temp = picturesPosition[i];
                picturesPosition[i] = picturesPosition[r];
                picturesPosition[r] = temp;
            }
        }

        /// <summary>
        /// アルファベットの正答をリストに設定
        /// </summary>
        public void SetAlphabetAns()
        {
            alphavetAns.Add("SHINE");
            alphavetAns.Add("FIGHT");
            alphavetAns.Add("DREAM");
            alphavetAns.Add("ANSER");
            alphavetAns.Add("BLOOD");
            alphavetAns.Add("READY");

            //正答の選択
            PictureInfo.alphabetNum = rand.Next(0, 6);
        }

        /// <summary>
        /// 絵画の登録
        /// </summary>
        private void SetPictures()
        {
            pictures.Add(new Picture("painting4_light", "picture_alphabet1", picturesPosition[1], 120, 87, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
            pictures.Add(new Picture("painting6_light", "picture_alphabet2", picturesPosition[2], 110, 85, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
            pictures.Add(new Picture("painting7_light", "picture_alphabet3", picturesPosition[3], 155, 74, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
            pictures.Add(new Picture("painting8_light", "picture_alphabet4", picturesPosition[4], 160, 72, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
            pictures.Add(new Picture("painting9_light", "picture_alphabet5", picturesPosition[5], 145, 122, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
            pictures.Add(new Picture("painting10", "transparent", picturesPosition[6], 129, 108, (floor.FloorRange.X / 32), (floor.FloorRange.Y / 32), gameDevice, floor));
        }

        /// <summary>
        /// 絵画に一つずつ答えを入れる
        /// </summary>
        private void SetAns()
        {
            for (int i = 0; i < 5; i++)
            {
                pictures[i].SetAlphabetInfo(PictureInfo.AlphabetNum);

                PictureInfo.AnsName.Add(pictures[i].Name.Substring(0,9));
                PictureInfo.AnsSize.Add(new Vector2(pictures[i].Width, pictures[i].Height - 40));
            }
            //pictureリストの順番をシャッフル
            for (int i = 4; i >= 0; i--)
            {
                int r = rand.Next(1, i + 1);
                var temp = pictures[i];
                pictures[i] = pictures[r];
                pictures[r] = temp;
            }
        }
        /// <summary>
        /// フロアに追加
        /// </summary>
        private void FloorAdd()
        {
            foreach (var p in pictures)
            {
                floor.AddObject(p);
            }
        }
    }
}
