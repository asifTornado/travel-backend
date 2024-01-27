using backEnd.Models;


namespace backEnd.Services.IServices
{
    public interface IAgentsService
    {
        Task<Agent?> GetAsync(int id);

        Task<List<Agent>> GetAllProfessionalAgents();
        Task CreateAgent(Agent agent);
        Task<List<Agent>> GetAllAgents();

        Task UpdateAsync(int id, Agent agent);

        Task RemoveAsync(int id);
    }
}

