using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Model.PassengersModel;

namespace DomainLayer.Model
{
	public class AddInFantModel
	{
		public string nationality { get; set; }
        public string dateOfBirth { get; set; }
        public string residentCountry { get; set; }
		public string gender { get; set; }
	   public InfantName name { get; set; }
	}
	public class InfantName
	{
		public string first { get; set; }	
		public string middle { get; set; }
        public string last { get; set; }
        public string title { get; set; }
		public string suffix { get; set; }
	}

}
