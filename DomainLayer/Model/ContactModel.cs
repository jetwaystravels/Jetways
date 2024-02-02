using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.ContactModel;

namespace DomainLayer.Model
{
    public class ContactModel
    {
        public string number { get; set; }
        public string contactTypeCode { get; set; }
        public List<_Phonenumber> phoneNumbers { get; set; }
        public _Address address { get; set; }
        public string emailAddress { get; set; }
        public string distributionOption { get; set; } = "Email";
        public _Name name { get; set; }
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
        public string lineOne { get; set; }
        //public string countryCode { get; set; }
        // public string provinceState { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }


    }
    public class _Phonenumber
    {
        public string type { get; set; }
        [Required]
        public string number { get; set; }
    }
    public class _Address
    {
        public string lineOne { get; set; }
        public string countryCode { get; set; }
        public string provinceState { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
    }

    public class _Name
    {
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
    }

    public class AddGSTInformation
    {
        public string contactTypeCode { get; set; }
        public List<GSTPhonenumber> phoneNumbers { get; set; }
        public string cultureCode { get; set; }
        public GSTAddress Address { get; set; }
        public string emailAddress { get; set; }
        public string customerNumber { get; set; }
        public string sourceOrganization { get; set; }
        public string distributionOption { get; set; }
        public string notificationPreference { get; set; }
        public string companyName { get; set; }

        public GSTName Name { get; set; }
        //public string lineOne { get; set; }
        //public string lineTwo { get; set; }
        //public string city { get; set; }
        //public int postalCode { get; set; }
        //public string provinceState { get; set; }        




    }

    public class GSTAddress
    {
        public string lineOne { get; set; }
        public string lineTwo { get; set; }
        public string lineThree { get; set; }
        public string countryCode { get; set; }
        public string provinceState { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }

    }
    public class GSTName
    {
        public string first { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
        public string suffix { get; set; }
    }
    public class GSTPhonenumber
    {
        public string type { get; set; }
        [Required]
        public string number { get; set; }
    }



}




