using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SentIO
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
    }
}
