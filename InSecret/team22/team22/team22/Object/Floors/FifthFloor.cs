#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;
#endregion

namespace team22.Object
{
    /// <summary>
    /// 5階のフロアー
    /// </summary>
    class FifthFloor : Floor
    {
        private DownElevator doElevator;

        public FifthFloor(GameDevice gameDevice) 
            : base (1, 3, 54, 8, gameDevice)
        {
            InsertObjects(); SetLight();

            mapLighting = new MapLighting(1, 3, 9, 54, "wall_floor_5", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        protected override void InsertObjects()
        {
            // AddObjectメソッドでゲームオブジェクトを追加する。
            Key key = new Key(new Vector2(32 * 50, 32 * 51), 1, 3, gameDevice);
            
            CameraFocusTarget doElevatorTarget = new CameraFocusTarget(new Vector2(32 * 3, 32 * 18), gameDevice);
            doElevator = new DownElevator(new Vector2(32 * 3, 32 * 6.5f), 1, 3, gameDevice, doElevatorTarget);
            
            GimmickInfo.List.Add(doElevator);
            GimmickInfo.List.Add(key);

            Hint shelf = new Hint("shelf1", new Vector2(FloorRange.X + 32 * 47, FloorRange.Bottom - 32 * 3f), FloorRange.X / 32, FloorRange.Y / 32, 87, 48, false, gameDevice);
            AddObject(shelf);

            HideObject desk4 = new HideObject("desk4", "vase1_hide2", new Vector2(FloorRange.X + 32 * 38, FloorRange.Bottom + 3), (int)FloorRange.X / 32, (int)FloorRange.Y / 32, 84, 96, gameDevice);
            AddObject(desk4);
            HideObject desk5 = new HideObject("desk5", "vase2_hide2", new Vector2(FloorRange.X + 32 * 34, FloorRange.Bottom + 3), (int)FloorRange.X / 32, (int)FloorRange.Y / 32, 84, 96, gameDevice);
            AddObject(desk5);
            HideObject desk6 = new HideObject("desk6", "vase2_hide2", new Vector2(FloorRange.X + 32 * 22, FloorRange.Bottom + 3), (int)FloorRange.X / 32, (int)FloorRange.Y / 32, 84, 96, gameDevice);
            AddObject(desk6);
            Hint desk2 = new Hint("desk1", new Vector2(FloorRange.X + 32 * 18, FloorRange.Bottom - 32 * 3f), FloorRange.X / 32, FloorRange.Y / 32, 84, 96, false, gameDevice);
            AddObject(desk2);

            HideObject plant1 = new HideObject("plant1", "vase1_hide2", new Vector2(FloorRange.X + 32 * 17, FloorRange.Bottom - 32 * 0.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant1);
            
            //PCテスト実装
            Computer computer = new Computer(new Vector2(32 * 10, FloorRange.Bottom - 32 * 2), 1, 3, gameDevice, this, key);
            AddObject(computer);

            HintsComputer hintCom = new HintsComputer(new Vector2(32 * 13, FloorRange.Bottom - 32 * 3f), 1, 3, gameDevice);
            AddObject(hintCom);
            Hint hintComputer = new Hint("transparent", new Vector2(32 * 13, FloorRange.Bottom - 32 * 2), 1, 3, 64, 32, true, gameDevice);
            hintComputer.KeyDialogue = "エレベーターのパス女";

            AddObject(hintComputer);

            // エネミーを生成
            EnemyManager enemy = new EnemyManager(gameDevice, this);
            enemy.Guard(new Vector2(32 * 15f, FloorRange.Bottom - 32 * 2), new Vector2(32 * 20.5f, FloorRange.Bottom - 32 * 2));
            enemy.CCTV(new Vector2(FloorRange.X + 32 * 22, FloorRange.Bottom - 32 * 0.5f), new Vector2(FloorRange.X + 32 * 39, FloorRange.Bottom - 32 * 1f));

            //enemy.SuperGuard();

            //CameraMoveEvent lookingAtCCTV = new CameraMoveEvent(new Vector2(FloorRange.X + 32 * 47, FloorRange.Bottom - 32 * 1f), gameDevice, printer);
            //lookingAtCCTV.KeyDialogue = "監視カメラ男";
            //AddObject(lookingAtCCTV);
        }

        public override void Draw(Renderer renderer, List<Player> player)
        {
            mapLighting.Draw(renderer, player);
        }

        public override void SetLight()
        {
            _lightObjects.Add(new AnimatedBackgroundLight("plant1_light", new Vector2(FloorRange.X + 32 * 17, FloorRange.Bottom - 32 * 2.5f), 1, 3, 64, 32, gameDevice));

            _lightObjects.Add(new BackGroundLight("window_light_R", new Vector2(32 * 41, 32 * 4.5f), 1, 3, 211, 188, gameDevice));
            _lightObjects.Add(new BackGroundLight("window_light_L", new Vector2(32 * 11, 32 * 4.3f), 1, 3, 211, 188, gameDevice));
            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 3, 32 * 6.5f), 1, 3, gameDevice, doElevator));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 11.8f, 32 * 49), 1, 47, gameDevice));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 18.8f, 32 * 49), 1, 47, gameDevice));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 25.8f, 32 * 49), 1, 47, gameDevice));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 32.8f, 32 * 49), 1, 47, gameDevice));
            //_lightObjects.Add(new BackGroundLight(new Vector2(32 * 39.8f, 32 * 49), 1, 47, gameDevice));
        }
    }
}
