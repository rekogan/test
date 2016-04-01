using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConvoyOfferSystem.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            OfferSystem os = new OfferSystem();

            List<string> outputLines = new List<string>();
            TestOutputWriter writer = new TestOutputWriter(outputLines);
            OfferInterpreter interpreter = new OfferInterpreter(writer);

            List<string> commands = new List<string>();
            commands.Add("DRIVER 42 alice");
            commands.Add("SHIPMENT 1 40");
            commands.Add("DRIVER 50 bob");
            commands.Add("DRIVER 20 carol");
            commands.Add("SHIPMENT 2 40");
            commands.Add("OFFER 1 PASS alice");
            commands.Add("OFFER 1 ACCEPT bob");

            foreach (var command in commands)
            {
                interpreter.ProcessCommand(command, os);
            }

            Assert.IsTrue(outputLines[0] == "alice");
            Assert.IsTrue(outputLines[1] == "bob");
            Assert.IsTrue(outputLines[2] == "bob");
        }
    }
}
