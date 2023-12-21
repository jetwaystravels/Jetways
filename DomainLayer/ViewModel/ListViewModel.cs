using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;

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

        public List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelist { get; set; }

        public List<SimpleAvailibilityaAddResponce> SimpleAvailibilityaAddResponcelistR { get; set; }



    }
}
