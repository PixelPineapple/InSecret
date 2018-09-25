using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using team22.Utility;

namespace team22.Object
{
    class EnemyManager
    {
        private GuardMan guardMan;
        private Light light;
        private CCTVPoint cctv;
        private Laser laser;

        private GameDevice gameDevice;
        private Floor floor;

        public static bool CCTVFlag;
        public static bool LaserFlag;
        public static bool Flag = true;//Trueで監視カメラ(CCTV)稼働
        public static bool playerHit;

        private Timer timer;

        public EnemyManager(GameDevice gameDevice, Floor floor)
        {
            CCTVFlag = false;
            LaserFlag = false;

            timer = new Timer(0.3f);
            timer.Initialize();

            this.floor = floor;
            this.gameDevice = gameDevice;
            playerHit = false;
        }

        /// <summary>
        /// 警備員と懐中電灯の実体生成及びリストへの追加
        /// </summary>
        /// <param name="leftPosition"></param>
        /// <param name="rightPosition"></param>
        public void Guard(Vector2 leftPosition, Vector2 rightPosition)
        {
            guardMan = new GuardMan(leftPosition, rightPosition, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, light);
            light = new Light(guardMan.GetWorldPosition(), floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, guardMan);
            guardMan.GuardLight(light);


            floor.AddObject(guardMan);

            floor.AddLight(light.BackgroundLight);
            floor.AddObject(light);
        }


        /// <summary>
        /// 3F用警備員と懐中電灯の実体生成及びリストへの追加
        /// </summary>
        /// <param name="leftPosition"></param>
        /// <param name="rightPosition"></param>
        public void SuperGuard(Vector2 leftPosition, Vector2 rightPosition)
        {
            guardMan = new SuperGuardMan(leftPosition, rightPosition, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, light);
            light = new Light(guardMan.GetWorldPosition(), floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, guardMan);
            guardMan.GuardLight(light);
            
            GimmickInfo.superList.Add(guardMan);
            GimmickInfo.superList.Add(light);
        }

        /// <summary>
        /// 監視カメラの実体生成及びリストへの追加
        /// </summary>
        /// <param name="leftPosition"></param>
        /// <param name="rightPosition"></param>
        public void CCTV(Vector2 leftPosition, Vector2 rightPosition)
        {
            cctv = new CCTVPoint(leftPosition, rightPosition, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, floor);
            floor.AddObject(cctv);
        }

        /// <summary>
        /// 横移動するレーザーの実体生成及びリストへの追加
        /// </summary>
        /// <param name="leftPosition"></param>
        /// <param name="rightPosition"></param>
        public void Laser(Vector2 leftPosition, Vector2 rightPosition, Vector2 velocity)
        {
            laser = new Laser(leftPosition, rightPosition, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, velocity);
            floor.AddObject(laser);
        }

        /// <summary>
        /// 斜め移動するレーザーの実体生成及びリストへの追加
        /// </summary>
        /// <param name="leftPosition"></param>
        /// <param name="rightPosition"></param>
        public void DiagonalLaser(Vector2 leftPosition, Vector2 rightPosition)
        {
            laser = new Laser(leftPosition, rightPosition, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, 1f);
            floor.AddObject(laser);
        }

        /// <summary>
        /// 縦移動するレーザーの実体生成及びリストへの追加（壁を貫通するため使用できません）
        /// </summary>
        /// <param name="position"></param>
        //public void LengthLaser(Vector2 position)
        //{
        //    laser = new Laser(position, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice);
        //    floor.AddObject(laser);
        //}

        public void Update()
        {
            if (PlayerInfo.IsMove || !PlayerInfo.IsVisible)
            {
                if (CCTVFlag)
                {
                    timer.Update();
                    if (timer.IsTime())
                    {
                        timer.Initialize();
                        CCTVFlag = false;
                    }
                }

                if (LaserFlag)
                {
                    timer.Update();
                    if (timer.IsTime())
                    {
                        timer.Initialize();
                        LaserFlag = false;
                    }
                }
            }
        }
    }
}
