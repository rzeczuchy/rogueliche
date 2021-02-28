using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace rogueliche
{
    public class GameSaver
    {
        private const string Savepath = "save.xml";

        public void SaveGame()
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(Savepath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Save");

                // save game state here

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void LoadGame()
        {
            using (XmlReader reader = XmlReader.Create(Savepath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                       // load game state here 
                    }
                }
            }
        }

        public void DeleteSave()
        {
            if (File.Exists(Savepath))
            {
                File.Delete(Savepath);
            }
        }
    }
}
