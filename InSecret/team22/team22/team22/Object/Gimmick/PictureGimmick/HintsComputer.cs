using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;

namespace team22.Object
{
    class HintsComputer : GameObject
    {
        private InputState input;
        private CountDownTimer timer;

        private int pictureNum;

        private bool isDraw;

        public HintsComputer(Vector2 position, int floorWidth, int floorHeight, GameDevice gameDevice)
            : base("desk1", position, floorWidth, floorHeight, 84, 96, gameDevice)
        {
            input = gameDevice.InputState;
            timer = new CountDownTimer(1.0f);
            pictureNum = 5;
            isDraw = false;
        }

        public HintsComputer(HintsComputer other)
            : this(other.worldPosition, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice)
        { }

        public override object Clone()
        {
            return new HintsComputer(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A))
            {
                timer.Initialize();
                isDraw = !isDraw;
                pictureNum = 5;
                if (!isDraw)
                {
                    PlayerInfo.IsMove = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDraw)
            {
                //PlayerInfo.IsMove = true;
                return;
            }
            PlayerInfo.IsMove = false;
            timer.Update();
            //表示する絵画の順番を更新していく
            if (timer.IsTime())
            {
                pictureNum++;
                timer.Initialize();
            }
            if (pictureNum > 5) pictureNum = 0;
        }

        public override void FrontDraw(Renderer renderer)
        {
            if (!isDraw) return;
            Vector2 freamPosition = GimmickFront.frontDisplayPosi;
            renderer.DrawTexture("pc", freamPosition);
            //絵画ナンバ－が6じゃなければ描画
            if (pictureNum != 5)
            {
                renderer.DrawTexture(PictureInfo.AnsName[pictureNum],
                    new Vector2(freamPosition.X + (GimmickFront.frontX / 2) - PictureInfo.AnsSize[pictureNum].X - 2,
                                freamPosition.Y + (GimmickFront.frontY / 2) - PictureInfo.AnsSize[pictureNum].Y - 2), new Vector2(2, 2));
            }
            else
            { renderer.DrawTexture("pc_hints", freamPosition); }
        }
    }
}
