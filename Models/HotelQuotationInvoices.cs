using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Models;


public class HotelQuotationInvoices{

    
    [Key]
    public int Id {get; set;}

    public int InvoiceId { get; set; }
    public int QuotationId { get; set; }

    public virtual HotelQuotation? Quotation {get; set;}

    public virtual HotelInvoice? Invoice {get; set;}

}
    



