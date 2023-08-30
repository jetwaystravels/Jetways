using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class AirAsiaTripSellRequest
    {

       
            public bool preventOverlap { get; set; }
            public List<_Key> keys { get; set; }
            public bool suppressPassengerAgeValidation { get; set; }
            public Passengers passengers { get; set; }
            public string currencyCode { get; set; }
            public int infantCount { get; set; }
     }
               

        public class _Key
        {
            public string journeyKey { get; set; }
            public string fareAvailabilityKey { get; set; }
            public string inventoryControl { get; set; }= "HoldSpace";
    }

    public class passkeytype
    {
        public string passengerkey { get; set; }     
        public string passengertypecode { get; set; }
        public int passengertypecount { get; set; }        
        public string? first { get; set; }      
        public string? middle { get; set; }        
        public string? last { get; set; }
        public string title { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string origin { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string identifier { get; set; }

        public string carrierCode { get; set; }        
        public string? Email { get; set; }        
        public string? mobile { get; set; }

    }

   


}

