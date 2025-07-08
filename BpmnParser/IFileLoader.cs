using System.Xml;

namespace BpmnParser
{
    public interface IFileLoader
    {
        XmlDocument? Load(string path);
    }
}
