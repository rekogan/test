﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem.Tests
{
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