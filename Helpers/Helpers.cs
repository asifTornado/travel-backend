using System.Globalization;
using System.IO;
using backEnd.Models;
using backEnd.Services;
using System.Text.Json;
using Org.BouncyCastle.Crypto.Operators;
using backEnd.Helpers.IHelpers;
using backEnd.Services.IServices;

namespace backEnd.Helpers
{
    public class HelperClass:IHelperClass
    {
        Dictionary<string, string> departmentHeads = new Dictionary<string, string>();
        IUsersService _usersService;
        IFileHandler _fileHandler;
   

    

       
        public HelperClass(IUsersService usersService, IFileHandler fileHandler) {
            this._usersService = usersService;
            this._fileHandler = fileHandler;
      

            departmentHeads.Add("Administration", "Ticketing Head Admin");
            departmentHeads.Add("ERP", "Ticketing Head ERP");
            departmentHeads.Add("Information Technology", "Ticketing Head IT");


        }





        public string GetCurrentTime()
        {
            var currentDate = DateTime.Now;
            var options = new CultureInfo("en-US").DateTimeFormat;
         
            options.ShortDatePattern = "ddd, MMM d, yyyy";
            options.ShortTimePattern = "h:mm:ss tt";
            string time = currentDate.ToString("f", options);
            return time;
        }


        


     


       

      



      


        

    }




    }



 