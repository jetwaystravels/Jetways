﻿using DomainLayer.Model;
using System.Collections;
using System.Xml;
//using travelAirservice;

namespace OnionArchitectureAPI.Services.Travelport
{
    public class TravelPortParsing
    {
        public List<GDSResModel.Segment> ParseLowFareSearchRsp2(string lowFareResponse, string contractType_, DateTime depDate)
        {
            List<GDSResModel.Segment> listOfSegment = new List<GDSResModel.Segment>();
            //ArrayList listofTPSegment = null;
            ArrayList listOfBound = new ArrayList();
            int contractId = 1;
            string TPtransactionId = string.Empty;
            string outBoundGroup = string.Empty;
            string inBoundGroup = string.Empty;
            int journeyIndex = 0;
            bool IsFlex = true;
            string _journeyTime = string.Empty;
            //List<Bond> listOfBound = new List<Bond>();
            GDSResModel.Segment seg = null;
            bool airSegstatus = false;
            XmlDocument flightDetailsList = new XmlDocument();
            XmlDocument FareInfoList = new XmlDocument();
            XmlDocument BrandList = null;
            string fareRoues = string.Empty;
            GDSResModel.Fare fare = null;
            Dictionary<string, string> baggageDetais = new Dictionary<string, string>();
            Dictionary<string, string> fareRuleInfo = new Dictionary<string, string>();

            GDSResModel.Leg leg = null;
            GDSResModel.PaxFare paxFare = new GDSResModel.PaxFare();
            GDSResModel.FareDetail fareDetails = new GDSResModel.FareDetail();
            bool IsDomestic = true;

            //string filePath = "C:\\Users\\Jet\\Desktop\\1072024_1.xml";
            //string lowFareResponse = File.ReadAllText(filePath);
            //List<Segment> data =  GetSegmentList(lowFareResponse);
            XmlDocument doc = new XmlDocument();
            XmlDocument airSegmentList = new XmlDocument();
            doc.LoadXml(lowFareResponse);
            //doc.LoadXml(lowFareResponse.Replace("C11", "CNN"));
            GDSResModel.Bond bond = new GDSResModel.Bond();
            bond.Legs = new List<GDSResModel.Leg>();
            //cancelTime = new FareCancellationTime();
            foreach (XmlNode rootNode in doc)
            {
                if (rootNode.Name.Equals("SOAP:Envelope", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (XmlNode root in rootNode.ChildNodes)
                    {
                        if (root.Name.Equals("SOAP:Body", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (XmlNode lowFareSearchRes in root)
                            {
                                if (lowFareSearchRes.Name.Equals("air:LowFareSearchRsp", StringComparison.OrdinalIgnoreCase))
                                {
                                    TPtransactionId = lowFareSearchRes.Attributes["TransactionId"].InnerText;
                                    foreach (XmlNode airPricingSoulution in lowFareSearchRes)
                                    {
                                        if (airPricingSoulution.Name.Equals("air:FlightDetailsList", StringComparison.OrdinalIgnoreCase))
                                        {
                                            flightDetailsList = new XmlDocument();
                                            flightDetailsList.LoadXml(airPricingSoulution.OuterXml);
                                        }
                                        if (airPricingSoulution.Name.Equals("air:RouteList", StringComparison.OrdinalIgnoreCase))
                                        {
                                            foreach (XmlNode routelist in airPricingSoulution)
                                            {
                                                if (routelist.Name.Equals("air:Route", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    foreach (XmlNode airroute in routelist)
                                                    {

                                                        if (airroute.Name.Equals("air:Leg", StringComparison.OrdinalIgnoreCase) && airroute.Attributes["Group"].Value.Equals("0"))//&& RQ.SearchDetails.Count == 1
                                                        {
                                                            outBoundGroup = airroute.Attributes["Group"].Value;
                                                        }
                                                        else if (airroute.Name.Equals("air:Leg", StringComparison.OrdinalIgnoreCase))// && RQ.SearchDetails.Count == 2 
                                                        {
                                                            if (airroute.Attributes["Group"].Value.Equals("0"))
                                                            {
                                                                outBoundGroup = airroute.Attributes["Group"].Value;
                                                            }
                                                            else if (airroute.Attributes["Group"].Value.Equals("1"))
                                                            {
                                                                inBoundGroup = airroute.Attributes["Group"].Value;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (airPricingSoulution.Name.Equals("air:FareInfoList", StringComparison.OrdinalIgnoreCase))
                                        {
                                            FareInfoList = new XmlDocument();
                                            FareInfoList.LoadXml(airPricingSoulution.OuterXml);
                                        }
                                        #region AirPricingSolution
                                        if (airPricingSoulution.Name.Equals("air:AirPricingSolution", StringComparison.OrdinalIgnoreCase))
                                        {
                                            fare = new GDSResModel.Fare();
                                            fareRoues = string.Empty;
                                            baggageDetais = new Dictionary<string, string>();
                                            fareRuleInfo = new Dictionary<string, string>();
                                            decimal TMarkup = 0;
                                            if (airPricingSoulution.Attributes["TotalPrice"].Value.Contains("INR"))
                                            {
                                                fare.TotalFareWithOutMarkUp = Convert.ToDecimal(airPricingSoulution.Attributes["TotalPrice"].Value.Remove(0, 3));
                                                if (fare.TotalFareWithOutMarkUp == 16839)
                                                {

                                                }
                                            }
                                            else
                                            {
                                                fare.TotalFareWithOutMarkUp = Convert.ToDecimal(airPricingSoulution.Attributes["ApproximateTotalPrice"].Value.Remove(0, 3));
                                            }
                                            if (airPricingSoulution.Attributes["BasePrice"].Value.Contains("INR"))
                                            {
                                                fare.BasicFare = Convert.ToDecimal(airPricingSoulution.Attributes["BasePrice"].Value.Remove(0, 3));
                                            }
                                            else
                                            {
                                                fare.BasicFare = Convert.ToDecimal(airPricingSoulution.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                            }

                                            if (airPricingSoulution.Attributes["Taxes"].Value.Contains("INR"))
                                            {
                                                //TMarkup = GetMarkup(Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)), Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)), Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)));
                                                fare.TotalTaxWithOutMarkUp = Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)) + TMarkup;

                                            }
                                            else
                                            {
                                                //TMarkup = GetMarkup(Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)), Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)), Convert.ToDecimal(airPricingSoulution.Attributes["Taxes"].Value.Remove(0, 3)));
                                                fare.TotalTaxWithOutMarkUp = Convert.ToDecimal(airPricingSoulution.Attributes["ApproximateTaxes"].Value.Remove(0, 3)) + TMarkup;
                                            }
                                            fare.PaxFares = new List<GDSResModel.PaxFare>();
                                            foreach (XmlNode lowfarepric in airPricingSoulution)
                                            {
                                                switch (lowfarepric.Name)
                                                {
                                                    case "air:Journey":

                                                        bond = new GDSResModel.Bond();
                                                        bond.Legs = new List<GDSResModel.Leg>();
                                                        int stop = 0;
                                                        journeyIndex = 0;
                                                        airSegstatus = false;
                                                        _journeyTime = null;
                                                        foreach (XmlNode airSegmentKeys in lowfarepric)
                                                        {
                                                            if (airSegmentKeys.Name.Equals("air:AirSegmentRef", StringComparison.OrdinalIgnoreCase)) // connected
                                                            {
                                                                leg = new GDSResModel.Leg();
                                                                if (airSegmentList != null && !string.IsNullOrEmpty(airSegmentList.InnerXml))
                                                                {
                                                                    foreach (XmlNode airSegmentlist in airSegmentList)
                                                                    {
                                                                        if (airSegmentlist.Name.Equals("air:AirSegmentList", StringComparison.OrdinalIgnoreCase))
                                                                        {
                                                                            foreach (XmlNode airSegment in airSegmentlist)
                                                                            {
                                                                                if (airSegment.Name.Equals("air:AirSegment", StringComparison.OrdinalIgnoreCase))
                                                                                {
                                                                                    //connected check <air:AirSegmentRef Key="eAo/mASqWDKAFoxI3AAAAA=="/>
                                                                                    if (airSegment.Attributes["Key"].Value.Equals(airSegmentKeys.Attributes["Key"].InnerText, StringComparison.Ordinal))
                                                                                    {
                                                                                        if (!airSegstatus && IsFlex)
                                                                                        {
                                                                                            //journeyIndex = depDate.Subtract(Convert.ToDateTime(airSegment.Attributes["DepartureTime"].Value).Date).Days;
                                                                                            //journeyIndex = depDate.DayOfYear - (Convert.ToDateTime(airSegment.Attributes["DepartureTime"].Value.Split('+')[0])).DayOfYear;
                                                                                            airSegstatus = true;
                                                                                        }
                                                                                        if (airSegment.Attributes["Group"].Value.Equals(outBoundGroup, StringComparison.OrdinalIgnoreCase))
                                                                                        {
                                                                                            leg.BoundType = "OutBound";
                                                                                            leg.Group = outBoundGroup;
                                                                                            bond.BoundType = "OutBound";
                                                                                        }
                                                                                        else if (airSegment.Attributes["Group"].Value.Equals(inBoundGroup, StringComparison.OrdinalIgnoreCase))
                                                                                        {
                                                                                            leg.BoundType = "InBound";
                                                                                            leg.Group = inBoundGroup;
                                                                                            bond.BoundType = "InBound";
                                                                                        }

                                                                                        if (airSegment.Attributes["NumberOfStops"] != null)
                                                                                        {
                                                                                            leg.NumberOfStops = airSegment.Attributes["NumberOfStops"].Value;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            leg.NumberOfStops = "0";
                                                                                        }
                                                                                        leg.FlightNumber = airSegment.Attributes["FlightNumber"].Value;
                                                                                        leg.AirlineName = airSegment.Attributes["Carrier"].Value;
                                                                                        leg.CarrierCode = airSegment.Attributes["Carrier"].Value;
                                                                                        leg.Origin = airSegment.Attributes["Origin"].Value;
                                                                                        leg.Destination = airSegment.Attributes["Destination"].Value;
                                                                                        leg.DepartureDate = airSegment.Attributes["DepartureTime"].Value.Split('T')[0];
                                                                                        leg.DepartureTime = airSegment.Attributes["DepartureTime"].Value.Split('T')[1];
                                                                                        leg.ArrivalDate = airSegment.Attributes["ArrivalTime"].Value.Split('T')[0];
                                                                                        leg.ArrivalTime = airSegment.Attributes["ArrivalTime"].Value.Split('T')[1];
                                                                                        leg.Duration = airSegment.Attributes["FlightTime"].Value;
                                                                                        leg.AircraftCode = airSegmentKeys.Attributes["Key"].InnerText;
                                                                                        switch (leg.AirlineName)
                                                                                        {
                                                                                            case "AI":
                                                                                                leg.FlightName = "AirIndia";
                                                                                                break;
                                                                                            case "9W":
                                                                                                leg.FlightName = "JetAirWays";
                                                                                                break;
                                                                                            case "UK":
                                                                                                leg.FlightName = "Vistara";
                                                                                                break;
                                                                                            case "OD":
                                                                                                leg.FlightName = "MALINDO AIRWAYS";
                                                                                                break;
                                                                                            case "TG":
                                                                                                leg.FlightName = "Thai Airways International";
                                                                                                break;
                                                                                            case "UL":
                                                                                                leg.FlightName = "SriLankan Airlines";
                                                                                                break;
                                                                                            default:
                                                                                                try
                                                                                                {
                                                                                                    leg.FlightName = "";// Utility.FileChangeMonitor.AirlineNames[leg.AirlineName.Trim()];
                                                                                                }
                                                                                                catch (SystemException sex_) { leg.FlightName = leg.AirlineName.Trim(); }
                                                                                                break;
                                                                                        }
                                                                                        foreach (XmlNode airSegmentChild in airSegment)
                                                                                        {
                                                                                            switch (airSegmentChild.Name)
                                                                                            {
                                                                                                case "air:AirAvailInfo":
                                                                                                    leg.ProviderCode = airSegmentChild.Attributes["ProviderCode"].Value;
                                                                                                    break;
                                                                                                case "air:FlightDetailsRef":
                                                                                                    leg.FlightDetailRefKey = airSegmentChild.Attributes["Key"].Value;
                                                                                                    if (flightDetailsList != null && !string.IsNullOrEmpty(flightDetailsList.InnerXml))
                                                                                                    {
                                                                                                        foreach (XmlNode flightDetaillist_ in flightDetailsList)
                                                                                                        {
                                                                                                            foreach (XmlNode flightDetails in flightDetaillist_)
                                                                                                            {
                                                                                                                if (flightDetails.Name.Equals("air:FlightDetails", StringComparison.OrdinalIgnoreCase) && flightDetails.Attributes["Key"].Value.Equals(airSegmentChild.Attributes["Key"].Value, StringComparison.Ordinal))
                                                                                                                {
                                                                                                                    if ((flightDetails.Attributes["OriginTerminal"]) != null)
                                                                                                                    {
                                                                                                                        leg.DepartureTerminal = flightDetails.Attributes["OriginTerminal"].Value;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        leg.DepartureTerminal = string.Empty;
                                                                                                                    }
                                                                                                                    if ((flightDetails.Attributes["DestinationTerminal"]) != null)
                                                                                                                    {
                                                                                                                        leg.ArrivalTerminal = flightDetails.Attributes["DestinationTerminal"].Value;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        leg.ArrivalTerminal = string.Empty;
                                                                                                                    }
                                                                                                                    _journeyTime = lowfarepric.Attributes["TravelTime"].Value;
                                                                                                                    if (!string.IsNullOrWhiteSpace(_journeyTime) && string.IsNullOrWhiteSpace(bond.JourneyTime))
                                                                                                                    {
                                                                                                                        bond.JourneyTime = (Convert.ToInt32(_journeyTime.Substring(1, 1)) * 24 * 60 + Convert.ToInt32((_journeyTime.Substring(_journeyTime.IndexOf('T') + 1, _journeyTime.IndexOf('H') - _journeyTime.IndexOf('T') - 1))) * 60 + Convert.ToInt32(_journeyTime.Substring(_journeyTime.IndexOf('H') + 1, _journeyTime.LastIndexOf('M') - _journeyTime.LastIndexOf('H') - 1))).ToString();
                                                                                                                    }
                                                                                                                    break;
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    break;
                                                                                            }
                                                                                        }
                                                                                        bond.Legs.Add(leg);
                                                                                        stop++;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        listOfBound.Add(bond);
                                                        break;
                                                    case "air:AirPricingInfo":
                                                        paxFare = new GDSResModel.PaxFare();
                                                        paxFare.Fare = new List<GDSResModel.FareDetail>();
                                                        if (lowfarepric.Attributes["Refundable"] != null)
                                                        {
                                                            paxFare.Refundable = bool.Parse(lowfarepric.Attributes["Refundable"].Value);
                                                        }
                                                        foreach (XmlNode airPricingInfo in lowfarepric)
                                                        {
                                                            if (airPricingInfo.Name.Equals("air:BookingInfo", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                if (airPricingInfo.Attributes["CabinClass"] != null)
                                                                {
                                                                    paxFare.FareBasisCode = airPricingInfo.Attributes["CabinClass"].Value;
                                                                }
                                                            }
                                                            if (airPricingInfo.Name.Equals("air:TaxInfo", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                if (airPricingInfo.Attributes["Category"] != null && airPricingInfo.Attributes["Amount"] != null)
                                                                {
                                                                    fareDetails = new GDSResModel.FareDetail();
                                                                    fareDetails.Amount = decimal.Parse(airPricingInfo.Attributes["Amount"].Value.Remove(0, 3));
                                                                    switch (airPricingInfo.Attributes["Category"].Value)
                                                                    {
                                                                        case "IN":
                                                                            fareDetails.ChargeCode = "UDF";
                                                                            fareDetails.ChargeDetail = "USER DEVELOPMENT FEE";
                                                                            break;
                                                                        case "JN":
                                                                            fareDetails.ChargeCode = "ST";
                                                                            fareDetails.ChargeDetail = "SERVICE TAX";
                                                                            break;
                                                                        case "WO":
                                                                            fareDetails.ChargeCode = "PSF";
                                                                            fareDetails.ChargeDetail = "PASSENGER SERVICE FEE";
                                                                            break;
                                                                        case "YM":
                                                                            fareDetails.ChargeCode = "ADF";
                                                                            fareDetails.ChargeDetail = "AIRPORT DEVELOPMENT FEE";
                                                                            break;
                                                                        case "YQ":
                                                                            fareDetails.ChargeCode = "YQ";
                                                                            fareDetails.ChargeDetail = "Fuel Expenses";
                                                                            break;
                                                                        case "YR":
                                                                            fareDetails.ChargeCode = "YR";
                                                                            fareDetails.ChargeDetail = "Fuel Expenses";
                                                                            break;
                                                                        default:
                                                                            fareDetails.ChargeCode = airPricingInfo.Attributes["Category"].Value;
                                                                            fareDetails.ChargeDetail = airPricingInfo.Attributes["Category"].Value;
                                                                            break;
                                                                    }
                                                                    paxFare.Fare.Add(fareDetails);
                                                                }
                                                            }


                                                            if (airPricingInfo.Name.Equals("air:PassengerType", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                switch (airPricingInfo.Attributes["Code"].Value)
                                                                {
                                                                    case "ADT":
                                                                        if (lowfarepric.Attributes["BasePrice"].Value.Contains("INR"))
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["BasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        else
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        paxFare.TotalTax = Convert.ToDecimal(lowfarepric.Attributes["Taxes"].Value.Remove(0, 3));
                                                                        paxFare.PaxType = GDSResModel.PAXTYPE.ADT;
                                                                        //if (AdultMarkUp != 0 && MarkUpType == MarkUP.Flat)
                                                                        //{
                                                                        //    paxFare.TotalTax += AdultMarkUp;
                                                                        //    paxFare.Fare[0].Amount += AdultMarkUp;
                                                                        //}
                                                                        //if (MarkUpType == MarkUP.Percentage && AdultMarkUp != 0)
                                                                        //{
                                                                        //    paxFare.TotalTax += (paxFare.TotalTax * (AdultMarkUp / 100));
                                                                        //    paxFare.Fare[0].Amount += (paxFare.TotalTax * (AdultMarkUp / 100));
                                                                        //}
                                                                        break;
                                                                    case "CNN":
                                                                        if (lowfarepric.Attributes["BasePrice"].Value.Contains("INR"))
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["BasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        else
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        paxFare.TotalTax = Convert.ToDecimal(lowfarepric.Attributes["Taxes"].Value.Remove(0, 3));
                                                                        paxFare.PaxType = GDSResModel.PAXTYPE.CHD;
                                                                        //if (ChildMarkUp != 0 && MarkUpType == MarkUP.Flat)
                                                                        //{
                                                                        //    paxFare.TotalTax += ChildMarkUp;
                                                                        //    paxFare.Fare[0].Amount += ChildMarkUp;
                                                                        //}
                                                                        //if (MarkUpType == MarkUP.Percentage && ChildMarkUp != 0)
                                                                        //{
                                                                        //    paxFare.TotalTax += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //    paxFare.Fare[0].Amount += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //}
                                                                        break;
                                                                    case "C11":
                                                                        if (lowfarepric.Attributes["BasePrice"].Value.Contains("INR"))
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["BasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        else
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        paxFare.TotalTax = Convert.ToDecimal(lowfarepric.Attributes["Taxes"].Value.Remove(0, 3));
                                                                        paxFare.PaxType = GDSResModel.PAXTYPE.CHD;
                                                                        //if (ChildMarkUp != 0 && MarkUpType == MarkUP.Flat)
                                                                        //{
                                                                        //    paxFare.TotalTax += ChildMarkUp;
                                                                        //    paxFare.Fare[0].Amount += ChildMarkUp;
                                                                        //}
                                                                        //if (MarkUpType == MarkUP.Percentage && ChildMarkUp != 0)
                                                                        //{
                                                                        //    paxFare.TotalTax += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //    paxFare.Fare[0].Amount += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //}
                                                                        break;
                                                                    case "CHD":
                                                                        if (lowfarepric.Attributes["BasePrice"].Value.Contains("INR"))
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["BasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        else
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        paxFare.TotalTax = Convert.ToDecimal(lowfarepric.Attributes["Taxes"].Value.Remove(0, 3));
                                                                        paxFare.PaxType = GDSResModel.PAXTYPE.CHD;
                                                                        //if (ChildMarkUp != 0 && MarkUpType == MarkUP.Flat)
                                                                        //{
                                                                        //    paxFare.TotalTax += ChildMarkUp;
                                                                        //    paxFare.Fare[0].Amount += ChildMarkUp;
                                                                        //}
                                                                        //if (MarkUpType == MarkUP.Percentage && ChildMarkUp != 0)
                                                                        //{
                                                                        //    paxFare.TotalTax += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //    paxFare.Fare[0].Amount += (paxFare.TotalTax * (ChildMarkUp / 100));
                                                                        //}
                                                                        break;
                                                                    case "INF":
                                                                        if (lowfarepric.Attributes["BasePrice"].Value.Contains("INR"))
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["BasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        else
                                                                        {
                                                                            paxFare.BasicFare = Convert.ToDecimal(lowfarepric.Attributes["ApproximateBasePrice"].Value.Remove(0, 3));
                                                                        }
                                                                        paxFare.TotalTax = Convert.ToDecimal(lowfarepric.Attributes["Taxes"].Value.Remove(0, 3));
                                                                        paxFare.PaxType = GDSResModel.PAXTYPE.INF;
                                                                        //if (InfantMarkUp != 0 && MarkUpType == MarkUP.Flat)
                                                                        //{
                                                                        //    paxFare.TotalTax += InfantMarkUp;
                                                                        //    paxFare.Fare[0].Amount += InfantMarkUp;
                                                                        //}
                                                                        //if (MarkUpType == MarkUP.Percentage && InfantMarkUp != 0)
                                                                        //{
                                                                        //    paxFare.TotalTax += (paxFare.TotalTax * (InfantMarkUp / 100));
                                                                        //    paxFare.Fare[0].Amount += (paxFare.TotalTax * (InfantMarkUp / 100));
                                                                        //}
                                                                        break;
                                                                }
                                                            }
                                                            if (airPricingInfo.Name.Equals("air:ChangePenalty", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                foreach (XmlNode changePenalty in airPricingInfo)
                                                                {
                                                                    if (changePenalty.Name.Equals("air:Amount", StringComparison.OrdinalIgnoreCase))
                                                                    {
                                                                        paxFare.ChangePenalty = Convert.ToDecimal(changePenalty.InnerText.Remove(0, 3)) * ((GDSResModel.Bond)listOfBound[0]).Legs.Count;
                                                                    }
                                                                }
                                                            }
                                                            if (airPricingInfo.Name.Equals("air:CancelPenalty", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                foreach (XmlNode cancelPenalty in airPricingInfo)
                                                                {
                                                                    if (cancelPenalty.Name.Equals("air:Amount", StringComparison.OrdinalIgnoreCase))
                                                                    {
                                                                        paxFare.CancelPenalty = Convert.ToDecimal(cancelPenalty.InnerText.Remove(0, 3)) * ((GDSResModel.Bond)listOfBound[0]).Legs.Count;
                                                                        if (paxFare.CancelPenalty == 0)
                                                                        {
                                                                            paxFare.Refundable = false;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //   if (airPricingInfo.Name.Equals("air:MiniFareRules", StringComparison.OrdinalIgnoreCase))
                                                            if (airPricingInfo.Name.Equals("air:FareRulesFilter", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                foreach (XmlNode miniFareRules in airPricingInfo)
                                                                {
                                                                    if (miniFareRules.Name.Equals("air:Refundability", StringComparison.OrdinalIgnoreCase))
                                                                    {
                                                                        if (miniFareRules.Attributes["Value"] != null)
                                                                        {
                                                                            if (miniFareRules.Attributes["Value"].Value.Equals("NonRefundable", StringComparison.OrdinalIgnoreCase))
                                                                            {
                                                                                paxFare.Refundable = false;
                                                                            }
                                                                            if (string.IsNullOrWhiteSpace(fareRoues))
                                                                            {
                                                                                fareRoues = miniFareRules.Attributes["Value"].Value;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (airPricingInfo.Name.Equals("air:FareInfoRef", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                if (FareInfoList != null && !string.IsNullOrEmpty(FareInfoList.InnerXml))
                                                                {
                                                                    foreach (XmlNode fareInfoList in FareInfoList)
                                                                    {
                                                                        if (fareInfoList.Name.Equals("air:FareInfoList", StringComparison.OrdinalIgnoreCase))
                                                                        {
                                                                            foreach (XmlNode fareInfo in fareInfoList)
                                                                            {
                                                                                if (fareInfo.Attributes["Key"].Value.Equals(airPricingInfo.Attributes["Key"].Value, StringComparison.Ordinal))
                                                                                {
                                                                                    //GetBrandDetails
                                                                                    string _BrandDesc = string.Empty;
                                                                                    try
                                                                                    {
                                                                                        #region GetBrandDetails
                                                                                        foreach (XmlNode BrandInfo in fareInfo)
                                                                                        {
                                                                                            if (BrandInfo.Name.Equals("air:Brand", StringComparison.OrdinalIgnoreCase))
                                                                                            {
                                                                                                if (BrandInfo.Attributes["BrandID"] != null)
                                                                                                {
                                                                                                    //string _BrandId = baggageAllowance.Attributes["BrandID"].Value;
                                                                                                    if (BrandList != null)
                                                                                                    {
                                                                                                        if (BrandList != null && !string.IsNullOrEmpty(BrandList.InnerXml))
                                                                                                        {
                                                                                                            foreach (XmlNode brandLists in BrandList)
                                                                                                            {
                                                                                                                if (brandLists.Name.Equals("air:BrandList", StringComparison.OrdinalIgnoreCase))
                                                                                                                {
                                                                                                                    foreach (XmlNode brand in brandLists)
                                                                                                                    {
                                                                                                                        if (brand.Attributes["BrandID"].Value.Equals(BrandInfo.Attributes["BrandID"].Value, StringComparison.OrdinalIgnoreCase))
                                                                                                                        {
                                                                                                                            foreach (XmlNode brandText in brand)
                                                                                                                            {
                                                                                                                                if (brandText.Name.Equals("air:Text", StringComparison.OrdinalIgnoreCase) && brandText.Attributes["Type"].Value.Equals("MarketingAgent", StringComparison.OrdinalIgnoreCase))
                                                                                                                                {
                                                                                                                                    _BrandDesc = brandText.InnerText;

                                                                                                                                }
                                                                                                                            }

                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        foreach (XmlNode brandList in lowFareSearchRes)
                                                                                                        {
                                                                                                            if (brandList.Name.Equals("air:BrandList", StringComparison.OrdinalIgnoreCase))
                                                                                                            {
                                                                                                                BrandList = new XmlDocument();
                                                                                                                BrandList.LoadXml(brandList.OuterXml);
                                                                                                                if (BrandList != null && !string.IsNullOrEmpty(BrandList.InnerXml))
                                                                                                                {
                                                                                                                    foreach (XmlNode brandLists in BrandList)
                                                                                                                    {
                                                                                                                        if (brandLists.Name.Equals("air:BrandList", StringComparison.OrdinalIgnoreCase))
                                                                                                                        {
                                                                                                                            foreach (XmlNode brand in brandLists)
                                                                                                                            {
                                                                                                                                if (brand.Attributes["BrandID"].Value.Equals(BrandInfo.Attributes["BrandID"].Value, StringComparison.OrdinalIgnoreCase))
                                                                                                                                {
                                                                                                                                    foreach (XmlNode brandText in brand)
                                                                                                                                    {
                                                                                                                                        if (brandText.Name.Equals("air:Text", StringComparison.OrdinalIgnoreCase) && brandText.Attributes["Type"].Value.Equals("MarketingAgent", StringComparison.OrdinalIgnoreCase))
                                                                                                                                        {
                                                                                                                                            _BrandDesc = brandText.InnerText;

                                                                                                                                        }
                                                                                                                                    }

                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }

                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        #endregion
                                                                                    }
                                                                                    catch
                                                                                    {

                                                                                    }
                                                                                    //air:BaggageAllowance>
                                                                                    foreach (XmlNode baggageAllowance in fareInfo)
                                                                                    {
                                                                                        if (baggageAllowance.Name.Equals("air:BaggageAllowance", StringComparison.OrdinalIgnoreCase))
                                                                                        {
                                                                                            foreach (XmlNode maxWeight in baggageAllowance)
                                                                                            {
                                                                                                if (maxWeight.Name.Equals("air:MaxWeight", StringComparison.OrdinalIgnoreCase))
                                                                                                {
                                                                                                    if (maxWeight.Attributes["Value"] != null)
                                                                                                    {
                                                                                                        paxFare.BaggageWeight = maxWeight.Attributes["Value"].Value;
                                                                                                        paxFare.BaggageUnit = "KG";// maxWeight.Attributes["Unit"].Value;
                                                                                                        if (!baggageDetais.ContainsKey(fareInfo.Attributes["Key"].Value))
                                                                                                            baggageDetais.Add(fareInfo.Attributes["Key"].Value, maxWeight.Attributes["Value"].Value + "|" + "KG|" + _BrandDesc);

                                                                                                    }
                                                                                                    break;
                                                                                                }
                                                                                                if (maxWeight.Name.Equals("air:NumberOfPieces", StringComparison.OrdinalIgnoreCase))
                                                                                                {
                                                                                                    paxFare.BaggageWeight = maxWeight.InnerText;
                                                                                                    paxFare.BaggageUnit = "PC";
                                                                                                    if (!baggageDetais.ContainsKey(fareInfo.Attributes["Key"].Value))
                                                                                                        baggageDetais.Add(fareInfo.Attributes["Key"].Value, maxWeight.InnerText + "|" + "PC|" + _BrandDesc);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (baggageAllowance.Name.Equals("air:FareRuleKey", StringComparison.OrdinalIgnoreCase))
                                                                                        {
                                                                                            paxFare.FareInfoKey = baggageAllowance.Attributes["FareInfoRef"].Value;
                                                                                            paxFare.FareInfoValue = baggageAllowance.InnerText;
                                                                                            if (!fareRuleInfo.ContainsKey(baggageAllowance.Attributes["FareInfoRef"].Value))
                                                                                                fareRuleInfo.Add(baggageAllowance.Attributes["FareInfoRef"].Value, baggageAllowance.InnerText);
                                                                                        }
                                                                                    }
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (airPricingInfo.Name.Equals("air:BookingInfo", StringComparison.OrdinalIgnoreCase))
                                                            {
                                                                foreach (GDSResModel.Bond bond_ in listOfBound)
                                                                {
                                                                    foreach (GDSResModel.Leg leg_ in bond_.Legs)
                                                                    {
                                                                        if (leg_.AircraftCode.Equals(airPricingInfo.Attributes["SegmentRef"].Value, StringComparison.Ordinal))
                                                                        {
                                                                            leg_.FareClassOfService = airPricingInfo.Attributes["BookingCode"].Value;
                                                                            leg_.AvailableSeat = airPricingInfo.Attributes["BookingCount"].Value;
                                                                            leg_.Cabin = airPricingInfo.Attributes["CabinClass"].Value;
                                                                            leg_.FareRulesKey = airPricingInfo.Attributes["FareInfoRef"].Value;
                                                                            leg_.FareRulesValue = fareRuleInfo[airPricingInfo.Attributes["FareInfoRef"].Value];
                                                                            if (baggageDetais != null && baggageDetais.ContainsKey(airPricingInfo.Attributes["FareInfoRef"].Value))
                                                                            {
                                                                                if (baggageDetais[airPricingInfo.Attributes["FareInfoRef"].Value].Split('|').Length == 3)
                                                                                {
                                                                                    leg_.BaggageWeight = baggageDetais[airPricingInfo.Attributes["FareInfoRef"].Value].Split('|')[0];
                                                                                    leg_.BaggageUnit = baggageDetais[airPricingInfo.Attributes["FareInfoRef"].Value].Split('|')[1];
                                                                                    if (!IsDomestic)
                                                                                        leg_.Remarks = baggageDetais[airPricingInfo.Attributes["FareInfoRef"].Value].Split('|')[2];
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                leg_.BaggageWeight = "15";
                                                                                leg_.BaggageUnit = "KG";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (paxFare.TotalFare == 0)
                                                        {
                                                            paxFare.TotalFare = paxFare.TotalTax + paxFare.BasicFare;
                                                        }
                                                        fare.PaxFares.Add(paxFare);
                                                        break;
                                                }
                                            }
                                        }
                                        #endregion airPricingSolution
                                        if (airPricingSoulution.Name.Equals("air:AirSegmentList", StringComparison.OrdinalIgnoreCase))
                                        {
                                            airSegmentList.LoadXml((airPricingSoulution).OuterXml);
                                        }
                                        if (listOfBound.Count > 0)
                                        {
                                            seg = new GDSResModel.Segment();
                                            seg.Bonds = new List<GDSResModel.Bond>();
                                            seg.Fare = new GDSResModel.Fare();
                                            seg.Fare = fare;
                                            seg.SegIndex = contractId.ToString();
                                            seg.JourneyIndex = journeyIndex;
                                            seg.NearByAirport = "";// IsNearByAirport;
                                            seg.FareIndicator = "";// FareIndicator;
                                            if (fareRoues == "")
                                            {
                                                seg.FareRule = "RefundableWithPenalty";
                                            }
                                            else
                                            {
                                                seg.FareRule = fareRoues;
                                            }

                                            //  seg.BondType = "OutBound";
                                            seg.EngineID = "";// AirService.Engine.TravelPort;
                                            seg.ItineraryKey = TPtransactionId;
                                            seg.SearchId = "";// EngineID;
                                            seg.IsSpecial = "";// IsSpecial;
                                            seg.JourneyType = "";// JourneyType;
                                            if (contractType_.Equals("RoundTrip"))
                                            {
                                                seg.IsRoundTrip = true;
                                                seg.BondType = "OutBound";
                                            }
                                            else
                                            {
                                                seg.BondType = contractType_;
                                            }
                                            foreach (GDSResModel.Bond bond_ in listOfBound)
                                            {
                                                seg.Bonds.Add(bond_);
                                            }
                                            if (seg.Fare.PaxFares != null && seg.Fare.PaxFares.Count > 0)
                                            {
                                                int tmfrom = 4;
                                                int tmto = 0;
                                                // cancelTime = CancellationCharges.GetCancellationTime();
                                                //foreach (AirLine ar in cancelTime.AirLines)
                                                //{
                                                //    if (seg.Bonds[0].Legs[0].AirlineName == ar.Code)
                                                //    {
                                                //        foreach (AirlineClass ac in ar.AirlineClasses)
                                                //        {
                                                //            if (seg.Bonds[0].Legs[0].FareClassOfService == ac.Type)
                                                //            {
                                                //                //airCodeMatch = true;
                                                //                tmfrom = Convert.ToInt32(ac.TimeFrom);
                                                //                tmto = Convert.ToInt32(ac.TimeTo);
                                                //            }
                                                //        }
                                                //    }
                                                //}
                                                //CheckFareRule(seg); // to do

                                                if (paxFare.CancelPenalty == 0)
                                                {
                                                    paxFare.Refundable = false;
                                                    seg.FareRule = "CAN-BEF " + tmfrom + "_" + tmto + ":" + seg.Fare.PaxFares[0].CancelPenalty + "|" + "CHG-BEF " + tmfrom + "_" + tmto + ":" + seg.Fare.PaxFares[0].ChangePenalty + "|" + "EMTFee-" + 250 + "|" + "Msg:As per as airline rules.";
                                                }

                                                else
                                                {
                                                    seg.FareRule = "CAN-BEF " + tmfrom + "_" + tmto + ":" + seg.Fare.PaxFares[0].CancelPenalty + "|" + "CHG-BEF " + tmfrom + "_" + tmto + ":" + seg.Fare.PaxFares[0].ChangePenalty + "|" + "EMTFee-" + 250;
                                                }
                                            }
                                            if (IsFlex)
                                            {
                                                if (journeyIndex == 0)
                                                {
                                                    listOfSegment.Add(seg);
                                                }
                                            }
                                            else
                                            {
                                                listOfSegment.Add(seg);
                                            }
                                            bond = new GDSResModel.Bond();
                                            listOfBound = new ArrayList();
                                            bond.Legs = new List<GDSResModel.Leg>();
                                            contractId++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return listOfSegment;
        }
    }
}