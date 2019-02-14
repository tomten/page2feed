using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Page2Feed.Web.Controllers
{
    public static class Xml
    {

        public static string XmlString<T>(this T obj)
        {
            var memoryStream = new MemoryStream();
            var stringWriter = new StreamWriter(
                memoryStream,
                Encoding.UTF8
            );
            new XmlSerializer(typeof(T)).Serialize(
                stringWriter,
                obj
            );
            var utf8bytes = memoryStream.GetBuffer();
            var xmlString = Encoding.UTF8.GetString(utf8bytes);
            return xmlString;
        }

    }

}