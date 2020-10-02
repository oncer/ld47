using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace SentIO
{
    class SaveData
    {
        private string path;
        private Dictionary<string, string> data;

        SaveData()
        {
            path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SentIO");
            Directory.CreateDirectory(path);
            path = Path.Join(path, "SaveData.xml");
            data = new Dictionary<string, string>();
            Load();
        }

        public string this[string key]
        {
            get
            {
                if (data.ContainsKey(key)) return data[key];
                return "";
            }
            set
            {
                data[key] = value;
                Save();
            }
        }

        private void Load()
        {
            try 
            {
                if (File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    foreach (XmlElement xmlItem in doc.SelectNodes("/Root/Item"))
                    {
                        data.Add(xmlItem.GetAttribute("key"), xmlItem.GetAttribute("value"));
                    }
                }
                else
                {
                    Save();
                }
            }
            catch (Exception)
            {
            }
        }

        private void Save()
        {
            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement xmlRoot = doc.CreateElement("Root");
            foreach (string key in data.Keys)
            {
                XmlElement xmlItem = doc.CreateElement("Item");
                xmlItem.SetAttribute("key", key);
                xmlItem.SetAttribute("value", data[key]);
                xmlRoot.AppendChild(xmlItem);
            }
            doc.AppendChild(xmlRoot);
            doc.Save(path);
        }

        private static SaveData _instance;
        public static SaveData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SaveData();
                }
                return _instance;
            }
        }
    }
}
