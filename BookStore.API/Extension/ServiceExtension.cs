using BookStore.Business.Dto;
using BookStore.Business.Service.Implement;
using BookStore.Business.Service.Interface;
using BookStore.Data.Helper;
using BookStore.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace BookStore.API.Extension
{
    public static class ServiceExtension
    {
        public static async Task<ThisUserObj> GetThisUserInfo(HttpContext httpContext, IUserService _userService)
        {
            ThisUserObj currentUser = new();

            var checkUser = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber);
            currentUser.userId = int.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber).Value);
            currentUser.email = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            currentUser.role = int.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value);

            var existedUser = await _userService.GetUserByEmailAsync(currentUser.email);
            if (existedUser == null)
            {
                throw new NotFoundException("Không tìm thấy user này");
            }

            return currentUser;
        }

        public static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(BookStoreContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        public static void AddSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vouchee.API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer YOUR_TOKEN_HERE\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public static void AddJWTServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
