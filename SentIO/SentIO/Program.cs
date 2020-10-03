using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SentIO
{
    public static class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet= CharSet.Auto)]
        public static extern int MessageBox(int hWnd, String text, String caption, uint type);
        [STAThread]
        static void Main()
        {
            string[] textFileNames = new string[]
            {
                "hello.txt",
                "are you there.txt",
                "help me_load PLEASE READ.txt",
                "HELLOOOOOOOO.txt",
                "OPEN ME INSTRUCTIONS INSIDE.txt"
            };
            if (File.Exists(Path.Join(SaveData.Instance.ExeDirectory, "interface.lock")))
            {
                bool created = false;
                for (int i = 0; i < textFileNames.Length; i++)
                {
                    string textFileName = Path.Join(SaveData.Instance.ExeDirectory, textFileNames[i]);
                    if (!File.Exists(textFileName))
                    {
                        File.WriteAllText(textFileName, "please delete interface.lock and try to reload me");
                        created = true;
                        break;
                    }
                }
                if (!created)
                {
                    MessageBox(0, "I dropped some text files, can you pick them up for me?", "Hello?", 0);
                }

                return;
            }

            using (var game = new MainGame())
                game.Run();
        }
    }
}
