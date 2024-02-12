using Bookingmanager_;
using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using OnionConsumeWebAPI.Extensions;
using Sessionmanager;
using System.Net.Http.Headers;
using Utility;
using static DomainLayer.Model.ReturnAirLineTicketBooking;
using static DomainLayer.Model.ReturnTicketBooking;

namespace OnionConsumeWebAPI.Controllers.RoundTrip
{
    public class RoundTripCommitBooking : Controller
    {
        string token = string.Empty;
        string ssrKey = string.Empty;
        string journeyKey = string.Empty;
        string uniquekey = string.Empty;
        string AirLinePNR = string.Empty;

        public async Task<IActionResult> RoundTripBookingView()
        {


            AirLinePNRTicket _AirLinePNRTicket = new AirLinePNRTicket();
            _AirLinePNRTicket.AirlinePNR = new List<ReturnTicketBooking>();
            Logs logs = new Logs();




            using (HttpClient client = new HttpClient())
            {
                string tokenview = string.Empty;
                //AirAsia
                string token = string.Empty;
                tokenview = HttpContext.Session.GetString("AirasiaTokan");

                if (!string.IsNullOrEmpty(tokenview))
                {
                    token = tokenview.Replace(@"""", string.Empty);
                    #region Commit Booking
                    string[] NotifyContacts = new string[1];
                    NotifyContacts[0] = "P";
                    Commit_BookingModel _Commit_BookingModel = new Commit_BookingModel();

                    _Commit_BookingModel.notifyContacts = true;
                    _Commit_BookingModel.contactTypesToNotify = NotifyContacts;
                    var jsonCommitBookingRequest = JsonConvert.SerializeObject(_Commit_BookingModel, Formatting.Indented);

                    //ApiRequests apiRequests = new ApiRequests();
                    //   responseModel = await apiRequests.OnPostAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceCommit_Booking = await client.PostAsJsonAsync(AppUrlConstant.AirasiaCommitBooking, _Commit_BookingModel);


                    if (responceCommit_Booking.IsSuccessStatusCode)
                    {
                        var _responceCommit_Booking = responceCommit_Booking.Content.ReadAsStringAsync().Result;
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_Commit_BookingModel) + "Url: " + AppUrlConstant.AirasiaCommitBooking + "\n Response: " + _responceCommit_Booking, "Commit", "AirAsiaRT");

                        var JsonObjCommit_Booking = JsonConvert.DeserializeObject<dynamic>(_responceCommit_Booking);
                    }
                    #endregion

                    #region Booking GET
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage responceGetBooking = await client.GetAsync(AppUrlConstant.AirasiaGetBoking);

                    if (responceGetBooking.IsSuccessStatusCode)
                    {
                        // string BaseURL1 = "http://localhost:5225/";
                        var _responceGetBooking = responceGetBooking.Content.ReadAsStringAsync().Result;
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject("GetFinalRequest") + "Url: " + AppUrlConstant.AirasiaGetBoking + "\n Response: " + _responceGetBooking, "FinalgetBooking", "AirAsiaRT");
                        var JsonObjGetBooking = JsonConvert.DeserializeObject<dynamic>(_responceGetBooking);
                        AirLinePNR = JsonObjGetBooking.data.recordLocator;

                    }
                }


                #endregion
                #region AirLinePNR
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

                            //IdentifierReturn identifierReturn = new IdentifierReturn();
                            //identifierReturn.identifier = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.identifier;
                            //identifierReturn.carrierCode = JsonObjPNRBooking.data.journeys[i].segments[j].identifier.carrierCode;
                            //segmentReturnobj.identifier = identifierReturn;

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
                    returnTicketBooking.journeys = journeysreturnList;
                    returnTicketBooking.passengers = ReturnpassengersList;
                    returnTicketBooking.passengerscount = Returnpassengercount;

                    // HttpContext.Session.SetString("PNRTicketBooking", JsonConvert.SerializeObject(returnTicketBooking));
                    _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                }
                //}


                #endregion
                #region Spicejet Commit
                //Spicejet
                token = string.Empty;
                tokenview = HttpContext.Session.GetString("SpicejetSignautre");
                if (!string.IsNullOrEmpty(tokenview))
                {

                    token = tokenview.Replace(@"""", string.Empty);
                    string passengernamedetails = HttpContext.Session.GetString("PassengerNameDetails");
                    List<passkeytype> passeengerlist = (List<passkeytype>)JsonConvert.DeserializeObject(passengernamedetails, typeof(List<passkeytype>));
                    string contactdata = HttpContext.Session.GetString("ContactDetails");
                    UpdateContactsRequest contactList = (UpdateContactsRequest)JsonConvert.DeserializeObject(contactdata, typeof(UpdateContactsRequest));
                    using (HttpClient client1 = new HttpClient())
                    {
                        #region Commit Booking
                        BookingCommitRequest _bookingCommitRequest = new BookingCommitRequest();
                        BookingCommitResponse _BookingCommitResponse = new BookingCommitResponse();
                        _bookingCommitRequest.Signature = token;
                        _bookingCommitRequest.ContractVersion = 420;
                        _bookingCommitRequest.BookingCommitRequestData = new BookingCommitRequestData();
                        _bookingCommitRequest.BookingCommitRequestData.SourcePOS = GetPointOfSale();
                        _bookingCommitRequest.BookingCommitRequestData.CurrencyCode = "INR";
                        _bookingCommitRequest.BookingCommitRequestData.PaxCount = Convert.ToInt16(passeengerlist.Count);
                        _bookingCommitRequest.BookingCommitRequestData.BookingContacts = new BookingContact[1];
                        _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0] = new BookingContact();
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
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit", "SpiceJetRT");

                        GetBookingRequest getBookingRequest = new GetBookingRequest();
                        GetBookingResponse _getBookingResponse = new GetBookingResponse();
                        getBookingRequest.Signature = token;
                        getBookingRequest.ContractVersion = 420;
                        getBookingRequest.GetBookingReqData = new GetBookingRequestData();
                        getBookingRequest.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
                        getBookingRequest.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
                        getBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator;

                        _getBookingResponse = await objSpiceJet.GetBookingdetails(getBookingRequest);
                        string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);

                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails", "SpiceJetRT");

                        if (_getBookingResponse != null)
                        {
                            ReturnTicketBooking returnTicketBooking = new ReturnTicketBooking();
                            //var resultsTripsell = responseTripsell.Content.ReadAsStringAsync().Result;
                            //var JsonObjTripsell = JsonConvert.DeserializeObject<dynamic>(resultsTripsell);
                            var totalAmount = _getBookingResponse.Booking.BookingSum.TotalCost;
                            returnTicketBooking.bookingKey = _getBookingResponse.Booking.BookingID.ToString();
                            ReturnPaxSeats _unitdesinator = new ReturnPaxSeats();
                            _unitdesinator.unitDesignatorPax = _getBookingResponse.Booking.Journeys[0].Segments[0].PaxSeats[0].UnitDesignator;
                            Contacts _contact = new Contacts();
                            _contact.phoneNumbers = _getBookingResponse.Booking.BookingContacts[1].HomePhone.ToString();
                            _contact.ReturnPaxSeats = _unitdesinator.unitDesignatorPax.ToString();
                            returnTicketBooking.airLines = "Spicejet";
                            returnTicketBooking.recordLocator = _getBookingResponse.Booking.RecordLocator;

                            var totalTax = "";// _getPriceItineraryRS.data.breakdown.journeys[journeyKey].totalTax;

                            #region Itenary segment and legs
                            int journeyscount = _getBookingResponse.Booking.Journeys.Length;
                            List<JourneysReturn> AAJourneyList = new List<JourneysReturn>();
                            for (int i = 0; i < journeyscount; i++)
                            {

                                JourneysReturn AAJourneyobj = new JourneysReturn();

                                //AAJourneyobj.flightType = JsonObjTripsell.data.journeys[i].flightType;
                                //AAJourneyobj.stops = JsonObjTripsell.data.journeys[i].stops;
                                AAJourneyobj.journeyKey = _getBookingResponse.Booking.Journeys[i].JourneySellKey;

                                int segmentscount = _getBookingResponse.Booking.Journeys[i].Segments.Length;
                                List<SegmentReturn> AASegmentlist = new List<SegmentReturn>();
                                for (int j = 0; j < segmentscount; j++)
                                {
                                    DesignatorReturn AADesignatorobj = new DesignatorReturn();
                                    AADesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[0].DepartureStation;
                                    AADesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].ArrivalStation;
                                    AADesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[0].STD;
                                    AADesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[segmentscount - 1].STA;
                                    AAJourneyobj.designator = AADesignatorobj;


                                    SegmentReturn AASegmentobj = new SegmentReturn();
                                    //AASegmentobj.isStandby = JsonObjTripsell.data.journeys[i].segments[j].isStandby;
                                    //AASegmentobj.isHosted = JsonObjTripsell.data.journeys[i].segments[j].isHosted;

                                    DesignatorReturn AASegmentDesignatorobj = new DesignatorReturn();

                                    AASegmentDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].DepartureStation;
                                    AASegmentDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].ArrivalStation;
                                    AASegmentDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].STD;
                                    AASegmentDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].STA;
                                    AASegmentobj.designator = AASegmentDesignatorobj;

                                    int fareCount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares.Length;
                                    List<FareReturn> AAFarelist = new List<FareReturn>();
                                    for (int k = 0; k < fareCount; k++)
                                    {
                                        FareReturn AAFareobj = new FareReturn();
                                        AAFareobj.fareKey = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].FareSellKey;
                                        AAFareobj.productClass = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].ProductClass;

