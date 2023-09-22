using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class TicketBookingServices : ITicketBooking
    {
        private readonly AppDbContext _dbContext;
        public TicketBookingServices(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        //     public List<TicketBooking> GetAllBookingRepo()
        //     {
        //var data = _dbContext.TicketBooking.ToList();
        //return data;

        //     }
        public List<TicketBooking> GetAllBookingRepo()
        {
            var data = _dbContext.TicketBooking.FromSqlRaw<TicketBooking>("spGetFlightDetails").ToList();
            return data;

        }

        public string AddTicketRepo(TicketBooking ticketBooking)
        {
            try
            {
                this._dbContext.Add(ticketBooking);
                this._dbContext.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
