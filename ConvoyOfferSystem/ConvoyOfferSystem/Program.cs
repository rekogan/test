using System;

namespace ConvoyOfferSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            OfferSystem offerSystem = new OfferSystem();
            OfferInterpreter interpreter = new OfferInterpreter(new ConsoleOutputWriter());
            while (true)
            {
                var line = Console.ReadLine();
                if (!interpreter.ProcessCommand(line, offerSystem))
                {
                    break;
                }
            }
        }
    }
}
