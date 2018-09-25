using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Def;
using team22.Device;

namespace team22.Object
{
    class SaruRoom : Floor
    {
        private Vector2 doorPosition;
        private Vector2 returnPosition;

        public SaruRoom(GameDevice gameDevice)
            : base(81, 14, 22, 8, gameDevice)
        {
            doorPosition = new Vector2(FloorRange.X + 32 * 2, FloorRange.Y + 32 * 5.3f);
            returnPosition = new Vector2(32 * 40, 32 * 20);
            InsertObjects();
            
            SetLight();

            mapLighting = new MapLighting(81, 14, 9, 22, "saru_room", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        protected override void InsertObjects()
        {
            AddObject(new Door(doorPosition, 81, 14, returnPosition, new Vector2(1, 14), gameDevice, false));

            List<Key> saruKeys = new List<Key>() {
                new Key(new Vector2(32 * 3, 32 * 40), 81, 14, gameDevice), // 二階の鍵
                new Key(new Vector2(32 * 50, 32 * 18), 81, 14, gameDevice), // 四階の鍵
            };

            foreach (var x in saruKeys) GimmickInfo.List.Add(x);

            AddObject(new SaruButton(new Vector2(32 * 97.5f, FloorRange.Bottom - 32 * 2.5f), 81, 14, this, saruKeys, gameDevice));

            Hint hintSaru = new Hint("transparent", new Vector2(FloorRange.X + 32 * 4, FloorRange.Bottom - 32 * 2), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, true, gameDevice);
            hintSaru.KeyDialogue = "猿ボタンに気になる女";
            AddObject(hintSaru);
        }

        public override void SetLight()
        {
            _lightObjects.Add(new BackGroundLight("Saru_Room_light", new Vector2(32 * 81, 32 * 14), 81, 14, 32 * 8, 32 * 22, gameDevice));
        }

        public override void Draw(Renderer renderer, List<Player> player)
        {
            mapLighting.Draw(renderer, player);

        }

    }
}
