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


namespace OnionConsumeWebAPI.Controllers
{

    public class CommitBookingController : Controller
    {


       // string BaseURL = "https://dotrezapi.test.I5.navitaire.com";


        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;

        ApiResponseModel responseModel;
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
                    
                        TicketBooking ticketBooking01 = new TicketBooking();                 
                        ticketBooking01.guestBooking = "Travel Mode";
                        ticketBooking01.Title = JsonObjGetBooking.data.contacts.P.name.title;
                        ticketBooking01.phoneNumber = JsonObjGetBooking.data.contacts.P.phoneNumbers[0].number;
                        ticketBooking01.bookingDateTime = JsonObjGetBooking.data.info.bookedDate;
                        ticketBooking01.Class = "Economics";
                        ticketBooking01.airLines = "Air Asia";
                        ticketBooking01.passengerName = JsonObjGetBooking.data.contacts.P.name.first;
                        ticketBooking01.emailId = JsonObjGetBooking.data.contacts.P.emailAddress;
                        ticketBooking01.seatNumber = "1A";
                        ticketBooking01.desination = JsonObjGetBooking.data.journeys[0].segments[0].designator.destination;
                        ticketBooking01.arrival = JsonObjGetBooking.data.journeys[0].segments[0].designator.arrival;
                        ticketBooking01.departure = JsonObjGetBooking.data.journeys[0].segments[0].designator.departure;
                        ticketBooking01.identifier = JsonObjGetBooking.data.journeys[0].segments[0].identifier.identifier;
                        ticketBooking01.carrierCode = JsonObjGetBooking.data.journeys[0].segments[0].identifier.carrierCode;
                        ticketBooking01.origin = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].designator.origin;
                        ticketBooking01.taxex = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[1].amount;
                        ticketBooking01.price = JsonObjGetBooking.data.journeys[0].segments[0].fares[0].passengerFares[0].serviceCharges[0].amount;
                        ticketBooking01.desinationTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.departureTerminal;
                        ticketBooking01.sourceTerminal = JsonObjGetBooking.data.journeys[0].segments[0].legs[0].legInfo.arrivalTerminal;
                        ticketBooking01.airlinePNR = JsonObjGetBooking.data.recordLocator;
                        ticketBooking01.bookingStatus = "Confirmed";
                        ticketBooking01.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
                        ticketBooking01.response = _responceGetBooking;

                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/TicketBooking/GetTicketBooking", ticketBooking01);
                        if (responsePassengers.IsSuccessStatusCode)
                        {
                            var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                        }

                        GSTDetails details = new GSTDetails();
                        if (details.Id != null)
                        {
                            details.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
                            details.airLinePNR = JsonObjGetBooking.data.recordLocator;
                            details.GSTNumber = JsonObjGetBooking.data.contacts.G.customerNumber;
                            details.GSTName = JsonObjGetBooking.data.contacts.G.companyName;
                            details.GSTEmail = JsonObjGetBooking.data.contacts.G.emailAddress;
                            details.status = 1;
                            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage responsePassengers1 = await client.PostAsJsonAsync(AppUrlConstant.BaseURL + "api/GSTDetails/GetGstDetails", details);
                            if (responsePassengers1.IsSuccessStatusCode)
                            {
                                var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                            }
                        }
                    

                }

                #endregion
            }
            return RedirectToAction("GetTicketBooking", "AirLinesTicket");
        }
       

    }
}
