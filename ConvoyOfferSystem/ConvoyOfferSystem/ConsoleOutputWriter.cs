using System;

namespace ConvoyOfferSystem
{
    class ConsoleOutputWriter : IOutputWriter
    {
        void IOutputWriter.WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
