using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
