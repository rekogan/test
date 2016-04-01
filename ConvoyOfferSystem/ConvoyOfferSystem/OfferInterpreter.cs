﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public class OfferInterpreter
    {
        private const string _endCommand = "end";
        private const string _noValidDriversString = "NOBODY";
        private IOutputWriter _outputWriter;

        public OfferInterpreter(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public bool ProcessCommand(string line, OfferSystem offerSystem)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Error: Valid command is missing");
                return true;
            }

            var codeAndComment = line.Split(new char[] { '#' });

            var parts = codeAndComment[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string command = parts[0].ToLowerInvariant();
            switch (command)
            {
                case "driver":
                    if (parts.Length < 3)
                    {
                        OutputInsufficientArguments(parts[0]);
                        return true;
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
                        return true;
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

                    var result = offerSystem.Shipment(shipmentId, shipCapacity);
                    if (result != null)
                    {
                        _outputWriter.WriteLine(result);
                    }
                    else
                    {
                        _outputWriter.WriteLine(_noValidDriversString);
                    }
                    break;

                case "offer":
                    if (parts.Length < 4)
                    {
                        OutputInsufficientArguments(parts[0]);
                        return true;
                    }

                    if (!int.TryParse(parts[1], out shipmentId))
                    {
                        OutputInvalidArgument(command, parts[1]);
                        return true;
                    }
                    OfferResult offerResult;
                    if (!Enum.TryParse<OfferResult>(parts[2].ToLowerInvariant(), out offerResult))
                    {
                        OutputInvalidArgument(command, parts[2]);
                        return true;
                    }
                    result = offerSystem.Offer(shipmentId, offerResult, GetDriverName(parts, 3));
                    if (offerResult != OfferResult.accept)
                    {
                        if (result != null)
                        {
                            _outputWriter.WriteLine(result);
                        }
                        else
                        {
                            _outputWriter.WriteLine(_noValidDriversString);
                        }
                    }
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

        private void OutputUnrecognizedCommand(string command)
        {
            _outputWriter.WriteLine(string.Format("Error: Unrecognized command \'{0}\'", command));
        }

        private void OutputInvalidArgument(string command, string arg)
        {
            _outputWriter.WriteLine(string.Format("Error: Invalid argument \'{0}\' for command \'{1}\'", arg, command));
        }

        private void OutputInsufficientArguments(string command)
        {
            _outputWriter.WriteLine(string.Format("Error: Insufficient arguments provided for command \'{0}\'", command));
        }
    }
}