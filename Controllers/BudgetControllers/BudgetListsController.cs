
using AutoMapper;
using backEnd.Helpers.IHelpers;
using backEnd.Models;
using backEnd.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Mappings;
using System.Diagnostics;
using backEnd.Services;

namespace backEnd.Controllers.BudgetControllers;




[Route("/")]
[ApiController]

public class BudgetListsController : ControllerBase
{


    private IMapper _imapper;
    private IRequestService _requestService;
    private IAgentsService _agentsService;
    private IMailer _mailer;
    private IUsersService _usersService;
    private IFileHandler _fileHandler;
    private INotifier _notifier;
    private IBudgetsService _budgetsService;
    private IHelperClass _helperClass;
    private ILogService _logService;
    private readonly IJwtTokenConverter _jwtTokenConverter;
    private readonly IIDCheckService _idCheckService;

   


    public BudgetListsController(IHelperClass helperClass, 
    IBudgetsService budgetsService, IFileHandler fileHandler, 
    IUsersService usersService, IAgentsService agentsService, 
    IMapper mapper, IRequestService requestService, 
    IMailer mailer, INotifier notifier, ILogService logService,
    IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService
    )
    {
        _imapper = mapper;
        _requestService = requestService;
        _agentsService = agentsService;
        _mailer = mailer;
        _usersService = usersService;
        _fileHandler = fileHandler;
        _notifier = notifier;
        _budgetsService = budgetsService;
        _helperClass = helperClass;
        _logService = logService;
        _jwtTokenConverter = jwtTokenConverter;
        _idCheckService = idCheckService;

    }
    

    
    [HttpPost]
    [Route("searchBudget")]
    public async Task<IActionResult> SearchBudget(IFormCollection data){
        Console.WriteLine("this is the data");
        Console.WriteLine(data["search"]);

            var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

           if(allowed == false){
            return Ok(false);
           } 
        


        Dictionary<string, string> budget = JsonSerializer.Deserialize<Dictionary<string, string>>(data["search"]);

        var result = await _budgetsService.SearchBudget(budget);
       
        return Ok(result);
        
    }


     [HttpPost]
    [Route("getBudgets")]
    public async Task<IActionResult> GetBudget(IFormCollection data){

             var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

           if(allowed == false){
            return Ok(false);
           } 
        
       
            var result = await _budgetsService.GetAllBudgets();

    
            return Ok(result);
    }







}




 

