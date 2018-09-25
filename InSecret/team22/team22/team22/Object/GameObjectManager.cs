#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using team22.Device;
using Microsoft.Xna.Framework.Input;
using team22.Device.EventDialogue;
#endregion

namespace team22.Object
{
    /// <summary>
    /// ゲームオブジェクトを管理するクラス
    /// </summary>
    class GameObjectManager
    {
        #region Fields
        private GameDevice gameDevice;
        private InputState input;
        private Camera2D camera;
        private Map map;
        private FloorNumber floorNum;

        private List<Floor> floorList;
        private List<Player> players;
        private List<GameObject> lockList;
        private List<GameObject> superList;
        private int playerNum;
        private float cameraSpeed;

        private bool isMan;

        private DialogueReader _dialogueReader;
        private Dialogue _dialogue;
        #endregion

        #region Getter Setter メソッド
        public Floor GetFloor(int index)
        {
            return floorList[index];
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameObjectManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            input = gameDevice.InputState;
            camera = gameDevice.Camera;

            playerNum = 0;
            cameraSpeed = 15;

            isMan = true;

            _dialogueReader = new DialogueReader("Master_会話.txt");
            _dialogue = new Dialogue(gameDevice.Font, gameDevice);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            GimmickInfo.List.Clear();
            GimmickInfo.SuperList.Clear();
            if (players != null)
            {
                players.Clear();
            }
            else
            {
                players = new List<Player>();
            }
            playerNum = 0;

            if (floorList != null)
            {
                floorList.Clear();
                RegisterFloor();
            }
            else
            {
                floorList = new List<Floor>();
                RegisterFloor();
            }

            if (lockList != null)
            {
                lockList = GimmickInfo.List;
            }
            else
            {
                lockList = new List<GameObject>();
                lockList = GimmickInfo.List;
            }

            if (superList != null)
            {
                superList = GimmickInfo.SuperList;
            }
            else
            {
                superList = new List<GameObject>();
                superList = GimmickInfo.SuperList;
            }

            camera.Position = new Vector2(32 * 12 + 16, floorList[0].FloorRange.Bottom - 32);
            floorNum = new FloorNumber(new Vector2(16, 48));
            isMan = true;
            PlayerInfo.IsMan = true;
            PlayerInfo.IsMove = true;
            PlayerInfo.IsVisible = true;
            PlayerInfo.ForceSwitch = false;
            PlayerInfo.IsSwitchable = true;
            PlayerInfo.AutoMove = false;
        }

        private void RegisterFloor()
        {
            floorList.Add(new FirstFloor(gameDevice));
            floorList.Add(new SecondFloor(gameDevice));
            floorList.Add(new ThirdFloor(gameDevice));
            floorList.Add(new FourthFloor(gameDevice));
            floorList.Add(new FifthFloor(gameDevice));
            floorList.Add(new SecurityOffice(gameDevice));
            floorList.Add(new SaruRoom(gameDevice));
        }

        /// <summary>
        /// プレイヤーを追加
        /// </summary>
        /// <param name="obj"></param>
        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        /// <summary>
        /// マップの追加
        /// </summary>
        /// <param name="map"></param>
        public void AddMap(Map map)
        {
            if (map == null) return;

            this.map = map;
        }

        /// <summary>
        /// マップとプレイやーの衝突
        /// </summary>
        public void HitMap()
        {
            if (map == null) return;

            foreach (var p in players)
            {
                map.Hit(p);
            }
        }

        /// <summary>
        /// playerとの衝突判定
        /// </summary>
        public void HitGameObjects()
        {
            //各フロアと
            foreach (Floor nankai in floorList)
            {
                if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
                {
                    foreach (var o in nankai.GetGameObjectsList())
                    {
                        if (o.IsDead() || players[0].IsDead()) continue;
                        if (o.Collision(players[0]))
                        {
                            //ヒット通知
                            o.Hit(players[0]);
                            players[0].Hit(o);
                            if (o.KeyDialogue != null)
                            {
                                _dialogue.SetTexts(_dialogueReader.getDialogue(o.KeyDialogue), o.KeyDialogue.Contains("男"));
                            }
                        }
                    }
                }
                else if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
                {
                    foreach (var o in nankai.GetGameObjectsList())
                    {
                        if (o.IsDead() || players[1].IsDead()) continue;
                        if (o.Collision(players[1]))
                        {
                            //ヒット通知
                            o.Hit(players[1]);
                            players[1].Hit(o);
                            if (o.KeyDialogue != null)
                            {
                                _dialogue.SetTexts(_dialogueReader.getDialogue(o.KeyDialogue), o.KeyDialogue.Contains("男"));
                            }
                        }
                    }
                }
            }
            //Lock関連オブジェクト
            foreach (var l in lockList)
            {
                if (l.IsDead() || players[playerNum].IsDead()) continue;
                if (l.Collision(players[playerNum]))
                {
                    //ヒット通知
                    l.Hit(players[playerNum]);
                    players[playerNum].Hit(l);
                }
            }
            foreach (var l1 in lockList)
            {
                foreach (var l2 in lockList)
                {
                    if (l1.IsDead() || l2.IsDead()) continue;
                    if (l1.Collision(l2))
                    {
                        //ヒット通知
                        l1.Hit(l2);
                        l2.Hit(l1);
                    }
                }
            }

            foreach (var l in superList)
            {
                if (l.IsDead() || players[playerNum].IsDead()) continue;
                if (l.Collision(players[playerNum]))
                {
                    l.Hit(players[playerNum]);
                    players[playerNum].Hit(l);
                }
            }
        }

