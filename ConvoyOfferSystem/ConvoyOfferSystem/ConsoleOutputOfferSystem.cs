using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public class ConsoleOutputOfferSystem : IOfferSystem
    {
        private const string _noValidDriversString = "NOBODY";
        SortedSet<Driver> _availableDrivers;
        Dictionary<string, Driver> _nameToDriver;
        Dictionary<int, int> _shipmentToCapacity;

        public ConsoleOutputOfferSystem()
        {
            _availableDrivers = new SortedSet<Driver>();
            _nameToDriver = new Dictionary<string, Driver>();
            _shipmentToCapacity = new Dictionary<int, int>();
        }

        // Assume unique driver names?
        public void Driver(string driverName, int capacity)
        {
            Driver d = new Driver(driverName, capacity);
            _availableDrivers.Add(d);
            _nameToDriver.Add(driverName, d);
        }

        public string Shipment(int shipmentId, int capacity)
        {
            _shipmentToCapacity.Add(shipmentId, capacity);

            // or is it capacity - 1?
            var candidates = _availableDrivers.GetViewBetween(new Driver(string.Empty, capacity), _availableDrivers.Max);
            return string.Empty;

            if (candidates.Count > 0)
            {
                Driver bestCandidate = candidates.First();
                foreach (var c in candidates)
                {
                    if (!c.RejectedExpiredOffers.Contains(shipmentId) &&
                        c.AcceptedOfferCount < bestCandidate.AcceptedOfferCount)
                    {
                        bestCandidate = c;
                    }
                }

                return bestCandidate.Name;
            }
            else
            {
                return _noValidDriversString;
            }
        }

        public string Offer(int shipmentId, OfferResult result, string driverName)
        {
            Driver driver;
            if (!_nameToDriver.TryGetValue(driverName, out driver))
            {
                return string.Format("Error: unrecognized driver \'{0}\'", driverName);
            }
            int requiredCapacity;
            if (!_shipmentToCapacity.TryGetValue(shipmentId, out requiredCapacity))
            {
                return string.Format("Error: unrecognized shipment ID \'{0}\'", shipmentId);
            }

            switch (result)
            {
                case OfferResult.Accept:
                    // Driver accepted the job, remove from pool of available drivers
                    _availableDrivers.Remove(driver);
                    driver.IncrementAcceptedOfferCount();
                    return null;

                case OfferResult.Expire:
                case OfferResult.Pass:
                    // Offer expired or driver rejected, return next best driver
                    driver.AddRejectedExpiredOffer(shipmentId);
                    return Shipment(shipmentId, requiredCapacity);

                default:
                    return null;
            }
        }
    }
}
