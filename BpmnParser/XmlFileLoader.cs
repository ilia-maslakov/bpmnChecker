using System.Xml;

namespace BpmnParser
{
    public class XmlFileLoader : IFileLoader
    {
        public XmlDocument? Load(string path)
        {
            if (!File.Exists(path)) return null;

            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                return doc;
            }
            catch
            {
                return null;
            }
        }
    }
}