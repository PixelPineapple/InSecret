using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using team22.Device;
using team22.Def;

namespace team22.Object
{
    /// <summary>
    /// 絵画
    /// </summary>
    class Picture : GameObject
    {
        #region Files
        private InputState input;
        private Floor floor;

        private string alphabetPictureName; //何個目の答えを入れるのか？
        private Rectangle alphabetRect;

        private int height;
        private int width;

        private bool isAlphabetDisplay;     //文字を表示しているか？
        #endregion

        public string Name
        {
            get { return name; }
        }

        public int Height
        { get { return height; } }

        public int Width
        { get { return width; } }

        public Picture(string pictureName, string alphabetPictureName, Vector2 position, int height, int width, int floorHeight, int floorWidth, GameDevice gameDevice, Floor floor)
            : base(pictureName, position, floorHeight, floorWidth, height, width, gameDevice)
        {
            input = gameDevice.InputState;
            this.alphabetPictureName = alphabetPictureName;
            this.floor = floor;
            this.height = height;
            this.width = width;
            isAlphabetDisplay = false;
        }

        public Picture(Picture other)
            : this(other.name, other.alphabetPictureName, other.worldPosition, other.height, other.width, (int)other.GetDrawingPosition().X, (int)other.GetDrawingPosition().Y, other.gameDevice, other.floor)
        { }

        public override object Clone()
        {
            return new Picture(this);
        }

        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Player && !isAlphabetDisplay && !alphabetPictureName.Contains("transparent")
                && !gameDevice.Camera.IsMoving && PlayerInfo.IsMove
                && (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)))
            {
                isAlphabetDisplay = true;
                PlayerInfo.IsMove = false;
            }
            else if (gameObject is Player && (input.GetKeyTrigger(Keys.X) || input.GetKeyTrigger(Buttons.A)) && isAlphabetDisplay)
            {
                isAlphabetDisplay = false;
                PlayerInfo.IsMove = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, drawingPosition - new Vector2(0, height));
        }

        public override void FrontDraw(Renderer renderer)
        {
            //アルファベットを表示するなら
            if (isAlphabetDisplay)
            {
                Vector2 flamePosition = GimmickFront.frontDisplayPosi;
                renderer.DrawTexture("picture_flame", flamePosition);
                renderer.DrawTexture(alphabetPictureName, flamePosition + new Vector2(32 * 11, 32 * 8), alphabetRect, Color.Red);
            }
        }

        //アルファベットの設定
        public void SetAlphabetInfo(int x)
        {
            alphabetRect = new Rectangle(48 * x, 0, 48, 96);
        }

    }
}
