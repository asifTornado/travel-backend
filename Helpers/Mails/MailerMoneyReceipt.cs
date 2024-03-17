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



public class MailerMoneyReceipt
{

     private IConfiguration _configuration;
     private string senderEmail;
     private string senderPassword;
     private string frontEnd;

     private IReportGenerator _reportGenerator;

     public MailerMoneyReceipt(IConfiguration configuration, IReportGenerator reportGenerator){
             _configuration = configuration;
             _reportGenerator = reportGenerator;
             
      senderEmail = _configuration.GetValue<string>("Mail:Email");
      senderPassword    = _configuration.GetValue<string>("Mail:Password");
      frontEnd    = _configuration.GetValue<string>("Url:FrontEnd");
     }


  

public async Task SendMoneyReceipt(string accountsMail, string filename, int moneyReceiptId, Request request, string token, string auditorMail = null){

    string subject = "New Money Receipt";

      var url = $"{frontEnd}email/moneyReceipt/{moneyReceiptId}/{token}";

    var message = new MimeMessage();
    message.To.Add(new MailboxAddress("", accountsMail));
    message.To.Add(new MailboxAddress("", auditorMail));

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
                <h4>Advance Money Requistion Form For The Trip Numbered {request.Id} has been submitted.  </h4>
                <p><a href='{url}'> Click Here For More Information</a></p>
               

            </body>
            </html>
        ";


       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "reports")); ;
       var filepath = Path.Combine(path, filename);

       builder.Attachments.Add(filepath);


   
      



      message.Body = builder.ToMessageBody();
     

     FireMail(message, senderEmail, senderPassword);
       
}

  public async Task SendMoneyReceiptAgain(string accountsMail, string filename, int moneyReceiptId, Request request, string token, string auditorMail = null){

    string subject ="Rectified Money Receipt";

    var url = $"{frontEnd}email/moneyReceipt/{moneyReceiptId}/{token}";

    var message = new MimeMessage();
    message.To.Add(new MailboxAddress("", accountsMail));
    message.To.Add(new MailboxAddress("", auditorMail));

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
                <h1>Advance Money Requistion Form For The Trip Numbered {request.Id} has been submitted again after rectification</h1>
                     <p><a href='{url}'> Click Here For More Information</a></p>
               

            </body>
            </html>
        ";


       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "reports")); ;
       var filepath = Path.Combine(path, filename);

       builder.Attachments.Add(filepath);


   
      



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



