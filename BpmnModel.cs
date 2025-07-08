namespace BpmnParser
{
    public class BpmnModel
    {
        public List<BpmnLane> Lanes { get; } = [];
        public List<BpmnFlow> Flows { get; } = [];
    }

    public class BpmnLane
    {
        public string Name { get; set; } = string.Empty;
        public List<BpmnTask> Tasks { get; } = [];
    }

    public class BpmnTask
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // userTask / serviceTask
    }

    public class BpmnFlow
    {
        public string SourceId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
    }
}