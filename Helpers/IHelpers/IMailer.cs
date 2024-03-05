using System.Collections.Generic;
using System.Threading.Tasks;
using backEnd.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;


namespace backEnd.Helpers.IHelpers;

public interface IMailer
{
    Task EmailRequest(Request request, string recipient, User auditor, string type, ControllerContext controllerContext, string token, User user = null);
    Task SendMail(List<Agent> Agents, int id, Request request);
    Task SendMailBook(Agent agent, Request request, Quotation quotation);
    Task SendMailConfirm(Agent agent, Request request, Quotation quotation);
    Task SendMailInvoiceRequester(Request request, Agent agent);
    Task SendMailInvoiceAccounts(Request request, Agent agent);
    Task SendMailApproved(Request request, string msg, int userId);
    // Task SendMailQuoteReceived(Request request, Agent agent, int userId);
    Task SendMailProcessed(Request request);
    Task Revert(Request request, Agent agent, string msg, string subject);
    void SeekSupervisorApproval(Request request, string quotation, string type, string token);
    Task FireMail(MimeMessage message, string senderEmail, string password);
    Task TEmailRequestsAccounts(List<Request> requests, string recipient, User auditor, User user = null);
    Task TEmailRequestsCustom(List<Request> requests, string recipient, User user = null);
    Task SendExpenseReport(string accountsMail, string filename, Request request, int expenseReportId, string token, string auditorMail = null);
    Task SendMailSeekInformation(Request request, string token);
    Task SeekSupervisorApprovalTrip(Request request, string token);
    Task SeekRectification(Request request, string token);
    Task PermanentlyRejected(Request request, string token);
    Task RequestApproved(Request request, string token);
    Task QuotationApproved(Request request, string token, string type);
    Task QuotationRejected(Request request, string token, string type);

    Task DepartmentHeadApproved(Request request, string token);
    Task DepartmentHeadRejected(Request request, string token);
    Task DepartmentHeadPermanentlyRejected(Request request, string token);

}