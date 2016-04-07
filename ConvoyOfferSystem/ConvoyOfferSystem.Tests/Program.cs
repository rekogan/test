using System;

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
                Console.WriteLine("All tests passed!");
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
