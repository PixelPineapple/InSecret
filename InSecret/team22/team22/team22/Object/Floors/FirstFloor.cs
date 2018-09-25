#region Using Statements
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using team22.Device;
using team22.Object.Effect;
#endregion

namespace team22.Object
{  
    /// <summary>
    /// 1階のフロアーを管理するクラス
    /// </summary>
    class FirstFloor : Floor
    {
        private PictureGimmickManager pgm;
        private UpElevator upElevator;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FirstFloor (GameDevice gameDvice)
            : base (1, 47, 54, 8, gameDvice)
        {
            InsertObjects();

            SetLight();

            mapLighting = new MapLighting(1, 47, 9, 54, "wall_floor", gameDevice.Camera, GetGameObjectsList(), _lightObjects);
            mapLighting.InitializeRenderTarget(gameDevice.Renderer);
        }

        /// <summary>
        /// 一階に何があるか？
        /// </summary>5
        protected override void InsertObjects()
        {
            // AddObjectメソッドでゲームオブジェクトを追加する。
            CameraFocusTarget elevatorTarget = new CameraFocusTarget(new Vector2(32 * 51, 32 * 40f), gameDevice);

            UpElevator upElevator = new UpElevator(new Vector2(32 * 49, 32 * 50.5f), 1, 47, gameDevice, elevatorTarget);
            GimmickInfo.List.Add(upElevator);

            //AddObject(upElevator);
            //絵画の生成
            pgm = new PictureGimmickManager(this, gameDevice);

            // ヒント１
            Hint hint1 = new Hint("raccoon_stone", new Vector2(32 * 4, 32 * 52.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, false, gameDevice);
            hint1.KeyDialogue = "ファースト女";
            AddObject(hint1);

            Hint hintElevator = new Hint("transparent", new Vector2(32 * 51, 32 * 52.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, false, upElevator,gameDevice);
            hintElevator.KeyDialogue = "エレベーターの仕掛け男";
            AddObject(hintElevator);

            HideObject plant1 = new HideObject("plant3", "vase3_hide1", new Vector2(32 * 11f, 32 * 54.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant1);

            HideObject plant2 = new HideObject("plant1", "vase1_hide1", new Vector2(32 * 24f, 32 * 54.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant2);

            HideObject plant3 = new HideObject("plant3", "vase3_hide1", new Vector2(32 * 37.5f, 32 * 54.5f), FloorRange.X / 32, FloorRange.Y / 32, 64, 32, gameDevice);
            AddObject(plant3);

            // エネミーを生成
            EnemyManager enemy = new EnemyManager(gameDevice, this);
            enemy.Guard(new Vector2(32 * 21.5f, 32 * 53), new Vector2(32 * 40.5f, 32 * 53));

            CameraMoveEvent lookingAtEnemy = new CameraMoveEvent(new Vector2(32 * 11f, 32 * 53), gameDevice, plant3);
            lookingAtEnemy.KeyDialogue = "警備員女";
            AddObject(lookingAtEnemy);
        }

        public override void Draw(Renderer renderer, List<Player> players)
        {
            mapLighting.Draw(renderer, players);
        }

        public override void SetLight()
        {
            _lightObjects.Add(new AnimatedBackgroundLight("plant3_light", new Vector2(32 * 11f, 32 * 52.5f), 1, 47, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("plant1_light", new Vector2(32 * 24f, 32 * 52.5f), 1, 47, 64, 32, gameDevice));
            _lightObjects.Add(new AnimatedBackgroundLight("plant3_light", new Vector2(32 * 37.5f, 32 * 52.5f), 1, 47, 64, 32, gameDevice));

            
            for (int index = 0; index < 6; index++)
            {
                if (pgm.GetPicturesList()[index].Name.Equals("painting4") || pgm.GetPicturesList()[index].Name.Equals("painting6"))
                {
                    _lightObjects.Add(new BackGroundLight(pgm.GetPicturesList()[index].Name + "_LightCone", new Vector2
                    (
                        pgm.GetPicturesList()[index].GetWorldPosition().X + (pgm.GetPicturesList()[index].GetPicWidth() / 2) - 42.5f,
                        pgm.GetPicturesList()[index].GetWorldPosition().Y - pgm.GetPicturesList()[index].GetPicHeight() + 7 /* lamp size */
                    )
                    , 1, 47, 93, 85, gameDevice));
                }
                else if (pgm.GetPicturesList()[index].Name.Equals("painting7") || pgm.GetPicturesList()[index].Name.Equals("painting8"))
                {
                    _lightObjects.Add(new BackGroundLight(pgm.GetPicturesList()[index].Name + "_LightCone", new Vector2
                    (
                        pgm.GetPicturesList()[index].GetWorldPosition().X + (pgm.GetPicturesList()[index].GetPicWidth() / 2) - 37f,
                        pgm.GetPicturesList()[index].GetWorldPosition().Y - pgm.GetPicturesList()[index].GetPicHeight() + 7 /* lamp size */
                    )
                    , 1, 47, 131, 74, gameDevice));
                }
                else if (pgm.GetPicturesList()[index].Name.Equals("painting9"))
                {
                    _lightObjects.Add(new BackGroundLight(pgm.GetPicturesList()[index].Name + "_LightCone", new Vector2
                    (
                        pgm.GetPicturesList()[index].GetWorldPosition().X + (pgm.GetPicturesList()[index].GetPicWidth() / 2) - 60.5f,
                        pgm.GetPicturesList()[index].GetWorldPosition().Y - pgm.GetPicturesList()[index].GetPicHeight() + 7 /* lamp size */
                    )
                    , 1, 47, 128, 121, gameDevice));
                }
                else if (pgm.GetPicturesList()[index].Name.Equals("painting10"))
                {
                    _lightObjects.Add(new BackGroundLight(pgm.GetPicturesList()[index].Name + "_LightCone", new Vector2
                    (
                        pgm.GetPicturesList()[index].GetWorldPosition().X + (pgm.GetPicturesList()[index].GetPicWidth() / 2) - 49.5f,
                        pgm.GetPicturesList()[index].GetWorldPosition().Y - pgm.GetPicturesList()[index].GetPicHeight() + 7 /* lamp size */
                    )
                    , 1, 47, 107, 99, gameDevice));
                }

                _lightObjects.Add(new BackGroundLight("light", new Vector2
                    (
                        pgm.GetPicturesList()[index].GetWorldPosition().X + (pgm.GetPicturesList()[index].GetPicWidth() / 2) - 100,
                        pgm.GetPicturesList()[index].GetWorldPosition().Y - pgm.GetPicturesList()[index].GetPicHeight() + 7 /* lamp size */
                    )
                    , 1, 47, 200, 200, gameDevice));
            }

            _lightObjects.Add(new ElevatorLight(new Vector2(32 * 49, 32 * 50.5f), 1, 47, gameDevice, upElevator));
        }

    }
}
