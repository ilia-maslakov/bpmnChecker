using System.Xml;

namespace BpmnParser
{
    public class BpmnParser : IBpmnParser
    {
        private readonly XmlDocument _doc;
        private readonly XmlNamespaceManager _ns;

        public BpmnParser(XmlDocument doc)
        {
            _doc = doc;
            _ns = new XmlNamespaceManager(doc.NameTable);
            _ns.AddNamespace("bpmn", "http://www.omg.org/spec/BPMN/20100524/MODEL");
        }

        public BpmnModel Parse()
        {
            var model = new BpmnModel();

            var lanes = _doc.SelectNodes("//bpmn:lane", _ns);
            foreach (XmlNode laneNode in lanes!)
            {
                var lane = new BpmnLane
                {
                    Name = laneNode.Attributes?["name"]?.Value ?? "<без имени>"
                };

                var refs = laneNode.SelectNodes("bpmn:flowNodeRef", _ns);
                foreach (XmlNode refNode in refs!)
                {
                    var id = refNode.InnerText;
                    var taskNode = _doc.SelectSingleNode($"//*[@id='{id}']", _ns);
                    if (taskNode == null) continue;

                    var type = taskNode.LocalName;
                    if (type != "userTask" && type != "serviceTask") continue;

                    lane.Tasks.Add(new BpmnTask
                    {
                        Id = id,
                        Name = taskNode.Attributes?["name"]?.Value ?? "<без имени>",
                        Type = type
                    });
                }

                model.Lanes.Add(lane);
            }

            var flows = _doc.SelectNodes("//bpmn:sequenceFlow", _ns);
            foreach (XmlNode flow in flows!)
            {
                model.Flows.Add(new BpmnFlow
                {
                    SourceId = flow.Attributes?["sourceRef"]?.Value ?? "?",
                    TargetId = flow.Attributes?["targetRef"]?.Value ?? "?"
                });
            }

            return model;
        }
    }
}