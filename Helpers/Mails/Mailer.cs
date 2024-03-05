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



public class TicketMailer:IMailer
{

     private IConfiguration _configuration;
     private string senderEmail;
     private string senderPassword;
     private string frontEnd;

     private IReportGenerator _reportGenerator;

     public TicketMailer(IConfiguration configuration, IReportGenerator reportGenerator){
             _configuration = configuration;
             _reportGenerator = reportGenerator;
             
      senderEmail = _configuration.GetValue<string>("Mail:Email");
      senderPassword    = _configuration.GetValue<string>("Mail:Password");
      frontEnd    = _configuration.GetValue<string>("Url:FrontEnd");
     }


  
  public async Task TEmailRequestsAccounts(List<Request> requests, string recipient, User auditor, User user = null){
                


   string subject = "Documents for new travel";


        
  
 foreach(var request in requests){

    var ticketQuotation = request.Quotations.Find(x => x.Confirmed == true);
    var hotelQuotation = request.HotelQuotations.Find(x => x.Confirmed == true);

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", user.MailAddress));
    message.To.Add(new MailboxAddress("", recipient));
    if(auditor != null){
        Console.WriteLine("found email");
    message.To.Add(new MailboxAddress("", auditor.MailAddress));
   }
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
                <h1>Travel Itinerary</h1>

                <!-- Traveller Section -->
                <section>
                    <h2>Traveller Information</h2>
                    <p>Name: {request.Requester.EmpName}</p>
                    <p>Email: {request.Requester.MailAddress}</p>
                    <p>Unique Id: {request.Requester.EmpId}</p>
                    <!-- Add more traveller information as needed -->
                </section>

                <!-- Tickets Section -->
                <section>
                    <h2>Tickets</h2>
                    <h3>Ticket Details: </h3>
                    <p>{ticketQuotation}</p>
                    <!-- Add more ticket information as needed -->
                </section>

                <!-- Hotels Section -->
                <section>
                    <h2>Hotels</h2>
                    <p>{hotelQuotation}</p>
                    <!-- Add more hotel information as needed -->
                </section>

            

                <!-- Add more sections as needed -->

            </body>
            </html>
        ";


       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads")); ;
       


      foreach(var invoice in ticketQuotation.Invoices){
        var filePath = Path.Combine(path, invoice.FilePath);
        var invoiceAttachment = builder.Attachments.Add(filePath);
        invoiceAttachment.ContentId = MimeUtils.GenerateMessageId();
      }


      foreach(var invoice in hotelQuotation.Invoices){
        var filePath = Path.Combine(path, invoice.FilePath);
        var invoiceAttachment = builder.Attachments.Add(filePath);
        invoiceAttachment.ContentId = MimeUtils.GenerateMessageId();
      }


      message.Body = builder.ToMessageBody();
     

     FireMail(message, senderEmail, senderPassword);

          
 }

  }


  public async Task TEmailRequestsCustom(List<Request> requests, string recipient, User user = null){
    
   string subject = "Documents for new travel";


        
  
 foreach(var request in requests){

    var ticketQuotation = request.Quotations.Find(x => x.Confirmed == true);
    var hotelQuotation = request.HotelQuotations.Find(x => x.Confirmed == true);

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", user.MailAddress));
    message.To.Add(new MailboxAddress("", recipient));

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
                <h1>Travel Itinerary</h1>

                <!-- Traveller Section -->
                <section>
                    <h2>Traveller Information</h2>
                    <p>Name: {request.Requester.EmpName}</p>
                    <p>Email: {request.Requester.MailAddress}</p>
                    <p>Unique Id: {request.Requester.EmpId}</p>
                    <!-- Add more traveller information as needed -->
                </section>

                <!-- Tickets Section -->
                <section>
                    <h2>Tickets</h2>
                    <h3>Ticket Details: </h3>
                    <p>{ticketQuotation}</p>
                    <!-- Add more ticket information as needed -->
                </section>

                <!-- Hotels Section -->
                <section>
                    <h2>Hotels</h2>
                    <p>{hotelQuotation}</p>
                    <!-- Add more hotel information as needed -->
                </section>

            

                <!-- Add more sections as needed -->

            </body>
            </html>
        ";


       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads")); ;
       


      foreach(var invoice in ticketQuotation.Invoices){
        var filePath = Path.Combine(path, invoice.FilePath);
        var invoiceAttachment = builder.Attachments.Add(filePath);
        invoiceAttachment.ContentId = MimeUtils.GenerateMessageId();
      }


      foreach(var invoice in hotelQuotation.Invoices){
        var filePath = Path.Combine(path, invoice.FilePath);
        var invoiceAttachment = builder.Attachments.Add(filePath);
        invoiceAttachment.ContentId = MimeUtils.GenerateMessageId();
      }


      message.Body = builder.ToMessageBody();
     

     FireMail(message, senderEmail, senderPassword);

          
 }

  }


