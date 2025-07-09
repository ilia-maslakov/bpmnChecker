using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace BpmnParser.Tests
{
    public class ConsolePrinterTests
    {
        [Fact]
        public void PrintsExclusiveGatewayWithConditions()
        {
            var model = new BpmnModel();

            model.AllNodes.Add("Gateway_1", new BpmnTask
            {
                Id = "Gateway_1",
                Type = "exclusiveGateway",
                Name = "Решение"
            });

            model.AllNodes.Add("Task_Yes", new BpmnTask
            {
                Id = "Task_Yes",
                Type = "userTask",
                FormKey = "order/approve",
                Name = "Одобрить"
            });

            model.AllNodes.Add("Task_No", new BpmnTask
            {
                Id = "Task_No",
                Type = "userTask",
                FormKey = "order/reject",
                Name = "Отклонить"
            });

            model.Flows.Add(new BpmnFlow
            {
                SourceId = "Gateway_1",
                TargetId = "Task_Yes",
                Condition = "${approve=='yes'}"
            });

            model.Flows.Add(new BpmnFlow
            {
                SourceId = "Gateway_1",
                TargetId = "Task_No",
                Condition = "${approve=='no'}"
            });

            model.Lanes.Add(new BpmnLane
            {
                Name = "Main",
                Tasks =
                {
                    new BpmnTask { Id = "Gateway_1", Type = "exclusiveGateway", Name = "Решение" },
                    new BpmnTask { Id = "Task_Yes", Type = "userTask", FormKey = "order/approve", Name = "Одобрить" },
                    new BpmnTask { Id = "Task_No", Type = "userTask", FormKey = "order/reject", Name = "Отклонить" }
                }
            });

            using var sw = new StringWriter();
            var printer = new ConsolePrinter(sw);
            printer.Print(model);

            var output = sw.ToString();

            Assert.Contains("exclusiveGateway", output);
            Assert.Contains("${approve=='yes'} -> order/approve", output);
            Assert.Contains("${approve=='no'} -> order/reject", output);
        }
    }
}
