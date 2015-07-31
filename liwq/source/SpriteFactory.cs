using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace liwq
{
    public class SpriteFactory : IDisposable
    {
        public static SpriteFactory SharedSpriteFactory { get; private set; }
        static SpriteFactory() { SharedSpriteFactory = new SpriteFactory(); }

        //---------------------------------------------------------------------

        protected Dictionary<string, Texture2D> _textureCaches = new Dictionary<string, Texture2D>();
        protected Dictionary<string, Sprite> _spriteCaches = new Dictionary<string, Sprite>();

        public void AddTexture(string name, Texture2D texture, bool overwrite = false)
        {
            if (this._textureCaches.ContainsKey(name) == true)
            {
                if (overwrite == true)
                    this._textureCaches.Remove(name);
                else
                    throw new System.ArgumentException("Texture:" + name + " already exits.", name);
            }
            this._textureCaches.Add(name, texture);
        }

        public bool RemoveTexture(string name)
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

        public void AddSprite(string name, Sprite sprite, bool overwrite = false)
        {
            if (this._spriteCaches.ContainsKey(name) == true)
            {
                if (overwrite == true)
                    this._spriteCaches.Remove(name);
                else
                    throw new System.ArgumentException("Sprite:" + name + " already exits.", name);
            }
            this._spriteCaches.Add(name, sprite);
        }

        public bool RemoveSprite(string name)
        {
            Sprite sprite;
            if (this._spriteCaches.TryGetValue(name, out sprite) == true)
            {
                if (this._spriteCaches.Remove(name) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddSpritePack(string packInfo, Texture2D texture)
        {
            //<TextureAtlas imagePath="pack.png" width="2047" height="1069">
            //    <sprite n="adventure-checkmark-box.png" x="1861" y="627" w="102" h="102" oX="2" oY="2" oW="106" oH="106" r="y"/>
            XElement textureElement = XElement.Parse(packInfo);
            string name = textureElement.SafeReadString("imagePath");
            this._textureCaches.Add(name, texture);

            foreach (var spriteElement in textureElement.Elements())
            {
                string n = spriteElement.SafeReadString("n");
                int x = spriteElement.SafeReadInt("x");
                int y = spriteElement.SafeReadInt("y");
                int w = spriteElement.SafeReadInt("w");
                int h = spriteElement.SafeReadInt("h");
                int oX = spriteElement.SafeReadInt("oX");
                int oY = spriteElement.SafeReadInt("oY");
                int oW = spriteElement.SafeReadInt("oW");
                int oH = spriteElement.SafeReadInt("oH");
                string r = spriteElement.SafeReadString("r");
                Sprite sprite = new Sprite(texture, new Rect(x, y, w, h), r == "y", new Size(oW, oH), new Point(oX, oY));
                this.AddSprite(n, sprite);
            }
        }
        
        public Sprite CreateSprite(string name)
        {
            if (this._spriteCaches.ContainsKey(name) == true)
            {
                return this._spriteCaches[name].Clone();
            }
            else if (this._textureCaches.ContainsKey(name) == true)
            {
                return new Sprite(this._textureCaches[name]);
            }
            return null;
        }

        public Sprite this[string name]
        {
            get { return this.CreateSprite(name); }
        }

        //---------------------------------------------------------------------

        public void Dispose()
        {
            foreach (var t in this._textureCaches)
                t.Value.Dispose();
            this._textureCaches.Clear();
            this._spriteCaches.Clear();
            GC.SuppressFinalize(this);
        }
        ~SpriteFactory()
        {
            this.Dispose();
        }
    }
}