        /// <summary>
        /// 不要なプレイヤーを削除
        /// </summary>
        private void RemoveDead()
        {
            players.RemoveAll(p => p.IsDead());
            lockList.RemoveAll(l => l.IsDead());
            superList.RemoveAll(x => x.IsDead());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
                if (players.Count != 0 && !camera.IsMoving && PlayerInfo.IsMove && !PlayerInfo.AutoMove)
                {
                    players[playerNum].Update(gameTime);
                }

                //players[playerNum].UpdateDialogue();

                ObjectUpdate(gameTime);

                HitMap();
                HitGameObjects();

                IsMan();

                RemoveDead();

                //どのフロアにいるのか表示のため
                for (int i = 0; i < floorList.Count(); i++)
                {
                    if (isMan && players[0].GetRectangle().Intersects(floorList[i].FloorRange))
                    {
                        FloorNumInfo.floorNum = i;
                    }
                    else if (!isMan && players[1].GetRectangle().Intersects(floorList[i].FloorRange))
                    {
                        FloorNumInfo.floorNum = i;

                    }
                }
            
            if(PlayerInfo.AutoMove)
            {
                camera.SetFocus(floorList[2].GetGameObjects(0));
                foreach (Player player in players)
                {
                    if (player.GetWorldPosition().X < floorList[2].GetGameObjects(0).GetWorldPosition().X)
                    {
                        player.SetWorldPosition(
                            new Vector2 (player.GetWorldPosition().X + 2, player.GetWorldPosition().Y));
                        player.SetDrawingPosition(
                            new Vector2(player.GetDrawingPosition().X + 2, player.GetDrawingPosition().Y));
                        player.FaceDirection = Def.Conditions.FaceDirection.Right;
                    }
                    else
                    {
                        player.SetWorldPosition(
                            new Vector2(player.GetWorldPosition().X - 2, player.GetWorldPosition().Y));
                        player.SetDrawingPosition(
                            new Vector2(player.GetDrawingPosition().X - 2, player.GetDrawingPosition().Y));
                        player.FaceDirection = Def.Conditions.FaceDirection.Left;
                    }

                    player.UpdateMotion(player.GetWorldPosition());

                    player.GetMotion().Update(gameTime);
                }
            }

            //else
            //{
            //    camera.SetFocus(floorList[2].GetGameObjects(0));
            //    floorList[2].GetGameObjects(0).Update(gameTime);
            //    players[0].Update(gameTime);
            //    players[1].Update(gameTime);
            //}

            // 書かなきゃ台詞があったら。
            if (_dialogue.HaveToWrite())
            {
                _dialogue.Update();
            }

        }

        public void ObjectUpdate(GameTime gameTime)
        {
            //各フロア
            foreach (Floor nankai in floorList)
            {
                if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
                {
                    nankai.Update(gameTime);
                    if (!camera.IsMoving)
                    { camera.SetPositionY(nankai.FloorRange.Bottom - 32); }
                    PlayerInfo.RightWall = nankai.FloorRange.Width;
                }
                else if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
                {
                    nankai.Update(gameTime);
                    if (!camera.IsMoving)
                    { camera.SetPositionY(nankai.FloorRange.Bottom - 32); }
                    PlayerInfo.RightWall = nankai.FloorRange.Width;

                    if (nankai is SecurityOffice)
                    {
                        Console.WriteLine(players[1].GetWorldPosition());
                        Console.WriteLine(players[1].GetDrawingPosition());
                    }
                }
            }
            //Lock関連
            foreach (var l in lockList)
            { l.Update(gameTime); }

            // SuperGuardman関連
            foreach (var l in superList)
            { l.Update(gameTime); }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            foreach(Floor nankai in floorList)
            {
                if (!GimmickInfo.EventFlag)
                {
                    if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
                    {
                        players[1].ShouldBeDrawn = false;
                        players[0].ShouldBeDrawn = true;

                        if (players[1].GetRectangle().Intersects(nankai.FloorRange))
                        {
                            players[1].ShouldBeDrawn = true;
                        }
                        nankai.Draw(renderer, players);
                    }
                    else if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
                    {
                        players[0].ShouldBeDrawn = false;
                        players[1].ShouldBeDrawn = true;
                        if (players[0].GetRectangle().Intersects(nankai.FloorRange))
                        {
                            players[0].ShouldBeDrawn = true;
                        }
                        nankai.Draw(renderer, players);
                    }
                }
                else
                {
                    if (superList[0].GetRectangle().Intersects(nankai.FloorRange))
                    {
                        Console.WriteLine(nankai.FloorRange.ToString());
                        nankai.Draw(renderer, players);
                    }
                }
            }
        }

