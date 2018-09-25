using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;//Vector2
using Microsoft.Xna.Framework.Graphics;//SpriteBatch, Texture2D
using Microsoft.Xna.Framework.Content;//ConyentManager
using System.Diagnostics;//Assert用

namespace team22.Device
{
    public class Renderer
    {
        #region Fields
        private ContentManager contentManager;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        //複数画像管理
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        #endregion

        public GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="content">Game1のコンテンツ管理者</param>
        /// <param name="graphics">Game1のグラフィック機器</param>
        public Renderer(ContentManager content, GraphicsDevice graphics)
        {
            contentManager = content;
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void LoadTexture(string name, string filepath = "./")
        {
            //ガード節
            //Dictionaryへの2重登録を回避
            if (textures.ContainsKey(name))
            {
#if DEBUG //DEBUGモードの時のみ有効
                System.Console.WriteLine("この" + name + "はKeyで、すでに登録されています");
#endif
                //処理終了
                return;
            }
             textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        /// <summary>
        /// 画像の登録
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        public void LoadTexture(string name, Texture2D texture)
        {
            if (textures.ContainsKey(name))
            {
#if DEBUG //DEBUGモードの時のみ有効
                System.Console.WriteLine("この" + name + "はKeyで、すでに登録されています");
#endif
                //処理終了
                return;
            }
            textures.Add(name, texture);
        }

        public void Unload()
        {
            textures.Clear();
        }

        public void Begin()
        {
            spriteBatch.Begin();
        }

        //カメラ用Begin
        public void Begin(Matrix matrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,
               BlendState.AlphaBlend,
               SamplerState.LinearClamp,
               DepthStencilState.None,
               RasterizerState.CullCounterClockwise,
               null,
               matrix
               );

        }

        /// <summary>
        /// ライト用
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        public void Begin(SpriteSortMode mode, BlendState state, Matrix matrix)
        {
            spriteBatch.Begin(mode,
                state,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                matrix);
        }

        public void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// 画像の描画
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="alpha">透明値（透明：0.0f, 不透明：1.0f）</param>
        public void DrawTexture(string name, Vector2 position, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            spriteBatch.Draw(textures[name], position, Color.White * alpha);
        }

        /// <summary>
        /// 画像の描画（色変更）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string name, Vector2 position, Color color, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            spriteBatch.Draw(textures[name], position, color * alpha);
        }

        /// <summary>
        /// 画像の描画（矩形取得＆色変更）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">画像の切り出し範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, Rectangle rect, Color color, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            spriteBatch.Draw(
                textures[name],  //画像
                position,        //位置
                rect,            //矩形の指定範囲（左上の座標x, y, 幅、高さ）
                color * alpha);
        }

        /// <summary>
        /// 画像の描画（矩形取得）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">画像の切り出し範囲</param>
        /// <param name="alpha">透明値</param>
        public void DrawTexture(string name, Vector2 position, Rectangle rect, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            spriteBatch.Draw(
                textures[name],  //画像
                position,        //位置
                rect,            //矩形の指定範囲（左上の座標x, y, 幅、高さ）
                Color.White * alpha
                );
        }

        /// <summary>
        /// 画像の表示（拡大縮小）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string name, Vector2 position, Vector2 scale, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,

                null,
                Color.White * alpha,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f);
        }


        /// <summary>
        /// 画像の表示（矩形取得＆拡大縮小）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="rect"></param>
        /// <param name="scale"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string name, Vector2 position, Rectangle rect, Vector2 scale, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                rect,
                Color.White * alpha,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f);
        }

        /// <summary>
        /// 画像の表示（拡大縮小）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string name, Vector2 position, Vector2 scale, Color color, float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,

                null,
                color * alpha,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f);
        }


        /// <summary>
        /// 画像表示（基準点変更＆回転）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="alpha"></param>
        public void DrawTexture(string name, Vector2 position, float rotation, Vector2 origin,
             float alpha = 1.0f)
        {
            spriteBatch.Draw(
                textures[name],
                position,
                new Rectangle(0, 0,
                textures[name].Width, textures[name].Height),
                Color.White * alpha,
                rotation,
                origin,
                1.0f,
                SpriteEffects.None,
                0);
        }

        public void DrawRenderTarget(Texture2D texture, Vector2 position, Color color, float alpha = 1.0f)
        {
            spriteBatch.Draw(texture, position, color * alpha);
        }

        /// <summary>
        /// 数字の描画（整数版、簡易）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="number">描画したい数字</param>
        /// <param name="alpha">透明値</param>
        public void DrawNumber(string name, Vector2 position, int number, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            number = Math.Max(number, 0);

            foreach (var n in number.ToString())
            {
                spriteBatch.Draw(
                    textures[name],
                    position,
                    new Rectangle((n - '0') * 32, 0, 32, 64),
                    Color.White);
                //1桁ずらす
                position.X += 32;
            }

        }

        public void DrawNumber(string name, Vector2 position, string number, int digit, float alpha = 1.0f)
        {
            Debug.Assert(
                textures.ContainsKey(name),
                "アセット名が間違えていませんか？\n" +
                "大文字小文字間違っていませんか？\n" +
                "LoadTextureメソッドで読み込んでいますか？\n" +
                "プログラムを確認してください\n");

            for (int i = 0; i < digit; i++)
            {
                if (number[i] == '.')
                {
                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle(10 * 32, 0, 32, 64),
                        Color.White);
                }
                else
                {
                    //1文字分の数値文字を取得
                    char n = number[i];

                    spriteBatch.Draw(
                        textures[name],
                        position,
                        new Rectangle((n - '0') * 32, 0, 32, 64),
                        Color.White);
                }

                position.X += 32;
            }
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font,
                text,
                position,
                color);
        }
    }
}
