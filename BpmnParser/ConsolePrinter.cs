using System.IO;

namespace BpmnParser
{
    public class ConsolePrinter(TextWriter writer) : IBpmnPrinter
    {
        private readonly TextWriter _writer = writer;

        public void Print(BpmnModel model)
        {
            _writer.WriteLine("Запуск BPMN-парсера...");

            foreach (var lane in model.Lanes)
            {
                _writer.WriteLine($"Lane: {lane.Name}");
                foreach (var task in lane.Tasks)
                {
                    var parts = new List<string>();

                    if (task.Type == "userTask")
                    {
                        parts.Add(string.IsNullOrEmpty(task.FormKey)
                            ? "formKey='<отсутствует>'"
                            : $"formKey='{task.FormKey}'");
                    }

                    if (!string.IsNullOrEmpty(task.Topic))
                        parts.Add($"topic='{task.Topic}'");

                    if (!string.IsNullOrEmpty(task.MessageRef))
                        parts.Add($"messageRef='{task.MessageRef}'");

                    if (!string.IsNullOrEmpty(task.Timer))
                        parts.Add($"timer='{task.Timer}'");

                    var info = $"{task.Type,-22} {task.Id,-20} [{string.Join(" ", parts)}] - {task.Name}";
                    _writer.WriteLine($"  {info}");
                }
            }

            foreach (var flow in model.Flows)
            {
                var sourceLabel = model.AllNodes.TryGetValue(flow.SourceId, out var s) ? GetLabel(s) : "?";
                var targetLabel = model.AllNodes.TryGetValue(flow.TargetId, out var t) ? GetLabel(t) : "?";
                var cond = string.IsNullOrWhiteSpace(flow.Condition) ? "" : $" [condition: {flow.Condition}]";
                _writer.WriteLine($"Flow: {flow.SourceId} ({sourceLabel}) -> {flow.TargetId} ({targetLabel}){cond}");
            }
        }

        private static string GetLabel(BpmnTask task)
        {
            if (!string.IsNullOrWhiteSpace(task.FormKey))
                return task.FormKey;
            if (!string.IsNullOrWhiteSpace(task.Topic))
                return task.Topic;
            return task.Type;
        }
    }
}