                                        var passengerFares = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares;

                                        int passengerFarescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares.Length;
                                        List<PassengerFareReturn> PassengerfarelistRT = new List<PassengerFareReturn>();
                                        for (int l = 0; l < passengerFarescount; l++)
                                        {
                                            PassengerFareReturn AAPassengerfareobject = new PassengerFareReturn();
                                            AAPassengerfareobject.passengerType = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].PaxType;

                                            var serviceCharges1 = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges;
                                            int serviceChargescount = _getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges.Length;
                                            List<ServiceChargeReturn> AAServicechargelist = new List<ServiceChargeReturn>();
                                            for (int m = 0; m < serviceChargescount; m++)
                                            {
                                                ServiceChargeReturn AAServicechargeobj = new ServiceChargeReturn();
                                                AAServicechargeobj.amount = Convert.ToInt32(_getBookingResponse.Booking.Journeys[i].Segments[j].Fares[k].PaxFares[l].ServiceCharges[m].Amount);



                                                AAServicechargelist.Add(AAServicechargeobj);
                                            }



                                            AAPassengerfareobject.serviceCharges = AAServicechargelist;

                                            PassengerfarelistRT.Add(AAPassengerfareobject);

                                        }
                                        AAFareobj.passengerFares = PassengerfarelistRT;

