using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;


namespace team22.Scene
{
    class Load : IScene
    {
        #region Fields
        private bool isEnd;
        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;
        private FontLoader fontLoader;
        private GameDevice gameDevice;
        private int totalResouceNum;
        #endregion

        //Texture用収納リスト
        private string[,] Texturelist()
        {
            string path = "./Texture/";
            string floorPath = path + "Floor/";
            string miscObjectPath = path + "Misc_Objects/";
            string miscObjectLightPath = path + "Misc_Objects/Lights/";
            string pictureGimmickPath = path + "PictureGimmick/";
            string characterPath = path + "Character/";
            string diamondRosePath = path + "DiamondRoseGimmick/";
            string saruPath = path + "SaruGimmick/";
            string endingPath = path + "Ending/";
            string lockPath = path + "LockGimmick/";
            string backgroundLightPath = path + "BackGroundLight/";
            string[,] list = new string[,]
                {
                    {"1280black", path },

                    #region BackGroundLight
                    {"PinkRoseButton_Light", backgroundLightPath },
                    #endregion

                    #region タイトル
                    {"title", path },
                    {"select_cursor", path },
                    {"prov_titlename", path },
                    {"select", path },
                    {"rulebutton", path },
                    {"rule", path },
                    {"entrance", path },
                    {"star1", path },
                    {"star2", path },
                    {"star_gameclear", path },
                    #endregion
                    

                    {"light", path },
                    {"window_light_R", path },
                    {"window_light_L", path },
                    {"ParticleLight", path },
                    {"SecurityRoomLights", path },

                    {"block", path },
                    {"block2", path },
                    {"black", path },
                    {"Man_Model", path },
                    {"Woman_Model", path },
                    {"Boundaries_Ue", path },
                    {"Boundaries_Shita", path },
                    {"transparent", path },
                    {"door1", path },
                    {"door2", path },
                    {"nextButton", path },
                    {"Saru_Room_light", path },

                    #region フロアー
                    {"floor_num", floorPath },
                    {"wall_floor", floorPath },
                    {"wall_floor_2", floorPath },
                    {"wall_floor_5", floorPath },
                    {"saru_room", floorPath },
                    {"security_room", floorPath },
                    {"floor_first", floorPath },
                    {"floor_second", floorPath },
                    {"floor_third", floorPath },
                    {"floor_fourth", floorPath },
                    {"floor_fifth", floorPath },
                    {"floor_saru", floorPath },
                    {"floor_security", floorPath },
                    #endregion

                    #region 絵画ギミック / PictureGimmick
                    {"computer_select", pictureGimmickPath },
                    {"picture_flame", pictureGimmickPath },
                    {"pc", pictureGimmickPath },
                    {"pass", pictureGimmickPath },
                    {"alphabet_select", pictureGimmickPath},
                    {"pc_enter", pictureGimmickPath },
                    {"pc_enter_select", pictureGimmickPath },
                    {"pc_text", pictureGimmickPath },
                    {"unlock", pictureGimmickPath },
                    {"pc_hints", pictureGimmickPath },
                    {"picture_alphabet1", pictureGimmickPath },
                    {"picture_alphabet2", pictureGimmickPath },
                    {"picture_alphabet3", pictureGimmickPath },
                    {"picture_alphabet4", pictureGimmickPath },
                    {"picture_alphabet5", pictureGimmickPath },
                    {"lock_pc", pictureGimmickPath },
                    {"painting4", pictureGimmickPath },
                    {"painting4_LightCone", pictureGimmickPath },
                    {"painting6", pictureGimmickPath },
                    {"painting6_LightCone", pictureGimmickPath },
                    {"painting7", pictureGimmickPath },
                    {"painting7_LightCone", pictureGimmickPath },
                    {"painting8", pictureGimmickPath },
                    {"painting8_LightCone", pictureGimmickPath },
                    {"painting9", pictureGimmickPath },
                    {"painting9_LightCone", pictureGimmickPath },
                    {"painting10", pictureGimmickPath },
                    {"painting10_LightCone", pictureGimmickPath },
                    {"painting4_light", pictureGimmickPath },
                    {"painting6_light", pictureGimmickPath },
                    {"painting7_light", pictureGimmickPath },
                    {"painting8_light", pictureGimmickPath },
                    {"painting9_light", pictureGimmickPath },

                    #endregion

                    #region Lock ギミック / LockGimmick
                    {"Elevator_Close", lockPath },
                    {"Elevator_Open", lockPath },
                    {"elev", lockPath},
                    {"elevator", lockPath },
                    {"elevator_Light", lockPath },
                    #endregion

                    #region Security Office ギミック
                    {"Hacking_Terminal", path + "SecurityOfficeGimmick/" },
                    {"Virus", path + "SecurityOfficeGimmick/" },
                    {"Firewall", path + "SecurityOfficeGimmick/" },
                    {"LockedFolder", path + "SecurityOfficeGimmick/" },
                    {"Computer_Tutorial", path + "SecurityOfficeGimmick/" },
                    #endregion

                    #region 宝物のギミック / DiamondRoseGimmick
                    {"treasure", diamondRosePath },
                    {"glass_back", diamondRosePath },
                    {"glass_front", diamondRosePath },
                    {"PinkRoseButton", diamondRosePath },
                    #endregion

                    #region 猿ギミック / SaruGimmick
                    {"Saru", saruPath },
                    {"Saru_Button", saruPath },
                    {"Saru_Limbs", saruPath },
                    {"Saru_Setsumei", saruPath },
                    #endregion

                    #region Character
                    {"Men_Portrait", characterPath },
                    {"Woman_Portrait", characterPath },
                    {"vase1_hide1", characterPath },
                    {"vase1_hide2", characterPath },
                    {"vase2_hide1", characterPath },
                    {"vase2_hide2", characterPath },
                    {"vase3_hide1", characterPath },
                    {"vase3_hide2", characterPath },
                    #endregion

                    #region Misc_Objects
                    // 石像
                    {"rabbit_stone", miscObjectPath },
                    {"bird_stone", miscObjectPath },
                    {"monkey_stone", miscObjectPath },
                    {"cat_stone", miscObjectPath },
                    {"bear_stone", miscObjectPath },
                    {"raccoon_stone", miscObjectPath },
                    // vase
                    {"vase1", miscObjectPath },
                    {"vase2", miscObjectPath },
                    {"vase3", miscObjectPath },
                    {"vase4", miscObjectPath },
                    // 植物
                    {"plant1", miscObjectPath },
                    {"plant2", miscObjectPath },
                    {"plant3", miscObjectPath },
                    // 事務室
                    {"shelf1", miscObjectPath },
                    {"desk1", miscObjectPath },
                    {"desk2", miscObjectPath },
                    {"desk3", miscObjectPath },
                    {"desk4", miscObjectPath },
                    {"desk5", miscObjectPath },
                    {"desk6", miscObjectPath },
                    {"printer", miscObjectPath },
                    // Misc
                    {"poster", miscObjectPath },
                    {"poster_small", miscObjectPath },
                    {"Object_Light", miscObjectPath },
                    #endregion

                    #region Misc_Objects/Lights
                    {"plant1_light", miscObjectLightPath },
                    {"plant2_light", miscObjectLightPath },
                    {"plant3_light", miscObjectLightPath },
                    {"vase_light", miscObjectLightPath },
                    #endregion

                    {"gameclear", endingPath },
                    {"gameclear_heri1", endingPath },
                    {"gameclear_heri2", endingPath },
                    {"gameclear_heri3", endingPath },
                    {"gameclearbutton", endingPath },
                    {"gameclearname", endingPath },
                    {"star_gameclear1", endingPath },
                    {"star_gameclear2", endingPath },

                    #region Particles
                    {"OutliningParticle", path + "Particle/" },
                    {"ShiningParticle", path + "Particle/" },
                    #endregion

                    #region エネミー
                    {"GuardMan", path + "Enemy/" },
                    {"light1", path + "Enemy/" },
                    {"light1_mask", path + "Enemy/" },
                    {"light2", path + "Enemy/" },
                    {"light2_mask", path + "Enemy/" },
                    {"CCTVL", path + "Enemy/" },
                    {"CCTVM", path + "Enemy/" },
                    {"CCTVpoint", path + "Enemy/" },
                    {"CCTVR", path + "Enemy/" },
                    {"laser", path + "Enemy/" },
                    {"questionMark", path + "Enemy/" },
                    {"exclamationMark", path + "Enemy/" },
                    {"Super_Guardman", path + "Enemy/" },
                    #endregion

                    #region GameOver
                    {"gameover", path + "GameOver/" },
                    {"gameover_lattice", path + "GameOver/" },
                    {"gameoverbutton", path + "GameOver/" },
                    {"gameovername", path + "GameOver/" },
                    #endregion

                };

            return list;
        }

