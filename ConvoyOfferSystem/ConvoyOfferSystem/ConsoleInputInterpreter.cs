using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public class ConsoleInputInterpreter
    {
        public const string _endCommand = "end";

        public static bool ProcessCommand(string line, IOfferSystem offerSystem)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Error: Valid command is missing");
            }
            var parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string command = parts[0].ToLowerInvariant();
            switch (command)
            {
                case "driver":
                    if (parts.Length < 3)
                    {
                        OutputInsufficientArguments(parts[0]);
                    }

                    int capacity;
                    if (!int.TryParse(parts[1], out capacity))
                    {
                        OutputInvalidArgument(command, parts[1]);
                        return true;
                    }

                    offerSystem.Driver(GetDriverName(parts, 2), capacity);
                    break;

                case "shipment":
                    if (parts.Length < 3)
                    {
                        OutputInsufficientArguments(parts[0]);
                    }

                    int shipmentId;
                    if (!int.TryParse(parts[1], out shipmentId))
                    {
                        OutputInvalidArgument(command, parts[1]);
                        return true;
                    }
                    int shipCapacity;
                    if (!int.TryParse(parts[2], out shipCapacity))
                    {
                        OutputInvalidArgument(command, parts[2]);
                        return true;
                    }

                    offerSystem.Shipment(shipmentId, shipCapacity);
                    break;

                case "offer":
                    if (parts.Length < 4)
                    {
                        OutputInsufficientArguments(parts[0]);
                    }

                    if (!int.TryParse(parts[1], out shipmentId))
                    {
                        OutputInvalidArgument(command, parts[1]);
                        return true;
                    }
                    OfferResult offerResult;
                    if (!Enum.TryParse<OfferResult>(parts[2], out offerResult))
                    {
                        OutputInvalidArgument(command, parts[2]);
                        return true;
                    }
                    offerSystem.Offer(shipmentId, offerResult, GetDriverName(parts, 3));
                    break;

                case _endCommand:
                    return false;

                default:
                    OutputUnrecognizedCommand(command);
                    break;

            }

            return true;
        }

        private static string GetDriverName(string[] parts, int startIndex)
        {
            StringBuilder driverName = new StringBuilder();
            for (int i = startIndex; i < parts.Length; i++)
            {
                if (i > startIndex)
                {
                    driverName.Append(" ");
                }
                driverName.Append(parts[i]);
            }

            return driverName.ToString();
        }

        private static void OutputUnrecognizedCommand(string command)
        {
            Console.WriteLine(string.Format("Error: Unrecognized command \'{0}\'", command));
        }

        private static void OutputInvalidArgument(string command, string arg)
        {
            Console.WriteLine(string.Format("Error: Invalid argument \'{0}\' for command \'{1}\'", arg, command));
        }

        private static void OutputInsufficientArguments(string command)
        {
            Console.WriteLine(string.Format("Error: Insufficient arguments provided for command \'{0}\'", command));
        }
    }
}
