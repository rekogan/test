using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var tests = new OfferSystemEndToEndTests();
            try
            {
                tests.RunAll();
            }
            catch (Exception e)
            {
                Console.WriteLine("Test failed!");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    }
}
