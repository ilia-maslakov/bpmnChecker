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
                    _writer.WriteLine($"  Task: {task.Id} - {task.Name} ({task.Type})");
            }

            foreach (var flow in model.Flows)
                _writer.WriteLine($"Flow: {flow.SourceId} -> {flow.TargetId}");
        }
    }
}