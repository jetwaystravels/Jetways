using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Bookingmanager_;
namespace DomainLayer.ViewModel
{
    public class ViewModel
    {
        public AirAsiaTripResponceModel passeengerlist { set; get; }
        public AirAsiaTripResponceModel passeengerlistR { set; get; }
        public AirAsiaTripResponceModel passeengerlistItanary { set; get; }
        public AirAsiaTripResponceModel passeengerlistItanaryR { set; get; }
        public SeatMapResponceModel Seatmaplist { set; get; }
        public SeatMapResponceModel SeatmaplistR { set; get; }
        public SSRAvailabiltyResponceModel Meals { set; get; }
        public SSRAvailabiltyResponceModel MealsR { set; get; }
        public SSRAvailabiltyResponceModel Baggage { set; get; }
        public List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist { get; set; }
        public List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelistR { get; set; }
        public PassengerModel _PassengerModel { get; set; }
        public List<AirAsiaTripResponceModel> passeengerlistRT { set; get; }
        public List<SeatMapResponceModel> SeatmaplistRT { set; get; }
        public List<SSRAvailabiltyResponceModel> MealslistRT { set; get; }
        public SimpleAvailabilityRequestModel MyProperty { get; set; }
        public SimpleAvailabilityRequestModel simpleAvailabilityRequestModelEdit { get; set; }
        public List<passkeytype> passkeytype { get; set; }
        public List<passkeytype> passengerNamedetails { get; set; }


        //AkasaAirModel
        public AirAsiaTripResponceModel AkPassenger { set; get; }
        public SeatMapResponceModel AkSeatmaplist { set; get; }
        public SSRAvailabiltyResponceModel AkMealslist { set; get; }
        public SSRAvailabiltyResponceModel AkBaggageDetails { set; get; }
        public AirAsiaTripResponceModel AkpasseengerItanary { set; get; }
    }
}