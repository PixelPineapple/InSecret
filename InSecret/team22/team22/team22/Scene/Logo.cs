using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Def;

namespace team22.Scene
{
    class Logo : IScene
    {
        private CountDownTimer timer;
        private bool endFlag;

        public Logo(GameDevice gameDevice)
        {
            timer = new CountDownTimer(2);
            endFlag = false;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("1280black", new Vector2(Screen.WidthHalf - 640, Screen.HeightHalf - 360));
            renderer.End();
        }

        public void Initialize()
        {
            timer.Initialize();
            endFlag = false;
        }

        public bool IsEnd()
        {
            return endFlag;
        }

        public SceneType Next()
        {
            return SceneType.Title;
        }

        public void Shutdown()
        {

        }

        public void Update(GameTime gameTime)
        {
            timer.Update();

            if (timer.IsTime())
            { endFlag = true; }
        }
    }
}
