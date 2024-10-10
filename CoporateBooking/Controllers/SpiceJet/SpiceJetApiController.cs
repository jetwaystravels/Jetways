using Microsoft.AspNetCore.Mvc;
using Sessionmanager;
using Bookingmanager_;
using Newtonsoft.Json;
using Utility;

namespace OnionConsumeWebAPI.Controllers
{
    public class SpiceJetApiController : Controller
    {
        #region Signature
        public async Task<LogonResponse> Signature(LogonRequest _logonRequestobj)
        {

            //string str=Newtonsoft.Json.JsonConvert.SerializeObject(_logonRequestobj);
            ISessionManager Sessionmanager = null;
            LogonResponse logonResponse = null;
            Sessionmanager = new SessionManagerClient();
            try
            {
                logonResponse = await Sessionmanager.LogonAsync(_logonRequestobj);
                return logonResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return logonResponse;
        }
        #endregion

        #region GetAvailabilityVer2Async
        public async Task<GetAvailabilityVer2Response> GetAvailabilityVer2Async(GetAvailabilityRequest _getAvailabilityRQ)
        {
            IBookingManager bookingManager = null;
            GetAvailabilityVer2Response getAvailabilityVer2Response = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getAvailabilityVer2Response = await bookingManager.GetAvailabilityVer2Async(_getAvailabilityRQ);
                return getAvailabilityVer2Response;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getAvailabilityVer2Response;
        }
        #endregion

        #region GetItineraryPriceAsync
        public async Task<PriceItineraryResponse> GetItineraryPriceAsync(PriceItineraryRequest _getItineraryPriceRQ)
        {
            IBookingManager bookingManager = null;
            PriceItineraryResponse getItineraryPriceResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getItineraryPriceResponse = await bookingManager.GetItineraryPriceAsync(_getItineraryPriceRQ);
                return getItineraryPriceResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getItineraryPriceResponse;
        }
        #endregion


        #region GetSellAsync
        public async Task<SellResponse> GetSellAsync(SellRequest _getSellRQ)
        {
            IBookingManager bookingManager = null;
            SellResponse getSellResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getSellResponse = await bookingManager.SellAsync(_getSellRQ);
                return getSellResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getSellResponse;
        }
        #endregion


        #region GetUpdateContactsAsync
        public async Task<UpdateContactsResponse> GetUpdateContactsAsync(UpdateContactsRequest _getUpdateContactsRQ)
        {
            IBookingManager bookingManager = null;
            UpdateContactsResponse getUpdateContactsResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getUpdateContactsResponse = await bookingManager.UpdateContactsAsync(_getUpdateContactsRQ);
                return getUpdateContactsResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getUpdateContactsResponse;
        }
        #endregion


        #region UpdatePassengers
        public async Task<UpdatePassengersResponse> UpdatePassengers(UpdatePassengersRequest _getUpdatePaxRQ)
        {
            IBookingManager bookingManager = null;
            UpdatePassengersResponse getUpdatePaxResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getUpdatePaxResponse = await bookingManager.UpdatePassengersAsync(_getUpdatePaxRQ);
                return getUpdatePaxResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getUpdatePaxResponse;
        }
        #endregion

        #region GetSSRAvailabilityForBooking
        public async Task<GetSSRAvailabilityForBookingResponse> GetSSRAvailabilityForBooking(GetSSRAvailabilityForBookingRequest _getSSrAvailRQ)
        {
            IBookingManager bookingManager = null;
            GetSSRAvailabilityForBookingResponse _res = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _res = await bookingManager.GetSSRAvailabilityForBookingAsync(_getSSrAvailRQ);
                return _res;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _res;
        }
        #endregion


        #region GetseatAvaialbility
        public async Task<GetSeatAvailabilityResponse> GetseatAvaialbility(GetSeatAvailabilityRequest _getSeatAvaiulabilityRQ)
        {
            IBookingManager bookingManager = null;
            GetSeatAvailabilityResponse getseatAvailabilityResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getseatAvailabilityResponse = await bookingManager.GetSeatAvailabilityAsync(_getSeatAvaiulabilityRQ);
                return getseatAvailabilityResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getseatAvailabilityResponse;
        }
        #endregion

        #region Assignseat
        public async Task<AssignSeatsResponse> Assignseat(AssignSeatsRequest _getAssignseatRQ)
        {
            IBookingManager bookingManager = null;
            AssignSeatsResponse getAssignseatResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getAssignseatResponse = await bookingManager.AssignSeatsAsync(_getAssignseatRQ);
                return getAssignseatResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getAssignseatResponse;
        }
        #endregion

        #region sellssR
        public async Task<SellResponse> sellssR(SellRequest _getSellRQ)
        {
            IBookingManager bookingManager = null;
            SellResponse getSellResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getSellResponse = await bookingManager.SellAsync(_getSellRQ);
                return getSellResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getSellResponse;
        }
        #endregion


        #region GetBookingFromState
        public async Task<GetBookingFromStateResponse> GetBookingFromState(GetBookingFromStateRequest _getBookingFromStateRQ)
        {
            IBookingManager bookingManager = null;
            GetBookingFromStateResponse _getBookingFromStateResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getBookingFromStateResponse = await bookingManager.GetBookingFromStateAsync(_getBookingFromStateRQ);
                return _getBookingFromStateResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _getBookingFromStateResponse;
        }
        #endregion

        #region BookingCommit
        public async Task<BookingCommitResponse> BookingCommit(BookingCommitRequest _getbookingCommitRQ)
        {
            IBookingManager bookingManager = null;
            BookingCommitResponse getBookiongCommitResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getBookiongCommitResponse = await bookingManager.BookingCommitAsync(_getbookingCommitRQ);
                return getBookiongCommitResponse;
            }
            catch (Exception ex)
            {
                Logs logs=new Logs ();
                logs.WriteLogs("Request: " + JsonConvert.SerializeObject(_getbookingCommitRQ) + "\n\n Response: " + ex.ToString(), "BookingCommit", "SpicejetOneWay");
                //return Ok(session);
            }
            return getBookiongCommitResponse;
        }
        #endregion


        #region GetBookingdetails
        public async Task<GetBookingResponse> GetBookingdetails(GetBookingRequest _getbookingRQ)
        {
            IBookingManager bookingManager = null;
            GetBookingResponse getBookiongResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                getBookiongResponse = await bookingManager.GetBookingAsync(_getbookingRQ);
                return getBookiongResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return getBookiongResponse;
        }
        #endregion

        #region Sessionlogout
        public async Task<LogoutResponse> Logout(LogoutRequest _logoutRequestobj)
        {

            //string str=Newtonsoft.Json.JsonConvert.SerializeObject(_logonRequestobj);
            ISessionManager Sessionmanager = null;
            LogoutResponse logoutResponse = null;
            Sessionmanager = new SessionManagerClient();
            try
            {
                logoutResponse = await Sessionmanager.LogoutAsync(_logoutRequestobj);
                return logoutResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return logoutResponse;
        }
        #endregion
    }
}
