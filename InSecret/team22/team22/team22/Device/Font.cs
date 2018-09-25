#region Using Statements
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace team22.Device
{
    class Font
    {
        #region Fields
        private ContentManager _contentManager;

        private Dictionary<string, SpriteFont> _fonts;
        #endregion

        public Font(ContentManager content)
        {
            _contentManager = content;

            _fonts = new Dictionary<string, SpriteFont>();
        }

        public void LoadFont(string name, string filepath = "./")
        {
            if (_fonts.ContainsKey(name))
            {
                return;
            }
            _fonts.Add(name, _contentManager.Load<SpriteFont>(filepath + name));
        }

        public SpriteFont GetFont(string name)
        {
            return _fonts[name];
        }

        public void Unload()
        {
            _fonts.Clear();
        }
    }
}
