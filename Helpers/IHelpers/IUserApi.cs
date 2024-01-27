using System.Threading.Tasks;

namespace backEnd.Helpers.IHelpers;

    public interface IUserApi
    {
        Task<string?> GetUsers();
    }
