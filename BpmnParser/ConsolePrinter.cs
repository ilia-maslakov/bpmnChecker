namespace BpmnParser
{
    public class ConsolePrinter(TextWriter writer) : IBpmnPrinter
    {
        private readonly TextWriter _writer = writer;

        public void Print(BpmnModel model)
        {
            foreach (var lane in model.Lanes)
            {
                _writer.WriteLine($"Lane: {lane.Name}");

                foreach (var task in lane.Tasks)
                {
                    // Спец.обработка exclusiveGateway с conditionExpression
                    if (task.Type == "exclusiveGateway")
                    {
                        var matchingFlows = model.Flows
                            .Where(f => f.SourceId == task.Id && !string.IsNullOrEmpty(f.Condition))
                            .ToList();

                        if (matchingFlows.Count > 0)
                        {
                            var parts = new List<string>();
                            foreach (var flow in matchingFlows)
                            {
                                if (model.AllNodes.TryGetValue(flow.TargetId, out var target))
                                {
                                    var label = !string.IsNullOrEmpty(target.Topic)
                                        ? $"'{target.Topic}'"
                                        : !string.IsNullOrEmpty(target.FormKey)
                                            ? target.FormKey
                                            : "usertask";

                                    parts.Add($"{flow.Condition} -> {label}");
                                }
                            }

                            var info = $"{task.Type,-22} {task.Id,-20} [{string.Join(", ", parts)}] - {task.Name}";
                            _writer.WriteLine($"  {info}");
                            continue;
                        }
                    }

                    // Стандартный вывод
                    var extras = new List<string>();

                    if (task.Type == "userTask")
                    {
                        extras.Add(string.IsNullOrEmpty(task.FormKey)
                            ? "formKey='<отсутствует>'"
                            : $"{task.FormKey}");
                    }
                    else if (!string.IsNullOrEmpty(task.FormKey))
                    {
                        extras.Add($"{task.FormKey}");
                    }

                    if (!string.IsNullOrEmpty(task.Topic))
                        extras.Add($"{task.Topic}");

                    if (!string.IsNullOrEmpty(task.MessageRef))
                        extras.Add($"messageRef='{task.MessageRef}'");

                    if (!string.IsNullOrEmpty(task.Timer))
                        extras.Add($"timer='{task.Timer}'");

                    var infoLine = $"{task.Type,-22} {task.Id,-20} [{string.Join(" ", extras)}] - {task.Name}";
                    _writer.WriteLine($"  {infoLine}");
                }
            }

            foreach (var flow in model.Flows)
            {
                var srcLabel = GetNodeLabel(model, flow.SourceId);
                var tgtLabel = GetNodeLabel(model, flow.TargetId);

                _writer.WriteLine($"Flow: {flow.SourceId} ({srcLabel}) -> {flow.TargetId} ({tgtLabel})");
            }
        }

        private static string GetNodeLabel(BpmnModel model, string id)
        {
            if (!model.AllNodes.TryGetValue(id, out var node))
                return "?";

            if (!string.IsNullOrEmpty(node.Topic))
                return node.Topic;
            if (!string.IsNullOrEmpty(node.FormKey))
                return node.FormKey;
            return node.Type ?? "?";
        }
    }
}
