using Microsoft.AspNetCore.Mvc;
using LokalnieEU.Models.User;
using LokalnieEU.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LokalnieEU.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public readonly IConfiguration _configuration;

        public UsersController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<User>>> Register([FromBody] UserDto userDto)
        {
            var response = await _userService.Register(userDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<UserResponse>>> Login([FromBody] UserLoginDto userDto)
        {
            var response = await _userService.Authenticate(userDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [Authorize(Roles = "user, admin")]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<ServiceResponse<UserResponse>>> UpdateUser([FromBody] UpdateUserDto userDto)
        {
            try
            {
                // Pobierz token użytkownika z nagłówka autoryzacji
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var token = authorizationHeader.Substring("Bearer ".Length);

                var response = await _userService.UpdateUser(token, userDto);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new ServiceResponse<UserResponse> { Success = false, Message = ex.Message });
            }


            //try
            //{
            //    int userId = _userService.GetUserIdFromClaims();
            //    var response = await _userService.UpdateUser(userId, userDto);

            //    if (!response.Success)
            //    {
            //        return BadRequest(response);
            //    }

            //    return Ok(response);
            //}
            //catch (Exception ex)
            //{
            //    return Unauthorized(new ServiceResponse<User> { Success = false, Message = ex.Message });
            //}
        }
    }
}
