
using AutoMapper;
using backEnd.Helpers.IHelpers;
using backEnd.Models;
using backEnd.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Mappings;
using System.Diagnostics;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Amazon.Util.Internal;
using backEnd.Helpers;


namespace backEnd.Controllers;




[Route("/")]
[ApiController]
public class ExpenseReportController : ControllerBase
{



    private IExpenseReportService _expenseReportService;
    private IRequestService _requestService;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;

    private IReportGenerator _reportGenerator;

    public ExpenseReportController(IReportGenerator reportGenerator,  IMailer mailer, IExpenseReportService expenseReportService, IRequestService requestService, IFileHandler fileHandler,  IUsersService usersService)
    {
       _expenseReportService = expenseReportService;
       _requestService = requestService;
       _fileHandler = fileHandler;
       _usersService = usersService;
       _mailer = mailer;
       _reportGenerator = reportGenerator;

    }



    [HttpPost]
    [Route("/sendExpenseReport")]
    public async Task<IActionResult> SendExpenseReport(IFormCollection data){

        var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var email = data["email"];
        var fileName = $"ExpenseReport_{request?.Id}.pdf";
        var auditor = await _usersService.GetAuditor();

        
     
        var pdf = await _reportGenerator.GenerateExpenseReport(fileName, expenseReport, this.ControllerContext);
        
        await _fileHandler.SaveFile(pdf, fileName);
       
        _mailer.SendExpenseReport(email, fileName, request, auditor.MailAddress);

        
        expenseReport.RequestId = request.Id;
        request.ExpenseReportGiven = true;
        await _requestService.UpdateAsync(request);
        await _expenseReportService.InsertExpenseReport(expenseReport);
        return Ok(true);
    }


    [HttpPost]
    [Route("/getExpenseReport")]
    public async Task<IActionResult> GetExpenseReport(IFormCollection data){
        var id = int.Parse(data["id"]);
        var result = await _expenseReportService.GetExpenseReportFromRequest(id);
        return Ok(result);
    }

    

}

 

