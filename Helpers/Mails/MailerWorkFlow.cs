using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using backEnd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MimeKit.Utils;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Mvc;




namespace backEnd.Helpers.Mails;



public class MailerWorkFlow
{

     private IConfiguration _configuration;
     private string senderEmail;
     private string senderPassword;
     private string frontEnd;

     private IReportGenerator _reportGenerator;

     public MailerWorkFlow(IConfiguration configuration, IReportGenerator reportGenerator){
             _configuration = configuration;
             _reportGenerator = reportGenerator;
             
      senderEmail = _configuration.GetValue<string>("Mail:Email");
      senderPassword    = _configuration.GetValue<string>("Mail:Password");
      frontEnd    = _configuration.GetValue<string>("Url:FrontEnd");
     }


  
public async Task WorkFlowMail(string? from, string? toEmail, int? tripNo, int? id, string? action, string? typePath, string? type){

    string subject = "New Expense Report";



    var url = $"{frontEnd}{typePath}/{id}";

    var message = new MimeMessage();
    
    
    message.To.Add(new MailboxAddress("", toEmail));

    message.Subject = subject;

    var builder = new BodyBuilder();
    builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h1>{type} for Trip Numbered {tripNo} has been {action} by {from}. Click on the link below for more information: </h1>

                 <a href='{url}' />      

            </body>
            </html>
        ";



   
      



      message.Body = builder.ToMessageBody();
     

     FireMail(message, senderEmail, senderPassword);

     }


  



    public async Task FireMail(MimeMessage message, string senderEmail, string password){
       Console.WriteLine("Sending mail");

        using (var client = new SmtpClient())
        {
            Console.WriteLine("Just ending email");
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(senderEmail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

}

 
}



