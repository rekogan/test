using System.Collections.Generic;

namespace ConvoyOfferSystem
{
    /// <summary>
    /// Manages the offer system using 3 commands.
    /// DRIVER adds a new driver to the system.
    /// SHIPMENT registers a new shipment and returns the best driver to send the offer to.
    /// OFFER registers a (ACCEPT/PASS/EXPIRE) offer result. Best driver is returned if
    /// the result is PASS or EXPIRE.
    /// </summary>
    public class OfferSystem
    {
        /// <summary>
        /// We use a sorted set because we want efficient insertion of new drivers and
        /// efficient retrieval of existing drivers whose capacity is >= some number.
        /// SortedSet is backe by a balanced BST in .NET and contains an operation
        /// called GetViewBetween that returns all values in the set between a lower
        /// and upper bound in O(log(N) + K) time, where N is the total size of
        /// the SortedSet and K is the number of values in the set that fall in that range.
        /// A SortedDictionary class exists in .NET but does not have the same functionality,
        /// so we used this for expediency.
        /// </summary>
        SortedSet<Driver> _availableDrivers;

        /// <summary>
        /// A mapping of driver names (unique) to the Driver object they are represented by.
        /// Used to efficiently lookup the Driver's properties when executing the OFFER command.
        /// </summary>
        Dictionary<string, Driver> _nameToDriver;

        /// <summary>
        /// Used to efficiently look up the capacity of shipments when 
        /// executing the OFFER command.
        /// </summary>
        Dictionary<uint, uint> _shipmentToCapacity;

        public OfferSystem()
        {
            _availableDrivers = new SortedSet<Driver>();
            _nameToDriver = new Dictionary<string, Driver>();
            _shipmentToCapacity = new Dictionary<uint, uint>();
        }

        /// <summary>
        /// Adds the given driver to the pool of available drivers
        /// </summary>
        /// <param name="driverName">Assumes unique driver names</param>
        /// <param name="capacity">Capacity of the driver</param>
        public void Driver(string driverName, uint capacity)
        {
            Driver d = new Driver(driverName, capacity);
            _availableDrivers.Add(d);
            _nameToDriver.Add(driverName, d);
        }

        /// <summary>
        /// Registers a shipment and returns the best driver based on 
        /// </summary>
        /// <param name="shipmentId">Unique positive integer</param>
        /// <param name="capacity">positive integer representing required capacity of the shipment</param>
        /// <returns></returns>
        public string Shipment(uint shipmentId, uint capacity)
        {
            if (!_shipmentToCapacity.ContainsKey(shipmentId))
            {
                _shipmentToCapacity.Add(shipmentId, capacity);
            }
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

        /// <summary>
        /// ACCEPT means driver has accepted, no output required.
        /// PASS and EXPIRE means driver has either rejected the offer,
        /// or it has expired. Add the shipmentId associated with this offer
        /// to the expired/rejected offers for the given driver and output 
        /// the name of the next best driver.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="result"></param>
        /// <param name="driverName"></param>
        /// <param name="hasError">If error was encountered, we want to output it even if it was accepted.</param>
        /// <returns></returns>
        public string Offer(uint shipmentId, OfferResult result, string driverName, out bool hasError)
        {
            hasError = false;

            Driver driver;
            if (!_nameToDriver.TryGetValue(driverName, out driver))
            {
                hasError = true;
                return string.Format("Error: unrecognized driver \'{0}\'", driverName);
            }
            uint requiredCapacity;
            if (!_shipmentToCapacity.TryGetValue(shipmentId, out requiredCapacity))
            {
                hasError = true;
                return string.Format("Error: unrecognized shipment ID \'{0}\'", shipmentId);
            }

            switch (result)
            {
                case OfferResult.accept:
                    return null;

                case OfferResult.expire:
                case OfferResult.pass:
                    // Offer expired or driver rejected, return next best driver
                    driver.AddRejectedExpiredOffer(shipmentId);
                    var bestDriver = GetBestDriver(shipmentId, requiredCapacity);
                    if (bestDriver != null)
                    {
                        driver.IncrementOfferCount();
                        return bestDriver.Name;
                    }
                    else
                    {
                        return null;
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculates the best driver for the job if offer was passed/exired or for a new shipment.
        /// Best driver is determined by (1) whether the truck can carry a load (capacity),
        /// (2) Has this shipment been offered to the driver before (don't send offer
        /// if this driver has already rejected this shipment or it has expired) and
        /// (3) How many total offers has the driver gotten (prioritize drivers with fewest
        /// offers if all else is equal).
        /// </summary>
        private Driver GetBestDriver(uint shipmentId, uint capacity)
        {
            if (_availableDrivers.Count == 0 || capacity > _availableDrivers.Max.Capacity)
            {
                return null;
            }

            var candidates = _availableDrivers.GetViewBetween(new Driver(string.Empty, capacity), _availableDrivers.Max);

            if (candidates.Count > 0)
            {
                int minOfferCount = int.MaxValue;
                Driver bestCandidate = null;
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
