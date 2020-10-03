using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SentIO.UI
{
    /// <summary>
    /// A texture set is a list of equally-sized textures
    /// </summary>
    public class TextureSet : List<Texture2D>
    {
        private TextureSet() { }

        public static TextureSet CreateEmptyCopy(Texture2D original)
        {
            var res = new TextureSet();
            res.OriginalTexture = original;
            return res;
        }

        public int Width { get => OriginalTexture != null ? OriginalTexture.Width : 0; }
        public int Height { get => OriginalTexture != null ? OriginalTexture.Height : 0; }

        public Texture2D OriginalTexture { get; private set; }

        public static TextureSet FromTexture(Texture2D texture)
        {
            var tileSet = new TextureSet();

            tileSet.Add(texture);
            tileSet.OriginalTexture = texture;

            return tileSet;
        }
    }
}
