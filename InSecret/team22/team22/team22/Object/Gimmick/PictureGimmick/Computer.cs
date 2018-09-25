using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class Computer : GameObject
    {
        private InputState input;
        private Floor floor;
        private AlphabetSelect alphabet;    //アルファベット選択画面
        private Key key;
        private Sound sound;

        public Computer(Vector2 worldPosition, int floorHeight, int floorWidth, GameDevice gameDevice, Floor floor, Key key)
            : base("lock_pc", worldPosition, floorHeight, floorWidth, 38, 28, gameDevice)
        {
            input = gameDevice.InputState;
            sound = gameDevice.Sound;
            this.floor = floor;
            this.key = key;
        }

        public Computer(Computer other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice, other.floor, other.key)
        { }

        public override object Clone()
        {
            return new Computer(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player
                && alphabet == null && !key.IsDead()
                && (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)))
            {
                alphabet = new AlphabetSelect(GimmickFront.frontDisplayPosi, floor.FloorRange.X / 32, floor.FloorRange.Y / 32, gameDevice, key);
                floor.AddObject(alphabet);
                PlayerInfo.IsMove = false;
                sound.PlaySE("pc_button");
            }
        }

        public override void Update(GameTime gameTime)
        {
            //アルファベットが死んだら
            if (alphabet != null && alphabet.IsDead())
            {
                //nullにして初期化
                alphabet = null;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition);
        }
    }
}
