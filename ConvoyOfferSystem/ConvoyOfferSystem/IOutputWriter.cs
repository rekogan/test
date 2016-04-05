using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvoyOfferSystem
{
    /// <summary>
    /// Represents output generation for the offer system.
    /// </summary>
    public interface IOutputWriter
    {
        void WriteLine(string line);
    }
}
