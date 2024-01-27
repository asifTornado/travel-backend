using backEnd.Helpers.IHelpers;
using System;

namespace backEnd.Helpers
{
    public class FileHandler:IFileHandler
    {
        public string GetUniqueFileName(string fileName)
        {
            var fileName2 = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName2)
                + "_"
                + Guid.NewGuid().ToString().Substring(0, 8)
                + Path.GetExtension(fileName2);
        }

        public async Task<string> SaveFile(string filename, IFormFile file)
        { 

             var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads")); 

            var filePath = Path.Combine(path, filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await file.CopyToAsync(stream);
            }

            return filename;
        }

       
        public async Task SaveFile(byte[]? pdf, string filename){
            var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", "reports"));
            var filePath = Path.Combine(path, filename);

            await System.IO.File.WriteAllBytesAsync(filePath, pdf);

        }
    }
}



