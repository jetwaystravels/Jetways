//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class GetItenaryModel
    {
        public List<Ssr1> ssrs { get; set; }
        public List<Key> keys { get; set; }
        public Passengers1 passengers { get; set; }
        public string currencyCode { get; set; }
    }
    public class Ssr1
    {
        public Market market { get; set; }
        public List<Item> items { get; set; }


    }
    public class Market
    {
        public Identifier1 identifier { get; set; }
        public string? destination { get; set; }
        public string? origin { get; set; }
        public string? departureDate { get; set; }
    }
    public class Identifier1
    {
        public string? identifier { get; set; }
        public string? carrierCode { get; set; }
    }
    public class Item
    {
        public string passengerType { get; set; }
        public List<SsrItem> ssrs { get; set; }
    }
    public class SsrItem
    {
        public string ssrCode { get; set; }
        public int count { get; set; }
        public Designatorr designator { get; set; }
    }

    public class Designatorr
    {
        public string? destination { get; set; }
        public string? origin { get; set; }
        public string? departureDate { get; set; }
    }

    public class Key
    {
        public string journeyKey { get; set; }
        public string fareAvailabilityKey { get; set; }
        public string standbyPriorityCode { get; set; }
        public string inventoryControl { get; set; }
    }
    public class Passengers1
    {
        public List<Type2> types { get; set; }
        public string residentCountry { get; set; }
    }
    public class Type2
    {
        public string type { get; set; }
        public int count { get; set; }
    }
}
