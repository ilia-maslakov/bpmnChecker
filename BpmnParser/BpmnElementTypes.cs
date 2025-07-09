namespace BpmnParser
{
    public static class BpmnElementTypes
    {
        public static readonly string[] Supported =
        [
            // Задачи
            "userTask",
            "serviceTask",
            "sendTask",

            // События
            "startEvent",
            "endEvent",
            "intermediateCatchEvent",
            "intermediateThrowEvent",

            // Шлюзы
            "exclusiveGateway",
            "parallelGateway"
        ];
    }
}