using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace team22.Device.EventDialogue
{
    class DialogueReader
    {
        #region Fields
        private Dictionary<string, string> dialogues;
        #endregion

        #region Getter Setter メソッド
        public Dictionary<string, string> GetDictDialogues()
        {
            return dialogues;
        }

        public string getDialogue(string key)
        {
            return dialogues[key];
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName"></param>
        public DialogueReader(string fileName)
        {
            Initialize(fileName);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="fileName"></param>
        private void Initialize(string fileName)
        {
            if (dialogues != null)
            {
                dialogues.Clear();
            }
            else
            {
                dialogues = new Dictionary<string, string>();
            }

            ReadData(fileName);
        }

        /// <summary>
        /// txtファイルを読み込む。
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadData(string fileName)
        {
            FileStream datafs = new FileStream(fileName, FileMode.Open);
            StreamReader dataSr = new StreamReader(datafs, Encoding.GetEncoding(932));

            while (!dataSr.EndOfStream)
            {
                string line = dataSr.ReadLine();
                string[] items = line.Split(new char[] { '『', '』' }, StringSplitOptions.RemoveEmptyEntries);

                if (items.Length != 2) continue;

                string keys = items[0];
                string dialogue = items[1];

                dialogues.Add(keys, dialogue);
            }

            dataSr.Close();
            datafs.Close();
        }
    }
}
