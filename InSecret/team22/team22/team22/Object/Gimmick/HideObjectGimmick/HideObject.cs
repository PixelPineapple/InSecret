using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;
using team22.Object.Effect;

namespace team22.Object
{
    class HideObject : GameObject
    {
        private InputState input;
        private string playerName;
        private float playerAlpha;
        private bool isHide;
        private ParticleManager _particleManager;
        private Floor _floor;

        public HideObject(string name, string playerName, Vector2 position, int floorWidth, int floorHeight, int height, int width, GameDevice gameDevice)
            : base(name, position, floorWidth, floorHeight, height, width, gameDevice)
        {
            input = gameDevice.InputState;
            this.playerName = playerName;
            isHide = false;
            drawingPosition = new Vector2(drawingPosition.X, drawingPosition.Y - height);
            worldPosition = new Vector2(worldPosition.X, worldPosition.Y - height);
            playerAlpha = 0;
            _particleManager = new ParticleManager();
        }

        public HideObject(string name, string playerName, Vector2 position, int floorWidth, int floorHeight, int height, int width, Floor floor, GameDevice gameDevice)
            : base(name, position, floorWidth, floorHeight, height, width, gameDevice)
        {
            input = gameDevice.InputState;
            this.playerName = playerName;
            isHide = false;
            drawingPosition = new Vector2(drawingPosition.X, drawingPosition.Y - height);
            worldPosition = new Vector2(worldPosition.X, worldPosition.Y - height);
            playerAlpha = 0;
            _floor = floor;
            _particleManager = new ParticleManager();
        }

        public HideObject(HideObject other)
            : this(other.name, other.playerName, other.worldPosition, other.height, other.width, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice)
        { }

        public override object Clone()
        {
            return new HideObject(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (!(gameObject is Player)) return;

            _particleManager.Add(new ShinyParticle(new Vector2(
                    gameDevice.Random.Next((int)drawingPosition.X, (int)drawingPosition.X + width),
                    gameDevice.Random.Next((int)drawingPosition.Y, (int)drawingPosition.Y + height)), Color.LightGoldenrodYellow));

            Player player = (Player)gameObject;

            if (isHide && !player.IsJump() && 
                input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A))
            {
                PlayerInfo.IsVisible = true;
                //player.ShouldBeDrawn = true;
                PlayerInfo.IsMove = true;
                isHide = false;
                playerAlpha = 0.0f;
            }
            else if (!isHide && !player.IsJump() && PlayerInfo.IsMove &&
                input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A))
            {
                PlayerInfo.IsVisible = false;
                //player.ShouldBeDrawn = false;
                PlayerInfo.IsMove = false;
                isHide = true;
                playerAlpha = 1.0f;
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            _particleManager.Update();
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(playerName, drawingPosition, playerAlpha);
            
            base.Draw(renderer);

            _particleManager.Draw(renderer);
        }
    }
}
