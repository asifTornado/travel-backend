using backEnd.Models;

namespace backEnd.Helpers.IHelpers;


public interface IJwtTokenConverter
{
    string GenerateToken(User user);
    int? ParseToken(string token);
}
