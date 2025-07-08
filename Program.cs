namespace BpmnParser
{
    class Program
    {
        static void Main(string[] args)
        {
            // utf8 in console
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length != 1)
            {
                Console.WriteLine("Использование: parsebpmn.exe <файл.bpmn>");
                return;
            }

            IFileLoader loader = new XmlFileLoader();
            var xmlDoc = loader.Load(args[0]);
            if (xmlDoc == null)
            {
                Console.WriteLine("Ошибка загрузки файла.");
                return;
            }

            IBpmnParser parser = new BpmnParser(xmlDoc);
            var model = parser.Parse();

            IBpmnPrinter printer = new ConsolePrinter();
            printer.Print(model);
        }
    }
}