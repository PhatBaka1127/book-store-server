using BookStore.API.Extension;
using BookStore.Business.Helper;
using BookStore.Business.Middleware;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDependencyInjection(builder.Configuration);
builder.Services.AddSwaggerServices(configuration);
builder.Services.AddJWTServices(configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                      });
});
builder.Services.AddControllers()
 .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.

app.MapControllers();

app.Run();
