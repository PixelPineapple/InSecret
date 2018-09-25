using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Def;
using team22.Device;
using team22.Utility;

namespace team22.Object
{
    class AlphabetSelect : GameObject
    {
        #region Fields
        private InputState input;
        private Key key;
        private CountDownTimer unLockTimer;     //UNLOCKが表示されている時間
        private List<Vector2> alphabetPosition = new List<Vector2>();   //アルファベット1文字ずつの位置
        private List<string> alphabetName = new List<string>();
        private List<int> alphabetNum = new List<int>();                //一文字ごとの切り取る数字
        private float scale = 1;
        private int alNum;
        private Random rand;
        private bool isUnLock;
        private Sound sound;
        #endregion

        public AlphabetSelect(Vector2 worldPosition, int floorHeight, int floorWidth, GameDevice gameDevice, Key key)
            : base("pc", worldPosition, floorHeight, floorWidth, GimmickFront.frontY, GimmickFront.frontX, gameDevice)
        {
            input = gameDevice.InputState;
            rand = gameDevice.Random;
            sound = gameDevice.Sound;
            this.key = key;
            unLockTimer = new CountDownTimer(1.0f);

            SetingAlphabet();
            isUnLock = false;

        }

        private void SetingAlphabet()
        {
            for (int i = 0; i < 5; i++)
            {
                int n = i * 2 + 1;
                alphabetPosition.Add(new Vector2(GetRectangle().Left + 96 + (72 * i), GetRectangle().Top + 24 * 7));
            }
            alphabetPosition.Add(new Vector2(GetRectangle().Right - 96 * 2 - 48, GetRectangle().Bottom - 48 * 3.2f));

            alphabetName.Add("picture_alphabet1");
            alphabetName.Add("picture_alphabet2");
            alphabetName.Add("picture_alphabet3");
            alphabetName.Add("picture_alphabet4");
            alphabetName.Add("picture_alphabet5");

            for (int i = 0; i < 5; i++)
            { alphabetNum.Add(rand.Next(5)); }
            alNum = 0;
        }

        public AlphabetSelect(AlphabetSelect other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice, other.key)
        { }

        public override object Clone()
        {
            return new AlphabetSelect(this);
        }

        public override void Hit(GameObject gameObject)
        {

        }

        public override void Update(GameTime gameTime)
        {
            ChangeAlphabet();

            //決定ボタン押されたら
            if (((input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)) && alNum == 5)
                || input.GetKeyTrigger(Keys.Enter))
            {
                int ansCnt = 0;
                foreach (var a in alphabetNum)
                {
                    //選択された文字の番号がすべて答えの番号（何番目か）と同じだったらunlock
                    if (a == PictureInfo.AlphabetNum) ansCnt++;
                }
                if (ansCnt >= 5)
                {
                    sound.PlaySE("pc_ok");
                    isUnLock = true;
                    KeyDialogue = "二階に行ける男";
                }
                else
                {
                    //死ぬ
                    isDead = true;
                    PlayerInfo.IsMove = true;
                    sound.PlaySE("pc_no");
                }
            }

            if (isUnLock)
            {
                unLockTimer.Update();
                if (unLockTimer.IsTime())
                {
                    key.IsOpen = true;
                    //死ぬ
                    isDead = true;
                    PlayerInfo.IsMove = true;
                }
            }
        }

        public override void Draw(Renderer renderer)
        { }

        /// <summary>
        /// 前に描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void FrontDraw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition, new Vector2(scale, scale));

            if (!isUnLock)
            {
                renderer.DrawTexture("pass", new Vector2(worldPosition.X + 32, worldPosition.Y + 24 * 3));
                for (int i = 0; i < 5; i++)
                {
                    if (i == alNum)
                    {
                        renderer.DrawTexture("alphabet_select", alphabetPosition[i]);
                        renderer.DrawTexture(alphabetName[i], alphabetPosition[i], new Rectangle(48 * alphabetNum[i], 0, 48, 96), Color.Blue);
                    }
                    else
                    { renderer.DrawTexture(alphabetName[i], alphabetPosition[i], new Rectangle(48 * alphabetNum[i], 0, 48, 96)); }
                }

                if (alNum == 5)
                {
                    renderer.DrawTexture("pc_enter_select", alphabetPosition[5]);
                    renderer.DrawTexture("pc_enter", alphabetPosition[5], Color.Blue);
                }
                else
                { renderer.DrawTexture("pc_enter", alphabetPosition[5]); }

                renderer.DrawTexture("pc_text", new Vector2(GetRectangle().Left + 48, GetRectangle().Top + 24 * 11));
            }
            else
            {
                renderer.DrawTexture("unlock", worldPosition);
            }

        }

        //選択の変更処理
        private void ChangeAlphabet()
        {
            if (input.GetKeyTrigger(Keys.Left) || input.GetKeyTrigger(Buttons.DPadLeft))
            {
                alNum--;
                if (alNum < 0) alNum = 5;
                sound.PlaySE("pc_move");
            }
            else if (input.GetKeyTrigger(Keys.Right) || input.GetKeyTrigger(Buttons.DPadRight))
            {
                alNum++;
                if (alNum > 5) alNum = 0;
                sound.PlaySE("pc_move");
            }

            if (alNum == 5) return;

            if (input.GetKeyTrigger(Keys.Up) || input.GetKeyTrigger(Buttons.DPadUp))
            {
                alphabetNum[alNum]--;
                if (alphabetNum[alNum] < 0) alphabetNum[alNum] = 5;
                sound.PlaySE("pc_move");
            }
            else if (input.GetKeyTrigger(Keys.Down) || input.GetKeyTrigger(Buttons.DPadDown))
            {
                alphabetNum[alNum]++;
                if (alphabetNum[alNum] > 5) alphabetNum[alNum] = 0;
                sound.PlaySE("pc_move");
            }
        }

    }
}
