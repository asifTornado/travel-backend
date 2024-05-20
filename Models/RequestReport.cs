using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using backEnd.Models.IModels;

namespace backEnd.Models;


public class RequestReport {
    public float AirTicket {get; set;} = 0;
    public float Hotel {get; set;} = 0;
    public float Transport {get; set;} = 0;
    public float Incidental {get; set;} = 0;
    public float Miscellaneuous {get; set;} = 0;
    public float Total {get; set;} = 0;
}