using System;
using System.Collections.Generic;

namespace ConvoyOfferSystem
{
    /// <summary>
    /// Encapsulates a driver for the Convoy network
    /// </summary>
    internal class Driver : IComparable<Driver>
    {
        public Driver(string name, uint capacity)
        {
            Name = name;
            Capacity = capacity;
            RejectedExpiredOffers = new HashSet<uint>();
            OfferCount = 0;
        }

        public string Name { get; set; }

        public uint Capacity { get; set; }

        /// <summary>
        /// Collection of offers that received a PASS or EXPIRE for this driver
        /// </summary>
        public HashSet<uint> RejectedExpiredOffers { get; private set; }

        /// <summary>
        /// Number of offers we have sent to this Driver, regardless of whether
        /// or not they were accepted.
        /// </summary>
        public int OfferCount { get; private set; }

        public void AddRejectedExpiredOffer(uint shipmentId)
        {
            RejectedExpiredOffers.Add(shipmentId);
        }

        public void IncrementOfferCount()
        {
            OfferCount++;
        }

        /// <summary>
        /// IComparable overload so Driver objects can be used as keys in a SortedSet.
        /// Compare capacity first, and use Name (which is unique for each driver)
        /// as a tie-breaker.
        /// </summary>
        int IComparable<Driver>.CompareTo(Driver other)
        {
            if (this.Capacity != other.Capacity)
            {
                return (int)this.Capacity - (int)other.Capacity;
            }
            else
            {
                return this.Name.CompareTo(other.Name);
            }
        }
    }
}
