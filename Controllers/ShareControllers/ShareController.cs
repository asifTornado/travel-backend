
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using System.Text.Json;
using backEnd.Helpers.IHelpers;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using MongoDB.Driver.Core.Authentication;
using Org.BouncyCastle.Ocsp;
using System.IO;
using MongoDB.Driver.Core.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Authorization;
using MailKit;
using AutoMapper;
using backEnd.Helpers;
using System.Security.AccessControl;
using backEnd.Services;
using Org.BouncyCastle.Asn1.X509;
using MimeKit.Encodings;
using Microsoft.VisualBasic;

namespace backEnd.Controllers.ShareControllers;


[Route("/")]
[ApiController]
public class ShareController: ControllerBase
{


private IConfiguration _configuration;
private IMailer _mailer;
public ShareController(IConfiguration configuration, IMailer mailer){
_configuration = configuration;
_mailer = mailer;
}


[HttpPost]
[Route("share")]

public async Task<IActionResult> Share(IFormCollection data){
    var request = JsonSerializer.Deserialize<Request>(data["request"]);
    var recipient = JsonSerializer.Deserialize<User>(data["recipient"]);
    var sharer = JsonSerializer.Deserialize<User>(data["sharer"]);
     

     string senderEmail = _configuration.GetValue<string>("Mail:Email");
     string password = _configuration.GetValue<string>("Mail:Password");
     string subject = $"{sharer.EmpName} has shared information about a travel request with you ";
     
     var message = new MimeMessage();
   message.From.Add(new MailboxAddress("", senderEmail));
    message.To.Add(new MailboxAddress("", recipient.MailAddress));
     var builder = new BodyBuilder();

     message.Subject = subject;
 
     builder.HtmlBody = $@"
     
       <!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Side-by-Side Cards</title>
    <style>
        body {{
            
            font-family: Arial, sans-serif;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            background-color: #f0f0f0;
        }}

        .container {{
            display: flex;
            justify-content: space-between;
            width: 80%;
            background-color: #2196f3;; /* Equivalent to color='indigo-darken-200' */
            border: 1px solid #000; /* Equivalent to border='1px solid #ddd' */
            margin-bottom: 20px; /* Equivalent to mb-[20px] */
            padding:20px
          
        }}

        .box {{
            flex: 1;
            padding: 10px;
            border: 1px solid #ddd;
            width:100%;
            background-color:white;
            padding:30px /* Equivalent to class='bg-blue-lighten-2' */
        }}

        .box:last-child {{
            margin-right: 0;
        }}

        .row {{
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
        }}

        .card {{
            padding: 10px;
        
            max-width: 100%;
            margin-bottom: 20px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Equivalent to elevated='2' */
            transition: box-shadow 0.3s ease-in-out;
        }}

        .card:hover {{
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2); /* Equivalent to hover */
        }}

        .card-title {{
            font-size: 1.25rem;
            margin-bottom: 10px;
            background-color: #2196f3; /* Equivalent to class='bg-blue-500 text-white' */
            padding: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3); /* Equivalent to elevation-6 */
            color: #fff;
            text-align: center;
        }}

        .pl-80 {{
            padding-left: 80px;
        }}

        .bg-green {{
            background-color: #4caf50; /* Equivalent to class='bg-green-500 text-white' */
            color: #fff;
        }}

        .text-black {{
            color: #000;
        }}

        .label{{
            font-weight:bold;
        }}


        span {{
         margin-right:10px
        }}
    </style>
</head>
<body>

<div class='container'>
    <div class='box'>
        <div class='card' >
            <div class='card-title'>
                Request Information
            </div>
            <div class='pl-80'>
                <div class='row'>
                    <span class='label'>Destination:</span>
                    <span>{ request.Destination }</span>
                </div>
                 <div class='row'>
                    <span class='label'>Mode:</span>
                    <span>{ request.Mode }</span>
                </div>
                   <div class='row'>
                    <span class='label'>Purpose:</span>
                    <span>{ request.Purpose }</span>
                </div>
               <div class='row'>
                    <span class='label'>Departure Date:</span>
                    <span>{ request.StartDate }</span>
                </div>
                 <div class='row'>
                    <span class='label'>Arrival Date:</span>
                    <span>{ request.EndDate }</span>
                </div>
                  <div class='row'>
                    <span class='label'>Request Date:</span>
                    <span>{ request.Date }</span>
                </div>
                <div class='row'>
                    <span class='label'>Request Status:</span>
                    <span>{ request.Status }</span>
                </div>
               <div class='row'>
                    <span class='label'>Current Handler:</span>
                    <span>{ request.CurrentHandler.EmpName }</span>
                </div>
                <!-- Add more rows as needed -->
            </div>
        </div>
    </div>

  
</div>



<div class='container'> 
  <div class='box'>
        <div class='card' >
            <div class='card-title bg-green'>
                Requester Information
            </div>
            <div class='pl-80 text-black'>
                <div class='row'>
                    <span class='label'>Name</span>
                    <span>{ request.Requester.EmpName }</span>
                </div>
               <div class='row'>
                    <span class='label'>Email</span>
                    <span>{ request.Requester.MailAddress }</span>
                </div>
                  <div class='row'>
                    <span class='label'>Designtation</span>
                    <span>{ request.Requester.Designation }</span>
                </div>
                       <div class='row'>
                    <span class='label'>Department</span>
                    <span>{ request.Requester.Department }</span>
                </div
                 <div class='row'>
                    <span class='label'>Supervisor</span>
                    <span>{ request.Requester.SuperVisor.EmpName }</span>
                </div
                    <div class='row'>
                    <span class='label'>Zonal Head</span>
                    <span>{ request.Requester.ZonalHead.EmpName }</span>
                </div

                      <div class='row'>
                    <span class='label'>Travel Handler</span>
                    <span>{ request.Requester.TravelHandler.EmpName }</span>
                </div
                <!-- Add more rows as needed -->
            </div>
        </div>
    </div>

</div>


</body>
</html>

     
     ";


message.Body = builder.ToMessageBody();

_mailer.FireMail(message, senderEmail, password);


            return Ok(true);

}





}