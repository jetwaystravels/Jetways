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
        //public int infantCount { get; set; }
    }


    public class _Key
    {
        public string journeyKey { get; set; }
        public string fareAvailabilityKey { get; set; }
        public string inventoryControl { get; set; } = "HoldSpace";
    }
    public class PassengerModel
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }

        public String PassengerType { get; set; }
        // Add other properties as needed
    }
    public class passkeytype
    {
        public string passengerkey { get; set; }
        public string passengertypecode { get; set; }
        //public int passengertypecount { get; set; }        
        public string? first { get; set; }
        public string? middle { get; set; }
        public string? last { get; set; }
        public string title { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string origin { get; set; }
        public string code { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string identifier { get; set; }

        public string carrierCode { get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }

    }



    public class Infanttype
    {
        public string passengerkey { get; set; }
        public string formattedDates { get; set; }
        public int passengertypecount { get; set; }
        public string? First { get; set; }
        public string? middle { get; set; }
        public string? Last { get; set; }
        public string Title { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string origin { get; set; }
        public string code { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }

    }






    public class AirAsiaTripSellRequestReturn
    {
        public bool preventOverlap { get; set; }
        public List<_KeyReturn> keys { get; set; }
        public bool suppressPassengerAgeValidation { get; set; }
        public PassengersReturn passengers { get; set; }
        public string currencyCode { get; set; }
        //public int infantCount { get; set; }
    }
    public class _KeyReturn
    {
        public string journeyKey { get; set; }
        public string fareAvailabilityKey { get; set; }
        public string inventoryControl { get; set; } = "HoldSpace";
    }
    public class passkeytypeReturn
    {
        public string passengerkey { get; set; }
        public string passengertypecode { get; set; }
        //public int passengertypecount { get; set; }        
        public string? first { get; set; }
        public string? middle { get; set; }
        public string? last { get; set; }
        public string title { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string origin { get; set; }
        public string code { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string identifier { get; set; }

        public string carrierCode { get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }

    }
    public class InfanttypeReturn
    {
        public string passengerkey { get; set; }
        public int passengertypecount { get; set; }
        public string? First { get; set; }
        public string? middle { get; set; }
        public string? Last { get; set; }
        public string Title { get; set; }
        public string nationality { get; set; }
        public string residentCountry { get; set; }
        public string gender { get; set; }
        public string dateOfBirth { get; set; }
        public string origin { get; set; }
        public string code { get; set; }
        public string departure { get; set; }
        public string destination { get; set; }
        public string identifier { get; set; }
        public string carrierCode { get; set; }
        public string? Email { get; set; }
        public string? mobile { get; set; }

    }

}

