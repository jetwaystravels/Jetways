using DomainLayer.Model;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System;
using OnionConsumeWebAPI.Extensions;
using OnionConsumeWebAPI.ApiService;
using OnionConsumeWebAPI.Models;
using static DomainLayer.Model.ReturnTicketBooking;


namespace OnionConsumeWebAPI.Controllers
{

	public class CommitBookingController : Controller
	{


		// string BaseURL = "https://dotrezapi.test.I5.navitaire.com";


		string token = string.Empty;
		string ssrKey = string.Empty;
		string journeyKey = string.Empty;
		string uniquekey = string.Empty;
		string AirLinePNR = string.Empty;

		ApiResponseModel responseModel;
		public async Task<IActionResult> booking()
		{
			AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
			_AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();


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

				ApiRequests apiRequests = new ApiRequests();
				//   responseModel = await apiRequests.OnPostAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);

				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);


				if (responceCommit_Booking.IsSuccessStatusCode)
				{
					var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
					var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
				}
				#endregion

				#region Booking GET
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.AirasiaGetBoking);

				if (responceGetBooking.IsSuccessStatusCode)
				{
					// string BaseURL1 = "http://localhost:5225/";
					var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
					var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
					AirLinePNR = JsonObjGetBooking.data.recordLocator;

				}
				#endregion
				#region AirLinePNR
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage responcepnrBooking = await client.GetAsync(AppUrlConstant.AirasiaPNRBooking + AirLinePNR);
				if (responcepnrBooking.IsSuccessStatusCode)
				{
					// string BaseURL1 = "http://localhost:5225/";
					var _responcePNRBooking = responcepnrBooking.Content.ReadAsStringAsync().Result;
					var JsonObjPNRBooking = JsonConvert.DeserializeObject<dynamic>(_responcePNRBooking);

					ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
					returnTicketBooking.recordLocator = JsonObjPNRBooking.data.recordLocator;
					returnTicketBooking.bookingKey = JsonObjPNRBooking.data.bookingKey;
				
                    if (JsonObjPNRBooking.data.contacts.G != null)
					{
						returnTicketBooking.customerNumber = JsonObjPNRBooking.data.contacts.G.customerNumber;
						returnTicketBooking.companyName = JsonObjPNRBooking.data.contacts.G.companyName;
						returnTicketBooking.emailAddress = JsonObjPNRBooking.data.contacts.G.emailAddress;
					}
					Contacts _contactobj = new Contacts();
					int PhoneNumberCount = JsonObjPNRBooking.data.contacts.P.phoneNumbers.Count;
					List<PhoneNumber> phoneNumberList = new List<PhoneNumber>();
					for (int p = 0; p < PhoneNumberCount; p++)
					{
						PhoneNumber phoneobject = new PhoneNumber();
						phoneobject.number = JsonObjPNRBooking.data.contacts.P.phoneNumbers[p].number;

						phoneNumberList.Add(phoneobject);

					}



					int JourneysReturnCount = JsonObjPNRBooking.data.journeys.Count;
					List<JourneysReturn> journeysreturnList = new List<JourneysReturn>();
					for (int i = 0; i < JourneysReturnCount; i++)
					{
						JourneysReturn journeysReturnObj = new JourneysReturn();
						journeysReturnObj.stops = JsonObjPNRBooking.data.journeys[i].stops;

						DesignatorReturn ReturnDesignatorobject = new DesignatorReturn();
						ReturnDesignatorobject.origin = JsonObjPNRBooking.data.journeys[0].designator.origin;
						ReturnDesignatorobject.destination = JsonObjPNRBooking.data.journeys[0].designator.destination;
						ReturnDesignatorobject.departure = JsonObjPNRBooking.data.journeys[0].designator.departure;
						ReturnDesignatorobject.arrival = JsonObjPNRBooking.data.journeys[0].designator.arrival;
						journeysReturnObj.designator = ReturnDesignatorobject;


						int SegmentReturnCount = JsonObjPNRBooking.data.journeys[i].segments.Count;
						List<SegmentReturn> segmentReturnsList = new List<SegmentReturn>();
						for (int j = 0; j < SegmentReturnCount; j++)
						{
							SegmentReturn segmentReturnobj = new SegmentReturn();
							segmentReturnobj.isStandby = JsonObjPNRBooking.data.journeys[i].segments[j].isStandby;
							segmentReturnobj.isHosted = JsonObjPNRBooking.data.journeys[i].segments[j].isHosted;


							DesignatorReturn designatorReturn = new DesignatorReturn();
							//var cityname = Citydata.GetAllcity().Where(x => x.cityCode == "DEL");
							designatorReturn.origin = JsonObjPNRBooking.data.journeys[i].segments[j].designator.origin;
							designatorReturn.destination = JsonObjPNRBooking.data.journeys[i].segments[j].designator.destination;
							designatorReturn.departure = JsonObjPNRBooking.data.journeys[i].segments[j].designator.departure;
							designatorReturn.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].designator.arrival;
							segmentReturnobj.designator = designatorReturn;



							var passengersegmentCount = JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment;
							int passengerReturnCount = ((Newtonsoft.Json.Linq.JContainer)passengersegmentCount).Count;
							List<PassengerSegment> passengerSegmentsList = new List<PassengerSegment>();
							foreach (var item in JsonObjPNRBooking.data.journeys[i].segments[j].passengerSegment)
							{
								PassengerSegment passengerSegmentobj = new PassengerSegment();
								passengerSegmentobj.passengerKey = item.Value.passengerKey;

								passengerSegmentsList.Add(passengerSegmentobj);
								int seatCount = item.Value.seats.Count;
								List<ReturnSeats> returnSeatsList = new List<ReturnSeats>();
								for (int q = 0; q < seatCount; q++)
								{
									ReturnSeats returnSeatsObj = new ReturnSeats();

									returnSeatsObj.unitDesignator = item.Value.seats[q].unitDesignator;
									returnSeatsList.Add(returnSeatsObj);
								}
								passengerSegmentobj.seats = returnSeatsList;
								//passengerSegmentsList.Add(passengerSegmentobj);
							}
							segmentReturnobj.passengerSegment = passengerSegmentsList;

							int ReturmFareCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares.Count;
							List<FareReturn> fareList = new List<FareReturn>();
							for (int k = 0; k < ReturmFareCount; k++)
							{
								FareReturn fareReturnobj = new FareReturn();
								fareReturnobj.productClass = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].productClass;

								int PassengerFareReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares.Count;
								List<PassengerFareReturn> passengerFareReturnList = new List<PassengerFareReturn>();
								for (int l = 0; l < PassengerFareReturnCount; l++)
								{
									PassengerFareReturn passengerFareReturnobj = new PassengerFareReturn();

									int ServiceChargeReturnCount = JsonObjPNRBooking.data.journeys[i].segments[j].fares[k].passengerFares[l].serviceCharges.Count;

									List<ServiceChargeReturn> serviceChargeReturnList = new List<ServiceChargeReturn>();
									for (int m = 0; m < ServiceChargeReturnCount; m++)
									{
										ServiceChargeReturn serviceChargeReturnobj = new ServiceChargeReturn();

										serviceChargeReturnobj.amount = JsonObjPNRBooking.data.journeys[i].segments[0].fares[j].passengerFares[k].serviceCharges[l].amount;
										serviceChargeReturnList.Add(serviceChargeReturnobj);


									}
									passengerFareReturnobj.serviceCharges = serviceChargeReturnList;
									passengerFareReturnList.Add(passengerFareReturnobj);

								}
								fareReturnobj.passengerFares = passengerFareReturnList;
								fareList.Add(fareReturnobj);

							}
							segmentReturnobj.fares = fareList;

							IdentifierReturn identifierReturn = new IdentifierReturn();
							identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
							identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
							segmentReturnobj.identifier = identifierReturn;

							var LegReturn = JsonObjPNRBooking.data.journeys[i].segments[j].legs;
							int Legcount = ((Newtonsoft.Json.Linq.JContainer)LegReturn).Count;
							List<LegReturn> legReturnsList = new List<LegReturn>();
							for (int n = 0; n < Legcount; n++)
							{
								LegReturn LegReturnobj = new LegReturn();
								LegReturnobj.legKey = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legKey;

								DesignatorReturn ReturnlegDesignatorobj = new DesignatorReturn();
								ReturnlegDesignatorobj.origin = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.origin;
								ReturnlegDesignatorobj.destination = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.destination;
								ReturnlegDesignatorobj.departure = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.departure;
								ReturnlegDesignatorobj.arrival = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].designator.arrival;
								LegReturnobj.designator = ReturnlegDesignatorobj;

								LegInfoReturn legInfoReturn = new LegInfoReturn();
								legInfoReturn.arrivalTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTerminal;
								legInfoReturn.arrivalTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.arrivalTime;
								legInfoReturn.departureTerminal = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTerminal;
								legInfoReturn.departureTime = JsonObjPNRBooking.data.journeys[i].segments[j].legs[n].legInfo.departureTime;
								LegReturnobj.legInfo = legInfoReturn;
								legReturnsList.Add(LegReturnobj);

							}
							segmentReturnobj.legs = legReturnsList;
							segmentReturnsList.Add(segmentReturnobj);

						}
						journeysReturnObj.segments = segmentReturnsList;
						journeysreturnList.Add(journeysReturnObj);
					}
					var Returnpassanger = JsonObjPNRBooking.data.passengers;
					int Returnpassengercount = ((Newtonsoft.Json.Linq.JContainer)Returnpassanger).Count;
					List<ReturnPassengers> ReturnpassengersList = new List<ReturnPassengers>();
					foreach (var items in JsonObjPNRBooking.data.passengers)
					{
						ReturnPassengers returnPassengersobj = new ReturnPassengers();
						returnPassengersobj.passengerKey = items.Value.passengerKey;
						returnPassengersobj.passengerTypeCode = items.Value.passengerTypeCode;
						//  passkeytypeobj.passengertypecount = items.Count;

						returnPassengersobj.name = new Name();
						returnPassengersobj.name.first = items.Value.name.first;
						ReturnpassengersList.Add(returnPassengersobj);

					}
					//returnTicketBooking.contacts= phoneNumberList;
					returnTicketBooking.journeys = journeysreturnList;
					returnTicketBooking.passengers = ReturnpassengersList;
					returnTicketBooking.passengerscount = Returnpassengercount;
					returnTicketBooking.PhoneNumbers = phoneNumberList;
					_AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
				}

				#endregion

			}
			return View(_AirLinePNRTicket);
			//return RedirectToAction("GetTicketBooking", "AirLinesTicket");
		}


	}
}



