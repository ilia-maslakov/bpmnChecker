namespace BpmnParser
{
    public static class ProgramRunner
    {
        public static int Run(string[] args, TextWriter output)
        {
            output.WriteLine("Запуск BPMN-парсера...");

            if (args.Length != 1)
            {
                output.WriteLine("Использование: parsebpmn.exe <файл.bpmn>");
                return 1;
            }

            XmlFileLoader loader = new();
            var xmlDoc = loader.Load(args[0]);
            if (xmlDoc == null)
            {
                output.WriteLine("Ошибка загрузки файла.");
                return 2;
            }

            BpmnParser parser = new(xmlDoc);
            var model = parser.Parse();

            ConsolePrinter printer = new(output);
            printer.Print(model);

            return 0;
        }
    }
}
