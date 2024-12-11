using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
    public class PassengersModel
    {

        public Name name { get; set; }
        public _Info info { get; set; }


        public class Name
        {

            public string first { get; set; }
            public string middle { get; set; }
            public string? mobile { get; set; }
            public string countryCode { get; set; }
            public string last { get; set; }
            public string title { get; set; }
        }

        public class _Info
        {
            public string nationality { get; set; }
            public string residentCountry { get; set; }
            public string gender { get; set; }
            public string dateOfBirth { get; set; }
        }

    }
}