//TicketBooking ticketBooking01 = new TicketBooking();
//ticketBooking01.guestBooking = "Travel Mode";
//ticketBooking01.Title = JsonObjGetBooking.data.contacts.P.name.title;
//ticketBooking01.phoneNumber = JsonObjGetBooking.data.contacts.P.phoneNumbers[0].number;
//ticketBooking01.bookingDateTime = JsonObjGetBooking.data.info.bookedDate;
//ticketBooking01.Class = "Economics";
//ticketBooking01.airLines = "Air Asia";
//ticketBooking01.passengerName = JsonObjGetBooking.data.contacts.P.name.first;
//ticketBooking01.emailId = JsonObjGetBooking.data.contacts.P.emailAddress;
//ticketBooking01.seatNumber = "1A";
//ticketBooking01.desination = JsonObjGetBooking.data.journeys[0].segments[0].designator.destination;
//ticketBooking01.arrival = JsonObjGetBooking.data.journeys[0].segments[0].designator.arrival;
//ticketBooking01.departure = JsonObjGetBooking.data.journeys[0].segments[0].designator.departure;
//ticketBooking01.identifier = JsonObjGetBooking.data.journeys[0].segments[0].identifier.identifier;
//ticketBooking01.carrierCode = JsonObjGetBooking.data.journeys[0].segments[0].identifier.carrierCode;
//ticketBooking01.origin = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].designator.origin;
//ticketBooking01.taxex = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[1].amount;
//ticketBooking01.price = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[0].amount;
//ticketBooking01.desinationTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.departureTerminal;
//ticketBooking01.sourceTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.arrivalTerminal;
//ticketBooking01.airlinePNR = JsonObjGetBooking.data.recordLocator;
//ticketBooking01.bookingStatus = "Confirmed";
//ticketBooking01.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
//ticketBooking01.response = _responceGetBooking;

//client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/TicketBooking/GetTicketBooking", ticketBooking01);
//if (responsePassengers.IsSuccessStatusCode)
//{
//    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
//}
//GSTDetails details = new GSTDetails();
//if (details.Id != null)
//{
//    details.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
//    details.airLinePNR = JsonObjGetBooking.data.recordLocator;
//    details.GSTNumber = JsonObjGetBooking.data.contacts.G.customerNumber;
//    details.GSTName = JsonObjGetBooking.data.contacts.G.companyName;
//    details.GSTEmail = JsonObjGetBooking.data.contacts.G.emailAddress;
//    details.status = 1;
//    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
//    HttpResponseMessage responsePassengers1 = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/GSTDetails/GetGstDetails", details);
//    if (responsePassengers1.IsSuccessStatusCode)
//    {
//        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
//    }
//}