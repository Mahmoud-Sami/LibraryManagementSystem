using LMS.Business.Commands.AddBook;
using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Helpers;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LMS.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in application settings.");

            builder.Services.AddScoped(_ => new DatabaseManager(connectionString));
            builder.Services.AddScoped<IBookRepository, BookRepository>(_ => new BookRepository(connectionString, new DatabaseManager(connectionString)));
            builder.Services.AddScoped<IUserRepository, UserRepository>(_ => new UserRepository(connectionString, new DatabaseManager(connectionString)));
            builder.Services.AddScoped<IBorrowRepositoy, BorrowRepositoy>(_ => new BorrowRepositoy(connectionString, new DatabaseManager(connectionString)));


            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AddBookCommand).Assembly);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/login";                          
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}