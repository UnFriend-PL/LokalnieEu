using LokalnieEU.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LokalnieEU.Services
{
    public interface IUserService
    {
       Task<ServiceResponse<User>> Register(UserDto userDto);
       Task<ServiceResponse<string>> Authenticate(UserLoginDto userDto);

    }
}
