using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backEnd.Migrations
{
    /// <inheritdoc />
    public partial class changedtheserializationtypeagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Professional = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "CONCAT('B', RIGHT('00000' + CAST(Id AS NVARCHAR(5)), 5))"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartureDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfTravelers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirTicketBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalBookingCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransportExpense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncidentalExpense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTripBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Initiated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelForBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrandOfficeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelForBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "REPLICATE('0', 7 - LEN(Id)) + CAST(Id AS VARCHAR(7))"),
                    EmpName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Available = table.Column<bool>(type: "bit", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Raters = table.Column<int>(type: "int", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numbers = table.Column<int>(type: "int", nullable: true),
                    SuperVisorId = table.Column<int>(type: "int", nullable: true),
                    ZonalHeadId = table.Column<int>(type: "int", nullable: true),
                    TravelHandlerId = table.Column<int>(type: "int", nullable: true),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preferences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasFrequentFlyerNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        name: "FK_Users_Users_TravelHandlerId",
                        column: x => x.TravelHandlerId,
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelForBrandsId = table.Column<int>(type: "int", nullable: true)
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
                name: "BudgetTravelers",
                columns: table => new
                {
                    BudgetsId = table.Column<int>(type: "int", nullable: false),
                    TravelersId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airline = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Custom = table.Column<bool>(type: "bit", nullable: true),
                    Objectives = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meetings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personnel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccomodationRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfNights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCost = table.Column<int>(type: "int", nullable: true),
                    RequesterId = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "REPLICATE('0', 7 - LEN(Id)) + CAST(Id AS VARCHAR(7))"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentNumbers = table.Column<int>(type: "int", nullable: true),
                    CurrentHandlerId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Booked = table.Column<bool>(type: "bit", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true),
                    Selected = table.Column<bool>(type: "bit", nullable: true),
                    BeingProcessed = table.Column<bool>(type: "bit", nullable: true),
                    Processed = table.Column<bool>(type: "bit", nullable: true),
                    SeekingInvoices = table.Column<bool>(type: "bit", nullable: true),
                    SeekingHotelInvoices = table.Column<bool>(type: "bit", nullable: true),
                    InTrip = table.Column<bool>(type: "bit", nullable: true),
                    Best = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BestHotel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelBooked = table.Column<bool>(type: "bit", nullable: true),
                    HotelConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    TicketInvoiceUploaded = table.Column<bool>(type: "bit", nullable: true),
                    HotelInvoiceUploaded = table.Column<bool>(type: "bit", nullable: true),
                    BudgetId = table.Column<int>(type: "int", nullable: true),
                    Budget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseReportGiven = table.Column<bool>(type: "bit", nullable: true),
                    DepartmentHeadApproved = table.Column<bool>(type: "bit", nullable: true),
                    SupervisorApproved = table.Column<bool>(type: "bit", nullable: true),
                    PermanentlyRejected = table.Column<bool>(type: "bit", nullable: true),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Averagerate = table.Column<string>(name: "Average_rate", type: "nvarchar(max)", nullable: true),
                    Actualrate = table.Column<string>(name: "Actual_rate", type: "nvarchar(max)", nullable: true),
                    HotelAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelLocationsId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalItemCost = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseReports_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HotelQuotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Linker = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteGiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotationText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected = table.Column<bool>(type: "bit", nullable: true),
                    Booked = table.Column<bool>(type: "bit", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true),
                    Hovered = table.Column<bool>(type: "bit", nullable: true),
                    Custom = table.Column<bool>(type: "bit", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromId = table.Column<int>(type: "int", nullable: true),
                    ToId = table.Column<int>(type: "int", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true)
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
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketId = table.Column<int>(type: "int", nullable: true),
                    From = table.Column<int>(type: "int", nullable: true),
                    To = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Requests_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Requests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Linker = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuoteGiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotationText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected = table.Column<bool>(type: "bit", nullable: true),
                    Booked = table.Column<bool>(type: "bit", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true),
                    Hovered = table.Column<bool>(type: "bit", nullable: true),
                    Custom = table.Column<bool>(type: "bit", nullable: true),
                    RequestIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: true)
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
                    AgentsId = table.Column<int>(type: "int", nullable: false),
                    RequestsId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseReportId = table.Column<int>(type: "int", nullable: true)
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
                    HotelApprovalsId = table.Column<int>(type: "int", nullable: false),
                    HotelApprovedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelApprovals", x => new { x.HotelApprovalsId, x.HotelApprovedId });
                    table.ForeignKey(
                        name: "FK_HotelApprovals_HotelQuotations_HotelApprovedId",
                        column: x => x.HotelApprovedId,
                        principalTable: "HotelQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelApprovals_Users_HotelApprovalsId",
                        column: x => x.HotelApprovalsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelInvoiceQuotations",
                columns: table => new
                {
                    InvoicesId = table.Column<int>(type: "int", nullable: false),
                    QuotationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInvoiceQuotations", x => new { x.InvoicesId, x.QuotationsId });
                    table.ForeignKey(
                        name: "FK_HotelInvoiceQuotations_HotelInvoices_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "HotelInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelInvoiceQuotations_HotelQuotations_QuotationsId",
                        column: x => x.QuotationsId,
                        principalTable: "HotelQuotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketApprovals",
                columns: table => new
                {
                    TicketApprovalsId = table.Column<int>(type: "int", nullable: false),
                    TicketApprovedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketApprovals", x => new { x.TicketApprovalsId, x.TicketApprovedId });
                    table.ForeignKey(
                        name: "FK_TicketApprovals_Quotations_TicketApprovedId",
                        column: x => x.TicketApprovedId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketApprovals_Users_TicketApprovalsId",
                        column: x => x.TicketApprovalsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketInvoiceQuotations",
                columns: table => new
                {
                    InvoicesId = table.Column<int>(type: "int", nullable: false),
                    QuotationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInvoiceQuotations", x => new { x.InvoicesId, x.QuotationsId });
                    table.ForeignKey(
                        name: "FK_TicketInvoiceQuotations_Quotations_QuotationsId",
                        column: x => x.QuotationsId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketInvoiceQuotations_TicketInvoices_InvoicesId",
                        column: x => x.InvoicesId,
                        principalTable: "TicketInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetTravelers_TravelersId",
                table: "BudgetTravelers",
                column: "TravelersId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_RequestId",
                table: "Costs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReports_RequestId",
                table: "ExpenseReports",
                column: "RequestId",
                unique: true,
                filter: "[RequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseReportId",
                table: "Expenses",
                column: "ExpenseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_FlyerNos_UserId",
                table: "FlyerNos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelApprovals_HotelApprovedId",
                table: "HotelApprovals",
                column: "HotelApprovedId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelInvoiceQuotations_QuotationsId",
                table: "HotelInvoiceQuotations",
                column: "QuotationsId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelLocations_HotelForBrandsId",
                table: "HotelLocations",
                column: "HotelForBrandsId");

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
                name: "IX_Notifications_TicketId",
                table: "Notifications",
                column: "TicketId");

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
                name: "IX_TicketApprovals_TicketApprovedId",
                table: "TicketApprovals",
                column: "TicketApprovedId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInvoiceQuotations_QuotationsId",
                table: "TicketInvoiceQuotations",
                column: "QuotationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SuperVisorId",
                table: "Users",
                column: "SuperVisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TravelHandlerId",
                table: "Users",
                column: "TravelHandlerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ZonalHeadId",
                table: "Users",
                column: "ZonalHeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "HotelInvoiceQuotations");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RequestAgents");

            migrationBuilder.DropTable(
                name: "TicketApprovals");

            migrationBuilder.DropTable(
                name: "TicketInvoiceQuotations");

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
