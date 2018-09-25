using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using team22.Device;
using team22.Scene;
using team22.Def;

namespace team22
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private GameDevice gameDevice;
        private Renderer renderer;
        private InputState input;
        private SceneManager sceneManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = Screen.Width;
            graphics.PreferredBackBufferHeight = Screen.Height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameDevice = new GameDevice(Content, GraphicsDevice);
            renderer = gameDevice.Renderer;
            input = gameDevice.InputState;

            sceneManager = new SceneManager();
            sceneManager.Add(SceneType.Load, new Load(gameDevice));
            sceneManager.Add(SceneType.Logo, new SceneFader(new Logo(gameDevice)));
            sceneManager.Add(SceneType.Title, new SceneFader(new Title(gameDevice)));
            sceneManager.Add(SceneType.GamePlay, new SceneFader(new GamePlay(gameDevice)));
            sceneManager.Add(SceneType.Ending, new SceneFader(new Ending(gameDevice)));
            sceneManager.Add(SceneType.GameOver, new GameOver(gameDevice));
            sceneManager.Change(SceneType.Load);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Texture2D fade = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[1 * 1];
            data[0] = new Color(0, 0, 0);
            fade.SetData(data);
            gameDevice.Renderer.LoadTexture("fade", fade);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (input.GetKeyTrigger(Buttons.Back) || input.GetKeyTrigger(Keys.Escape) || gameDevice.GameEnd)
                this.Exit();
            
            // TODO: Add your update logic here
            gameDevice.Update(gameTime);
            sceneManager.Update(gameTime);


            // MouseState mouse = Mouse.GetState();
            // Console.WriteLine("X = " + mouse.X + ", Y = " + mouse.Y);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            sceneManager.Draw(renderer);

            base.Draw(gameTime);
        }
    }
}
