using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UI.Desktop
{
    internal static class Shared
    {
        public static string ConnectionString = "Server=localhost;Database=library_db;Uid=root;Pwd=admin";
        
        public static IUserRepository GetUserRepository()
        {
            return new UserRepository(ConnectionString, new DataAccess.Helpers.DatabaseManager(ConnectionString));
        }
    }
}
