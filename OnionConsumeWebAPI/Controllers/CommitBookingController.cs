using DomainLayer.Model;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace OnionConsumeWebAPI.Controllers
{

    public class CommitBookingController : Controller
    {
	
		
		string BaseURL = "https://dotrezapi.test.I5.navitaire.com";
		string token = string.Empty;
		string ssrKey = string.Empty;
		string journeyKey = string.Empty;
		string uniquekey = string.Empty;

		public async Task<IActionResult> booking()
		{
			string tokenview = HttpContext.Session.GetString("AirasiaTokan");
			token = tokenview.Replace(@"""", string.Empty);

			using (HttpClient client = new HttpClient())
			{
   			#region Commit Booking
				string[] NotifyContacts = new string[1];
				NotifyContacts[0] = "P";
				Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();

				_Commit_BookingModel.notifyContacts = true;
				_Commit_BookingModel.contactTypesToNotify = NotifyContacts;
				var jsonCommitBookingRequest = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v3/booking", _Commit_BookingModel);


				if (responceCommit_Booking.IsSuccessStatusCode)
				{
					var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
					var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
				}
				#endregion

				#region Booking GET
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responceGetBooking = await client.GetAsync(BaseURL + "/api/nsk/v1/booking");
				
				if (responceGetBooking.IsSuccessStatusCode)
				{
					var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
					var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);

					var bookingKey = JsonObjGetBooking.data.bookingKey;
                    var recordLocator = JsonObjGetBooking.data.recordLocator;              

                }	

				#endregion
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> PostBooking()
		{
			TicketBooking ticketBooking1 = new TicketBooking();
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("http://localhost:5225/");
			//HttpResponseMessage response = await client.PostAsync("api/TicketBooking/GetTicketBooking");
			//if (response.IsSuccessStatusCode)
			//{
			//	var results = response.Content.ReadAsStringAsync().Result;
			//	//users = JsonConvert.DeserializeObject<List<User>>(results);

			//}
			return View();

		}

	}
}
