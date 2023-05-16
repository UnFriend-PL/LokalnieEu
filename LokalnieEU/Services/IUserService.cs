using LokalnieEU.Models.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace LokalnieEU.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> Register(UserDto userDto);
        Task<ServiceResponse<UserResponse>> Authenticate(UserLoginDto userDto);
        Task<ServiceResponse<UserResponse>> UpdateUser(string jwt, UpdateUserDto userDto);
        Task<ServiceResponse<string>> UpdateUserPasswordAsync(string jwt, UpdateUserPasswordDto userPasswordDto);
    }
}
