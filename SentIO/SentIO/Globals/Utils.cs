using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SentIO.Globals
{
    public struct Size
    {
        private Point p;

        public int Width => p.X;
        public int Height => p.Y;

        public Size(int w, int h)
        {
            p = new Point(w, h);
        }
    }

    public static class Utils
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static Color ToColor(this string hexCode)
        {
            hexCode = hexCode.Replace("#", "");

            try
            {
                if (hexCode.Length == 6)
                {

                    var r = int.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    var g = int.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    var b = int.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                    return new Color(r, g, b);
                }

                if (hexCode.Length == 8)
                {
                    var a = int.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    var r = int.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    var g = int.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                    var b = int.Parse(hexCode.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                    return new Color(r, g, b, a);
                }
            }
            catch (Exception) { }
            throw new ArgumentException($"Color code '{hexCode}' is invalid!");
        }

        public static string ToHexString(this Color color)
        {
            return string.Format("#{0:X}{1:X}{2:X}{3:X}", color.R, color.G, color.B, color.A);
        }
    }
}
