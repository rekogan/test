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
            OfferCount = 0;
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public HashSet<int> RejectedExpiredOffers { get; private set; }

        public int OfferCount { get; private set; }

        public void AddRejectedExpiredOffer(int shipmentId)
        {
            // check dups?
            RejectedExpiredOffers.Add(shipmentId);
        }

        public void IncrementOfferCount()
        {
            OfferCount++;
        }

        int IComparable<Driver>.CompareTo(Driver other)
        {
            if (this.Capacity != other.Capacity)
            {
                return this.Capacity - other.Capacity;
            }
            else
            {
                return this.Name.CompareTo(other.Name);
            }
        }
    }
}
