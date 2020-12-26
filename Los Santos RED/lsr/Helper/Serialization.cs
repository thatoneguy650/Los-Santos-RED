using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LosSantosRED.lsr.Helper
{
    public static class Serialization
    {
        public static void SerializeParams<T>(List<T> paramList, string FileName)
        {
            XDocument doc = new XDocument();
            XmlSerializer serializer = new XmlSerializer(paramList.GetType());
            XmlWriter writer = doc.CreateWriter();
            serializer.Serialize(writer, paramList);
            writer.Close();
            File.WriteAllText(FileName, doc.ToString());
            Game.Console.Print(string.Format("SerializeParams Using Default Data {0}", FileName));
        }
        public static List<T> DeserializeParams<T>(string FileName)
        {
            XDocument doc = new XDocument(XDocument.Load(FileName));
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            XmlReader reader = doc.CreateReader();
            List<T> result = (List<T>)serializer.Deserialize(reader);
            reader.Close();
            Game.Console.Print(string.Format("DeserializeParams Using Saved Data {0}", FileName));
            return result;
        }
    }
}