   public async Task EmailRequest(Request request, string recipient, User auditor, string type, ControllerContext controllerContext, string token, User user = null){
   

   string subject;
        
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";


    var ticketQuotation = request.Quotations.Find(x => x.Confirmed == true);
    var hotelQuotation = request.HotelQuotations.Find(x => x.Confirmed == true);

      var url = $"{frontEnd}email/request/{request.Id}/{token}";

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", user.MailAddress));
    message.To.Add(new MailboxAddress("", recipient));
    if(auditor != null){
        Console.WriteLine("found email");
    message.To.Add(new MailboxAddress("", auditor.MailAddress));
   }


    var builder = new BodyBuilder();
   

       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads")); ;
       
  if(type == "all"){
        subject = "Invoices For Air Ticket and Hotel";
         builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h1>Travel Itinerary</h1>

                <!-- Traveller Section -->
                <section>
                    <h2>Traveller Information</h2>
                    <p>Name: {request.Requester.EmpName}</p>
                    <p>Email: {request.Requester.MailAddress}</p>
                    <p>Unique Id: {request.Requester.EmpId}</p>
                    <!-- Add more traveller information as needed -->
                </section>

                <!-- Tickets Section -->
                <section>
                    <h2>Tickets</h2>
                    <h3>Ticket Details: </h3>
                    <p>{ticketQuotation?.QuotationText}</p>
                    <!-- Add more ticket information as needed -->
                </section>

                <!-- Hotels Section -->
                <section>
                    <h2>Hotels</h2>
                    <p>{hotelQuotation?.QuotationText}</p>
                    <!-- Add more hotel information as needed -->
                </section>

            
               <h1>Click on the link below for more information</h1>
                <a href='{url}' />  
            </body>
            </html>
        ";

  

      foreach(var invoiceFilePath in ticketQuotation.Invoices){
        var filePath = Path.Combine(path, invoiceFilePath.Filename);
        var invoice = builder.Attachments.Add(filePath);
        invoice.ContentId = MimeUtils.GenerateMessageId();
      }


         foreach(var hotelInvoice in hotelQuotation.Invoices){
        var filePath = Path.Combine(path, hotelInvoice.Filename);
        var invoice = builder.Attachments.Add(filePath);
        invoice.ContentId = MimeUtils.GenerateMessageId();
      }

      }else if(type == "air-ticket"){

        subject = "Invoices For Air Ticket and Hotel";
         builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h1>Travel Itinerary</h1>

                <!-- Traveller Section -->
                <section>
                    <h2>Traveller Information</h2>
                    <p>Name: {request.Requester.EmpName}</p>
                    <p>Email: {request.Requester.MailAddress}</p>
                    <p>Unique Id: {request.Requester.EmpId}</p>
                    <!-- Add more traveller information as needed -->
                </section>

                <!-- Tickets Section -->
                <section>
                    <h2>Tickets</h2>
                    <h3>Ticket Details: </h3>
                    <p>{ticketQuotation?.QuotationText}</p>
                    <!-- Add more ticket information as needed -->
                </section>

                <!-- Hotels Section -->
                <section>
                    <h2>Hotels</h2>
                    <p>{hotelQuotation?.QuotationText}</p>
                    <!-- Add more hotel information as needed -->
                </section>

            

                <!-- Add more sections as needed -->

            </body>
            </html>
        ";

  

      foreach(var invoiceFilePath in ticketQuotation.Invoices){
        var filePath = Path.Combine(path, invoiceFilePath.Filename);
        var invoice = builder.Attachments.Add(filePath);
        invoice.ContentId = MimeUtils.GenerateMessageId();
      }

      }else{

           subject = "Invoices Hotel";
         builder.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h1>Travel Itinerary</h1>

                <!-- Traveller Section -->
                <section>
                    <h2>Traveller Information</h2>
                    <p>Name: {request.Requester.EmpName}</p>
                    <p>Email: {request.Requester.MailAddress}</p>
                    <p>Unique Id: {request.Requester.EmpId}</p>
                    <!-- Add more traveller information as needed -->
                </section>

                <!-- Tickets Section -->
                <section>
                    <h2>Tickets</h2>
                    <h3>Ticket Details: </h3>
                    <p>{ticketQuotation?.QuotationText}</p>
                    <!-- Add more ticket information as needed -->
                </section>

                <!-- Hotels Section -->
                <section>
                    <h2>Hotels</h2>
                    <p>{hotelQuotation?.QuotationText}</p>
                    <!-- Add more hotel information as needed -->
                </section>

            

                <!-- Add more sections as needed -->

            </body>
            </html>
        ";

      foreach(var invoiceFilePath in hotelQuotation.Invoices){
        var filePath = Path.Combine(path, invoiceFilePath.Filename);
        var invoice = builder.Attachments.Add(filePath);
        invoice.ContentId = MimeUtils.GenerateMessageId();
      }

      }
        

        var CSStatement = await _reportGenerator.GenerateCSStatement(type, request, controllerContext);
        builder.Attachments.Add(CSStatement);
        message.Subject = subject;
        message.Body = builder.ToMessageBody();
     

        FireMail(message, senderEmail, senderPassword);




   }
   

    

