using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;//WAV
using Microsoft.Xna.Framework.Media;//MP3
using System.Diagnostics;//Assert


namespace team22.Device
{
    public class Sound
    {
        #region Fields
        private ContentManager contentManager;

        private Dictionary<string, Song> bgms;//MP3管理用
        private Dictionary<string, SoundEffect> soundEffects;//WAV管理用
        private Dictionary<string, SoundEffectInstance> seInstances;//WAVインスタンス管理用（WAVの高度な利用）

        private List<SoundEffectInstance> sePlayList;//WAVインスタンス再生リスト

        private string currentBGM; //現在再生中のアセット名
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content"></param>
        public Sound(ContentManager content)
        {
            contentManager = content;

            //BGMは繰り返し再生
            MediaPlayer.IsRepeating = true;

            //各Dictionaryの実体生成
            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();

            //再生Listの実体生成
            sePlayList = new List<SoundEffectInstance>();

            //何んの再生していないのでnull初期化
            currentBGM = null;
        }

        /// <summary>
        /// Assert用メッセージ
        /// </summary>
        /// <param name="name">アセット名 </param>
        ///<returns></returns>
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名（" + name + "）がありません\n" +
                "アセット名の確認、Dictionaryに登録されているか確認してください\n";
        }

        #region BGM関連処理

        /// <summary>
        /// BGM(MP3)の読み込み
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filepath"></param>
        public void LoadBGM(string name, string filepath = "./")
        {
            //すでに登録されているか？
            if (bgms.ContainsKey(name))
            {
                return;
            }
            //MP3の読み込みとDictionaryへの登録
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        /// <summary>
        /// 停止中か？
        /// </summary>
        /// <returns></returns>
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        /// 再生中か？
        /// </summary>
        /// <returns></returns>
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }

        /// <summary>
        /// 一時停止中か？
        /// </summary>
        /// <returns></returns>
        public bool IsPausedBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// BGM停止
        /// </summary>
        public void StopBGM()
        {
            MediaPlayer.Stop();
            currentBGM = null;
        }


        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            //同じ曲か？
            if (currentBGM == name)
            {
                //同じ曲だったら何もしない
                return;
            }

            //BGMは再生中か？
            if (IsPlayingBGM())
            {
                //再生中の場合、停止処理する
                StopBGM();
            }

            //ボリューム設定（BGMはSEに比べて半分が普通)
            MediaPlayer.Volume = 0.5f;

            //現在のBGM名を設定
            currentBGM = name;

            //再生開始
            MediaPlayer.Play(bgms[name]);
        }

        /// <summary>
        /// BGMループフラグの変更
        /// </summary>
        /// <param name="loopFLag"></param>
        public void ChangeBGMLoopFlag(bool loopFLag)
        {
            MediaPlayer.IsRepeating = loopFLag;
        }
        #endregion

        #region WAV関連

        /// <summary>
        /// SE(WAV)データの読み込み
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filepath"></param>
        public void LoadSE(string name, string filepath = "./")
        {
            //すでに登録されているか？
            if (soundEffects.ContainsKey(name))
            {
                return;
            }

            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }


        public void CreateSEInstance(string name)
        {
            //すでに登録されているか?
            if (seInstances.ContainsKey(name))
            {
                return;
            }

            //WAV用ディクショナリに登録されてないとムリ
            Debug.Assert(
                soundEffects.ContainsKey(name),
                "先に" + name + "の読み込み処理をしてください");

            //WAVデータのインスタンス生成
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }

        /// <summary>
        /// 単純SE再生（連続で呼ばれた場合、音は重なる。停止不可）
        /// </summary>
        /// <param name="name"></param>
        public void PlaySE( string name )
        {
            //WAV用ディクショナリをチェック
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }

        public void PlaySEInstance( string name, bool loopFlag = false)
        {
            //WAVインスタンス用ディクショナリをチェック
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));

            var data = seInstances[name];
            data.IsLooped = loopFlag;
            data.Play();
            sePlayList.Add(data);
        }

        /// <summary>
        /// sePlayListにある再生中の音を停止
        /// </summary>
        public void StoppedSE()
        {
            foreach( var se in sePlayList)
            {
                if(se.State == SoundState.Playing)
                {
                    se.Stop();
                }
            }
        }

        /// <summary>
        /// 再生中のものを一時停止
        /// </summary>
        public void PauseSE()
        {
            foreach( var se in sePlayList)
            {
                if( se.State == SoundState.Playing)
                {
                    se.Pause();
                }
            }
        }

        /// <summary>
        /// 停止中のモノはListから削除
        /// </summary>
        public void RemoveSE()
        {
            sePlayList.RemoveAll(se => (se.State == SoundState.Stopped));
        }
        #endregion

        public void Unload()
        {
            bgms.Clear();
            soundEffects.Clear();
            sePlayList.Clear();
        }
    }
}
