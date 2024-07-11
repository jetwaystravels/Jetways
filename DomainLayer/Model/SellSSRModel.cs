using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class SellSSRModel
    {      
            public int count { get; set; }
            public string note { get; set; }
            public bool forceWaveOnSell { get; set; }
            public string currencyCode { get; set; }
            public int ssrSellMode { get; set; }

    }
}