    public async Task SendMail(List<Agent> Agents, int id, Request request)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $"Quotation is required for a new travel request";
       
           

           


          
         foreach(var agent in Agents){

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = agent.Email;


                 string html = $@"
                 <div>
  <table>
   <tr>
    <th>
        Requester Name
    </th>
    <td>
        {request.Requester.EmpName}
    </td>

   </tr>

   <tr>
      <tr>
    <th>
        Unique Id
    </th>
    <td>
        {request.Requester.EmpId}
    </td>

   </tr>

   <tr>
    <th>
        Requester Mail
    </th>
    <td>
        {request.Requester.MailAddress}
    </td>

   </tr>

   <tr>
    <th>
        Requester Designation
    </th>
    <td>
        {request.Requester.Designation}
    </td>

   </tr>


   <tr>
    <th>
        Purpose of Travel
    </th>
    <td>
        {request.Purpose}
    </td>

   </tr>


   <tr>
    <th>
        Destination
    </th>
    <td>
        {request.Destination}
    </td>

   </tr>


   
   <tr>
    <th>
        Departure Date
    </th>
    <td>
        {request.StartDate}
    </td>

   </tr>


      
   <tr>
    <th>
        Arrival Date
    </th>
    <td>
        {request.EndDate}
    </td>

   </tr>

  </table>



</div>
            <p>Hameem Group is seeking quotation for a travel for {request.Requester.EmpName} ({request.Requester.Designation}) with the id {request.Requester.EmpId} . Please click the link below to give your quotations</p>
            <a href=""http://localhost:5173/#/travel/quotation/{id}/{agent.Id}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

        FireMail(message, senderEmail, senderPassword);

         }

    }


     public async Task SendMailBook(Agent agent, Request request, Quotation quotation)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"Your Quotation for Hameem Groups Travel Number {request.Number} for {request.Requester.EmpName} ({request.Requester.Designation}) with the unique id {request.Requester.EmpId} has been accepted";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = agent.Email;


              string html = $@"
<!DOCTYPE html>
<html>
<head>
  <style>
    body {{
      font-family: Arial, sans-serif;
    }}
    .section-title {{
      background-color: #f1f1f1;
      padding: 10px;
      font-weight: bold;
      margin-top: 20px;
      margin-bottom: 10px;
      
    }}
    table {{
      width: 90%;
      border-collapse: collapse;
    }}
    th, td {{
      padding: 8px;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }}
    th {{
      background-color: #f1f1f1;
    }}
  </style>
</head>
<body>
  <p>Hameem Group has accepted your quotation for the travel request listed below. Please book the ticket and wait for confirmation.</p>
  
  <div class='section-title'>Travel Request Information</div>
  <table>
    <tbody>
      <tr><th>Destination</th><td>{request.Destination}</td></tr>
      <tr><th>Number of Nights</th><td>{request.NumberOfNights}</td></tr>
      <tr><th>Mode of Transport</th><td>{request.Mode}</td></tr>
      <tr><th>Start Date</th><td>{request.StartDate}</td></tr>
      <tr><th>End Date</th><td>{request.EndDate}</td></tr>
       <tr><th>Traveller</th><td>{request.Requester.EmpName}</td></tr>
       <tr><th>Traveller's Designation</th><td>{request.Requester.Designation}</td></tr>
    </tbody>
  </table>

  <div class='section-title'>Your Quotation</div>
  <div>{quotation.QuotationText}</div>
