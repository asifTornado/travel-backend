using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backEnd.services;


namespace backEnd.Models;



public class Hierarchy{

[Key]
public int Id { get; set; }
public User? User {get; set;}
public User? Supervisor {get; set;}
public User? TravelHandler {get; set;} 
public User? ZonalHead {get; set;}

 





}


