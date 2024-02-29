using backEnd.Helpers;
using backEnd.Models;
using backEnd.Models.DTOs;
using backEnd.Services;
using backEnd.Helpers.IHelpers;
using System.Text.Json;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using System.Numerics;

namespace backEnd.Helpers
{

    public class ReportGenerator : IReportGenerator
    {
       
       private IFileHandler _fileHandler;
       public ReportGenerator(IFileHandler fileHandler){
        _fileHandler = fileHandler;
       }


        public async Task<byte[]?> GenerateExpenseReport(string fileName, ExpenseReport expenseReport, ControllerContext controllerContext)
        {
            var pdf = await new ViewAsPdf("ExpenseReport", expenseReport){
            FileName = fileName,
            PageSize = Size.A4,
            PageOrientation = Orientation.Portrait,
            PageMargins = { Left = 15, Right = 15, Top = 20, Bottom = 20 }
        }.BuildFile(controllerContext);

        return pdf;
        }


           public async Task<byte[]?> GenerateAdvancePaymentForm(string fileName,  MoneyReceipt moneyReceipt, ControllerContext controllerContext)
        {
            var pdf = await new ViewAsPdf("MoneyReceipt", moneyReceipt){
            FileName = fileName,
            PageSize = Size.A4,
            PageOrientation = Orientation.Portrait,
            PageMargins = { Left = 15, Right = 15, Top = 20, Bottom = 20 }
        }.BuildFile(controllerContext);

        return pdf;
        }


        public async Task<string?> GenerateCSStatement(string type, Request request, ControllerContext controllerContext){
                   string fileName = $"request_{request.Id}_{type}_CSStatement.pdf";


                   var pdf = await new ViewAsPdf("CSStatement", new ReportDTO(){Type=type, Request=request}){
                     FileName = fileName,
                     PageSize = Size.A4,
                     PageOrientation = Orientation.Portrait,
                     PageMargins = {Left = 15, Right = 15, Top = 20, Bottom = 20}
                   }.BuildFile(controllerContext);

             

                  var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "reports")); ;
                  var filepath = Path.Combine(path, fileName);

                  await _fileHandler.SaveFile(pdf, filepath);

                  return filepath;
        }
    }
}


