using LokalnieEU.Database;
using LokalnieEU.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LokalnieEU.Services
{
    /// <summary>
    /// The UserService class provides functionality for registering, authenticating, and updating users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        public readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Registers a new user into the system.
        /// </summary>
        /// <param name="userDto">The user information to register.</param>
        /// <returns>ServiceResponse containing the new User object if successful.</returns>
        public async Task<ServiceResponse<User>> Register(UserDto userDto)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);

            if (user != null)
            {
                return CreateResponse<User>(false, "User already exists.");
            }

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User newUser = CreateUser(userDto, passwordHash, passwordSalt, "User");

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreateResponse<User>(true, "User registered successfully.", newUser);
        }

        /// <summary>
        /// Authenticates a user and returns a response with user data and a token.
        /// </summary>
        /// <param name="userDto">The user login information.</param>
        /// <returns>ServiceResponse containing UserResponse object if successful.</returns>
        public async Task<ServiceResponse<UserResponse>> Authenticate(UserLoginDto userDto)
        {
            User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userDto.Email);

            if (user == null)
            {
                return CreateResponse<UserResponse>(false, "User not found.");
            }

            if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return CreateResponse<UserResponse>(false, "Password incorrect.");
            }

            user.LastLoginTime = DateTime.Now;
            await _context.SaveChangesAsync();

            string token = await CreateUserTokenAsync(user);
            UserResponse userResponse = new UserResponse(user, token);

            return CreateResponse<UserResponse>(true, "Authentication successful.", userResponse);
        }

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="token">The token of the user to update.</param>
        /// <param name="userDto">The new user information.</param>
        /// <returns>ServiceResponse containing UserResponse object if successful.</returns>
        public async Task<ServiceResponse<UserResponse>> UpdateUser(string token, UpdateUserDto userDto)
        {
            ServiceResponse<UserResponse> response = new ServiceResponse<UserResponse>();

            try
            {
                int userId = GetUserIdFromToken(token);

                User user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                }
                else
                {
                    user.Name = userDto.Name;
                    user.Surname = userDto.Surname;
                    user.Email = userDto.Email;
                    user.Phone = userDto.Phone;

                    if (!string.IsNullOrEmpty(userDto.Password))
                    {
                        CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                    }

                    user.LastModifiedTime = DateTime.Now;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    UserResponse userResponse = new UserResponse(user, token);
                    response.Data = userResponse;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <param name="token">The token of the user to update.</param>
        /// <param name="userPasswordDto">The old and new password information.</param>
        /// <returns>ServiceResponse containing a string message if successful.</returns>
        public async Task<ServiceResponse<string>> UpdateUserPasswordAsync(string token, UpdateUserPasswordDto userPasswordDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            try
            {
                int userId = GetUserIdFromToken(token);

                User user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                }
                else
                {
                    if (VerifyPasswordHash(userPasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                    {
                        CreatePasswordHash(userPasswordDto.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);
                        user.PasswordHash = newPasswordHash;
                        user.PasswordSalt = newPasswordSalt;

                        user.LastModifiedTime = DateTime.Now;

                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        response.Data = "Password changed";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Incorrect old password.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Creates a ServiceResponse with given parameters.
        /// </summary>
        /// <typeparam name="T">The type of data in the ServiceResponse.</typeparam>
        /// <param name="success">The success status of the response.</param>
        /// <param name="message">The message of the response.</param>
        /// <param name="data">The data of the response.</param>
        /// <returns>A new ServiceResponse object.</returns>
        private ServiceResponse<T> CreateResponse<T>(bool success, string message, T data = default)
        {
            return new ServiceResponse<T>
            {
                Success = success,
                Message = message,
                Data = data,
            };
        }

        /// <summary>
        /// Creates a User object from the provided UserDto and password hash and salt.
        /// </summary>
        /// <param name="userDto">The user information.</param>
        /// <param name="passwordHash">The hashed password.</param>
        /// <param name="passwordSalt">The salt for the password.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A new User object.</returns>
        private User CreateUser(UserDto userDto, byte[] passwordHash, byte[] passwordSalt, string role)
        {
            return new User
            {
                Name = userDto.Name,
                Surname = userDto.Surname,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Role = role,
                LastModifiedTime = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
        }

        /// <summary>
        /// Extracts the user's ID from the provided JWT token.
        /// </summary>
        /// <param name="token">The JWT token containing the user's ID.</param>
        /// <returns>The user's ID extracted from the token.</returns>
        private int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);

            return userId;
        }

        /// <summary>
        /// Checks if the provided email is associated with the specified role.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <param name="role">The role to validate against.</param>
        /// <returns>True if the email is associated with the role, false otherwise.</returns>
        private async Task<bool> IsEmailAllowedForRole(string email, string role)
        {
            var roleEmail = await _context.RoleEmails.FirstOrDefaultAsync(x => x.Email == email);
            if (roleEmail == null)
            {
                return false;
            }

            return roleEmail.Role.Contains(role);
        }

        /// <summary>
        /// Generates a password hash and salt for the provided password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="passwordHash">The output password hash.</param>
        /// <param name="passwordSalt">The output password salt.</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies if a provided password matches the stored hash and salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="storedHash">The stored password hash.</param>
        /// <param name="storedSalt">The stored password salt.</param>
        /// <returns>True if the password matches the stored hash and salt, false otherwise.</returns>
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Creates a JWT token for a given user with the specified role. 
        /// </summary>
        /// <param name="user">The user for whom the token is to be created.</param>
        /// <param name="role">The role of the user. Default is "user".</param>
        /// <param name="morePermission">If true, checks if the email is allowed for the role and assigns the role if allowed. Default is false.</param>
        /// <returns>A JWT token for the user.</returns>
        private async Task<string> CreateUserTokenAsync(User user, string role = "user", bool morePermission = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
                    new Claim(ClaimTypes.Name, user.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token wygasa po 7 dniach
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            if (morePermission && await IsEmailAllowedForRole(user.Email, role))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            else
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "user"));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
