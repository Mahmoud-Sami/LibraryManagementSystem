using LMS.Business.Commands.AddBook;
using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Helpers;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using LMS.UI.Desktop.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.UI.Desktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();

            ConfigureServices(services);

            using (ServiceProvider sp = services.BuildServiceProvider())
            {
                var frmMain = sp.GetRequiredService<frmMain>();
                Application.Run(frmMain);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Server=localhost;Database=library_db;Uid=root;Pwd=admin";
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AddBookCommand).Assembly);
            });
            services.AddScoped<frmMain>();
            services.AddScoped<frmRegister>();
            services.AddScoped<frmBooks>();

            services.AddScoped(_ => new DatabaseManager(connectionString));
            services.AddScoped<IBookRepository, BookRepository>(_ => new BookRepository(connectionString, new DatabaseManager(connectionString)));
            services.AddScoped<IUserRepository, UserRepository>(_ => new UserRepository(connectionString, new DatabaseManager(connectionString)));

        }
    }
}