using AutoMapper;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using backEnd.Models;
using backEnd.Services;
using backEnd.Helpers;
using backEnd.Helpers.IHelpers;
using backEnd.Services.IServices;
using backEnd.Helpers;
using backEnd.Helpers.Mails;
using System.Text.Json;
using System.Text.Json.Serialization;
using backEnd.Factories;
using backEnd.Factories.IFactories;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Amazon.Runtime.SharedInterfaces.Internal;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });;

// Add services to the container
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();




builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IAgentsService, AgentsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IReportGenerator, ReportGenerator>();
builder.Services.AddScoped<IIDCheckService, IDCheckService>();
builder.Services.AddScoped<MoneyReceiptService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<MailerWorkFlow>();
builder.Services.AddScoped<MailerMoneyReceipt>();
builder.Services.AddScoped<ReportService>();


builder.Services.AddScoped<ICounterService, CounterService>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBudgetsService, BudgetsService>();
builder.Services.AddScoped<IHotelForBrandService, HotelForBrandService>();
builder.Services.AddScoped<IQuotationService, QuotationService>();
builder.Services.AddScoped<IChartService, ChartService>();
builder.Services.AddScoped<IExpenseReportService, ExpenseReportService>();
builder.Services.AddScoped<ITripService, TripService>();

builder.Services.AddScoped<ILogService, LogService>();




builder.Services.AddTransient<TravelContext>();
builder.Services.AddScoped<IJwtTokenConverter, JwtTokenConverter>();
builder.Services.AddScoped<IMailer, TicketMailer>();
builder.Services.AddScoped<IFileHandler, FileHandler>();
builder.Services.AddScoped<IUserApi, UserApi>();
builder.Services.AddScoped<IHelperClass, HelperClass>();
builder.Services.AddScoped<INotifier, Notifier>();  
builder.Services.AddScoped<IConnection, Connection>();

// builder.Services.AddCoreAdmin();
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Maps>();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("FeedClientApp",
        new CorsPolicyBuilder()
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .Build());
});


builder.Services.AddAutoMapper(typeof(Program)); 


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
var path = app.Environment.ContentRootPath;
RotativaConfiguration.Setup(path, "./wwwroot/Rotativa");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors("FeedClientApp");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRotativa();



app.Run();
