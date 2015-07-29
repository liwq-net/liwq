using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace liwq
{
    public class TextureFactory
    {
        public static TextureFactory SharedTextureFactory { get; private set; }
        static TextureFactory() { SharedTextureFactory = new TextureFactory(); }

        protected Dictionary<string, Texture2D> _textureCaches = new Dictionary<string, Texture2D>();
        public void Add(string name, Texture2D texture)
        {
            this._textureCaches.Add(name, texture);
        }

        public bool Remove(string name)
        {
            Texture2D texture;
            if (this._textureCaches.TryGetValue(name, out texture) == true)
            {
                if (this._textureCaches.Remove(name) == true)
                {
                    texture.Dispose();
                    return true;
                }
            }
            return false;
        }

        public Texture2D this[string name]
        {
            get { return this._textureCaches[name]; }
        }
    }
}
