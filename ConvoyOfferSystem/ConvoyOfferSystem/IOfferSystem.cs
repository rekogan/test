using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    public interface IOfferSystem
    {
        void Driver(string driverName, int capacity);

        string Shipment(int shipmentId, int capacity);

        string Offer(int shipmentId, OfferResult result, string driverName);
    }
}
