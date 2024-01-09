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
using Bookingmanager_;
using Utility;
using Sessionmanager;

namespace OnionConsumeWebAPI.Controllers.AirAsia
{

    public class SGCommitBookingController : Controller
    {

        Logs logs = new Logs();
        string BaseURL = "https://dotrezapi.test.I5.navitaire.com";


        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;

        public async Task<IActionResult> booking()
        {

            
            string tokenview = HttpContext.Session.GetString("SpicejetSignature");
            token = tokenview.Replace(@"""", string.Empty);

            string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");


            List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));

            
                string contactdata = HttpContext.Session.GetString("ContactDetails");


            UpdateContactsRequest contactList = (UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(UpdateContactsRequest));



            using (HttpClient client = new HttpClient())
            {
                #region Commit Booking

                BookingCommitRequest _bookingCommitRequest = new BookingCommitRequest();
                BookingCommitResponse _BookingCommitResponse = new BookingCommitResponse();
                _bookingCommitRequest.Signature = token;
                _bookingCommitRequest.ContractVersion = 420;
                _bookingCommitRequest.BookingCommitRequestData=new BookingCommitRequestData();
                _bookingCommitRequest.BookingCommitRequestData.SourcePOS = GetPointOfSale();
                _bookingCommitRequest.BookingCommitRequestData.CurrencyCode = "INR";
                _bookingCommitRequest.BookingCommitRequestData.PaxCount = Convert.ToInt16(passeengerlist.Count);
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts = new BookingContact[1];
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0]=new BookingContact();
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].TypeCode = "P";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names = new BookingName[1];
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0] = new BookingName();
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].State = MessageState.New;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].FirstName = passeengerlist[0].first;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].MiddleName = passeengerlist[0].middle;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].LastName = passeengerlist[0].last;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].Names[0].Title = passeengerlist[0].title;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].EmailAddress = contactList.updateContactsRequestData.BookingContactList[0].EmailAddress; //"vinay.ks@gmail.com"; //passeengerlist.Email;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].HomePhone = "9457000000"; //contactList.updateContactsRequestData.BookingContactList[0].HomePhone; //"9457000000"; //passeengerlist.mobile;
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine1 = "A";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine2 = "B";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].City = "Delhi";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CountryCode = "IN";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CultureCode = "en-GB";
                _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].DistributionOption = DistributionOption.Email;


                SpiceJetApiController objSpiceJet = new SpiceJetApiController();
                _BookingCommitResponse = await objSpiceJet.BookingCommit(_bookingCommitRequest);

                string Str3 = JsonConvert.SerializeObject(_BookingCommitResponse);
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit");


                GetBookingRequest getBookingRequest = new GetBookingRequest();
                GetBookingResponse _getBookingResponse=new GetBookingResponse();
                getBookingRequest.Signature = token;
                getBookingRequest.ContractVersion = 420;
                getBookingRequest.GetBookingReqData = new GetBookingRequestData();
                getBookingRequest.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                getBookingRequest.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                getBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator;

                _getBookingResponse = await objSpiceJet.GetBookingdetails(getBookingRequest);
                string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);

                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails");

                if (_getBookingResponse!= null)
                {
                    string BaseURL1 = "http://localhost:5225/";
                  
                    TicketBooking ticketBooking01 = new TicketBooking();
                    ticketBooking01.guestBooking = "Travel Mode";
                    ticketBooking01.Title = _getBookingResponse.Booking.Passengers[0].Names[0].Title;
                    ticketBooking01.phoneNumber = _getBookingResponse.Booking.BookingContacts[0].HomePhone;
                    ticketBooking01.bookingDateTime = _getBookingResponse.Booking.BookingInfo.BookingDate;
                    ticketBooking01.Class = "Economics";
                    ticketBooking01.airLines = "SpiceJet";
                    ticketBooking01.passengerName = _getBookingResponse.Booking.Passengers[0].Names[0].FirstName;
                    ticketBooking01.emailId = _getBookingResponse.Booking.BookingContacts[0].EmailAddress;
                    if(_getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats.Length >0)
                     ticketBooking01.seatNumber = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                    ticketBooking01.desination = _getBookingResponse.Booking.Journeys[0].Segments[0].ArrivalStation;
                    ticketBooking01.arrival = _getBookingResponse.Booking.Journeys[0].Segments[0].STA;
                    ticketBooking01.departure = _getBookingResponse.Booking.Journeys[0].Segments[0].STD;
                    ticketBooking01.identifier = Convert.ToInt32(_getBookingResponse.Booking.Journeys[0].Segments[0].FlightDesignator.FlightNumber);
                    ticketBooking01.carrierCode = _getBookingResponse.Booking.Journeys[0].Segments[0].FlightDesignator.CarrierCode;
                    ticketBooking01.origin = _getBookingResponse.Booking.Journeys[0].Segments[0].DepartureStation;
                    ticketBooking01.taxex = _getBookingResponse.Booking.Journeys[0].Segments[0].Fares[0].PaxFares[0].ServiceCharges[1].Amount.ToString();
                    ticketBooking01.price = Convert.ToInt32(_getBookingResponse.Booking.Journeys[0].Segments[0].Fares[0].PaxFares[0].ServiceCharges[0].Amount);
                    ticketBooking01.desinationTerminal = _getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].LegInfo.DepartureTerminal;
                    ticketBooking01.sourceTerminal = _getBookingResponse.Booking.Journeys[0].Segments[0].Legs[0].LegInfo.ArrivalTerminal;
                    ticketBooking01.airlinePNR = _getBookingResponse.Booking.RecordLocator;
                    ticketBooking01.bookingStatus = "Confirmed";
                    ticketBooking01.bookingReferenceNumber = _getBookingResponse.Booking.BookingID.ToString();
                    ticketBooking01.response = _responceGetBooking;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage responsePassengers = await client.PostAsJsonAsync(BaseURL1 + "api/TicketBooking/GetTicketBooking", ticketBooking01);
                    if (responsePassengers.IsSuccessStatusCode)
                    {
                        var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                    }
                    //GSTDetails details = new GSTDetails();
                    //details.bookingReferenceNumber = JsonObjGetBooking.data.bookingKey;
                    //details.airLinePNR = JsonObjGetBooking.data.recordLocator;
                    //details.GSTNumber = JsonObjGetBooking.data.contacts.G.customerNumber;
                    //details.GSTName = JsonObjGetBooking.data.contacts.G.companyName;
                    //details.GSTEmail = JsonObjGetBooking.data.contacts.G.emailAddress;
                    //details.status = 1;
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //HttpResponseMessage responsePassengers1 = await client.PostAsJsonAsync(BaseURL1 + "api/GSTDetails/GetGstDetails", details);
                    //if (responsePassengers1.IsSuccessStatusCode)
                    //{
                    //    var _responsePassengers = responsePassengers.Content.ReadAsStringAsync().Result;
                    //}


                    //LogOut 
                    #region LogOut
                    LogoutRequest _logoutRequestobj = new LogoutRequest();
                    LogoutResponse _logoutResponse = new LogoutResponse();
                    _logoutRequestobj.ContractVersion = 420;
                    _logoutRequestobj.Signature = token;
                     objSpiceJet = new SpiceJetApiController();
                    _logoutResponse = await objSpiceJet.Logout(_logoutRequestobj);

                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_logoutRequestobj) + "\n Response: " + JsonConvert.SerializeObject(_logoutResponse), "Logout");


                    #endregion

                }


                #endregion



            }
            return RedirectToAction("GetTicketBooking", "AirLinesTicket");
        }

        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = Bookingmanager_.MessageState.New;
                SourcePOS.OrganizationCode = "APITESTID";
                SourcePOS.AgentCode = "AG";
                SourcePOS.LocationCode = "";
                SourcePOS.DomainCode = "WWW";
            }
            catch (Exception e)
            {
                string exp = e.Message;
                exp = null;
            }

            return SourcePOS;
        }
    }
}
