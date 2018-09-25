using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Object;

namespace team22.Device
{
    class MapLighting
    {
        #region Fields
        private RenderTarget2D _backGroundCanvas; // 前景のキャンバス
        private RenderTarget2D _foreGroundCanvas; // 背景のキャンバス
        private RenderTarget2D _foreGroundShadowCanvas; // 前景影のキャンバス
        private List<GameObject> _gameObjects;
        private List<GameObject> _lightObjects;
        private Camera2D _camera;
        private string _backgroundImage;
        private int _canvasPositionX;
        private int _canvasPositionY;
        private int _height;
        private int _width;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="width">長さ</param>
        /// <param name="height">高さ</param>
        public MapLighting(int x, int y, int height, int width, string backgroundImage, 
            Camera2D camera, List<GameObject> gameObjects, List<GameObject> lightObjects)
        {
            _canvasPositionX = x * 32;
            _canvasPositionY = y * 32;
            _height = height * 32;
            _width = width * 32;
            _backgroundImage = backgroundImage;
            _camera = camera;
            _gameObjects = gameObjects;
            _lightObjects = lightObjects;
        }

        public void InitializeRenderTarget(Renderer renderer)
        {
            _backGroundCanvas = new RenderTarget2D(renderer.GraphicsDevice,
                _width,
                _height,
                false, SurfaceFormat.Color, DepthFormat.None);
            _foreGroundCanvas = new RenderTarget2D(renderer.GraphicsDevice,
                _width,
                _height,
                false, SurfaceFormat.Color, DepthFormat.None);
            _foreGroundShadowCanvas = new RenderTarget2D(renderer.GraphicsDevice,
                _width,
                _height,
                false, SurfaceFormat.Color, DepthFormat.None);
        }
        
        public void UnloadRenderTarget()
        {
            if(_backGroundCanvas != null)
            {
                try
                {
                    _backGroundCanvas.Dispose();
                    _backGroundCanvas = null;
                }
                catch { }
            }
            if (_foreGroundCanvas != null)
            {
                try
                {
                    _foreGroundCanvas.Dispose();
                    _foreGroundCanvas = null;
                }
                catch { }
            }
            if (_foreGroundShadowCanvas != null)
            {
                try
                {
                    _foreGroundShadowCanvas.Dispose();
                    _foreGroundShadowCanvas = null;
                }
                catch { }
            }
        }

        public void Draw(Renderer renderer, List<Player> players)
        {
            //// BackGroundSpriteに描画する
            renderer.GraphicsDevice.SetRenderTarget(_backGroundCanvas);
            renderer.GraphicsDevice.Clear(Color.Transparent);

            renderer.Begin();
            renderer.DrawTexture(_backgroundImage, Vector2.Zero, Color.White);
            renderer.End();

            // ForeGroundSpriteに描画する
            renderer.GraphicsDevice.SetRenderTarget(_foreGroundCanvas);
            renderer.GraphicsDevice.Clear(Color.Transparent);

            renderer.Begin();
            renderer.DrawTexture(_backgroundImage, Vector2.Zero, Color.White);
            
            // エレベーター
            foreach (var x in GimmickInfo.List)
            {
                if (x.GetRectangle().Intersects(new Rectangle(_canvasPositionX, _canvasPositionY, _width, _height)))
                {
                    x.Draw(renderer);
                }
            }

            foreach (var obj in _gameObjects)
            {
                obj.Draw(renderer);
            }

            foreach (Player player in players)
            {
                if (player.ShouldBeDrawn)
                {
                    player.Draw(renderer);
                    var min = Vector2.Zero;
                    var max = new Vector2(_width - 32, _height - 96);
                    player.SetDrawingPosition(Vector2.Clamp(player.GetDrawingPosition(), min, max));
                }
            }

             renderer.End();

            // ForeGroundShadowに描画する
            renderer.GraphicsDevice.SetRenderTarget(_foreGroundShadowCanvas);
            renderer.GraphicsDevice.Clear(Color.Black);

            renderer.Begin();
            renderer.DrawRenderTarget(_foreGroundCanvas, Vector2.Zero, Color.White);
            foreach (var x in _lightObjects)
            {
                x.Draw(renderer);
            }
            renderer.End();

            // BackBufferに描画する
            renderer.GraphicsDevice.SetRenderTarget(null);
            renderer.GraphicsDevice.Clear(Color.Transparent);
            

            //renderer.Begin(_camera.GetMatrix());
            ////renderer.DrawTexture(_backgroundImage, new Vector2(_pointX * 32, _pointY * 32), Color.White);
            //foreach (GameObject obj in _gameObjects)
            //{
            //    obj.Draw(renderer);
            //}
            //player.Draw(renderer);
            //renderer.End();

            var blendState = new BlendState()
            {
                AlphaBlendFunction = BlendFunction.ReverseSubtract,
                AlphaDestinationBlend = Blend.One,
                AlphaSourceBlend = Blend.BlendFactor,
                BlendFactor = new Color(170, 170, 130, 255),
                ColorBlendFunction = BlendFunction.ReverseSubtract,
                ColorDestinationBlend = Blend.One,
                ColorSourceBlend = Blend.BlendFactor,
                ColorWriteChannels = ColorWriteChannels.All,
                ColorWriteChannels1 = ColorWriteChannels.All,
                ColorWriteChannels2 = ColorWriteChannels.All,
                ColorWriteChannels3 = ColorWriteChannels.All,
                MultiSampleMask = -1
            };

            renderer.Begin(SpriteSortMode.BackToFront, blendState, _camera.GetMatrix());
            renderer.DrawRenderTarget(_backGroundCanvas, new Vector2(_canvasPositionX, _canvasPositionY), Color.White);
            renderer.End();

            renderer.Begin(_camera.GetMatrix());
            renderer.DrawRenderTarget(_foreGroundCanvas, new Vector2(_canvasPositionX, _canvasPositionY), Color.White);
            renderer.End();

            renderer.Begin(SpriteSortMode.FrontToBack, blendState, _camera.GetMatrix());
            renderer.DrawRenderTarget(_foreGroundShadowCanvas, new Vector2(_canvasPositionX, _canvasPositionY), Color.White);
            renderer.End();
        }
    }
}