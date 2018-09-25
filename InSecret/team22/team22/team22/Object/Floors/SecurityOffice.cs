using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Object.Gimmick.GuardRoomGimmick;

namespace team22.Object
{
    class SecurityOffice : Floor
    {
        private Vector2 doorPosition;
        private Vector2 returnPosition;

        public SecurityOffice(GameDevice gameDevice)
            : base(57, 14, 22, 8, gameDevice)
        {
            doorPosition = new Vector2(FloorRange.X + 32 * 2, FloorRange.Y + 32 * 5.3f);
            returnPosition = new Vector2(32 * 20, 32 * 20);
            InsertObjects();

            SetLight();

            mapLighting = new MapLighting(57, 14, 9, 22, "security_room", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        protected override void InsertObjects()
        {
            AddObject(new Door(doorPosition, 57, 14, returnPosition, new Vector2(1, 14), gameDevice, false));
            AddObject(new HintsPoster(new Vector2(FloorRange.X + 32 * 7, FloorRange.Y + 32 * 5.8f), 57, 14, gameDevice));
            AddObject(new GuardComputer(new Vector2(FloorRange.X + 32 * 14, FloorRange.Y + 32 * 6), 57, 14, this, gameDevice));
        }

        public override void Draw(Renderer renderer, List<Player> player)
        {
            mapLighting.Draw(renderer, player);
        }

        public override void SetLight()
        {
            _lightObjects.Add(new BackGroundLight("SecurityRoomLights", new Vector2(FloorRange.X, FloorRange.Y), 57, 14, 288, 704, gameDevice));

            //_lightObjects.Add(new BackGroundLight(new Vector2(FloorRange.X + 32 * 6, FloorRange.Bottom - 32 * 4f), 1, 47, gameDevice));
        }
    }
}
