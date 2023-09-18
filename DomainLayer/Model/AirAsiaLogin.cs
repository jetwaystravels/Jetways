﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DomainLayer.Model
{
    public class AirAsiaLogin
    {
        public _credentials credentials { get; set; }
        public string applicationName { get; set; } = "";
    }
    public class _credentials
    {
        [Key]
        public string   username  { get; set; }
        public string alternateIdentifier { get; set; }
        public string password { get; set; }
        public string domain { get; set; }
        public string location { get; set; }
        public string channelType { get; set; }
        public string loginRole { get; set; }
    }
}
