using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    class Program
    {
        public const string _endCommand = "END";

        static void Main(string[] args)
        {
            var offerSystem = new OfferSystem();
            while (true)
            {
                var line = Console.ReadLine();
                ProcessCommand(line, offerSystem);

                
            }
        }

        private static void ProcessCommand(string line, OfferSystem offerSystem)
        {
            var parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLowerInvariant();
            switch (command)
            {
                case "driver":
                    //string[] args = new string[parts.Length - 1];
                    //offerSystem.Driver(Array.Copy(parts, 1, args, 0, parts.Length - 1));
            }
        }
    }
}
