using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.Services.IServices;




public interface IChartService
{
    Task<JsonResult> GetTimeSeriesData(int timespan);
}