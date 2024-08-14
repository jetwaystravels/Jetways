using DomainLayer.Model;
using DomainLayer.ViewModel;
using Indigo;
using IndigoBookingManager_;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utility;

namespace OnionArchitectureAPI.Services.Indigo
{
    public class _updateContact:ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _updateContact(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetSessionValue(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }

        Logs logs = new Logs();
        public async Task<UpdateContactsResponse> GetUpdateContacts(string Signature, string emailAddress, string contactnumber, string companyName,string customerNumber, string _AirLineWay = "")
        {
            UpdateContactsRequest _ContactModel6E = new UpdateContactsRequest();
            //  _ContactModel.emailAddress = passengerdetails.Email;
            _ContactModel6E.updateContactsRequestData = new UpdateContactsRequestData();
            _ContactModel6E.Signature = Signature;
            _ContactModel6E.ContractVersion = 456;
            _ContactModel6E.updateContactsRequestData.BookingContactList = new BookingContact[1];
            _ContactModel6E.updateContactsRequestData.BookingContactList[0] = new BookingContact();
            _ContactModel6E.updateContactsRequestData.BookingContactList[0].EmailAddress = emailAddress;
            //if (customerNumber != null)
            //{
                _ContactModel6E.updateContactsRequestData.BookingContactList[0].TypeCode = "I";
                _ContactModel6E.updateContactsRequestData.BookingContactList[0].CompanyName = companyName;//"Indigo";
                _ContactModel6E.updateContactsRequestData.BookingContactList[0].CustomerNumber = customerNumber; //GSTNumber Re_ Assistance required for SG API Integration\GST Logs.zip\GST Logs
            //}

            _ContactModel6E.updateContactsRequestData.BookingContactList[0].HomePhone = contactnumber;
            _getapi  objIndigo = new _getapi();
            UpdateContactsResponse responseAddContact6E = await objIndigo.GetUpdateContacts(_ContactModel6E);
            SetSessionValue("ContactDetails", JsonConvert.SerializeObject(_ContactModel6E));
            string Str1 = JsonConvert.SerializeObject(responseAddContact6E);
            if (_AirLineWay.ToLower() == "oneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_ContactModel6E) + "\n\n Response: " + JsonConvert.SerializeObject(responseAddContact6E), "UpdateContact", "IndigoOneWay");

            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_ContactModel6E) + "\n\n Response: " + JsonConvert.SerializeObject(responseAddContact6E), "UpdateContact", "IndigoRT");
            }
            return (UpdateContactsResponse)responseAddContact6E;

        }
        public async Task<UpdatePassengersResponse> UpdatePassengers(string Signature, List<passkeytype> passengerdetails,string _Airlineway="")
        {
            UpdatePassengersResponse updatePaxResp = null;
            UpdatePassengersRequest updatePaxReq = null;

            try
            {
                updatePaxReq = new UpdatePassengersRequest(); //Assign Signature generated from Session
                updatePaxReq.Signature = Signature;
                updatePaxReq.ContractVersion = 456;
                updatePaxReq.updatePassengersRequestData = new UpdatePassengersRequestData();
                updatePaxReq.updatePassengersRequestData.Passengers = GetPassenger(passengerdetails);

                try
                {
                    _getapi objIndigo = new _getapi();
                    updatePaxResp = await objIndigo.UpdatePassengers(updatePaxReq);

                    string Str2 = JsonConvert.SerializeObject(updatePaxResp);
                    Logs logs = new Logs();
                    if (_Airlineway.ToLower() == "oneway")
                    {
                        logs.WriteLogs("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(updatePaxResp), "UpdatePassenger", "IndigoOneWay");
                    }
                    else
                    {
                        logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(updatePaxResp), "UpdatePassenger", "IndigoRT");
                    }
                    //return (UpdatePassengersResponse)updatePaxResp;
                }
                catch (Exception ex)
                {
                    logs.WriteLogs("Request: " + JsonConvert.SerializeObject(updatePaxReq) + "\n\n Response: " + JsonConvert.SerializeObject(ex.ToString()), "UpdatePassengerException", "IndigoRT");

                }
            }
            catch (Exception ex)
            {


            }
            SetSessionValue("PassengerNameDetails", JsonConvert.SerializeObject(passengerdetails));
            return (UpdatePassengersResponse)updatePaxResp;
        }
        public class Paxes
        {
            public List<passkeytype> Adults_ { get; set; }
            public List<passkeytype> Childs_ { get; set; }

            public List<passkeytype> Infant_ { get; set; }
        }
        Paxes _paxes = new Paxes();
        public Passenger[] GetPassenger(List<passkeytype> travellers_)
        {

            _paxes.Adults_ = new List<passkeytype>();
            _paxes.Childs_ = new List<passkeytype>();
            _paxes.Infant_ = new List<passkeytype>();
            for (int i = 0; i < travellers_.Count; i++)
            {
                if (travellers_[i].passengertypecode == "ADT")
                    _paxes.Adults_.Add(travellers_[i]);
                else if (travellers_[i].passengertypecode == "CHD")
                    _paxes.Childs_.Add(travellers_[i]);
                else if (travellers_[i].passengertypecode == "INFT" || travellers_[i].passengertypecode == "INF")
                    _paxes.Infant_.Add(travellers_[i]);

            }
            SetSessionValue("PaxArray", JsonConvert.SerializeObject(_paxes));
            Passenger[] passengers = null;
            try
            {



                int chdPax = 0;
                int infFax = 0;
                if (_paxes.Childs_ != null)
                {
                    chdPax = _paxes.Childs_.Count;
                }
                if (_paxes.Infant_ != null)
                {
                    infFax = _paxes.Infant_.Count;
                }
                passengers = new Passenger[_paxes.Adults_.Count + chdPax]; //Assign Passenger Information 
                Passenger p1 = null;
                int PassCnt = 0;
                for (int cntAdt = 0; cntAdt < _paxes.Adults_.Count; cntAdt++)
                {
                    p1 = new Passenger();
                    p1.PassengerNumberSpecified = true;
                    p1.PassengerNumber = Convert.ToInt16(PassCnt);
                    p1.Names = new BookingName[1];
                    p1.Names[0] = new BookingName();
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].first))
                    {
                        p1.Names[0].FirstName = Convert.ToString(_paxes.Adults_[cntAdt].first.Trim()).ToUpper();
                    }
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].middle))
                    {
                        p1.Names[0].MiddleName = Convert.ToString(_paxes.Adults_[cntAdt].middle.Trim()).ToUpper();
                    }
                    if (!string.IsNullOrEmpty(_paxes.Adults_[cntAdt].last))
                    {
                        p1.Names[0].LastName = Convert.ToString(_paxes.Adults_[cntAdt].last.Trim()).ToUpper();
                    }
                    p1.Names[0].Title = _paxes.Adults_[cntAdt].title.ToUpper().Replace(".", "");
                    p1.PassengerInfo = new PassengerInfo();
                    if (_paxes.Adults_[cntAdt].title.ToUpper().Replace(".", "") == "MR")
                    {
                        p1.PassengerInfo.Gender = Gender.Male;
                        p1.PassengerInfo.WeightCategory = WeightCategory.Male;
                    }
                    else
                    {
                        p1.PassengerInfo.Gender = Gender.Female;
                        p1.PassengerInfo.WeightCategory = WeightCategory.Female;
                    }
                    p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                    p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                    p1.PassengerTypeInfos[0].DOBSpecified = true;
                    p1.PassengerTypeInfos[0].PaxType = _paxes.Adults_[cntAdt].passengertypecode.ToString().ToUpper();
                    if (_paxes.Infant_ != null && _paxes.Infant_.Count > 0)
                    {
                        if (cntAdt < _paxes.Infant_.Count)
                        {
                            p1.Infant = new PassengerInfant();
                            p1.Infant.DOBSpecified = true;
                            p1.Infant.DOB = Convert.ToDateTime("2023-08-01");//Convert.ToDateTime(_paxes.Infant_[cntAdt].dateOfBirth);
                                                                             //p1.Infant.Gender = Gender.Male;
                            if (_paxes.Infant_[cntAdt].title.ToUpper().Replace(".", "") == "MR")
                            {
                                p1.Infant.Gender = Gender.Male;
                            }
                            else
                            {
                                p1.Infant.Gender = Gender.Female;
                            }
                            p1.Infant.Names = new BookingName[1];
                            p1.Infant.Names[0] = new BookingName();
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].first))
                            {
                                p1.Infant.Names[0].FirstName = Convert.ToString(_paxes.Infant_[cntAdt].first.Trim());
                            }
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].middle))
                            {
                                p1.Infant.Names[0].MiddleName = Convert.ToString(_paxes.Infant_[cntAdt].middle.Trim());
                            }
                            if (!string.IsNullOrEmpty(_paxes.Infant_[cntAdt].last))
                            {
                                p1.Infant.Names[0].LastName = Convert.ToString(_paxes.Infant_[cntAdt].last.Trim());
                            }
                            p1.Infant.Names[0].Title = _paxes.Infant_[cntAdt].title.Replace(".", "");
                            p1.Infant.Nationality = _paxes.Infant_[cntAdt].nationality;
                            p1.Infant.ResidentCountry = _paxes.Infant_[cntAdt].residentCountry;
                            p1.State = MessageState.New;
                        }

                    }

                    passengers[PassCnt] = p1;
                    PassCnt++;
                }
                if (_paxes.Childs_ != null)
                {
                    for (int cntChd = 0; cntChd < _paxes.Childs_.Count; cntChd++)
                    {
                        p1 = new Passenger();

                        p1.PassengerNumberSpecified = true;
                        p1.PassengerNumber = Convert.ToInt16(PassCnt);
                        p1.Names = new BookingName[1];
                        p1.Names[0] = new BookingName();

                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].first))
                        {
                            p1.Names[0].FirstName = Convert.ToString(_paxes.Childs_[cntChd].first).ToUpper();
                        }
                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].middle))
                        {
                            p1.Names[0].MiddleName = Convert.ToString(_paxes.Childs_[cntChd].middle).ToUpper();
                        }
                        if (!string.IsNullOrEmpty(_paxes.Childs_[cntChd].last))
                        {
                            p1.Names[0].LastName = Convert.ToString(_paxes.Childs_[cntChd].last).ToUpper();
                        }
                        p1.Names[0].Title = _paxes.Childs_[cntChd].title.ToUpper().Replace(".", "");
                        p1.PassengerInfo = new PassengerInfo();
                        if (_paxes.Childs_[cntChd].title.ToUpper().Replace(".", "") == "Mr")
                        {
                            p1.PassengerInfo.Gender = Gender.Male;
                            p1.PassengerInfo.WeightCategory = WeightCategory.Male;
                        }
                        else
                        {
                            p1.PassengerInfo.Gender = Gender.Female;
                            p1.PassengerInfo.WeightCategory = WeightCategory.Female;
                        }
                        p1.PassengerTypeInfos = new PassengerTypeInfo[1];
                        p1.PassengerTypeInfos[0] = new PassengerTypeInfo();
                        p1.PassengerTypeInfos[0].DOBSpecified = true;
                        p1.PassengerTypeInfos[0].PaxType = _paxes.Childs_[cntChd].passengertypecode.ToString().ToUpper();
                        passengers[PassCnt] = p1;
                        PassCnt++;
                    }
                }
            }
            catch (SystemException sex_)
            {
            }
            return passengers;
        }
    }

}
