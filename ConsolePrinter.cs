namespace BpmnParser
{
    public class ConsolePrinter : IBpmnPrinter
    {
        public void Print(BpmnModel model)
        {
            Console.WriteLine("Дорожки и задачи:");
            foreach (var lane in model.Lanes)
            {
                Console.WriteLine($"  Дорожка: {lane.Name}");
                foreach (var task in lane.Tasks)
                {
                    Console.WriteLine($"    [{task.Type}] {task.Name} (id={task.Id})");
                }
            }

            Console.WriteLine("\nСвязи:");
            foreach (var flow in model.Flows)
            {
                Console.WriteLine($"  {flow.SourceId} → {flow.TargetId}");
            }
        }
    }
}