using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Middlewares;
using ToDoListAPI.Repositories;
using ToDoListAPI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace ToDoListApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            builder.Services.AddScoped<ITodoService, TodoService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidateAudience = true,
             ValidAudience = builder.Configuration["Jwt:Audience"],
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
         };
     });
            builder.Services.AddAuthorization();

            // Настроить политику CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyCorsPolicy", policy =>
                {
                    policy.WithOrigins("http://example.com") // Разрешить запросы с указанного домена
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseLogging();

            app.UseHttpsRedirection();
            app.UseCors("MyCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
