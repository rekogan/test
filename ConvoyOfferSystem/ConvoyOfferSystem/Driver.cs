using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    internal class Driver : IComparable<Driver>
    {
        public Driver(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
            RejectedExpiredOffers = new HashSet<int>();
            AcceptedOfferCount = 0;
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public HashSet<int> RejectedExpiredOffers { get; private set; }

        public int AcceptedOfferCount { get; private set; }

        public void AddRejectedExpiredOffer(int shipmentId)
        {
            // check dups?
            RejectedExpiredOffers.Add(shipmentId);
        }

        public void IncrementAcceptedOfferCount()
        {
            AcceptedOfferCount++;
        }

        int IComparable<Driver>.CompareTo(Driver other)
        {
            return this.Capacity - other.Capacity;
        }
    }
}
