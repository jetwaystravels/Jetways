using IndigoBookingManager_;
using IndigoSessionmanager_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Indigo
{
    public class _getapi
    {
        #region Signature
        public async Task<LogonResponse> Signature(LogonRequest _logonRequestobj)
        {
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

        #region GetAvailability
        public async Task<GetAvailabilityVer2Response> GetTripAvailability(GetAvailabilityRequest _getAvailabilityReturnRQ)
        {
            IBookingManager bookingManager = null;
            GetAvailabilityVer2Response _getAvailabilityVer2ReturnResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getAvailabilityVer2ReturnResponse = await bookingManager.GetAvailabilityVer2Async(_getAvailabilityReturnRQ);
                return _getAvailabilityVer2ReturnResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _getAvailabilityVer2ReturnResponse;
        }
        #endregion

        #region Sell
        public async Task<SellResponse> sell(SellRequest _SellRQ)
        {
            IBookingManager bookingManager = null;
            SellResponse _SellResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _SellResponse = await bookingManager.SellAsync(_SellRQ);
                return _SellResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _SellResponse;
        }
        #endregion

        #region getPriceitenary
        public async Task<PriceItineraryResponse> GetItineraryPrice(PriceItineraryRequest _getPriceItineraryRQ)
        {
            IBookingManager bookingManager = null;
            PriceItineraryResponse _getPriceItineraryRS = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getPriceItineraryRS = await bookingManager.GetItineraryPriceAsync(_getPriceItineraryRQ);
                return _getPriceItineraryRS;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _getPriceItineraryRS;
        }
        #endregion

        #region GetUpdateContacts
        public async Task<UpdateContactsResponse> GetUpdateContacts(UpdateContactsRequest UpdateContactsRequest)
        {
            IBookingManager bookingManager = null;
            UpdateContactsResponse _responseAddContactRS = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _responseAddContactRS = await bookingManager.UpdateContactsAsync(UpdateContactsRequest);
                return _responseAddContactRS;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _responseAddContactRS;
        }
        #endregion

        #region UpdatePassengers
        public async Task<UpdatePassengersResponse> UpdatePassengers(UpdatePassengersRequest updatePaxReq)
        {
            IBookingManager bookingManager = null;
            UpdatePassengersResponse updatePaxResp = null;
            bookingManager = new BookingManagerClient();
            try
            {
                updatePaxResp = await bookingManager.UpdatePassengersAsync(updatePaxReq);
                return updatePaxResp;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return updatePaxResp;
        }
        #endregion


        #region GetseatAvailability
        public async Task<GetSeatAvailabilityResponse> GetseatAvailability(GetSeatAvailabilityRequest _getseatAvailabilityRequest)
        {
            IBookingManager bookingManager = null;
            GetSeatAvailabilityResponse _getSeatAvailabilityResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getSeatAvailabilityResponse = await bookingManager.GetSeatAvailabilityAsync(_getseatAvailabilityRequest);
                return _getSeatAvailabilityResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _getSeatAvailabilityResponse;
        }
        #endregion


        #region GetMealAvailabilityForBooking
        public async Task<GetSSRAvailabilityForBookingResponse> GetMealAvailabilityForBooking(GetSSRAvailabilityForBookingRequest _req)
        {
            IBookingManager bookingManager = null;
            GetSSRAvailabilityForBookingResponse _res = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _res = await bookingManager.GetSSRAvailabilityForBookingAsync(_req);
                return _res;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _res;
        }
        #endregion

        #region _sellssR
        public async Task<SellResponse> _sellssR(SellRequest sellSsrRequest)
        {
            IBookingManager bookingManager = null;
            SellResponse SellssRResponse = null;
            bookingManager = new BookingManagerClient();
            try
            {
                SellssRResponse = await bookingManager.SellAsync(sellSsrRequest);
                return SellssRResponse;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return SellssRResponse;
        }
        #endregion

        #region _sellssR
        public async Task<AssignSeatsResponse> _Assignseat(AssignSeatsRequest _AssignseatReq)
        {
            IBookingManager bookingManager = null;
            AssignSeatsResponse _AssignseatRes = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _AssignseatRes = await bookingManager.AssignSeatsAsync(_AssignseatReq);
                return _AssignseatRes;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _AssignseatRes;
        }
        #endregion

        #region BookingCommit
        public async Task<BookingCommitResponse> BookingCommit(BookingCommitRequest _bookingCommitRequest)
        {
            IBookingManager bookingManager = null;
            BookingCommitResponse _bookingCommitRes = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _bookingCommitRes = await bookingManager.BookingCommitAsync(_bookingCommitRequest);
                return _bookingCommitRes;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _bookingCommitRes;
        }
        #endregion


        #region GetBookingdetails
        public async Task<GetBookingResponse> GetBookingdetails(GetBookingRequest _getbookingRequest)
        {
            IBookingManager bookingManager = null;
            GetBookingResponse _getbookingRes = null;
            bookingManager = new BookingManagerClient();
            try
            {
                _getbookingRes = await bookingManager.GetBookingAsync(_getbookingRequest);
                return _getbookingRes;
            }
            catch (Exception ex)
            {
                //return Ok(session);
            }
            return _getbookingRes;
        }
        #endregion
    }
}
