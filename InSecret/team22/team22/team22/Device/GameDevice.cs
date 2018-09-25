using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace team22.Device
{
    class GameDevice
    {
        #region Fields
        private Renderer renderer;
        private InputState input;
        private Sound sound;
        private static Random rand;
        private ContentManager contents;
        private Camera2D camera;
        private Font font;
        private bool gameEnd;
        #endregion

        #region Get メソッド
        public Renderer Renderer
        {
            get { return renderer; }
        }

        public InputState InputState
        {
            get { return input; }
        }

        public Sound Sound
        {
            get { return sound; }
        }

        public ContentManager ContentManager
        {
            get { return contents; }
        }

        public Random Random
        {
            get { return rand; }
        }

        public Camera2D Camera
        {
            get { return camera; }
        }

        public Font Font
        {
            get { return font; }
        }

        public bool GameEnd
        {
            get { return gameEnd; }
            set { gameEnd = value; }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="graphicsDevice"></param>
        public GameDevice(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            contents = contentManager;
            renderer = new Renderer(contentManager, graphicsDevice);
            input = new InputState();
            sound = new Sound(contentManager);
            font = new Font(contentManager);
            rand = new Random();
            camera = new Camera2D();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            input.Update();
            camera.Update();
        }
    }
}
