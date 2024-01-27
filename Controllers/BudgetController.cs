
using AutoMapper;
using backEnd.Helpers.IHelpers;
using backEnd.Models;
using backEnd.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Mappings;
using System.Diagnostics;

namespace backEnd.Controllers;




[Route("/")]
[ApiController]

public class BudgetController : ControllerBase
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

   


    public BudgetController(IHelperClass helperClass, 
    IBudgetsService budgetsService, IFileHandler fileHandler, 
    IUsersService usersService, IAgentsService agentsService, 
    IMapper mapper, IRequestService requestService, 
    IMailer mailer, INotifier notifier, ILogService logService,
    IJwtTokenConverter jwtTokenConverter
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

    }
    

    [HttpPost]
    [Route("insertBudget")]

    public async Task<IActionResult> InsertBudget(IFormCollection data){
            var budget = JsonSerializer.Deserialize<Budget>(data["budget"]);
            var time = _helperClass.GetCurrentTime();
            
            budget.CreationDate = time;
            

            
            
            Console.WriteLine("This is the budget data");
            Console.WriteLine(data["budget"]);
            await _budgetsService.CreateBudget(budget);
            return Ok();
    }


    [HttpPost]
    [Route("getBudget")]
    public async Task<IActionResult> GetBudget(IFormCollection data){
       
            var result = await _budgetsService.GetAsync(int.Parse(data["id"]));

    
            return Ok(result);
    }


    [HttpPost]
    [Route("getBudgets")]
    public async Task<IActionResult> GetBudgets(IFormCollection data){
            var result = await _budgetsService.GetAllBudgets();
            return Ok(result);
    }



    
    [HttpPost]
    [Route("deleteBudget")]
    public async Task<IActionResult> DeleteBudget(IFormCollection data){
        Console.WriteLine("this is the id");
        Console.WriteLine(data["id"]);
        await _budgetsService.RemoveAsync(int.Parse(data["id"]));
            return Ok(data["id"]);
    }

    

    [HttpPost]
    [Route("updateBudget")]
    public async Task<IActionResult> ReplaceBudget(IFormCollection data){
        var budget = JsonSerializer.Deserialize<Budget>(data["budget"]);
        await _budgetsService.UpdateAsync(budget.Id, budget);
        return Ok();
    }




    
    [HttpPost]
    [Route("searchBudget")]
    public async Task<IActionResult> SearchBudget(IFormCollection data){
        Console.WriteLine("this is the data");
        Console.WriteLine(data["search"]);
        


        Dictionary<string, string> budget = JsonSerializer.Deserialize<Dictionary<string, string>>(data["search"]);

        var result = await _budgetsService.SearchBudget(budget);
       
        return Ok(result);
        
    }



       
    [HttpPost]
    [Route("initiate")]
    public async Task<IActionResult> Initiate(IFormCollection data){
        
        var budget = JsonSerializer.Deserialize<Budget>(data["budget"]);
        var budgetFromDB = await _budgetsService.GetAsync(budget.Id);

       foreach(var traveler in budgetFromDB.Travelers){              
        var request = new Request();     
        var agents = await _agentsService.GetAllProfessionalAgents();
        var token = _jwtTokenConverter.GenerateToken(traveler);

        request.AgentNumbers = agents.Count;
        request.Purpose = budget.Subject;
        request.Destination = budget.Destination;
        request.StartDate = budget.DepartureDate;
        request.EndDate = budget.ArrivalDate;
        request.Date = _helperClass.GetCurrentTime();
        request.Requester = traveler;
        request.Status = "Seeking Information From Traveler";
        request.SupervisorApproved = false;
        request.DepartmentHeadApproved = true;
        

        request.CurrentHandlerId = request.Requester.Id;
            
        

        request.Id = await _requestService.CreateAsync(request);   

        request.BudgetId = budgetFromDB.Id;

        budgetFromDB.Requests.Add(request);
            
       
       _mailer.SendMailSeekInformation(request, token);
       await _notifier.InsertNotification("A new request requires your information", request.Requester.Id, request.Requester.Id, request.Id, Events.RequestRaised );
       await _logService.InsertLog(request.Id, request.Requester.Id, request.CurrentHandlerId, Events.RequestRaised);
        
        }


    
    
        
    // end of creating request for each traveler
      

      
       budgetFromDB.Initiated = "Yes";

       await _budgetsService.UpdateAsync(budgetFromDB.Id, budgetFromDB);


    

       

        return Ok(budgetFromDB);

        

      
    
        
    }



}




 

