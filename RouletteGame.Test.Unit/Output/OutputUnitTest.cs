using System.IO;
using NUnit.Framework;
using RouletteGame.Output;

namespace RouletteGame.Test.Unit.Output
{
    [TestFixture]
    public class OutputUnitTest
    {
        private StringWriter fakeConsole;
        private ConsoleOutput uut;
        [SetUp]
        public void Setup()
        {
            fakeConsole = new StringWriter();
            System.Console.SetOut(fakeConsole);
            uut = new ConsoleOutput();
        }

        [Test]
        public void Report_Text_CorrectOutput()
        {
            uut.Report("Test string");
            Assert.That(fakeConsole.ToString().Contains("Test string"));
        }
        
    }
}