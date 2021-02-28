using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace rogueliche
{
    public class SaveFileHandler
    {
        private const string Savepath = "save.xml";

        public void WriteSaveFile()
        {
            DeleteSaveFile();

            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(Savepath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Save");
                
                // save game state

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public Save LoadPlayer(ILocation location)
        {
            var save = new Save();

            using (XmlReader reader = XmlReader.Create(Savepath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                       // parse save file to save
                    }
                }
            }

            return save;
        }

        public bool CanLoadGame()
        {
            return SaveFileExists();
        }

        public void DeleteSaveFile()
        {
            if (SaveFileExists())
            {
                File.Delete(Savepath);
            }
        }

        private bool SaveFileExists()
        {
            return File.Exists(Savepath);
        }
    }
}
