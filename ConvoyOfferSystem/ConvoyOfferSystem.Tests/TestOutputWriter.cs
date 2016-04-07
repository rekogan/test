using System.Collections.Generic;

namespace ConvoyOfferSystem.Tests
{
    /// <summary>
    /// Writes output to given collection of strings for testing purposes.
    /// </summary>
    internal class TestOutputWriter : IOutputWriter
    {
        List<string> _outputLines;
        public TestOutputWriter(List<string> outputLines)
        {
            _outputLines = outputLines;
        }

        void IOutputWriter.WriteLine(string line)
        {
            _outputLines.Add(line);
        }
    }
}
