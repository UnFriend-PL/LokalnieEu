using LokalnieEU.Database;
using LokalnieEU.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace LokalnieEU.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        public readonly IConfiguration _configuration;

        public UserService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<User>> Register(UserDto userDto)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);

            if (user != null)
            {
                response.Success = false;
                response.Message = "User already exists.";
            }
            else
            {
                User newUser = new User
                {
                    Name = userDto.Name,
                    Surname = userDto.Surname,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                    Role = "User", // domyślna rola przy rejestracji
                    LastModifiedTime = DateTime.Now,
                };

                CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt;

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                response.Data = newUser;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Authenticate(UserLoginDto userDto)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            User user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userDto.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Password incorrect.";
            }
            else
            {
                user.LastLoginTime = DateTime.Now;
                await _context.SaveChangesAsync();
                response.Data = await CreateUserTokenAsync(user); // metoda tworzenia tokena
            }

            return response;
        }
        private async Task<bool> IsEmailAllowedForRole(string email, string role)
        {
            var roleEmail = await _context.RoleEmails.FirstOrDefaultAsync(x => x.Email == email);
            if (roleEmail == null)
            {
                return false;
            }

            return roleEmail.Role.Contains(role);
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

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
        private async Task<string> CreateUserTokenAsync(User user, string role = "user", bool morePermission = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value!); // Uwaga: Twój klucz powinien być przechowywany bezpiecznie, np. w pliku konfiguracyjnym
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Email.ToString()),
                    // Możesz dodać więcej claimów według potrzeb
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token wygasa po 7 dniach
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            if (morePermission && await IsEmailAllowedForRole(user.Email, role))
            {
                // Ustaw rolę na podaną wartość, jeśli morePermission jest true i email jest dozwolony dla roli
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            else
            {
                // Ustaw domyślną rolę jako "user"
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "user"));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