        //BGM用収納リスト
        private string[,] BGMList()
        {
            string path = "./BGM/";
            string[,] list = new string[,]
                {
                    {"title_bgm", path },
                    {"gameplay_bgm",path },
                    {"gameover",path },
                };
            return list;
        }

        //SE用収納リスト
        private string[,] SEList()
        {
            string path = "./SE/";
            string[,] list = new string[,]
                {
                    {"click", path },
                    {"cat", path },
                    {"page",path },
                    {"warning",path },
                    {"man_walk", path },
                    {"door", path },
                    {"pc_move", path },
                    {"pc_ok", path },
                    {"pc_button", path },
                    {"pc_no", path },
                    {"metal", path },
                    {"autodoor", path },
                };

            return list;
        }

        //Font用収納リスト
        private string[,] FontList()
        {
            string path = "./Font/";
            string[,] list = new string[,]
            {
                {"dialogue", path },
            };
            return list;
        }


        public Load(GameDevice gameDevice)
        {
            //画像読み込みオブジェクトの実体生成
            textureLoader = new TextureLoader(gameDevice.Renderer, Texturelist());
            bgmLoader = new BGMLoader(gameDevice.Sound, BGMList());
            seLoader = new SELoader(gameDevice.Sound, SEList());
            fontLoader = new FontLoader(gameDevice.Font, FontList());
            this.gameDevice = gameDevice;
        }

