using LokalnieEU.Database;
using LokalnieEU.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace LokalnieEU
{
    public class Server
    {
        public static void Main(string[] args)
        {
            // Start building the web application
            var builder = WebApplication.CreateBuilder(args);

            // Configure Cross-Origin Resource Sharing (CORS) for local development
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:8080")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();

            // Configure the database context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Configure JWT authentication
            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
                };
            });

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Enable Swagger in development environment
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use routing before authorization
            app.UseRouting();

            // Use CORS after routing, but before authorization
            app.UseCors("MyPolicy");

            // Use authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Define endpoints after authorization using endpoint registration
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}