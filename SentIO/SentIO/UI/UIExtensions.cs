using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SentIO.UI
{
    public static class UIExtensions
    {
        public static TextureSet LoadTextureSet(this ContentManager content, string fileName, int tileWidth = -1, int tileHeight = -1)
        {
            int partWidth = (tileWidth != -1) ? tileWidth : 16;
            int partHeight = (tileHeight != -1) ? tileHeight : 16;

            Texture2D original = content.Load<Texture2D>(fileName);

            int xCount = original.Width / partWidth;
            int yCount = original.Height / partHeight;

            Texture2D[] r = new Texture2D[xCount * yCount];
            int dataPerPart = partWidth * partHeight;

            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {

                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    Color[] partData = new Color[dataPerPart];

                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }
                    part.SetData(partData);
                    r[index++] = part;
                }

            TextureSet result = TextureSet.CreateEmptyCopy(original);
            foreach (var element in r.Cast<Texture2D>())
                result.Add(element);

            return result;
        }
    }
}
