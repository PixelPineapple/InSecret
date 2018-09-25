using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using team22.Device;

namespace team22.Object
{
    /// <summary>
    /// ブロック
    /// </summary>
    class Block : GameObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameDevice"></param>
        public Block(Vector2 position,GameDevice gameDevice)
            :base("block", position, 32, 32, gameDevice)
        {

        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public Block(Block other)
            :this(other.worldPosition,other.gameDevice)
        { }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Block(this); //Blockは必須
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// 衝突
        /// </summary>
        /// <param name="gameObject"></param>
        public override void Hit(GameObject gameObject)
        {
            
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, worldPosition);
        }
    }
}
