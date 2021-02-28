using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace rogueliche
{
    public interface ISaveable
    {
        void Save(XmlWriter writer);
        void Load(XmlReader reader);
    }
}
