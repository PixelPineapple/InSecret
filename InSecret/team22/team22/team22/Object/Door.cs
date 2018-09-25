using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class Door : GameObject
    {
        #region Files
        private InputState input;
        private GameObject player;
        private Vector2 teleportationPosi;
        private Vector2 floorPosition;  //これから向かうフロアの原点
        private bool isTeleportation;
        private bool isHitPlayer;

        private Sound sound;
        #endregion

        public Door(Vector2 position, int floorWidth, int floorHeight, Vector2 teleportationPosi, Vector2 floorPosition, GameDevice gameDevice, bool teleportation)
            : base("door1", position, floorWidth, floorHeight, 96, 60, gameDevice)
        {
            input = gameDevice.InputState;
            isTeleportation = teleportation;
            this.teleportationPosi = teleportationPosi;
            this.floorPosition = floorPosition * 32;
            isHitPlayer = false;
            sound = gameDevice.Sound;
        }

        public Door(Door other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X / 32, (int)other.GetDrawingPosition().Y / 32, other.teleportationPosi, other.floorPosition, other.gameDevice, other.isTeleportation)
        {

        }

        public override object Clone()
        {
            return new Door(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && input.GetKeyTrigger(Keys.X) && !isHitPlayer)
            {
                player = gameObject;
                sound.PlaySE("door");
                isHitPlayer = true;
                if (isTeleportation)
                {
                    //フロアの画面中心にカメラをセット
                    gameDevice.Camera.SetFixedPosition(teleportationPosi, isTeleportation);
                    //プレイヤーのXを画面の中心から-32*9にする
                    gameObject.SetWorldPosition(teleportationPosi + new Vector2(-32 * 9, 0));
                    gameObject.SetDrawingPosition((gameObject.GetWorldPosition()) - floorPosition);
                    Console.WriteLine("draw x={0} y={1}", player.GetDrawingPosition().X / 32, player.GetDrawingPosition().Y / 32);
                    Console.WriteLine("world x={0} y={1}", player.GetWorldPosition().X / 32, player.GetWorldPosition().Y / 32);
                }
                else
                {
                    gameObject.SetWorldPosition(teleportationPosi);
                    gameDevice.Camera.SetFixedPosition(teleportationPosi, isTeleportation);
                    gameObject.SetDrawingPosition(teleportationPosi - floorPosition);
                    Console.WriteLine("draw x={0} y={1}", player.GetDrawingPosition().X / 32, player.GetDrawingPosition().Y / 32);
                    Console.WriteLine("world x={0} y={1}", player.GetWorldPosition().X / 32, player.GetWorldPosition().Y / 32);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (player != null && !player.GetRectangle().Equals(GetRectangle()))
            {
                isHitPlayer = false;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition + new Vector2(0, -32));
        }
    }
}
