using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
	public class TicketBooking
	{
		[Key]
		public int id { get; set; }
		public string recordLocator { get; set; }
		public string bookingKey { get; set; }
		public string currencyCode { get; set; }
		public DateTime bookedDate { get; set; }
		public DateTime createdDate { get; set; }
		public DateTime expirationDate { get; set; }
		public string agentCode { get; set; }
		public string organizationCode { get; set; }
		public string destination { get; set; }
		public DateTime arrival { get; set; }
		public DateTime departure { get; set; }
		public int identifier { get; set; }
		public string carrierCode { get; set; }
		public int payments { get; set; }
		public string response { get; set; }
		
	}
	
	
}