        public void Draw(Renderer renderer)
        {
            //gameDevice.Renderer.DrawTexture("load", new Vector2(20, 20));

            //終了判定
            if (textureLoader.IsEnd()
             && bgmLoader.IsEnd() 
             && seLoader.IsEnd()
             && fontLoader.IsEnd())
            {
                isEnd = true;
            }
        }

        public void Initialize()
        {
            isEnd = false;

            //画像読み込みオブジェクトを初期化
            textureLoader.Initialize();
            bgmLoader.Initialize();
            seLoader.Initialize();
            fontLoader.Initialize();

            //全リソース数を計算
            totalResouceNum = textureLoader.Count() + 
                bgmLoader.Count() + 
                seLoader.Count() + 
                fontLoader.Count();
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public SceneType Next()
        {
            return SceneType.Logo;
        }

        public void Update(GameTime gameTime)
        {
            //画像読み込みが終了していないか？
            if (!textureLoader.IsEnd())
            { textureLoader.Update(); }
            else if (!bgmLoader.IsEnd())
            { bgmLoader.Update(); }
            else if (!seLoader.IsEnd())
            { seLoader.Update(); }
            else if (!fontLoader.IsEnd())
            { fontLoader.Update(); }
        }

        public void Shutdown()
        {

        }
    }
}