</body>
</html>
";
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

        FireMail(message, senderEmail, senderPassword);

     

    }





     public async Task SendMailConfirm(Agent agent, Request request, Quotation quotation)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"Hameem Group has confirmed the travel request number {request.Number} for the traveller {request.Requester.EmpName} ({request.Requester.Designation}) with the unique id {request.Requester.EmpId} ";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = agent.Email;


                 string html = $@"
            <p>Hameem Group has confirmed the travel request number {request.Number} for {request.Requester.EmpName} ({request.Requester.Designation}). Please go to the link below and submit your invoice</p>
                <a href=""http://localhost:5173/#/travel/confirm/{request.Id}/{agent.Id}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);
        Console.WriteLine("sending confirmation mail");
       

        FireMail(message, senderEmail, senderPassword);

     

    }




    public async Task SendMailInvoiceRequester(Request request, Agent agent)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"You have received invoice for the travel request number {request.Number} from {agent.Name} ";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = request.Requester.MailAddress;
      


                 string html = $@"
                 <h3>You have received invoice for the travel request number {request.Number} for {request.Requester.EmpName} ({request.Requester.Designation}) from {agent.Name}  </h3>
            <p>Click on the link below for more info</p>
                <a href=""http://localhost:5173/#/travel/showRequest/{request.Id}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
     
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

       FireMail(message, senderEmail, senderPassword);

     

    }




     public async Task SendMailInvoiceAccounts(Request request, Agent agent)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"You have received invoice for the travel request number {request.Number} from {agent.Name} raised by {request.Requester.EmpName} ";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = request.Requester.TravelHandler.MailAddress;
      


                 string html = $@"
            <p>Click on the link below for more info</p>
                <a href=""http://localhost:5173/#/travel/showRequest/{request.Id}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
     
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

       FireMail(message, senderEmail, senderPassword);
     

    }


    

      public async Task SendMailApproved(Request request, string msg, int userId)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"You travel request has been approved";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = request.Requester.MailAddress;
      


                 string html = $@"
               {msg}
            <p>Click on the link below for more info</p>
                <a href=""http://localhost:5173/#/travel/email/{request.Id}/{userId}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
     
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

        FireMail(message, senderEmail, senderPassword);

     

    }


  






      public async Task SendMailProcessed(Request request)
    {
        

        Console.WriteLine("Sending Email");

    
    
                string subject = $@"You travel request with the number {request.Number} has been processed by accounts ";
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = request.Requester.MailAddress;
      


                 string html = $@"
            <p>Click on the link below for more info</p>
                <a href=""http://localhost:5173/#/travel/request/{request.Id}"" style='text-decoration: underline; color:dodgerblue'>Click  Here</a>";
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
     
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

       FireMail(message, senderEmail, senderPassword);
     

    }




       public async Task Revert(Request request, Agent agent, string msg, string subject)
    {
        

        Console.WriteLine("Sending Email");

    
    
            
       
           

           


          
        

            
        // string senderEmail = "asifdummymail@gmail.com";
        // string senderPassword = "torw blya mtym yutl";
        string? recipientEmail = agent.Email;
      


                 string html = msg;
           
            
            
                string body = html;
        

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
     
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = body;
        message.Body = bodyBuilder.ToMessageBody();

        Console.WriteLine("This is the address from which the email is sent");
        Console.WriteLine(senderEmail);

     FireMail(message, senderEmail, senderPassword);

     

    }



public void SeekSupervisorApproval(Request request, string quotation, string type, string token){
    Console.WriteLine("Supervisor Approval Called");
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", senderEmail));
    message.To.Add(new MailboxAddress("", request.Requester.SuperVisor.MailAddress));
    message.Subject = $"{request.Requester.EmpName} is seeking your approval for a {type} quotation";

    var builder  = new BodyBuilder();
     
    Console.WriteLine("this is the email and the password");
    Console.WriteLine(senderEmail);
    // Console.WriteLine(password);

    builder.HtmlBody = $@"
    
      <h3>{request.Requester.EmpName} is seeking your approval for a {type} quotation <h3>

      <p>Here is  the quotation:</p>
      <h4>{ quotation }</h4>
       
       <p><a href='{frontEnd}{request.Id}/{token}'> Click Here To Give Your Approval</a></p>
    
    ";
    
    message.Body = builder.ToMessageBody();


    FireMail(message, senderEmail, senderPassword);




}


