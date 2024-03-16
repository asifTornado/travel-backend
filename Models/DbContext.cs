using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
using backEnd.Models.ContextConfigurations;


namespace backEnd.Models;





public class  TravelContext: DbContext
{   

    static readonly string ConnectionString = "Server=HR-Asif-Lap\\SQLEXPRESS;Database=master;Trusted_Connection=true;TrustServerCertificate=True";
    public DbSet<Request> Requests { get; set; }
    public DbSet<Quotation> Quotations {get; set;}
    public DbSet<HotelQuotation> HotelQuotations {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Agent> Agents {get; set;}
    public DbSet<Budget> Budgets {get; set;}
    public DbSet<Cost> Costs {get; set;}
    public DbSet<Hotels> Hotels {get; set;}
    public DbSet<HotelForBrands> HotelForBrands {get; set;}
    public DbSet<HotelLocations> HotelLocations {get; set;}
    public DbSet<TicketInvoice> TicketInvoices {get; set;}
    public DbSet<HotelInvoice>  HotelInvoices {get; set;}
    public DbSet<Notification> Notifications {get; set;}
    public DbSet<Message> Messages {get; set;}
    public DbSet<FlyerNos> FlyerNos {get; set;}
    public DbSet<Log> Logs {get; set;}
    public DbSet<ExpenseReport> ExpenseReports {get; set;}
    public DbSet<Expenses> Expenses {get; set;}
    public DbSet<MoneyReceipt> MoneyReceipts {get; set;}

    public DbSet<Role> Roles {get; set;}

    public DbSet<UserRoles> UserRole {get; set;}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace "YourConnectionString" with your MySQL connection string
            
       
        
         
         optionsBuilder.EnableSensitiveDataLogging();
         optionsBuilder.UseSqlServer(ConnectionString);
   

  
      
    }





    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {

    
   modelBuilder.ApplyConfiguration(new BudgetConfiguration());
   modelBuilder.ApplyConfiguration(new LogConfiguration());
   modelBuilder.ApplyConfiguration(new RequestConfiguration());
   modelBuilder.ApplyConfiguration(new UserConfiguration());
   modelBuilder.ApplyConfiguration(new HotelForBrandsConfiguration());
   modelBuilder.ApplyConfiguration(new HotelLocationsConfiguration());
   modelBuilder.ApplyConfiguration(new QuotationConfiguration());
   modelBuilder.ApplyConfiguration(new HotelQuotationConfiguration());
   modelBuilder.ApplyConfiguration(new MessageConfiguration());
   modelBuilder.ApplyConfiguration(new TicketInvoiceConfiguration());
   modelBuilder.ApplyConfiguration(new HotelInvoiceConfiguration());
   modelBuilder.ApplyConfiguration(new ExpenseReportConfiguration());
   modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
   modelBuilder.ApplyConfiguration(new HotelConfiguration());
   modelBuilder.ApplyConfiguration(new MoneyReceiptConfiguration());
   modelBuilder.ApplyConfiguration(new RoleConfiguration());
  


      
   

      




  




        


        base.OnModelCreating(modelBuilder);

       
    }
}