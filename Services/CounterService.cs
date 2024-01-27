using backEnd.Models;
using MongoDB.Driver;
using backEnd.Services.IServices;

namespace backEnd.Services
{
    public class CounterService:ICounterService
    {
        private readonly IMongoCollection<UserMongo> _user;
        private TravelContext _travelContext;

      
    


   


        public CounterService(TravelContext travelContext)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("travel");
            _user = mongoDatabase.GetCollection<UserMongo>("users");
            _travelContext = travelContext;
       
      
       
        }



        public async Task SeedUsers(){
             
            var users  = await _user.Find( _ => true).ToListAsync();
            var newUsers = new List<User>();

            foreach(var user in users){
                var newUser = new User{
                    EmpName=user.EmpName,
                    EmpCode = user.EmpCode,
                    Designation = user.Designation,
                    Department = user.Department,
                    Password = user.Password,
                    MobileNo = user.MobileNo,
                    Extension = user.Extension,
                    Location = user.Location,
                    Unit = user.Unit,
                    MailAddress = user.MailAddress,
                    UserType = user.UserType,
                   };

                   newUsers.Add(newUser);
            }

            _travelContext.Users.AddRange(newUsers);

            await _travelContext.SaveChangesAsync();
        }




     

       

    }
}




