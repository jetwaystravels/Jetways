using DomainLayer.Model;
using DomainLayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Interface
{
    public interface Itb_Booking
    {
        String PostTicketDataRepo(AirLineFlightTicketBooking ticketObject);
    }
}
