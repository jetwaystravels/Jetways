using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Packaging.Signing;
using OnionConsumeWebAPI.Extensions;
using static DomainLayer.Model.PassengersModel;
using static DomainLayer.Model.SeatMapResponceModel;

namespace OnionConsumeWebAPI.Controllers
{
	public class RoundAATripsellController : Controller
	{
		string token = string.Empty;
		string ssrKey = string.Empty;
		string journeyKey = string.Empty;
		string uniquekey = string.Empty;
		public IActionResult RoundAATripsellView()
		{

			List<SelectListItem> Title = new()
			{
				new SelectListItem { Text = "Mr", Value = "Mr" },
				new SelectListItem { Text = "Ms" ,Value = "Ms" },
				new SelectListItem { Text = "Mrs", Value = "Mrs"},

			};
			ViewBag.Title = Title;
			string passenger = HttpContext.Session.GetString("keypassenger");
			string passengerR = HttpContext.Session.GetString("keypassengerReturn");

			string passengerInfant = HttpContext.Session.GetString("keypassengerItanary");
			string passengerInfantReturn = HttpContext.Session.GetString("keypassengerItanaryReturn");

			string Seatmap = HttpContext.Session.GetString("Seatmap");
			string SeatmapR = HttpContext.Session.GetString("SeatmapReturn");


			string Meals = HttpContext.Session.GetString("Meals");
			string MealsR = HttpContext.Session.GetString("MealsReturn");
			ViewModel vm = new ViewModel();
			if (passengerInfant != null)
			{
				AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
				AirAsiaTripResponceModel passeengerlistItanary = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerInfant, typeof(AirAsiaTripResponceModel));
				SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
				SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
				vm.passeengerlist = passeengerlist;
				vm.passeengerlistItanary = passeengerlistItanary;
				vm.Seatmaplist = Seatmaplist;
				vm.Meals = Mealslist;
			}
			else
			{
				AirAsiaTripResponceModel passeengerlist = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
				AirAsiaTripResponceModel passeengerlistR = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerR, typeof(AirAsiaTripResponceModel));

				SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
				SeatMapResponceModel SeatmaplistR = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));

				SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
				SSRAvailabiltyResponceModel MealslistR = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));

				vm.passeengerlist = passeengerlist;
				vm.passeengerlistR = passeengerlistR;

				vm.Seatmaplist = Seatmaplist;
				vm.SeatmaplistR = SeatmaplistR;

				vm.Meals = Mealslist;
				vm.MealsR = MealslistR;

			}
			return View(vm);


		}

		public async Task<IActionResult> PostReturnContactData(ContactModel contactobject)
		{
			string tokenview = HttpContext.Session.GetString("AirasiaTokan");
			token = tokenview.Replace(@"""", string.Empty);

			using (HttpClient client = new HttpClient())
			{
				ContactModel _ContactModel = new ContactModel();
				//  _ContactModel.emailAddress = passengerdetails.Email;
				_ContactModel.emailAddress = contactobject.emailAddress;
				_Phonenumber Phonenumber = new _Phonenumber();
				List<_Phonenumber> Phonenumberlist = new List<_Phonenumber>();
				Phonenumber.type = "Home";
				Phonenumber.number = contactobject.number;
				//Phonenumber.number = passengerdetails.mobile;
				Phonenumberlist.Add(Phonenumber);
				_Phonenumber Phonenumber1 = new _Phonenumber();
				Phonenumber1.type = "Other";
				Phonenumber1.number = contactobject.number;
				Phonenumberlist.Add(Phonenumber1);
				foreach (var item in Phonenumberlist)
				{
					_ContactModel.phoneNumbers = Phonenumberlist;
				}
				_ContactModel.contactTypeCode = "P";

				_Address Address = new _Address();
				Address.lineOne = "123 Main Street";
				Address.countryCode = "IN";
				Address.provinceState = "TN";
				Address.city = "Chennai";
				Address.postalCode = "600028";
				_ContactModel.address = Address;

				_Name Name = new _Name();
				Name.first = "Vadivel";
				Name.middle = "raja";
				Name.last = "VR";
				Name.title = "MR";
				_ContactModel.name = Name;

				var jsonContactRequest = JsonConvert.SerializeObject(_ContactModel, Formatting.Indented);
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responseAddContact = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking/contacts", _ContactModel);
				if (responseAddContact.IsSuccessStatusCode)
				{
					var _responseAddContact = responseAddContact.Content.ReadAsStringAsync().Result;
					var JsonObjAddContact = JsonConvert.DeserializeObject<dynamic>(_responseAddContact);
				}

			}
			return RedirectToAction("RoundAATripsellView", "RoundAATripsell");
		}
		public async Task<IActionResult> PostReturnTravllerData(List<passkeytype> passengerdetails, List<Infanttype> infanttype)
		{
			string tokenview = HttpContext.Session.GetString("AirasiaTokan");
			token = tokenview.Replace(@"""", string.Empty);

			using (HttpClient client = new HttpClient())
			{
				PassengersModel _PassengersModel = new PassengersModel();
				for (int i = 0; i < passengerdetails.Count; i++)
				{
					if (passengerdetails[i].passengertypecode != null)
					{
						Name name = new Name();
						_Info Info = new _Info();
						if (passengerdetails[i].title == "Mr")
						{
							Info.gender = "Male";
						}
						else
						{
							Info.gender = "Female";
						}

						name.title = passengerdetails[i].title;
						name.first = passengerdetails[i].first;
						name.last = passengerdetails[i].last;
						name.middle = "";
						Info.dateOfBirth = "";
						Info.nationality = "IN";
						Info.residentCountry = "IN";
						_PassengersModel.name = name;
						_PassengersModel.info = Info;
						var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel, Formatting.Indented);
						client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
						HttpResponseMessage responsePassengers = await client.PutAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + passengerdetails[i].passengerkey, _PassengersModel);
						if (responsePassengers.IsSuccessStatusCode)
						{
							var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
							var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
						}
					}
				}
				AddInFantModel _PassengersModel1 = new AddInFantModel();
				for (int i = 0; i < infanttype.Count; i++)
				{
					if (infanttype[i].code != null)
					{
						_PassengersModel1.nationality = "IN";
						_PassengersModel1.dateOfBirth = "2023-10-01";
						_PassengersModel1.residentCountry = "IN";
						_PassengersModel1.gender = "Male";

						InfantName nameINF = new InfantName();
						nameINF.first = infanttype[i].First;
						nameINF.middle = "";
						nameINF.last = infanttype[i].Last;
						nameINF.title = "Mr";
						nameINF.suffix = "";
						_PassengersModel1.name = nameINF;


						var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
						client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
						HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v3/booking/passengers/" + infanttype[i].passengerkey + "/infant", _PassengersModel1);
						if (responsePassengers.IsSuccessStatusCode)
						{
							var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
							var JsonObjPassengers = JsonConvert.DeserializeObject<dynamic>(_responsePassengers);
						}

						// STRAT Get INFO
						// var jsonPassengers = JsonConvert.SerializeObject(_PassengersModel1, Formatting.Indented);
						client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
						HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.URLAirasia + "/api/nsk/v1/booking");
						if (responceGetBooking.IsSuccessStatusCode)
						{
							var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
							var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
						}
						//END
					}
				}

			}
			//return RedirectToAction("GetSSRAvailabilty", "AATripsell", passengerdetails);
			return RedirectToAction("RoundAATripsellView", "RoundAATripsell");

		}


		public async Task<IActionResult> PostReturnSeatMealsData(List<string> unitKey, List<string> ssrKey)
		{
			string tokenview = HttpContext.Session.GetString("AirasiaTokan");
			token = tokenview.Replace(@"""", string.Empty);
			if (token == "" || token == null)
			{
				return RedirectToAction("Index");
			}
			string UnitkeyOneway = unitKey[0];
			string UnitkeyReturnway = unitKey[1];
			string passenger = HttpContext.Session.GetString("keypassenger");
			string Seatmap = HttpContext.Session.GetString("Seatmap");
			string Meals = HttpContext.Session.GetString("Meals");
			AirAsiaTripResponceModel passeengerKeyList = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passenger, typeof(AirAsiaTripResponceModel));
			SeatMapResponceModel Seatmaplist = (SeatMapResponceModel)JsonConvert.DeserializeObject(Seatmap, typeof(SeatMapResponceModel));
			SSRAvailabiltyResponceModel Mealslist = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(Meals, typeof(SSRAvailabiltyResponceModel));
			int passengerscount = passeengerKeyList.passengerscount;
			var data = Seatmaplist.datalist.Count;
			string legkey = passeengerKeyList.journeys[0].segments[0].legs[0].legKey;
			int Seatcount = unitKey.Count;
			if (Seatcount <= 0)
			{
				for (int i = 0; i < data; i++)
				{
					for (int j = 0; j < passengerscount; j++)
					{

						string unitKey1 = string.Empty;
						//mealskey = ssrKey[i];
						string passengerkey = passeengerKeyList.passengers[j].passengerKey;
						using (HttpClient client = new HttpClient())
						{
							string journeyKey = passeengerKeyList.journeys[0].journeyKey;
							SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
							_SeatAssignmentModel.journeyKey = journeyKey;
							var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							//HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
							HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaAutoSeat + passengerkey, _SeatAssignmentModel);

							if (responceSeatAssignment.IsSuccessStatusCode)
							{
								var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
								var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
							}

						}
					}
				}
			}
			else
			{
				int seatid = 0;
				for (int i = 0; i < data; i++)
				{
					for (int j = 0; j < passengerscount; j++)
					{

						string unitKey1 = string.Empty;
						//mealskey = ssrKey[i];
						string passengerkey = passeengerKeyList.passengers[j].passengerKey;
						using (HttpClient client = new HttpClient())
						{

							string journeyKey = passeengerKeyList.journeys[0].journeyKey;

							unitKey1 = unitKey[seatid];
							string[] unitKey2 = unitKey1.Split('_');
							string pas_unitKey = unitKey2[1];
							SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
							_SeatAssignmentModel.journeyKey = journeyKey;

							var jsonSeatAssignmentRequest = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaSeatSelect + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
							//HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/seats/auto/" + passengerkey, _SeatAssignmentModel);
							if (responceSeatAssignment.IsSuccessStatusCode)
							{
								var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
								var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
							}

						}
						seatid++;
					}
				}
				int mealssr = Mealslist.legSsrs.Count;
				for (int j = 0; j < mealssr; j++)
				{
					for (int i = 0; i < passengerscount; i++)
					{
						string mealskey = string.Empty;
						mealskey = ssrKey[i];
						//string ssrkey = Meals[0].SSRAvailabiltyResponceModel[0].legSsrs[0].legssrs[0].legpassengers.ssrKey;
						using (HttpClient client = new HttpClient())
						{

							SellSSRModel _sellSSRModel = new SellSSRModel();
							_sellSSRModel.count = 1;
							_sellSSRModel.note = "DevTest";
							_sellSSRModel.forceWaveOnSell = false;
							_sellSSRModel.currencyCode = "INR";


							var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
							if (responseSellSSR.IsSuccessStatusCode)
							{
								var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
								var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
							}
						}
					}

				}
			}

			//ReturnFlight
			string passengerR = HttpContext.Session.GetString("keypassengerReturn");
			string SeatmapR = HttpContext.Session.GetString("SeatmapReturn");
			string MealsR = HttpContext.Session.GetString("MealsReturn");
			AirAsiaTripResponceModel passeengerlistR = (AirAsiaTripResponceModel)JsonConvert.DeserializeObject(passengerR, typeof(AirAsiaTripResponceModel));
			SeatMapResponceModel SeatmaplistR = (SeatMapResponceModel)JsonConvert.DeserializeObject(SeatmapR, typeof(SeatMapResponceModel));
			SSRAvailabiltyResponceModel MealslistR = (SSRAvailabiltyResponceModel)JsonConvert.DeserializeObject(MealsR, typeof(SSRAvailabiltyResponceModel));
			int passengerscountR = passeengerlistR.passengerscount;
			var dataR = SeatmaplistR.datalist.Count;
			string legkeyR = passeengerlistR.journeys[0].segments[0].legs[0].legKey;
			int SeatcountR = unitKey.Count;
			if (SeatcountR <= 0)
			{
				for (int i = 0; i < dataR; i++)
				{
					for (int j = 0; j < passengerscountR; j++)
					{

						string unitKey1 = string.Empty;
						//mealskey = ssrKey[i];
						string passengerkeyR = passeengerlistR.passengers[j].passengerKey;
						using (HttpClient client = new HttpClient())
						{
							string journeyKeyR = passeengerlistR.journeys[1].journeyKey;
							SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
							_SeatAssignmentModel.journeyKey = journeyKeyR;
							var jsonSeatAssignmentRequestR = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							//HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v2/booking/passengers/" + passengerkey + "/seats/" + pas_unitKey, _SeatAssignmentModel);
							HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(AppUrlConstant.AirasiaAutoSeat + passengerkeyR, _SeatAssignmentModel);

							if (responceSeatAssignment.IsSuccessStatusCode)
							{
								var _responseSeatAssignment = responceSeatAssignment.Content.ReadAsStringAsync().Result;
								var JsonObjSeatAssignment = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignment);
							}

						}
					}
				}
			}
			else
			{
				int seatidR = 0;
				int unikeySeat = 1;
				for (int i = 0; i < dataR; i++)
				{
					for (int j = 0; j < passengerscountR; j++)
					{
						UnitkeyReturnway = string.Empty;
						//mealskey = ssrKey[i];
						string passengerkeyR = passeengerlistR.passengers[j].passengerKey;
						using (HttpClient client = new HttpClient())
						{
							string journeyKeyR = passeengerlistR.journeys[0].journeyKey;
							UnitkeyReturnway = unitKey[unikeySeat];
							string[] unitKey2R = UnitkeyReturnway.Split('_');
							string pas_unitKeyR = unitKey2R[1];
							SeatAssignmentModel _SeatAssignmentModel = new SeatAssignmentModel();
							_SeatAssignmentModel.journeyKey = journeyKeyR;

							var jsonSeatAssignmentRequestR = JsonConvert.SerializeObject(_SeatAssignmentModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							HttpResponseMessage responceSeatAssignmentR = await client.PostAsJsonAsync(AppUrlConstant.AirasiaSeatSelect + passengerkeyR + "/seats/" + pas_unitKeyR, _SeatAssignmentModel);
							//HttpResponseMessage responceSeatAssignment = await client.PostAsJsonAsync(BaseURL + "/api/nsk/v1/booking/seats/auto/" + passengerkey, _SeatAssignmentModel);
							if (responceSeatAssignmentR.IsSuccessStatusCode)
							{
								var _responseSeatAssignmentR = responceSeatAssignmentR.Content.ReadAsStringAsync().Result;
								var JsonObjSeatAssignmentR = JsonConvert.DeserializeObject<dynamic>(_responseSeatAssignmentR);
							}

						}
						seatidR++;
					}
				}

				int mealssr = Mealslist.legSsrs.Count;
				for (int j = 0; j < mealssr; j++)
				{

					for (int i = 0; i < passengerscount; i++)
					{
						string mealskey = string.Empty;
						mealskey = ssrKey[i];
						//string ssrkey = Meals[0].SSRAvailabiltyResponceModel[0].legSsrs[0].legssrs[0].legpassengers.ssrKey;
						using (HttpClient client = new HttpClient())
						{

							SellSSRModel _sellSSRModel = new SellSSRModel();
							_sellSSRModel.count = 1;
							_sellSSRModel.note = "DevTest";
							_sellSSRModel.forceWaveOnSell = false;
							_sellSSRModel.currencyCode = "INR";


							var jsonSellSSR = JsonConvert.SerializeObject(_sellSSRModel, Formatting.Indented);
							client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
							client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
							HttpResponseMessage responseSellSSR = await client.PostAsJsonAsync(AppUrlConstant.URLAirasia + "/api/nsk/v2/booking/ssrs/" + mealskey, _sellSSRModel);
							if (responseSellSSR.IsSuccessStatusCode)
							{
								var _responseresponseSellSSR = responseSellSSR.Content.ReadAsStringAsync().Result;
								var JsonObjresponseresponseSellSSR = JsonConvert.DeserializeObject<dynamic>(_responseresponseSellSSR);
							}
						}
					}

				}
			}
			return RedirectToAction("Payment", "PaymentGateway");
		}

	}
}
