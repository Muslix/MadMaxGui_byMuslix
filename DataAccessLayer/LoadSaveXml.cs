using Domain;
using System.IO;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class LoadSaveXml : ILoadSaveXml
    {
        public Config loadData(string filename)
        {
            XmlSerializer sr = new XmlSerializer(typeof(Config));
            TextReader reader = new StreamReader(filename);
            Config test = (Config)sr.Deserialize(reader);
            reader.Close();
            return test;
        }

        public void savedata(object obj, string filename)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }
    }
}
