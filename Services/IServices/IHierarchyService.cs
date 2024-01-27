using backEnd.Models;

namespace backEnd.Services.IServices
{
    public interface IHierarchyService
    {
        void CreateHierarchy(Hierarchy hierarchy);
        Hierarchy GetHierarchyById(int id);
        void UpdateHierarchy(Hierarchy hierarchy);
        void DeleteHierarchy(int id);
    }
}
