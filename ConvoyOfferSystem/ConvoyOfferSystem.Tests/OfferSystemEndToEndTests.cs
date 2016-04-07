using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConvoyOfferSystem.Tests
{
    /// <summary>
    /// End-to-end tests of the offer system.
    /// </summary>
    internal class OfferSystemEndToEndTests
    {
        OfferSystem _os;
        OfferInterpreter _interpreter;

        internal void RunAll()
        {
            TestComments();
            TestDriver();
            TestShipment();
            TestOffer();
            TestScenario1();
        }

        internal void TestComments()
        {
            var outputLines = Initialize();
            _interpreter.ProcessCommand("DRIVER 42 alice #foobar", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 40 #asdf", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            _interpreter.ProcessCommand("DRIVER 42 #foobar", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1].ToLowerInvariant().StartsWith("error"));
        }

        internal void TestDriver()
        {
            var outputLines = Initialize();
            
            // add single driver
            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            Assert.IsTrue(outputLines.Count == 0);

            _interpreter.ProcessCommand("SHIPMENT 1 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            // add multiple drivers with the same capacity
            _interpreter.ProcessCommand("DRIVER 42 bob", _os);
            _interpreter.ProcessCommand("SHIPMENT 2 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "bob");

            // driver with multi-word name
            _interpreter.ProcessCommand("DRIVER 50 john doe", _os);
            _interpreter.ProcessCommand("SHIPMENT 3 45", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "john doe");
        }

        internal void TestShipment()
        {
            var outputLines = Initialize();

            // no drivers added yet - NOBODY
            _interpreter.ProcessCommand("SHIPMENT 1 10", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == OfferInterpreter.NoValidDriversString);

            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            _interpreter.ProcessCommand("DRIVER 30 bob", _os);
            _interpreter.ProcessCommand("DRIVER 20 charlie", _os);
            _interpreter.ProcessCommand("DRIVER 20 carol", _os);

            // no drivers have sufficient capacity - NOBODY
            _interpreter.ProcessCommand("SHIPMENT 2 50", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == OfferInterpreter.NoValidDriversString);

            // shipment capacity exactly equals highest driver capacity
            _interpreter.ProcessCommand("SHIPMENT 3 42", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            // same shipment requested again - alice is the only driver capable of taking it again
            _interpreter.ProcessCommand("SHIPMENT 3 42", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            // alice and bob both have capacity, but bob has had fewer offers
            _interpreter.ProcessCommand("SHIPMENT 4 25", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "bob");
        }

        internal void TestOffer()
        {
            var outputLines = Initialize();

            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 40", _os);
            Assert.IsTrue(outputLines[0] == "alice");
            
            // no output for accepted offer
            _interpreter.ProcessCommand("OFFER 1 ACCEPT alice", _os);
            Assert.IsTrue(outputLines.Count == 1);
            
            // clear offer system and reinitialize
            outputLines = Initialize();

            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            _interpreter.ProcessCommand("DRIVER 45 charlie", _os);
            _interpreter.ProcessCommand("DRIVER 30 bob", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 44", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "charlie");

            // no valid drivers with this capacity
            _interpreter.ProcessCommand("OFFER 1 PASS charlie", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == OfferInterpreter.NoValidDriversString);

            // already offered to charlie and he passed, no more valid drivers
            _interpreter.ProcessCommand("SHIPMENT 1 44", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == OfferInterpreter.NoValidDriversString);

            // alice has 2 offers and charlie has 1, offer a shipment to charlie that he rejects
            _interpreter.ProcessCommand("SHIPMENT 3 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");
            _interpreter.ProcessCommand("SHIPMENT 4 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");
            _interpreter.ProcessCommand("OFFER 3 PASS charlie", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            // nonexistant shipment ID
            _interpreter.ProcessCommand("OFFER 2 ACCEPT alice", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1].ToLowerInvariant().StartsWith("error"));

            // nonexistant driver
            _interpreter.ProcessCommand("OFFER 1 ACCEPT foobar", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1].ToLowerInvariant().StartsWith("error"));
        }

        // Supplied test scenario from the assignment prompt
        internal void TestScenario1()
        {
            var outputLines = Initialize();

            _interpreter.ProcessCommand("DRIVER 42 alice", _os);
            _interpreter.ProcessCommand("SHIPMENT 1 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "alice");

            _interpreter.ProcessCommand("DRIVER 50 bob", _os);
            _interpreter.ProcessCommand("DRIVER 20 carol", _os);
            _interpreter.ProcessCommand("SHIPMENT 2 40", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "bob");

            _interpreter.ProcessCommand("OFFER 1 PASS alice", _os);
            Assert.IsTrue(outputLines[outputLines.Count - 1] == "bob");

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
