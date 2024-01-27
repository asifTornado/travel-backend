
namespace backEnd.Mappings;

public static class Events {
    public static string SupervisorApprovalTicket {get;} = "Seeking Supervisor Approval For Air Ticket";
    public static string SupervisorApprovedTicket {get;} = "Supervisor Approved Air Ticket";
    public static string SupervisorApprovalTrip {get;} = "Supervisor Approval For Trip";
    public static string SupervisorRejectedTicket {get;} = "Supervisor Rejected Air Ticket";
    public static string SupervisorApprovalHotel {get;} = "Seeking Supervisor Approval For Hotel";
    public static string SupervisorApprovedHotel {get;} = "Supervisor Approved Hotel";
    public static string SupervisorRejectHotel {get;} = "Supervisor Rejected Hotel";
    public static string ZonalHeadApproval {get;} = "Seeking Zonal Head's Approval For Travel Request";
    public static string ZonalHeadApproved {get;} = "Zonal Head Approved Travel Request";
    public static string ZonalHeadReject {get;} = "Zonal Head Rejected Travel Request";
    public static string RequestRaised {get;} = "Travel Request Raised";
    public static string QuotationSent {get;} = "Quotation Sent";
    public static string HotelQuotationSent {get;} = "Hotel Quotation Sent";


    public static string QuotationBooked {get;} = "Quotation Booked";

    public static string QuotationUnbooked {get;} = "Quotation Unbooked";

    public static string HotelQuotationBooked {get;} = "Hotel Quotation Booked";

    public static string HotelQuotationUnbooked {get;} = "Hotel Quotation Unbooked";

    public static string QuotationConfirmed {get;} = "Quotation Confirmed";

    public static string QuotationRevoked {get;} = "Quotation Revoked";


    public static string HotelQuotationConfirmed {get;} = "Hotel Quotation Confirmed";

    public static string HotelQuotationRevoked {get;} = "Hotel Quotation Revoked";


    public static string AirTicketInvoiceSent {get;} = "Air Ticket Invoice Sent";

    public static string HotelInvoiceSent {get;} = "Hotel Invocie Sent";


    public static string Processed {get;} = "Request Processing Complete";

    public static string MailedAccountsAndAudit {get;} = "Invoices Mailed To Accounts and Audit";

    public static string GiveInfo {get;} = "Essential information submitted by traveler";

    public static string PermanentlyRejected {get;} = "Permanently rejected a request";


    public static string TripRejected {get;} = "Your trip was rejected by your supervisor";

    public static string DepartmentHeadReject {get;} = "Trip rejected by department head";

    public static string DepartmentHeadApprove {get;} = "Trip approved by department head";

    public static string DepartmentHeadPermanentlyReject {get;} = "Trip permanently rejected by department head";






    
}