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
            _ns.AddNamespace("camunda", "http://camunda.org/schema/1.0/bpmn");
        }

        public BpmnModel Parse()
        {
            var model = new BpmnModel();

            var typesToParse = new[]
            {
                "userTask", "serviceTask", "sendTask", "startEvent", "endEvent",
                "intermediateCatchEvent", "intermediateThrowEvent",
                "exclusiveGateway", "parallelGateway"
            };

            foreach (var type in typesToParse)
            {
                var nodes = _doc.SelectNodes($"//bpmn:{type}", _ns);
                foreach (XmlNode node in nodes!)
                {
                    var task = new BpmnTask
                    {
                        Id = node.Attributes?["id"]?.Value ?? "?",
                        Name = node.Attributes?["name"]?.Value ?? "<без имени>",
                        Type = type,
                        FormKey = node.Attributes?["camunda:formKey"]?.Value,
                        Topic = node.Attributes?["camunda:topic"]?.Value
                    };

                    var messageDef = node.SelectSingleNode("bpmn:messageEventDefinition", _ns);
                    if (messageDef != null)
                        task.MessageRef = messageDef.Attributes?["messageRef"]?.Value;

                    var timerDef = node.SelectSingleNode("bpmn:timerEventDefinition", _ns);
                    if (timerDef != null)
                        task.Timer = timerDef.InnerText.Trim();

                    model.AllNodes[task.Id] = task;
                }
            }

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
                    if (model.AllNodes.TryGetValue(id, out var task))
                    {
                        if (task.Type is not ("parallelGateway"))
                            lane.Tasks.Add(task);
                    }
                }

                model.Lanes.Add(lane);
            }

            var flows = _doc.SelectNodes("//bpmn:sequenceFlow", _ns);
            foreach (XmlNode flow in flows!)
            {
                var conditionNode = flow.SelectSingleNode("bpmn:conditionExpression", _ns);
                model.Flows.Add(new BpmnFlow
                {
                    SourceId = flow.Attributes?["sourceRef"]?.Value ?? "?",
                    TargetId = flow.Attributes?["targetRef"]?.Value ?? "?",
                    Condition = conditionNode?.InnerText?.Trim()
                });
            }

            return model;
        }
    }
}
