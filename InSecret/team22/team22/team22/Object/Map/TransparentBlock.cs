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
    class TransparentBlock : GameObject
    {
        #region Fields
        private bool _switch;
        #endregion
        #region Getter Setter メソッド
        public bool Switch
        {
            get { return _switch; }
            set { _switch = value; }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="position"></param>
        /// <param name="gameDevice"></param>
        public TransparentBlock(Vector2 position, GameDevice gameDevice)
            : base("block", position, 32, 32, gameDevice)
        {
            _switch = false;
        }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="other"></param>
        public TransparentBlock(TransparentBlock other)
            : this(other.worldPosition, other.gameDevice)
        { }

        /// <summary>
        /// 複製
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TransparentBlock(this); //Blockは必須
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (PlayerInfo.AutoMove)
            {
                _switch = true;
            }
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
            base.Draw(renderer);
        }
    }
}
