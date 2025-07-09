using System;
using System.IO;
using Xunit;

namespace BpmnParser.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void Run_InvalidArgs_PrintsUsage()
        {
            using var sw = new StringWriter();
            var code = ProgramRunner.Run([], sw);
            var output = sw.ToString();

            Assert.Equal(1, code);
            Assert.Contains("Использование", output);
        }

        [Fact]
        public void Run_NonexistentFile_PrintsError()
        {
            using var sw = new StringWriter();
            var code = ProgramRunner.Run(["no-such-file.bpmn"], sw);
            var output = sw.ToString();

            Assert.Equal(2, code);
            Assert.Contains("Ошибка загрузки", output);
        }

        [Fact]
        public void Run_ValidFile_PrintsModel()
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, """
<definitions xmlns="http://www.omg.org/spec/BPMN/20100524/MODEL">
  <process id="p1">
    <laneSet>
      <lane id="lane1" name="Lane 1">
        <flowNodeRef>task1</flowNodeRef>
      </lane>
    </laneSet>
    <userTask id="task1" name="Task 1" />
  </process>
</definitions>
""");

            using var sw = new StringWriter();
            var code = ProgramRunner.Run([path], sw);
            var output = sw.ToString();

            Assert.Equal(0, code);
            Assert.Contains("Lane 1", output);
        }
    }
}