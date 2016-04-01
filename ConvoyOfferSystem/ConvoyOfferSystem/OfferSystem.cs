using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public class OfferSystem
    {
        SortedSet<Driver> _availableDrivers;
        Dictionary<string, Driver> _nameToDriver;
        Dictionary<int, int> _shipmentToCapacity;

        public OfferSystem()
        {
            _availableDrivers = new SortedSet<Driver>();
            _nameToDriver = new Dictionary<string, Driver>();
            _shipmentToCapacity = new Dictionary<int, int>();
        }

        // Assume unique driver names?
        public void Driver(string driverName, int capacity)
        {
            // TODO _availableDrivers should contain multiple drivers for a given capacity because SortedSet doesn't accept dups
            Driver d = new Driver(driverName, capacity);
            _availableDrivers.Add(d);
            _nameToDriver.Add(driverName, d);
        }

        public string Shipment(int shipmentId, int capacity)
        {
            _shipmentToCapacity.Add(shipmentId, capacity);
            var driver = GetBestDriver(shipmentId, capacity);
            if (driver != null)
            {
                _nameToDriver[driver.Name].IncrementOfferCount();
                return driver.Name;
            }
            else
            {
                return null;
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
                case OfferResult.accept:
                    // Driver accepted the job, remove from pool of available drivers
                    //_availableDrivers.Remove(driver);
                    driver.IncrementOfferCount();
                    return null;

                case OfferResult.expire:
                case OfferResult.pass:
                    // Offer expired or driver rejected, return next best driver
                    driver.AddRejectedExpiredOffer(shipmentId);
                    return GetBestDriver(shipmentId, requiredCapacity).Name;

                default:
                    return null;
            }
        }

        private Driver GetBestDriver(int shipmentId, int capacity)
        {
            if (capacity > _availableDrivers.Max.Capacity)
            {
                return null;
            }

            var candidates = _availableDrivers.GetViewBetween(new Driver(string.Empty, capacity), _availableDrivers.Max);

            if (candidates.Count > 0)
            {
                int minOfferCount = int.MaxValue;
                Driver bestCandidate = candidates.First();
                foreach (var c in candidates)
                {
                    if (!c.RejectedExpiredOffers.Contains(shipmentId) &&
                        c.OfferCount < minOfferCount)
                    {
                        bestCandidate = c;
                        minOfferCount = c.OfferCount;
                    }
                }

                return bestCandidate;
            }
            else
            {
                return null;
            }
        }
    }
}
