   using System;
   using backEnd.Models;
   using Microsoft.AspNetCore.Mvc;

    namespace backEnd.Helpers.IHelpers;
    
        public interface IReportGenerator
        {
           Task<byte[]?> GenerateExpenseReport(string fileName, ExpenseReport expenseReport, ControllerContext controllerContext);
           Task<string?> GenerateCSStatement(string type, Request request, ControllerContext controllerContext);
           Task<byte[]?> GenerateAdvancePaymentForm(string fileName,  MoneyReceipt moneyReceipt, ControllerContext controllerContext);
        }
    
