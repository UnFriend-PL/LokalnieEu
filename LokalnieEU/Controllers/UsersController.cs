using Microsoft.AspNetCore.Mvc;
using LokalnieEU.Models.User;
using LokalnieEU.Services;

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
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] UserLoginDto userDto)
        {
            var response = await _userService.Authenticate(userDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        //private string CreateUserToken(User user)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    {
        //        new Claim(ClaimTypes.Name, user.Login);
        //        new Claim(ClaimTypes.Role, "User");
        //    };

        //    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)); 
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds
        //    );
        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        //    return jwt;
        //}
    }
}
