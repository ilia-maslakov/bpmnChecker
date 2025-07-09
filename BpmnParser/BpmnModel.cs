namespace BpmnParser
{
    public class BpmnModel
    {
        public List<BpmnLane> Lanes { get; } = [];
        public List<BpmnFlow> Flows { get; } = [];
        public Dictionary<string, BpmnTask> AllNodes { get; } = [];
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
        public string? FormKey { get; set; }
        public string? Topic { get; set; }
        public string? MessageRef { get; set; }
        public string? Timer { get; set; }
    }

    public class BpmnFlow
    {
        public string SourceId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public string? Condition { get; set; }
    }
}