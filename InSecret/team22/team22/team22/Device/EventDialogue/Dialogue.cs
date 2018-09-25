using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Def;

namespace team22.Device
{
    class Dialogue
    {
        public enum DialogueStatus
        {
            READY,
            WRITING,
            FINISHED,
        }
        
        #region Fields
        private int _cnt;
        private List<string> _texts;
        private string _type;
        private int _num;
        private int _span;

        private int _lineCnt;
        private int _line;
        private Font _fonts;
        private DialogueStatus _dialogueStatus;
        private CountDownTimer afterFinishedWritingTimer;
        private bool _isMan;

        private string previousTexts;

        private InputState _inputState;

        private CountDownTimer _nextButtonTimer;
        private bool _nextButtonSwitch;
        private string previousWords;
        #endregion

        #region Getter Setter メソッド
        public void SetTexts(string texts, bool isMan)
        {
            if (_dialogueStatus != DialogueStatus.WRITING)//if (!previousTexts.Equals(texts))
            {
                if (_texts.Count != 0) _texts.Clear();
                var words = texts.Split(new char[] { '&' });
                for (int i = 0; i < words.Count(); i++)
                {
                    _texts.Add(words[i]);
                }
                _dialogueStatus = DialogueStatus.WRITING;
                previousTexts = texts;
            }

            _isMan = isMan;
        }

        public bool HaveToWrite()
        {
            return _texts.Count != 0;
        }

        public DialogueStatus GetDialogueStatus()
        {
            return _dialogueStatus;
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Dialogue(Font fonts, GameDevice gameDevice)
        {
            _inputState = gameDevice.InputState;
            _fonts = fonts;
            Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _dialogueStatus = DialogueStatus.READY;
            _cnt = 0;
            _num = 0;
            if (_texts != null)
            {
                _texts.Clear();
            }
            else
            {
                _texts = new List<string>();
            }
            _type = "";
            _span = 4;
            _lineCnt = 0;
            _line = 0;
            afterFinishedWritingTimer = null;
            _isMan = false;
            previousTexts = "";
            _nextButtonTimer = new CountDownTimer(0.5f);
            _nextButtonSwitch = true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (_dialogueStatus == DialogueStatus.WRITING)
            {
                // カウント加算
                _cnt++;

                if (_lineCnt > 0)
                {
                    _lineCnt++;
                    if (_lineCnt > _span * 4)
                    {
                        _type += "\n";
                        _line++;
                        _num = 0;
                        _lineCnt = 0;
                        _cnt = 0;
                    }
                }

                if (_num < _texts[_line].Length &&
                    _cnt % _span == _span - 1)
                {
                    _type += _texts[_line][_num];
                    _num++;

                    if (_num >= _texts[_line].Length &&
                        _line < _texts.Count - 1)
                    {
                        _lineCnt++;
                    }
                    else if (_num >= _texts[_line].Length &&
                        _line == _texts.Count - 1)
                    {
                        afterFinishedWritingTimer = new CountDownTimer(3);
                    }
                }

                // 書いたセリフを削除するまでに、何分？
                if (afterFinishedWritingTimer != null)
                {
                    afterFinishedWritingTimer.Update();
                    //Console.WriteLine("Dialogue.cs = Timer = " + timer.Now());
                    if (afterFinishedWritingTimer.IsTime())
                    {
                        Initialize();
                    }
                }

                if ( _inputState.GetKeyTrigger(Keys.X))
                {
                    _span = 1;
                }

                _nextButtonTimer.Update();

                // nextButtonは透明かどうか
                if(_nextButtonTimer.IsTime())
                {
                    _nextButtonSwitch = !_nextButtonSwitch;
                    _nextButtonTimer.Initialize();
                }
            }
        }

        public void Draw(Renderer renderer, string fontName)
        {
            if (_dialogueStatus == DialogueStatus.WRITING)
            {
                if (_isMan) renderer.DrawTexture("Men_Portrait", new Vector2(64, 64 * 8f));
                else renderer.DrawTexture("Woman_Portrait", new Vector2(64, 64 * 8f));

                renderer.DrawString(_fonts.GetFont(fontName), _type, new Vector2(64 * 2.9f, 64 * 8.7f), Color.White);

                if (_nextButtonSwitch)
                    renderer.DrawTexture("nextButton", new Vector2(Screen.Width - 64, Screen.Height - 64), 1);
                else
                    renderer.DrawTexture("nextButton", new Vector2(Screen.Width - 64, Screen.Height - 64), 0);
            }
        }
    }
}
