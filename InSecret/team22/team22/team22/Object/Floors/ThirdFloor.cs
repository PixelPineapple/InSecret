using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Object.Gimmick.DiamondRoseGimmick;

namespace team22.Object
{
    class ThirdFloor : Floor
    {
        #region Fields
        private List<ParallelButton> _diamondRoseKeys;

        private UpElevator upElevator;
        private DownElevator doElevator;
        #endregion

        public ThirdFloor(GameDevice gameDevice)
            : base (1, 25, 54 , 8, gameDevice)
        {
            InsertObjects();

            SetLight();

            mapLighting = new MapLighting(1, 25, 9, 54, "wall_floor_2", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        protected override void InsertObjects()
        {
            _diamondRoseKeys = new List<ParallelButton>()
            {
                new ParallelButton(new Vector2 (32 * 21, 32 * 31f), 1, 25, 64, 32, gameDevice),
                new ParallelButton(new Vector2 (32 * 33, 32 * 31f), 1, 25, 64, 32, gameDevice),
            };
            AddObject(new DiamondRose(new Vector2(32 * 27, 32 * 30.5f), 1, 25, _diamondRoseKeys, gameDevice, this)); // 三階に最初のゲームオブジェクトにならなきゃ。

            
            Hint hint1 = new Hint("transparent", new Vector2(32 * 21f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, true, gameDevice);
            hint1.KeyDialogue = "三階早く来て男";
            AddObject(hint1);

            Hint tokubetsuKeibin =  new Hint("transparent", new Vector2(32 * 3.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, true, gameDevice);
            tokubetsuKeibin.KeyDialogue = "3階の警備員男";
            AddObject(tokubetsuKeibin);

            Hint hint2 = new Hint("transparent", new Vector2(32 * 33f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, true, gameDevice);
            hint2.KeyDialogue = "三階早く来て女";
            AddObject(hint2);

            HideObject plant4 = new HideObject("plant3", "vase3_hide2", new Vector2(32 * 46f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant4);
            
            HideObject vase3 = new HideObject("vase3", "vase3_hide2", new Vector2(FloorRange.X + 32 * 41, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase3);
            HideObject vase1 = new HideObject("vase1", "vase1_hide2", new Vector2(FloorRange.X + 32 * 37.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase1);

            CameraFocusTarget upElevatorTarget = new CameraFocusTarget(new Vector2(32 * 3, 32 * 40), gameDevice);
            CameraFocusTarget doElevatorTarget = new CameraFocusTarget(new Vector2(32 * 50, 32 * 17f), gameDevice);

            GimmickInfo.List.Add(new DownElevator(new Vector2(32 * 3, 32 * 28.5f), 1, 25, gameDevice, upElevatorTarget));
            GimmickInfo.List.Add(new UpElevator(new Vector2(32 * 49, 32 * 28.5f), 1, 25, gameDevice, doElevatorTarget));

            foreach (var x in _diamondRoseKeys)
            {
                AddObject(x);
            }

            HideObject vase4 = new HideObject("vase4", "vase1_hide1", new Vector2(FloorRange.X + 32 * 15.5f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase4);
            HideObject vase2 = new HideObject("vase2", "vase2_hide1", new Vector2(FloorRange.X + 32 * 12f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(vase2);

            HideObject plant3 = new HideObject("plant3", "vase3_hide1", new Vector2(32 * 8f, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant3);

            EnemyManager enemy = new EnemyManager(gameDevice, this);
            enemy.SuperGuard(new Vector2(FloorRange.X + 32 * 13, FloorRange.Bottom - 32 * 2f), new Vector2(FloorRange.X + 32 * 17, FloorRange.Bottom - 32 * 2f));
        }

        //public override void Draw(Renderer renderer, List<Player> player)
        //{
        //    // 背景画像
        //    mapLighting.Draw(renderer, player);
        //}

        public override void SetLight()
        {
            _lightObjects.Add(new AnimatedBackgroundLight("PinkRoseButton_Light", new Vector2(32 * 21, 32 * 31f), 1, 25, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("PinkRoseButton_Light", new Vector2(32 * 33, 32 * 31f), 1, 25, 64, 32, gameDevice));

            _lightObjects.Add(new AnimatedBackgroundLight("plant3_light", new Vector2(32 * 46f, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 41, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 37.5f, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));

            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 15.5f, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("vase_light", new Vector2(FloorRange.X + 32 * 12f, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("plant3_light", new Vector2(32 * 8f, FloorRange.Bottom - 32 * 2.5f), 1, 25, 64, 32, gameDevice));

            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 49, 32 * 28.5f), 1, 25, gameDevice, upElevator));
            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 3, 32 * 28.5f), 1, 25, gameDevice, doElevator));
        }
    }
}
