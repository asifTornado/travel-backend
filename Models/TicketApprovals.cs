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


public class TicketApprovals{

    
    [Key]
    public int Id {get; set;}

    public int UserId { get; set; }
    public int QuotationId { get; set; }

    public virtual Quotation? Quotation {get; set;}

    public virtual User? User {get; set;}

}
    



