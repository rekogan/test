using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem.Tests
{
    internal class OfferSystemEndToEndTests
    {
        OfferSystem _os;
        OfferInterpreter _interpreter;

        internal OfferSystemEndToEndTests()
        {
            
        }

        internal void RunAll()
        {
            TestComments();
            TestDriver();
            TestScenario1();
        }

        internal void TestComments()
        {
            var outputLines = Initialize();
            _interpreter.ProcessCommand("DRIVER 42 alice #foobar", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 40 #asdf", _os);
            Assert.IsTrue(outputLines[0] == "alice");

            _interpreter.ProcessCommand("DRIVER 42 #foobar", _os);
            Assert.IsTrue(outputLines[1].ToLowerInvariant().StartsWith("error"));
        }

        internal void TestDriver()
        {
            var outputLines = Initialize();

            List<string> commands = new List<string>();
            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            Assert.IsTrue(outputLines.Count == 0);

            _interpreter.ProcessCommand("SHIPMENT 1 40", _os);
            Assert.IsTrue(outputLines[0] == "alice");
        }

        internal void TestScenario1()
        {
            var outputLines = Initialize();

            List<string> commands = new List<string>();
            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 40", _os);
            Assert.IsTrue(outputLines[0] == "alice");

            _interpreter.ProcessCommand("DRIVER 50 bob", _os);
            _interpreter.ProcessCommand("DRIVER 20 carol", _os);
            _interpreter.ProcessCommand("SHIPMENT 2 40", _os);
            Assert.IsTrue(outputLines[1] == "bob");

            _interpreter.ProcessCommand("OFFER 1 PASS alice", _os);
            Assert.IsTrue(outputLines[2] == "bob");

            _interpreter.ProcessCommand("OFFER 1 ACCEPT bob", _os);
        }

        private List<string> Initialize()
        {
            List<string> outputLines = new List<string>();
            _os = new OfferSystem();
            TestOutputWriter writer = new TestOutputWriter(outputLines);
            _interpreter = new OfferInterpreter(writer);

            return outputLines;
        }

        private void ClearOfferSystem()
        {
            _os = new OfferSystem();
        }
    }
}
