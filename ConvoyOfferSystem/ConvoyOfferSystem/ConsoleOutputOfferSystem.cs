using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public class ConsoleOutputOfferSystem : IOfferSystem
    {
        SortedSet<Driver> _drivers;

        public ConsoleOutputOfferSystem()
        {
            _drivers = new SortedSet<ConvoyOfferSystem.Driver>();
        }

        // Assume unique driver names?
        public void Driver(string driverName, int capacity)
        {
            _drivers.Add(new Driver(driverName, capacity));
        }

        public string Shipment(int shipmentId, int capacity)
        {
            // or is it capacity - 1?
            var candidates = _drivers.GetViewBetween(new Driver(string.Empty, capacity), _drivers.Max);
            return string.Empty;

            if (candidates.Count > 0)
            {
                Driver bestCandidate = candidates.First();
                foreach (var c in candidates)
                {
                    if (!c.RejectedExpiredOffers.Contains(shipmentId))
                    {
                        if (c.AcceptedOfferCount < bestCandidate.AcceptedOfferCount)
                        {
                            bestCandidate = c;
                        }
                    }
                }

                return bestCandidate.Name;
            }
            else
            {
                return "nobody";
            }
        }

        public string Offer(int shipmentId, OfferResult result, string driverName)
        {
            return string.Empty;
        }
    }
}
