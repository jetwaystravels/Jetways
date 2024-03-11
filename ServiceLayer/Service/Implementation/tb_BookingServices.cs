using DomainLayer.Model;
using DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.DbContextLayer;
using ServiceLayer.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service.Implementation
{
    public class tb_BookingServices: Itb_Booking
    {
        private readonly AppDbContext _dbContext;
        public tb_BookingServices(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public string PostTicketDataRepo(AirLineFlightTicketBooking ticketObject)
        {
            try
            {
                

                var outputParameter = new Microsoft.Data.SqlClient.SqlParameter("@OutputParameter", System.Data.SqlDbType.NVarChar, 30);
                outputParameter.Direction = System.Data.ParameterDirection.Output;

                // Execute  output stored procedure retrieve jetwaysId
                var jetid= _dbContext.tb_DailyNumber.FromSqlRaw<tb_DailyNumber>("EXECUTE sp_jetwaysIdGenerate @OutputParameter OUTPUT", outputParameter).ToList();
                var jetwaysId = jetid[0].Autogenratednumber;
                tb_Booking tbobj =  new tb_Booking();
                    tbobj.BookingID = ticketObject.BookingID;
                    tbobj.jetwaysId = jetwaysId;
                    tbobj.AirLineID = ticketObject.tb_Booking.AirLineID;
                    tbobj.Origin = ticketObject.tb_Booking.Origin;
                    tbobj.RecordLocator = ticketObject.tb_Booking.RecordLocator;
                    tbobj.CurrencyCode = ticketObject.tb_Booking.CurrencyCode;
                    tbobj.BookedDate = ticketObject.tb_Booking.BookedDate;
                    tbobj.Destination = ticketObject.tb_Booking.Destination;
                    tbobj.DepartureDate = ticketObject.tb_Booking.DepartureDate;
                    tbobj.ArrivalDate = ticketObject.tb_Booking.ArrivalDate;
                    tbobj.TotalAmount = ticketObject.tb_Booking.TotalAmount;
                    tbobj.SpecialServicesTotal = ticketObject.tb_Booking.SpecialServicesTotal;
                    tbobj.SpecialServicesTotal_Tax = ticketObject.tb_Booking.SpecialServicesTotal;
                    tbobj.SeatTotalAmount = ticketObject.tb_Booking.SeatTotalAmount;
                    tbobj.SeatTotalAmount_Tax = ticketObject.tb_Booking.SeatTotalAmount_Tax;
                    tbobj.ExpirationDate = ticketObject.tb_Booking.ExpirationDate;
                    tbobj.BookingDoc = ticketObject.tb_Booking.BookingDoc;
                    tbobj.CreatedDate = ticketObject.tb_Booking.CreatedDate;
                    tbobj.Createdby = ticketObject.tb_Booking.Createdby;
                    tbobj.ModifyBy = ticketObject.tb_Booking.ModifyBy;
                    tbobj.ModifiedDate = ticketObject.tb_Booking.ModifiedDate;
                    tbobj.Status = ticketObject.tb_Booking.Status;
                    this._dbContext.Add(tbobj);
                    //this._dbContext.SaveChanges();
                    
                    tb_PassengerTotal tb_PassengerTotalobj = new tb_PassengerTotal();
                    tb_PassengerTotalobj.BookingID = ticketObject.BookingID;
                    tb_PassengerTotalobj.TotalBookingAmount = ticketObject.tb_PassengerTotal.TotalBookingAmount;
                    tb_PassengerTotalobj.totalBookingAmount_Tax = ticketObject.tb_PassengerTotal.totalBookingAmount_Tax;
                    tb_PassengerTotalobj.TotalMealsAmount = ticketObject.tb_PassengerTotal.TotalMealsAmount;
                    tb_PassengerTotalobj.TotalMealsAmount_Tax = ticketObject.tb_PassengerTotal.TotalMealsAmount_Tax;
                    tb_PassengerTotalobj.TotalSeatAmount = ticketObject.tb_PassengerTotal.TotalSeatAmount;
                    tb_PassengerTotalobj.TotalSeatAmount_Tax = ticketObject.tb_PassengerTotal.TotalMealsAmount_Tax;
                    tb_PassengerTotalobj.Createdby = ticketObject.tb_PassengerTotal.Createdby;
                    tb_PassengerTotalobj.Modifyby = ticketObject.tb_PassengerTotal.Modifyby;
                    tb_PassengerTotalobj.Status = ticketObject.tb_PassengerTotal.Status;
                    tb_PassengerTotalobj.CreatedDate = ticketObject.tb_PassengerTotal.CreatedDate;
                    tb_PassengerTotalobj.ModifiedDate = ticketObject.tb_PassengerTotal.ModifiedDate;
                    this._dbContext.Add(tb_PassengerTotalobj);
                    //this._dbContext.SaveChanges();


                var journeyDataCount = ticketObject.tb_journeys.Count;
                for (int k = 0; k < journeyDataCount; k++)
                {
                    tb_journeys tb_Journeysobj = new tb_journeys();
                    tb_Journeysobj.BookingID = ticketObject.BookingID;
                    tb_Journeysobj.JourneyKey = ticketObject.tb_journeys[k].JourneyKey;
                    tb_Journeysobj.Stops = ticketObject.tb_journeys[k].Stops;
                    tb_Journeysobj.Origin = ticketObject.tb_journeys[k].Origin;
                    tb_Journeysobj.Destination = ticketObject.tb_journeys[k].Destination;
                    tb_Journeysobj.JourneyKeyCount = ticketObject.tb_journeys[k].JourneyKeyCount;
                    tb_Journeysobj.FlightType = ticketObject.tb_journeys[k].FlightType;
                    tb_Journeysobj.DepartureDate = ticketObject.tb_journeys[k].DepartureDate;
                    tb_Journeysobj.ArrivalDate = ticketObject.tb_journeys[k].ArrivalDate;
                    tb_Journeysobj.CreatedDate = ticketObject.tb_journeys[k].CreatedDate;
                    tb_Journeysobj.Createdby = ticketObject.tb_journeys[k].Createdby;
                    tb_Journeysobj.ModifiedDate = ticketObject.tb_journeys[k].ModifiedDate;
                    tb_Journeysobj.Modifyby = ticketObject.tb_journeys[k].Modifyby;
                    tb_Journeysobj.Status = ticketObject.tb_journeys[k].Status;
                    this._dbContext.Add(tb_Journeysobj);
                    //this._dbContext.SaveChanges();

                }
                var SegementDataCount = ticketObject.tb_Segments.Count;                    
                    for(int i = 0; i< SegementDataCount;i++)
                    {
                        //tb_Segments tb_Segments = new tb_Segments();
                            tb_Segments segmentReturnobj = new tb_Segments();                        
                            segmentReturnobj.BookingID = ticketObject.BookingID; 
                            segmentReturnobj.journeyKey = ticketObject.tb_Segments[i].journeyKey;
                            segmentReturnobj.SegmentKey = ticketObject.tb_Segments[i].SegmentKey;
                            segmentReturnobj.SegmentCount = ticketObject.tb_Segments[i].SegmentCount;
                            segmentReturnobj.Origin = ticketObject.tb_Segments[i].Origin;
                            segmentReturnobj.Destination = ticketObject.tb_Segments[i].Destination;
                            segmentReturnobj.DepartureDate = ticketObject.tb_Segments[i].DepartureDate;
                            segmentReturnobj.ArrivalDate = ticketObject.tb_Segments[i].ArrivalDate;
                            segmentReturnobj.Identifier = ticketObject.tb_Segments[i].Identifier;
                            segmentReturnobj.CarrierCode = ticketObject.tb_Segments[i].CarrierCode;
                            segmentReturnobj.Seatnumber = ticketObject.tb_Segments[i].Seatnumber;
                            segmentReturnobj.MealCode = ticketObject.tb_Segments[i].MealCode;
                            segmentReturnobj.MealDiscription = ticketObject.tb_Segments[i].MealDiscription;
                            segmentReturnobj.DepartureTerminal = ticketObject.tb_Segments[i].DepartureTerminal;
                            segmentReturnobj.ArrivalTerminal = ticketObject.tb_Segments[i].ArrivalTerminal;
                            segmentReturnobj.CreatedDate = ticketObject.tb_Segments[i].CreatedDate;
                            segmentReturnobj.ModifiedDate = ticketObject.tb_Segments[i].ModifiedDate;
                            segmentReturnobj.Createdby = ticketObject.tb_Segments[i].Createdby;
                            segmentReturnobj.Modifyby = ticketObject.tb_Segments[i].Modifyby;
                            segmentReturnobj.Status = ticketObject.tb_Segments[i].Status;
                            this._dbContext.Add(segmentReturnobj);
                       // this._dbContext.SaveChanges();

                    }
                    var passengerDatacount = ticketObject.tb_PassengerDetails.Count;
                    for (int j = 0; j < passengerDatacount; j++)
                    {
                        tb_PassengerDetails tb_PassengerDetails = new tb_PassengerDetails();
                        tb_PassengerDetails.BookingID = ticketObject.BookingID;
                        tb_PassengerDetails.PassengerKey = ticketObject.tb_PassengerDetails[j].PassengerKey;
                        tb_PassengerDetails.Title = ticketObject.tb_PassengerDetails[j].Title;
                        tb_PassengerDetails.FirstName = ticketObject.tb_PassengerDetails[j].FirstName;
                        tb_PassengerDetails.LastName = ticketObject.tb_PassengerDetails[j].LastName;
                        tb_PassengerDetails.TypeCode = ticketObject.tb_PassengerDetails[j].TypeCode;
                        tb_PassengerDetails.ModifiedDate = ticketObject.tb_PassengerDetails[j].ModifiedDate;
                        tb_PassengerDetails.CreatedDate = ticketObject.tb_PassengerDetails[j].CreatedDate;
                        tb_PassengerDetails.Createdby = ticketObject.tb_PassengerDetails[j].Createdby;
                        tb_PassengerDetails.ModifyBy = ticketObject.tb_PassengerDetails[j].ModifyBy;
                        //tb_PassengerDetails.Dob = ticketObject.tb_PassengerDetails[j].Dob;
                        tb_PassengerDetails.TotalAmount_tax = ticketObject.tb_PassengerDetails[j].TotalAmount_tax;
                        this._dbContext.Add(tb_PassengerDetails);
                        //this._dbContext.SaveChanges();

                    }
                    tb_AirCraft tb_Air = new tb_AirCraft();
                    tb_Air.AirlineID = ticketObject.tb_AirCraft.AirlineID;
                    tb_Air.AirCraftName = ticketObject.tb_AirCraft.AirCraftName;
                    tb_Air.AirCraftDescription = ticketObject.tb_AirCraft.AirCraftDescription;
                    tb_Air.Modifyby = ticketObject.tb_AirCraft.Modifyby;
                    tb_Air.Createdby = ticketObject.tb_AirCraft.Createdby;
                    tb_Air.CreatedDate = ticketObject.tb_AirCraft.CreatedDate;
                    tb_Air.Modifieddate = ticketObject.tb_AirCraft.Modifieddate;
                    tb_Air.Status = ticketObject.tb_AirCraft.Status;
                                
                    this._dbContext.Add(tb_Air);
                    this._dbContext.SaveChanges();
                
                return "Success";


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           // throw new NotImplementedException();
        }
       
    }
}


//tb_Airlines tb_Airlinesobj = new tb_Airlines();
//tb_Airlinesobj.AirlineID = ticketObject.tb_Airlines.AirlineID;
//tb_Airlinesobj.AirlneName = ticketObject.tb_Airlines.AirlneName;
//tb_Airlinesobj.AirlineDescription = ticketObject.tb_Airlines.AirlineDescription;
//tb_Airlinesobj.CreatedDate = ticketObject.tb_Airlines.CreatedDate;
//tb_Airlinesobj.Modifieddate = ticketObject.tb_Airlines.Modifieddate;
//tb_Airlinesobj.Createdby = ticketObject.tb_Airlines.Createdby;
//tb_Airlinesobj.Modifyby = ticketObject.tb_Airlines.Modifyby;
