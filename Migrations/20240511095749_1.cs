using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    Professional = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TripId = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true),
                    Brand = table.Column<string>(type: "TEXT", nullable: true),
                    Destination = table.Column<string>(type: "TEXT", nullable: true),
                    DepartureDate = table.Column<string>(type: "TEXT", nullable: true),
                    ArrivalDate = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfDays = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfTravelers = table.Column<string>(type: "TEXT", nullable: true),
                    AirTicketBudget = table.Column<string>(type: "TEXT", nullable: true),
                    HotelBudget = table.Column<string>(type: "TEXT", nullable: true),
                    TotalBookingCost = table.Column<string>(type: "TEXT", nullable: true),
                    TransportExpense = table.Column<string>(type: "TEXT", nullable: true),
                    IncidentalExpense = table.Column<string>(type: "TEXT", nullable: true),
                    TotalTripBudget = table.Column<string>(type: "TEXT", nullable: true),
                    Initiated = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<string>(type: "TEXT", nullable: true),
                    Custom = table.Column<bool>(type: "INTEGER", nullable: true),
                    TicketsApprovedByAccounts = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeekingAccountsApprovalForTickets = table.Column<bool>(type: "INTEGER", nullable: true),
                    CurrentHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true),
                    Rejected = table.Column<bool>(type: "INTEGER", nullable: true),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: true),
                    TicketsMoneyDisbursed = table.Column<bool>(type: "INTEGER", nullable: true),
                    AmountDisbursedTickets = table.Column<string>(type: "TEXT", nullable: true),
                    TicketsAccountNumber = table.Column<string>(type: "TEXT", nullable: true),
                    TicketsAccountHolderNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BeingProcessed = table.Column<bool>(type: "INTEGER", nullable: true),
                    BeingProcessedAccounts = table.Column<bool>(type: "INTEGER", nullable: true),
                    BeingProcessedAudit = table.Column<bool>(type: "INTEGER", nullable: true),
                    AccountsProcessed = table.Column<bool>(type: "INTEGER", nullable: true),
                    AuditProcessed = table.Column<bool>(type: "INTEGER", nullable: true),
                    CurrentAccountsHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    CurrentAuditHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    AccountsPrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true),
                    AuditPrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelForBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Brand = table.Column<string>(type: "TEXT", nullable: true),
                    BrandOfficeAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelForBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Filename = table.Column<string>(type: "TEXT", nullable: true),
                    FilePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    SourceId = table.Column<int>(type: "INTEGER", nullable: true),
                    From = table.Column<int>(type: "INTEGER", nullable: true),
                    To = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Event = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Filename = table.Column<string>(type: "TEXT", nullable: true),
                    FilePath = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmpId = table.Column<string>(type: "TEXT", nullable: true),
                    EmpName = table.Column<string>(type: "TEXT", nullable: true),
                    EmpCode = table.Column<string>(type: "TEXT", nullable: true),
                    Designation = table.Column<string>(type: "TEXT", nullable: true),
                    MailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: true),
                    Wing = table.Column<string>(type: "TEXT", nullable: true),
                    Team = table.Column<string>(type: "TEXT", nullable: true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    UserType = table.Column<string>(type: "TEXT", nullable: true),
                    Available = table.Column<bool>(type: "INTEGER", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    Raters = table.Column<int>(type: "INTEGER", nullable: true),
                    Extension = table.Column<string>(type: "TEXT", nullable: true),
                    MobileNo = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Numbers = table.Column<int>(type: "INTEGER", nullable: true),
                    SuperVisorId = table.Column<int>(type: "INTEGER", nullable: true),
                    ZonalHeadId = table.Column<int>(type: "INTEGER", nullable: true),
                    PassportNo = table.Column<string>(type: "TEXT", nullable: true),
                    PreferenceImage = table.Column<string>(type: "TEXT", nullable: true),
                    Preferences = table.Column<string>(type: "TEXT", nullable: true),
                    HasFrequentFlyerNo = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_SuperVisorId",
                        column: x => x.SuperVisorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_ZonalHeadId",
                        column: x => x.ZonalHeadId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HotelLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocationName = table.Column<string>(type: "TEXT", nullable: true),
                    HotelForBrandsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelLocations_HotelForBrands_HotelForBrandsId",
                        column: x => x.HotelForBrandsId,
                        principalTable: "HotelForBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetTicketApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BudgetId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTicketApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetTicketApprovals_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetTicketApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetTravelers",
                columns: table => new
                {
                    BudgetsId = table.Column<int>(type: "INTEGER", nullable: false),
                    TravelersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTravelers", x => new { x.BudgetsId, x.TravelersId });
                    table.ForeignKey(
                        name: "FK_BudgetTravelers_Budgets_BudgetsId",
                        column: x => x.BudgetsId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetTravelers_Users_TravelersId",
                        column: x => x.TravelersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlyerNos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Airline = table.Column<string>(type: "TEXT", nullable: true),
                    Number = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlyerNos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlyerNos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TripId = table.Column<string>(type: "TEXT", nullable: false),
                    Custom = table.Column<bool>(type: "INTEGER", nullable: true),
                    Objectives = table.Column<string>(type: "TEXT", nullable: true),
                    Meetings = table.Column<string>(type: "TEXT", nullable: true),
                    Items = table.Column<string>(type: "TEXT", nullable: true),
                    Personnel = table.Column<string>(type: "TEXT", nullable: true),
                    Destination = table.Column<string>(type: "TEXT", nullable: true),
                    Purpose = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<string>(type: "TEXT", nullable: true),
                    AccomodationRequired = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfNights = table.Column<string>(type: "TEXT", nullable: true),
                    TotalCost = table.Column<int>(type: "INTEGER", nullable: true),
                    RequesterId = table.Column<int>(type: "INTEGER", nullable: true),
                    Number = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    AgentNumbers = table.Column<int>(type: "INTEGER", nullable: true),
                    PrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<string>(type: "TEXT", nullable: true),
                    EndDate = table.Column<string>(type: "TEXT", nullable: true),
                    Booked = table.Column<bool>(type: "INTEGER", nullable: true),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Selected = table.Column<bool>(type: "INTEGER", nullable: true),
                    BeingProcessed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeekingInvoices = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeekingHotelInvoices = table.Column<bool>(type: "INTEGER", nullable: true),
                    InTrip = table.Column<bool>(type: "INTEGER", nullable: true),
                    Best = table.Column<string>(type: "TEXT", nullable: true),
                    BestHotel = table.Column<string>(type: "TEXT", nullable: true),
                    HotelBooked = table.Column<bool>(type: "INTEGER", nullable: true),
                    HotelConfirmed = table.Column<bool>(type: "INTEGER", nullable: true),
                    TicketInvoiceUploaded = table.Column<bool>(type: "INTEGER", nullable: true),
                    HotelInvoiceUploaded = table.Column<bool>(type: "INTEGER", nullable: true),
                    BudgetId = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestBudget = table.Column<string>(type: "TEXT", nullable: true),
                    ExpenseReportGiven = table.Column<bool>(type: "INTEGER", nullable: true),
                    DepartmentHeadApproved = table.Column<bool>(type: "INTEGER", nullable: true),
                    SupervisorApproved = table.Column<bool>(type: "INTEGER", nullable: true),
                    PermanentlyRejected = table.Column<bool>(type: "INTEGER", nullable: true),
                    Activities = table.Column<string>(type: "TEXT", nullable: true),
                    MoneyReceiptId = table.Column<int>(type: "INTEGER", nullable: false),
                    MoneyReceiptSubmitted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Approvals = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Budgets_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_CurrentHandlerId",
                        column: x => x.CurrentHandlerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelName = table.Column<string>(type: "TEXT", nullable: true),
                    HotelAddress = table.Column<string>(type: "TEXT", nullable: true),
                    HotelLocationsId = table.Column<int>(type: "INTEGER", nullable: true),
                    Rooms = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_HotelLocations_HotelLocationsId",
                        column: x => x.HotelLocationsId,
                        principalTable: "HotelLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Item = table.Column<string>(type: "TEXT", nullable: true),
                    ItemCost = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfItems = table.Column<string>(type: "TEXT", nullable: true),
                    TotalItemCost = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Costs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeName = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Department = table.Column<string>(type: "TEXT", nullable: true),
                    StartDate = table.Column<string>(type: "TEXT", nullable: true),
                    EndDate = table.Column<string>(type: "TEXT", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: true),
                    SupervisorApproved = table.Column<bool>(type: "INTEGER", nullable: true),
                    Submitted = table.Column<bool>(type: "INTEGER", nullable: true),
                    Approvals = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true),
                    Rejected = table.Column<bool>(type: "INTEGER", nullable: true),
                    TravelManagerSubmitted = table.Column<bool>(type: "INTEGER", nullable: true),
                    Disbursed = table.Column<bool>(type: "INTEGER", nullable: true),
                    AmountDisbursed = table.Column<string>(type: "TEXT", nullable: true),
                    BankAccountHolderName = table.Column<string>(type: "TEXT", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Users_CurrentHandlerId",
                        column: x => x.CurrentHandlerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HotelQuotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Linker = table.Column<Guid>(type: "TEXT", nullable: true),
                    RequestIds = table.Column<string>(type: "TEXT", nullable: true),
                    QuoteGiver = table.Column<string>(type: "TEXT", nullable: true),
                    QuotationText = table.Column<string>(type: "TEXT", nullable: true),
                    Selected = table.Column<bool>(type: "INTEGER", nullable: true),
                    Booked = table.Column<bool>(type: "INTEGER", nullable: true),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Hovered = table.Column<bool>(type: "INTEGER", nullable: true),
                    Custom = table.Column<bool>(type: "INTEGER", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true),
                    Approved = table.Column<bool>(type: "INTEGER", nullable: true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalCosts = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelQuotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelQuotations_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HotelQuotations_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    Event = table.Column<string>(type: "TEXT", nullable: false),
                    FromId = table.Column<int>(type: "INTEGER", nullable: true),
                    ToId = table.Column<int>(type: "INTEGER", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoneyReceipts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    AdvanceMoneyInHand = table.Column<string>(type: "TEXT", nullable: true),
                    Section = table.Column<string>(type: "TEXT", nullable: true),
                    I = table.Column<string>(type: "TEXT", nullable: true),
                    Designation = table.Column<string>(type: "TEXT", nullable: true),
                    RequiredTK = table.Column<string>(type: "TEXT", nullable: true),
                    Taka = table.Column<string>(type: "TEXT", nullable: true),
                    AsAdvanceAgainst = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: true),
                    SerialNo = table.Column<string>(type: "TEXT", nullable: true),
                    Processed = table.Column<bool>(type: "INTEGER", nullable: true),
                    SupervisorApproved = table.Column<bool>(type: "INTEGER", nullable: true),
                    Submitted = table.Column<bool>(type: "INTEGER", nullable: true),
                    Approvals = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentHandlerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrevHandlerIds = table.Column<string>(type: "TEXT", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true),
                    Rejected = table.Column<bool>(type: "INTEGER", nullable: true),
                    Disbursed = table.Column<bool>(type: "INTEGER", nullable: true),
                    AmountDisbursed = table.Column<string>(type: "TEXT", nullable: true),
                    BankAccountHolderName = table.Column<string>(type: "TEXT", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyReceipts_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoneyReceipts_Users_CurrentHandlerId",
                        column: x => x.CurrentHandlerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Linker = table.Column<Guid>(type: "TEXT", nullable: true),
                    QuoteGiver = table.Column<string>(type: "TEXT", nullable: true),
                    QuotationText = table.Column<string>(type: "TEXT", nullable: true),
                    Selected = table.Column<bool>(type: "INTEGER", nullable: true),
                    Booked = table.Column<bool>(type: "INTEGER", nullable: true),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Hovered = table.Column<bool>(type: "INTEGER", nullable: true),
                    Custom = table.Column<bool>(type: "INTEGER", nullable: true),
                    RequestIds = table.Column<string>(type: "TEXT", nullable: true),
                    RequestId = table.Column<int>(type: "INTEGER", nullable: true),
                    Approved = table.Column<bool>(type: "INTEGER", nullable: true),
                    AgentId = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalCosts = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Quotations_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestAgents",
                columns: table => new
                {
                    AgentsId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAgents", x => new { x.AgentsId, x.RequestsId });
                    table.ForeignKey(
                        name: "FK_RequestAgents_Agents_AgentsId",
                        column: x => x.AgentsId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestAgents_Requests_RequestsId",
                        column: x => x.RequestsId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    ExpenseType = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    ExpenseReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    Voucher = table.Column<string>(type: "TEXT", nullable: true),
                    VoucherGiven = table.Column<bool>(type: "INTEGER", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseReports_ExpenseReportId",
                        column: x => x.ExpenseReportId,
                        principalTable: "ExpenseReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelApprovals_HotelQuotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "HotelQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelQuotationInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelQuotationInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelQuotationInvoices_HotelInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "HotelInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelQuotationInvoices_HotelQuotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "HotelQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketApprovals_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketApprovals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketQuotationInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuotationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketQuotationInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketQuotationInvoices_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketQuotationInvoices_TicketInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "TicketInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTicketApprovals_BudgetId",
                table: "BudgetTicketApprovals",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTicketApprovals_UserId",
                table: "BudgetTicketApprovals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTravelers_TravelersId",
                table: "BudgetTravelers",
                column: "TravelersId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_RequestId",
                table: "Costs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_CurrentHandlerId",
                table: "ExpenseReports",
                column: "CurrentHandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_RequestId",
                table: "ExpenseReports",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseReportId",
                table: "Expenses",
                column: "ExpenseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FlyerNos_UserId",
                table: "FlyerNos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelApprovals_QuotationId",
                table: "HotelApprovals",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelApprovals_UserId",
                table: "HotelApprovals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelLocations_HotelForBrandsId",
                table: "HotelLocations",
                column: "HotelForBrandsId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelQuotationInvoices_InvoiceId",
                table: "HotelQuotationInvoices",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelQuotationInvoices_QuotationId",
                table: "HotelQuotationInvoices",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelQuotations_AgentId",
                table: "HotelQuotations",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelQuotations_RequestId",
                table: "HotelQuotations",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_HotelLocationsId",
                table: "Hotels",
                column: "HotelLocationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_RequestId",
                table: "Logs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RequestId",
                table: "Messages",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyReceipts_CurrentHandlerId",
                table: "MoneyReceipts",
                column: "CurrentHandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_MoneyReceipts_RequestId",
                table: "MoneyReceipts",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_AgentId",
                table: "Quotations",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_RequestId",
                table: "Quotations",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAgents_RequestsId",
                table: "RequestAgents",
                column: "RequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BudgetId",
                table: "Requests",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CurrentHandlerId",
                table: "Requests",
                column: "CurrentHandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketApprovals_QuotationId",
                table: "TicketApprovals",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketApprovals_UserId",
                table: "TicketApprovals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketQuotationInvoices_InvoiceId",
                table: "TicketQuotationInvoices",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketQuotationInvoices_QuotationId",
                table: "TicketQuotationInvoices",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SuperVisorId",
                table: "Users",
                column: "SuperVisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ZonalHeadId",
                table: "Users",
                column: "ZonalHeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetTicketApprovals");

            migrationBuilder.DropTable(
                name: "BudgetTravelers");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "FlyerNos");

            migrationBuilder.DropTable(
                name: "HotelApprovals");

            migrationBuilder.DropTable(
                name: "HotelQuotationInvoices");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MoneyReceipts");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RequestAgents");

            migrationBuilder.DropTable(
                name: "TicketApprovals");

            migrationBuilder.DropTable(
                name: "TicketQuotationInvoices");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "ExpenseReports");

            migrationBuilder.DropTable(
                name: "HotelInvoices");

            migrationBuilder.DropTable(
                name: "HotelQuotations");

            migrationBuilder.DropTable(
                name: "HotelLocations");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "TicketInvoices");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "HotelForBrands");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
