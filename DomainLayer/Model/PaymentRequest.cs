using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Model
{
	
	public class PaymentRequest
	{
		public string PaymentMethodCode { get; set; }
		public decimal Amount { get; set; }
		public PaymentFields PaymentFields { get; set; }
		public string CurrencyCode { get; set; }
		public int Installments { get; set; }
	}

	public class PaymentFields
	{
		public string ACCTNO { get; set; }
		public decimal AMT { get; set; }
	}
}
