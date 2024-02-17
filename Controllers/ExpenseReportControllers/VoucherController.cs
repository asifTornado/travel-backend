
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

using backEnd.Services;


namespace backEnd.Controllers.ExpenseReportControllers;




[Route("/")]
[ApiController]
public class VoucherController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;
    private IFileHandler _fileHandler;



    public VoucherController(IUsersService usersService, IExpenseReportService expenseReportService, IFileHandler fileHandler)
    {
      _expenseReportService = expenseReportService;
      _fileHandler = fileHandler;
    }

  
 
  

  [HttpPost]
  [Route("uploadVoucher")]
  public async Task<IActionResult> UploadVoucher(IFormCollection data){
    var file = data.Files[0];
    var fileName = _fileHandler.GetUniqueFileName(file.FileName);
    var filePath = await _fileHandler.SaveFile(fileName, file);
    

    return Ok(filePath);

  } 
  






   
    

}

 