        /// <summary>
        /// 前に描画
        /// </summary>
        /// <param name="renderer"></param>
        public void FrontDraw(Renderer renderer)
        {
            // 段階を描画
            foreach (Floor nankai in floorList)
            {
                if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
                {
                    nankai.FrontDraw(renderer, players[playerNum]);
                    players[0].FrontDraw(renderer); // 会話を描画
                }
                else if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
                {
                    nankai.FrontDraw(renderer, players[playerNum]);
                    players[1].FrontDraw(renderer);
                }
            }

            floorNum.Draw(renderer);

            if (_dialogue.HaveToWrite())
            {
                _dialogue.Draw(renderer, "dialogue");
            }
        }

        public GameObject GetPlayer()
        {
            if (players[playerNum] != null || !players[playerNum].IsDead())
            { return players[playerNum]; }

            return null;
        }

        /// <summary>
        /// 男を操作しているか？キャラクター変更
        /// </summary>
        private void IsMan()
        {
            if (camera.IsMoving)
            { FloorNumInfo.IsDraw = false; }
            else
            { FloorNumInfo.IsDraw = true; }

            //Aが押されたらboolを反対に
            if (((!input.GetKeyTrigger(Keys.Z) && !input.GetKeyTrigger(Buttons.B))
                || players[playerNum].IsJump()
                || camera.IsMoving
                || camera.IsTeleportation
                || !PlayerInfo.IsMove)
                && !PlayerInfo.IsSwitchable && !PlayerInfo.AutoMove) { return; }

            else if (((input.GetKeyTrigger(Keys.Z) || input.GetKeyTrigger(Buttons.B))
                && PlayerInfo.IsSwitchable
                && !camera.IsMoving
                && !camera.IsTeleportation
                && !players[playerNum].IsJump()
                && PlayerInfo.IsMove)
                || PlayerInfo.ForceSwitch)
            {
                isMan = !isMan;
                PlayerInfo.IsMan = isMan;
                FloorNumInfo.IsDraw = false;

                //isManがTかFかでplayerNumを0か1に変更
                if (isMan)
                {
                    playerNum = 0;
                    camera.SetFocus(players[0], cameraSpeed);
                    foreach (Floor nankai in floorList)
                    {
                        if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
                        {
                            camera.SetPositionY(nankai.FloorRange.Bottom - 32);
                        }
                    }
                }
                else if (!isMan)
                {
                    playerNum = 1;
                    camera.SetFocus(players[1], cameraSpeed);
                    foreach (Floor nankai in floorList)
                    {
                        if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
                        {
                            camera.SetPositionY(nankai.FloorRange.Bottom - 32);
                        }
                    }
                }
                if (PlayerInfo.ForceSwitch)
                {
                    PlayerInfo.IsSwitchable = false;
                    PlayerInfo.ForceSwitch = false;
                }
            } // elseif の「 } 」
            else
            {
                return;
            }
        }


        //    if (camera.IsMoving)
        //    { FloorNumInfo.IsDraw = false; }
        //    else
        //    { FloorNumInfo.IsDraw = true; }

        //    //Aが押されたらboolを反対に
        //    if (((!input.GetKeyTrigger(Keys.Z) && !input.GetKeyTrigger(Buttons.B))
        //        || players[playerNum].IsJump()
        //        || camera.IsMoving
        //        || camera.IsTeleportation
        //        || !PlayerInfo.IsMove)
        //        && !PlayerInfo.IsSwitchable) { return; }

        //    else if (((input.GetKeyTrigger(Keys.Z) || input.GetKeyTrigger(Buttons.B))
        //        && PlayerInfo.IsSwitchable
        //        && !camera.IsMoving
        //        && !camera.IsTeleportation
        //        && !players[playerNum].IsJump()
        //        && PlayerInfo.IsMove)
        //        || PlayerInfo.ForceSwitch)
        //    {
        //        isMan = !isMan;
        //        FloorNumInfo.IsDraw = false;

        //        //isManがTかFかでplayerNumを0か1に変更
        //        if (isMan)
        //        {
        //            playerNum = 0;
        //            camera.SetFocus(players[0], cameraSpeed);
        //            foreach (Floor nankai in floorList)
        //            {
        //                if (isMan && players[0].GetRectangle().Intersects(nankai.FloorRange))
        //                {
        //                    camera.SetPositionY(nankai.FloorRange.Bottom - 32);
        //                }
        //            }
        //        }
        //        else if (!isMan)
        //        {
        //            playerNum = 1;
        //            camera.SetFocus(players[1], cameraSpeed);
        //            foreach (Floor nankai in floorList)
        //            {
        //                if (!isMan && players[1].GetRectangle().Intersects(nankai.FloorRange))
        //                {
        //                    camera.SetPositionY(nankai.FloorRange.Bottom - 32);
        //                }
        //            }
        //        }
        //        if (PlayerInfo.ForceSwitch)
        //        {
        //            PlayerInfo.IsSwitchable = false;
        //            PlayerInfo.ForceSwitch = false;
        //        }
        //    } // elseif の「 } 」
        //    else
        //    {
        //        return;
        //    }
        //}

    }
}
