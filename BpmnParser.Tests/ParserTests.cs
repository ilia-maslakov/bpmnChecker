using System.Xml;
using Xunit;

namespace BpmnParser.Tests
{
    public class ParserTests
    {
        private const string SampleXml = """
<definitions xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\">
  <process id=\"p1\">
    <laneSet>
      <lane id=\"lane1\" name=\"Lane 1\">
        <flowNodeRef>task1</flowNodeRef>
      </lane>
    </laneSet>
    <userTask id=\"task1\" name=\"Task 1\" />
    <serviceTask id=\"task2\" name=\"Task 2\" />
    <sequenceFlow id=\"flow1\" sourceRef=\"task1\" targetRef=\"task2\" />
  </process>
</definitions>
""";

        [Fact]
        public void Parse_ReturnsModelWithLanesTasksAndFlows()
        {
            var doc = new XmlDocument();
            doc.LoadXml(SampleXml);
            var parser = new BpmnParser(doc);

            var model = parser.Parse();

            Assert.Single(model.Lanes);
            var lane = model.Lanes[0];
            Assert.Equal("Lane 1", lane.Name);
            Assert.Single(lane.Tasks);
            var task = lane.Tasks[0];
            Assert.Equal("task1", task.Id);
            Assert.Equal("Task 1", task.Name);
            Assert.Equal("userTask", task.Type);

            Assert.Single(model.Flows);
            var flow = model.Flows[0];
            Assert.Equal("task1", flow.SourceId);
            Assert.Equal("task2", flow.TargetId);
        }
    }
}
