namespace backEnd.Helpers.IHelpers;

    public interface IFileHandler
    {
        string GetUniqueFileName(string fileName);
        Task SaveFile(byte[]? pdf, string filename);
        Task<string> SaveFile(string filename, IFormFile file);
    }


