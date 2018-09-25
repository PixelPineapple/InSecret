using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace team22.Device
{
    public class CSVReader
    {
        #region fields
        private List<string[]> stringData;
        #endregion

        public CSVReader()
        {
            stringData = new List<string[]>();
        }

        public void Read(string filename)
        {
            stringData.Clear();
            try
            {
                //csvファイルを開く
                using (var sr = new System.IO.StreamReader(@"Content/" + filename))
                {
                    //ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        //読み込んだ1行をカンマごとに分けて配列に格納する
                        var values = line.Split(',');

                        //Listに登録する
                        stringData.Add(values);

                        //出力する
                        foreach (var v in values)
                        {
                            System.Console.Write("{0}", v);
                        }
                        System.Console.WriteLine();
                    }
                }
            }
            catch (System.Exception e)
            {
                //ファイルオープンがが失敗したとき
                System.Console.WriteLine(e.Message);
            }
        }

        public List<string[]> GetData()
        {
            return stringData;
        }

        public string[][] GetArrayData()
        {
            return stringData.ToArray();
        }

        public int[][] GetIntData()
        {
            var data = GetArrayData();
            int y = data.Count();
            int x = data[0].Count();

            int[][] intData = new int[y][];
            for (int i = 0; i < y; i++)
            {
                intData[i] = new int[x];
            }

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    intData[i][j] = int.Parse(data[i][j]);
                }
            }
            return intData;
        }

        public string[,] GetStringMatrix()
        {
            var data = GetArrayData();
            int y = data.Count();
            int x = data[0].Count();

            string[,] result = new string[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    result[i, j] = data[i][j];
                }
            }

            return result;
        }

        public int[,] GetIntMatrix()
        {
            var data = GetIntData();
            int y = data.Count();
            int x = data[0].Count();

            int[,] result = new int[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    result[i, j] = data[i][j];
                }
            }

            return result;
        }

    }
}
