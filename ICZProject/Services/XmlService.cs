using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ICZProject.Services
{
    public class XmlService: IFileService
    {
        public string Read(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public T Deserialize<T>(string input) where T : class
        {
            var ser = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string Serialize<T>(T objectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, objectToSerialize);
                return textWriter.ToString();
            }
        }
    }
}