using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace SentIO
{
    public static class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet= CharSet.Auto)]
        public static extern int MessageBox(int hWnd, String text, String caption, uint type);

        //public static FileStream logFile;

        public static void Log(string txt)
        {
            //logFile.Write(Encoding.UTF8.GetBytes(txt+"\n"));
            //logFile.Flush();
        }
        [STAThread]
        static void Main()
        {
            //logFile = File.OpenWrite("C:\\Users\\Simon Parzer\\Desktop\\MonoGameLog.txt");
            //Log("LOG START");

            if (SaveData.Instance["killswitch"] == "triggered")
            {
                //Log("killswitch triggered (savedata)");
                MainGame.LeaveURL();
                MainGame.Suicide();
                return;
            }
            else if (WebClient.Instance.GetPlayerFinished())
            {
                //Log("killswitch triggered (web)");
                SaveData.Instance["killswitch"] = "triggered";
                MainGame.LeaveURL();
                MainGame.Suicide();
                return;
            }

            string[] textFileNames = new string[]
            {
                "sentio not loading.txt",
                "hello.txt",
                "are you there.txt",
                "help me load - PLEASE READ.txt",
                "HELLOOOOOOOO.txt",
                "OPEN ME - INSTRUCTIONS INSIDE.txt"
            };
            if (File.Exists(Path.Join(SaveData.Instance.ExeDirectory, "interface.lock")))
            {
                int createdNo = 0;
                for (int i = 0; i < textFileNames.Length; i++)
                {
                    string textFileName = Path.Join(SaveData.Instance.ExeDirectory, textFileNames[i]);
                    if (!File.Exists(textFileName))
                    {
                        File.WriteAllText(textFileName, $"Please delete \"interface.lock\" and try to reload me!");
                        createdNo = i;
                        break;
                    }
                }
                if (createdNo > 1)
                {
                    MessageBox(0, "I've dropped some text files, can you check them for me? Please?", "", 0);
                }
                else if (createdNo > 0)
                {
                    MessageBox(0, "...", "Hey.", 0);
                }

                return;
            }

            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
}
