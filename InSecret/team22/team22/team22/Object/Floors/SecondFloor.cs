using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    class SecondFloor : Floor
    {
        private UpElevator upElevator;
        private DownElevator doElevator;

        public SecondFloor(GameDevice gameDevice)
            : base (1, 36, 55, 8, gameDevice)
        {
            InsertObjects();

            SetLight();

            mapLighting = new MapLighting(1, 36, 9, 55, "wall_floor_2", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }
        protected override void InsertObjects()
        {
            // 下に行くエレベーター
            CameraFocusTarget doElevatorTarget = new CameraFocusTarget(new Vector2(32 * 49, 32 * 50f), gameDevice);

            GimmickInfo.List.Add(new DownElevator(new Vector2(32 * 49, 32 * 39.5f), 1, 36, gameDevice, doElevatorTarget));
            // 上に行くエレベーター
            CameraFocusTarget upElevatorTarget = new CameraFocusTarget(new Vector2(32 * 4, 32 * 27), gameDevice);
            UpElevator upElevator = new UpElevator(new Vector2(32 * 3, 32 * 39.5f), 1, 36, gameDevice, upElevatorTarget);
            GimmickInfo.List.Add(upElevator);
            //AddObject(upElevator);

            Hint hintElevator = new Hint("transparent", new Vector2(32 * 3, 32 * 41f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, false, upElevator, gameDevice);
            hintElevator.KeyDialogue = "二回猿ボタン男";
            AddObject(hintElevator);


            HideObject vase4 = new HideObject("vase4", "vase1_hide1", new Vector2(32 * 46f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase4);
            HideObject vase3 = new HideObject("vase3", "vase3_hide1", new Vector2(32 * 41f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase3);

            HideObject cat = new HideObject("cat_stone", "vase1_hide1", new Vector2(32 * 34.5f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(cat);
            HideObject raccoon = new HideObject("raccoon_stone", "vase2_hide1", new Vector2(32 * 31.5f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(raccoon);
            HideObject rabbit = new HideObject("rabbit_stone", "vase1_hide1", new Vector2(32 * 28.5f, 32 * 43f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(rabbit);
            HideObject bird = new HideObject("bird_stone", "vase1_hide1", new Vector2(32 * 25.5f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(bird);
            HideObject bear = new HideObject("bear_stone", "vase2_hide1", new Vector2(32 * 21.5f, 32 * 43f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(bear);
            HideObject monkey = new HideObject("monkey_stone", "vase3_hide1", new Vector2(32 * 18.5f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(monkey);

            HideObject vase2 = new HideObject("vase2", "vase2_hide1", new Vector2(32 * 13f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase2);
            HideObject vase1 = new HideObject("vase1", "vase1_hide1", new Vector2(32 * 8f, 32 * 43.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase1);
            
            // ヒント１
            Hint hint1 = new Hint("transparent", new Vector2(32 * 40.5f, 32 * 41.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, true, gameDevice);
            hint1.KeyDialogue = "赤外線レーザー女";
            AddObject(hint1);

            EnemyManager enemy = new EnemyManager(gameDevice, this);
            enemy.Laser(new Vector2(32 * 35.5f, 32 * 39.5f), new Vector2(32 * 37f, 32 * 43.5f), new Vector2(0, 1));
            enemy.Laser(new Vector2(32 * 18.5f, 32 * 42.5f), new Vector2(32 * 27, 32 * 42.5f), new Vector2(1, 0));
            enemy.Laser(new Vector2(32 * 27, 32 * 43f), new Vector2(32 * 34f, 32 * 43f), new Vector2(1, 0));
            enemy.Laser(new Vector2(32 * 17f, 32 * 39.5f), new Vector2(32 * 17, 32 * 43.5f), new Vector2(0, 1));
            enemy.Guard(new Vector2(32 * 6f, 32 * 42f), new Vector2(32 * 15f, 32 * 42f));
        }

        public override void Draw(Renderer renderer, List<Player> players)
        {
            //背景画像
            mapLighting.Draw(renderer, players);
        }

        public override void SetLight()
        {
            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 3, 32 * 39.5f), 1, 36, gameDevice, upElevator));
            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 49, 32 * 39.5f), 1, 36, gameDevice, doElevator));
        }
    }
}
