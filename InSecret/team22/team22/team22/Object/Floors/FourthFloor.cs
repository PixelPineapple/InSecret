using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Def;
using team22.Device;

namespace team22.Object
{
    class FourthFloor : Floor
    {
        private Vector2 saruRoom;
        private Vector2 securityOffice;

        private UpElevator upElevator;
        private DownElevator doElevator;

        public FourthFloor(GameDevice gameDevice)
            : base (1, 14, 54, 8, gameDevice)
        {
            securityOffice = new Vector2(32 * 79 - (32 * Screen.widthMax / 2), FloorRange.Y + (32 * Screen.heightMax / 2));

            saruRoom = new Vector2(32 * 103 - (32 * Screen.widthMax / 2), FloorRange.Y + (32 * Screen.heightMax / 2));
            InsertObjects();

            SetLight();

            mapLighting = new MapLighting(1, 14, 9, 54, "wall_floor_2", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        protected override void InsertObjects()
        {
            CameraFocusTarget upElevatorTarget = new CameraFocusTarget(new Vector2(32 * 4, 32 * 5), gameDevice);
            CameraFocusTarget doElevatorTarget = new CameraFocusTarget(new Vector2(32 * 50, 32 * 29), gameDevice);

            DownElevator downElevator = new DownElevator(new Vector2(32 * 49, 32 * 17.5f), 1, 14, gameDevice, doElevatorTarget);
            UpElevator upElevator = new UpElevator(new Vector2(32 * 3, 32 * 17.5f), 1, 14, gameDevice, upElevatorTarget);
            GimmickInfo.List.Add(downElevator);
            GimmickInfo.List.Add(upElevator);

            HideObject plant4 = new HideObject("plant1", "vase2_hide2", new Vector2(FloorRange.X + 32 * 46, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant4);
            

            HideObject vase4 = new HideObject("vase1", "vase1_hide2", new Vector2(FloorRange.X + 32 * 31.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase4);
            HideObject vase3 = new HideObject("vase3", "vase3_hide2", new Vector2(FloorRange.X + 32 * 28.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase3);
            HideObject vase2 = new HideObject("vase2", "vase2_hide2", new Vector2(FloorRange.X + 32 * 24.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase2);
            HideObject vase1 = new HideObject("vase4", "vase1_hide2", new Vector2(FloorRange.X + 32 * 16.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase1);

            HideObject plant2 = new HideObject("plant2", "vase2_hide2", new Vector2(FloorRange.X + 32 * 12.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant2);
            HideObject plant1 = new HideObject("plant3", "vase1_hide2", new Vector2(FloorRange.X + 32 * 7.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant1);


            AddObject(new Door(new Vector2(32 * 20, 32 * 19f), 1, 14, securityOffice, new Vector2(57, 14), gameDevice, true));
            AddObject(new Door(new Vector2(32 * 40, 32 * 19f), 1, 14, saruRoom, new Vector2(81, 14), gameDevice, true));

            EnemyManager enemy = new EnemyManager(gameDevice, this);
            enemy.Guard(new Vector2(32 * 15f, FloorRange.Bottom - 32 * 2), new Vector2(32 * 35f, FloorRange.Bottom - 32 * 2));
            enemy.Guard(new Vector2(32 * 27f, FloorRange.Bottom - 32 * 2), new Vector2(32 * 47f, FloorRange.Bottom - 32 * 2));
            enemy.CCTV(new Vector2(FloorRange.X + 32 * 38.5f, FloorRange.Bottom - 32 * 0.5f), new Vector2(FloorRange.X + 32 * 48.5f, FloorRange.Bottom - 32 * 0.5f));
            enemy.CCTV(new Vector2(FloorRange.X + 32 * 29, FloorRange.Bottom - 32 * 0.5f), new Vector2(FloorRange.X + 32 * 39, FloorRange.Bottom - 32 * 0.5f));
            enemy.CCTV(new Vector2(FloorRange.X + 32 * 14, FloorRange.Bottom - 32 * 0.5f), new Vector2(FloorRange.X + 32 * 24, FloorRange.Bottom - 32 * 0.5f));
            enemy.CCTV(new Vector2(FloorRange.X + 32 * 5, FloorRange.Bottom - 32 * 0.5f), new Vector2(FloorRange.X + 32 * 15, FloorRange.Bottom - 32 * 0.5f));
        }

        public override void Draw(Renderer renderer, List<Player> player)
        {
            // 背景画像
            mapLighting.Draw(renderer, player);
        }

        public override void SetLight()
        {

            _lightObjects.Add(new AnimatedBackgroundLight("plant1_light", new Vector2(FloorRange.X + 32 * 46, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));
            
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 31.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 28.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 24.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 16.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));

            _lightObjects.Add(new AnimatedBackgroundLight("plant2_light", new Vector2(FloorRange.X + 32 * 12.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("plant3_light", new Vector2(FloorRange.X + 32 * 7.5f, FloorRange.Bottom - 32 * 2.5f), 1, 14, 64, 32, gameDevice));


            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 3, 32 * 17.5f), 1, 14, gameDevice, upElevator));
            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 49, 32 * 17.5f), 1, 14, gameDevice, doElevator));

            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 4.8f, FloorRange.Bottom - 32 * 4f), 1, 47, gameDevice));

            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 18.8f, FloorRange.Bottom - 32 * 4f), 1, 47, gameDevice));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 25.8f, FloorRange.Bottom - 32 * 4f), 1, 47, gameDevice));

            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 39.8f, FloorRange.Bottom - 32 * 4f), 1, 47, gameDevice));
        }
    }
}
