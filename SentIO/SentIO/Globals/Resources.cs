﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SentIO.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Globals
{
    public static class Resources
    {
        public static SoundEffect SfxChar { get; set; }
        public static SoundEffect SfxCharStop { get; set; }

        public static SpriteFont ConsoleFont { get; set; }

        public static TextureSet FaceTexture { get; set; }

        public static Color TextColor1 { get; set; } = Color.White;

        // light white
        public static Color TextColor2 { get; set; } = "e1f6f4".ToColor();

        public static Color BGColor1 { get; set; } = Color.Black;

        public static Color BGColor2 { get; set; } = "0a4552".ToColor();

    }
}
