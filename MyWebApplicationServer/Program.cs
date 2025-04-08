using Microsoft.EntityFrameworkCore;
using MyWebApplicationServer.Data;

namespace Project.MyWebApplicationServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin() // Тут сделать для приложения от мобилки
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(5164);
                options.ListenAnyIP(7205, listenOptions =>
                {
                    listenOptions.UseHttps();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<LibraryContext>(opt =>
                            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next();
            });


            app.MapControllers();

            app.Run();
        }
    }
}
