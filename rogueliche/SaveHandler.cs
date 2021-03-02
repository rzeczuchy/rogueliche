using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace rogueliche
{
    public class SaveHandler
    {
        private const string Savepath = "save.xml";

        public void SaveGame(Player player)
        {
            DeleteSave();

            var save = new Save()
            {
                PlayerKillCount = player.KillCount,
                PlayerBrokenWeapons = player.BrokenWeapons,
                PlayerMaxHealth = player.MaxHealth,
                PlayerHealth = player.Health,
                PlayerLvl = player.Lvl,
                PlayerExp = player.Exp,
                PlayerExpToNextLvl = player.ExpToNextLvl,
                PlayerMaxExertion = player.MaxExertion,
                PlayerExertion = player.Exertion,
            };

            var serializer = new XmlSerializer(save.GetType());
            var settings = new XmlWriterSettings { Indent = true };
            using (var writer = XmlWriter.Create(Savepath, settings))
            {
                serializer.Serialize(writer, save);
            }
        }

        public Save LoadGame()
        {
            var save = new Save();

            var serializer = new XmlSerializer(save.GetType());
            using (var reader = XmlReader.Create(Savepath))
            {
                if (serializer.CanDeserialize(reader))
                {
                    save = (Save)serializer.Deserialize(reader);
                }
                else
                {
                    throw new Exception("Could not deserialize save file to a Save object.");
                }
            }

            return save;
        }

        public bool CanLoadGame()
        {
            return SaveFileExists();
        }

        public void DeleteSave()
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
