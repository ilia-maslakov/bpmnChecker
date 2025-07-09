using System.Xml;
using Xunit;

namespace BpmnParser.Tests
{
    public class ParserTests
    {
        private const string SampleXml = """
<definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <process id="p1">
    <laneSet>
      <lane id="lane1" name="Lane 1">
        <flowNodeRef>task1</flowNodeRef>
      </lane>
    </laneSet>
    <userTask id="task1" name="Task 1" />
    <serviceTask id="task2" name="Task 2" />
    <sequenceFlow id="flow1" sourceRef="task1" targetRef="task2" />
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

        [Fact]
        public void Parse_TaskWithMessageRefAndTimer()
        {
            var xml = """
<definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <process id="p1">
    <laneSet>
      <lane id="lane1" name="Lane 1">
        <flowNodeRef>send1</flowNodeRef>
        <flowNodeRef>timer1</flowNodeRef>
      </lane>
    </laneSet>
    <sendTask id="send1" name="Send" >
      <messageEventDefinition messageRef="myMsg" />
    </sendTask>
    <intermediateCatchEvent id="timer1" name="Timer">
      <timerEventDefinition>
        <timeDuration>PT5M</timeDuration>
      </timerEventDefinition>
    </intermediateCatchEvent>
  </process>
</definitions>
""";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var parser = new BpmnParser(doc);

            var model = parser.Parse();

            Assert.Single(model.Lanes);
            var lane = model.Lanes[0];
            Assert.Equal(2, lane.Tasks.Count);

            var send = lane.Tasks[0];
            Assert.Equal("send1", send.Id);
            Assert.Equal("sendTask", send.Type);
            Assert.Equal("myMsg", send.MessageRef);

            var timer = lane.Tasks[1];
            Assert.Equal("timer1", timer.Id);
            Assert.Equal("intermediateCatchEvent", timer.Type);
            Assert.Equal("PT5M", timer.Timer);
        }

        [Fact]
        public void Parse_ParallelGateway_ExcludedFromLaneTasks()
        {
            var xml = """
<definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <process id="p1">
    <laneSet>
      <lane id="lane1" name="Lane 1">
        <flowNodeRef>pg1</flowNodeRef>
        <flowNodeRef>task1</flowNodeRef>
      </lane>
    </laneSet>
    <parallelGateway id="pg1" name="Gateway" />
    <userTask id="task1" name="Task" />
  </process>
</definitions>
""";

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var parser = new BpmnParser(doc);

            var model = parser.Parse();

            Assert.Equal(2, model.AllNodes.Count);
            Assert.Single(model.Lanes);
            var lane = model.Lanes[0];
            Assert.Single(lane.Tasks);
            Assert.Equal("task1", lane.Tasks[0].Id);
        }
    }
}