                                        AAFarelist.Add(AAFareobj);




                                    }
                                    AASegmentobj.fares = AAFarelist;
                                    IdentifierReturn AAIdentifierobj = new IdentifierReturn();

                                    AAIdentifierobj.identifier = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.FlightNumber;
                                    AAIdentifierobj.carrierCode = _getBookingResponse.Booking.Journeys[i].Segments[j].FlightDesignator.CarrierCode;

                                    AASegmentobj.identifier = AAIdentifierobj;

                                    var leg = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs;
                                    int legcount = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs.Length;
                                    List<LegReturn> AALeglist = new List<LegReturn>();
                                    for (int n = 0; n < legcount; n++)
                                    {
                                        LegReturn AALeg = new LegReturn();
                                        //AALeg.legKey = JsonObjTripsell.data.journeys[i].segments[j].legs[n].legKey;
                                        DesignatorReturn AAlegDesignatorobj = new DesignatorReturn();
                                        AAlegDesignatorobj.origin = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].DepartureStation;
                                        AAlegDesignatorobj.destination = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].ArrivalStation;
                                        AAlegDesignatorobj.departure = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STD;
                                        AAlegDesignatorobj.arrival = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].STA;
                                        AALeg.designator = AAlegDesignatorobj;

                                        LegInfoReturn AALeginfoobj = new LegInfoReturn();
                                        AALeginfoobj.arrivalTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.ArrivalTerminal;
                                        AALeginfoobj.arrivalTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTA;
                                        AALeginfoobj.departureTerminal = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.DepartureTerminal;
                                        AALeginfoobj.departureTime = _getBookingResponse.Booking.Journeys[i].Segments[j].Legs[n].LegInfo.PaxSTD;
                                        AALeg.legInfo = AALeginfoobj;
                                        AALeglist.Add(AALeg);

                                    }

                                    AASegmentobj.legs = AALeglist;
                                    AASegmentlist.Add(AASegmentobj);
                                }
                                AAJourneyobj.segments = AASegmentlist;
                                AAJourneyList.Add(AAJourneyobj);

                            }

                            #endregion
                            string stravailibitilityrequest = HttpContext.Session.GetString("SpicejetAvailibilityRequest");
                            GetAvailabilityRequest availibiltyRQ = JsonConvert.DeserializeObject<GetAvailabilityRequest>(stravailibitilityrequest);

                            var passanger = _getBookingResponse.Booking.Passengers;
                            int passengercount = availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxCount;
                            ReturnPassengers passkeytypeobj = new ReturnPassengers();
                            passkeytypeobj.name = new Name();
                            passkeytypeobj.name.first = _getBookingResponse.Booking.Passengers[0].Names[0].FirstName;




                            List<ReturnPassengers> passkeylist = new List<ReturnPassengers>();
                            int a = 0;
                            foreach (var items in availibiltyRQ.TripAvailabilityRequest.AvailabilityRequests[0].PaxPriceTypes)
                            {
                                for (int i = 0; i < items.PaxCount; i++)
                                {


                                    passkeytypeobj.passengerKey = a.ToString();
                                    //if (items.PaxType == "ADT")
                                    //{
                                    passkeytypeobj.passengerTypeCode = items.PaxType;
                                    //}

                                    passkeylist.Add(passkeytypeobj);
                                    a++;
                                }
                                //passengerkey12 = passkeytypeobj.passengerKey;


                            }

                            returnTicketBooking.journeys = AAJourneyList;
                            returnTicketBooking.passengers = passkeylist;
                            returnTicketBooking.passengerscount = passengercount;
                            returnTicketBooking.contacts = _contact;

                            //HttpContext.Session.SetString("SGkeypassengerRT", JsonConvert.SerializeObject(returnTicketBooking));
                            _AirLinePNRTicket.AirlinePNR.Add(returnTicketBooking);
                        }
                        #endregion
                    }
                }
                #endregion
                return View(_AirLinePNRTicket);
                //return RedirectToAction("AirLineTicketBooking", "AirLinesTicket");

            }
        }
        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = MessageState.New;
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
