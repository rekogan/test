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
            OfferSystem offerSystem = new OfferSystem();
            while (true)
            {
                var line = Console.ReadLine();
                if (!ConsoleInOutInterpreter.ProcessCommand(line, offerSystem))
                {
                    break;
                }
            }
        }
    }
}
