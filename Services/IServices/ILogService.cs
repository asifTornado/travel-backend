

using System.Threading.Tasks;
using backEnd.Models;
using backEnd.Models.DTOs;





namespace backEnd.Services.IServices
{
    public interface ILogService
    {
        Task<List<LogDTO>> GetLogs(int id);

        Task InsertLog(int? requestId, int? from, int? to, string Event);

        Task<List<LogDTO>> GetLogsForTrip(List<int> requestIds);

        Task InsertLogs(List<int> requests, int? from, int? to, string Event);
    }
}

