using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            IOfferSystem offerSystem = new ConsoleOutputOfferSystem();
            while (true)
            {
                var line = Console.ReadLine();
                if (!ConsoleInputInterpreter.ProcessCommand(line, offerSystem))
                {
                    break;
                }
            }
        }
    }
}
