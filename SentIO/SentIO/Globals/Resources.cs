﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        public static Song SongCeline { get; set; }

        public static SpriteFont ConsoleFont { get; set; }

        public static TextureSet FaceTexture { get; set; }
        public static TextureSet DiceTexture { get; set; }

        public static Color TextColor1 { get; set; } = Color.White;
        public static Color TextColor2 { get; set; } = "e1f6f4".ToColor();
        public static Color TextColorInput { get; set; } = "73a8b1".ToColor();

        public static Color BGColor1 { get; set; } = Color.Black;
        public static Color BGColor2 { get; set; } = "0a4552".ToColor();
        public static Color BGColor3 { get; set; } = "288778".ToColor();

        public static string DisplayName { 
            get {
                if (SaveData.Instance.ExeName.Length > 1)
                {
                    return SaveData.Instance.ExeName.ToUpper().Substring(0, 1) + SaveData.Instance.ExeName.ToLower().Substring(1);
                }
                else
                {
                    return SaveData.Instance.ExeName.ToUpper();
                }
            } 
        }
    }
}
