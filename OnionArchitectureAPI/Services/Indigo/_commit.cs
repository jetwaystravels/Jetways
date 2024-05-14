using DomainLayer.Model;
using Indigo;
using IndigoBookingManager_;
using Newtonsoft.Json;
using Utility;

namespace OnionArchitectureAPI.Services.Indigo
{
    public class _commit
    {

        Logs logs = new Logs();
        _getapi _obj = new _getapi();
        public async Task<BookingCommitResponse> commit(string Signature, UpdateContactsRequest contactList, List<passkeytype> passeengerlist,string _Airlineway="")
        {
            BookingCommitRequest _bookingCommitRequest = new BookingCommitRequest();
            BookingCommitResponse _BookingCommitResponse = new BookingCommitResponse();
            _bookingCommitRequest.Signature = Signature;
            _bookingCommitRequest.ContractVersion = 456;
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
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].HomePhone = contactList.updateContactsRequestData.BookingContactList[0].HomePhone; //"9457000000"; //passeengerlist.mobile;
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine1 = "A";
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].AddressLine2 = "B";
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].City = "Delhi";
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CountryCode = "IN";
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].CultureCode = "en-GB";
            _bookingCommitRequest.BookingCommitRequestData.BookingContacts[0].DistributionOption = DistributionOption.Email;

            _BookingCommitResponse = await _obj.BookingCommit(_bookingCommitRequest);

            string Str3 = JsonConvert.SerializeObject(_BookingCommitResponse);
            if (_Airlineway.ToLower() == "oneway")
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingCommitRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingCommitResponse), "BookingCommit", "IndigoRT");
            }
            return (BookingCommitResponse)_BookingCommitResponse;

        }

        public async Task<AddPaymentToBookingResponse> AddpaymenttoBook(string Signature, decimal Amount, string _Airlineway = "")
        {
            AddPaymentToBookingRequest _bookingpaymentRequest = new AddPaymentToBookingRequest();
            AddPaymentToBookingResponse _BookingPaymentResponse = new AddPaymentToBookingResponse();
            _bookingpaymentRequest.Signature = Signature;
            _bookingpaymentRequest.ContractVersion = 456;
            _bookingpaymentRequest.addPaymentToBookingReqData = new AddPaymentToBookingRequestData();
            _bookingpaymentRequest.addPaymentToBookingReqData.MessageStateSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.MessageState = MessageState.New;
            _bookingpaymentRequest.addPaymentToBookingReqData.WaiveFeeSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.WaiveFee = false;
            _bookingpaymentRequest.addPaymentToBookingReqData.PaymentMethodTypeSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.PaymentMethodType = RequestPaymentMethodType.AgencyAccount;
            _bookingpaymentRequest.addPaymentToBookingReqData.PaymentMethodCode = "AG";
            _bookingpaymentRequest.addPaymentToBookingReqData.QuotedCurrencyCode = "INR";
            _bookingpaymentRequest.addPaymentToBookingReqData.QuotedAmountSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.QuotedAmount = Amount;
            _bookingpaymentRequest.addPaymentToBookingReqData.AccountNumber = "OTI122";
            _bookingpaymentRequest.addPaymentToBookingReqData.InstallmentsSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.Installments = 1;
            _bookingpaymentRequest.addPaymentToBookingReqData.ExpirationSpecified = true;
            _bookingpaymentRequest.addPaymentToBookingReqData.Expiration = Convert.ToDateTime("0001-01-01T00:00:00");

            _BookingPaymentResponse = await _obj.Addpayment(_bookingpaymentRequest);

            string Str3 = JsonConvert.SerializeObject(_BookingPaymentResponse);
            if (_Airlineway.ToLower() == "oneway")
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingpaymentRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingPaymentResponse), "BookingPayment", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_bookingpaymentRequest) + "\n\n Response: " + JsonConvert.SerializeObject(_BookingPaymentResponse), "BookingPayment", "IndigoRT");
            }
            return (AddPaymentToBookingResponse)_BookingPaymentResponse;

        }

        public async Task<GetBookingResponse> GetBookingdetails(string Signature, BookingCommitResponse _BookingCommitResponse,string _Airlineway="")
        {
            GetBookingRequest getBookingRequest = new GetBookingRequest();
            GetBookingResponse _getBookingResponse = new GetBookingResponse();
            getBookingRequest.Signature = Signature;
            getBookingRequest.ContractVersion = 456;
            getBookingRequest.GetBookingReqData = new GetBookingRequestData();
            getBookingRequest.GetBookingReqData.GetBookingBy = GetBookingBy.RecordLocator;
            getBookingRequest.GetBookingReqData.GetByRecordLocator = new GetByRecordLocator();
            getBookingRequest.GetBookingReqData.GetByRecordLocator.RecordLocator = _BookingCommitResponse.BookingUpdateResponseData.Success.RecordLocator;


            _getBookingResponse = await _obj.GetBookingdetails(getBookingRequest);
            string _responceGetBooking = JsonConvert.SerializeObject(_getBookingResponse);
            if (_Airlineway.ToLower() == "oneway")
            {
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails", "IndigoOneWay");
            }
            else
            {
                logs.WriteLogsR("Request: " + JsonConvert.SerializeObject(_getBookingResponse) + "\n\n Response: " + JsonConvert.SerializeObject(_getBookingResponse), "GetBookingDetails", "IndigoRT");

            }
            return (GetBookingResponse)_getBookingResponse;

        }
        public PointOfSale GetPointOfSale()
        {
            PointOfSale SourcePOS = null;
            try
            {
                SourcePOS = new PointOfSale();
                SourcePOS.State = MessageState.New;
                SourcePOS.OrganizationCode = "";
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