public async Task SendExpenseReport(string accountsMail, string filename, Request request, int expenseReportId, string token, string auditorMail = null){

    string subject = "New Expense Report";

      var url = $"{frontEnd}email/expenseReport/{expenseReportId}/{token}";

    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", senderEmail));
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
                <h1>Expense Report for Trip Number {request.Id} has been submitted. Click on the link below for more information. </h1>

                     <a href='{url}' />            

            </body>
            </html>
        ";


       var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "reports")); ;
       var filepath = Path.Combine(path, filename);

       builder.Attachments.Add(filepath);


   
      



      message.Body = builder.ToMessageBody();
     

     FireMail(message, senderEmail, senderPassword);
       
}


public async Task SendMailSeekInformation(Request request, string token){
    string subject = "Seeking Information Regarding Trip";
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", senderEmail));
    message.To.Add(new MailboxAddress("", request.Requester?.MailAddress));

    message.Subject = subject;
    var builder = new BodyBuilder();
    builder.HtmlBody = $@"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
             
            </head>
        
            <body>
                <h4>Information is required regarding your trip number {request.BudgetId}. Please click on the link below to give your info</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here To Give Your Information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();

    FireMail(message, senderEmail, senderPassword);


}


    public async Task SeekSupervisorApprovalTrip(Request request, string token){
    string subject = $"A trip by {request.Requester.EmpName} requires your approval";
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("", senderEmail));
    message.To.Add(new MailboxAddress("", request.Requester.SuperVisor.MailAddress));

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
                <h4>{request.Requester.EmpName} is seeking your approval regarding the trip number {request.BudgetId}. Please click on the link below to give your approval</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here To Give Your Approval</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();

    FireMail(message, senderEmail, senderPassword);


}

    public async Task SeekRectification(Request request, string token)
    {
        string subject = "Your trip information needs modification";
        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("", request.Requester.MailAddress));
        message.From.Add(new MailboxAddress("", senderEmail));
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
                <h4>Your trip with the id {request.BudgetId} has been rejected by your supervisor. Please modify the trip information and resend it back to your approval</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here To Modify Your Trip Information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();

    FireMail(message, senderEmail, senderPassword);


    }

    public async Task PermanentlyRejected(Request request, string token)
    {
        string subject = $"Your trip with the id {request.BudgetId} has been permanently rejected";
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", request.Requester.MailAddress));
        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = $@"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h4>Your trip with the id {request.BudgetId} has been permanently rejected by your supervisor</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here for more information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();
    FireMail(message, senderEmail, senderPassword);
    }

    public async Task RequestApproved(Request request, string token)
    {
         string subject = $"Your trip with the id {request.BudgetId} has been approved by your supervisor";
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", request.Requester.MailAddress));
        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = $@"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h4>Your trip with the id {request.BudgetId} has been approved  by your supervisor</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here for more information</a></p>
                </div>
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

 
    public Task QuotationApproved(Request request, string token, string type)
    {
        throw new NotImplementedException();
    }

    public Task QuotationRejected(Request request, string token, string type)
    {
        throw new NotImplementedException();
    }

    public async Task DepartmentHeadApproved(Request request, string token)
    {
         string subject = $"Your trip with the id {request.BudgetId} has been approved by your department head";
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", request.Requester.MailAddress));
        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = $@"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h4>Your trip with the id {request.BudgetId} has been approved  by your department head</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here for more information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();
    FireMail(message, senderEmail, senderPassword);
    }

    public async Task DepartmentHeadRejected(Request request, string token)
    {
         string subject = $"The trip numbered {request.BudgetId} needs modification";
        var message = new MimeMessage();
        message.To.Add(new MailboxAddress("", request.Requester.SuperVisor.MailAddress));
        message.From.Add(new MailboxAddress("", senderEmail));
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
                <h4>The trip with the id {request.BudgetId} has been rejected by your department head. Please modify the trip information and resend it back for approval</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here To Modify Your Trip Information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();

    FireMail(message, senderEmail, senderPassword);
    }

    public async Task DepartmentHeadPermanentlyRejected(Request request, string token)
    {
         string subject = $"Your trip with the id {request.BudgetId} has been permanently rejected";
        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("", senderEmail));
        message.To.Add(new MailboxAddress("", request.Requester.MailAddress));
        BodyBuilder builder = new BodyBuilder();
        builder.HtmlBody = $@"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Your Email Subject</title>
            </head>
        
            <body>
                <h4>Your trip with the id {request.BudgetId} has been permanently rejected by your department head</h1>
                <div>
                    <p><a href='{frontEnd}unapproved-request/{token}/{request.Id}'> Click Here for more information</a></p>
                </div>
            </body>
            </html>
    
    ";

    message.Body = builder.ToMessageBody();
    FireMail(message, senderEmail, senderPassword);
        
    }
}



