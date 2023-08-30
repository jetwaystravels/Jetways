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
