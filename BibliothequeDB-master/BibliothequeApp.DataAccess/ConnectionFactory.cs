using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace BibliothequeApp.DataAccess
{
    public static class ConnectionFactory
    {
        private static IConfigurationRoot? _config;

        static ConnectionFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            _config = builder.Build();
        }

        public static SqlConnection CreateConnection()
        {
            string cs = _config.GetConnectionString("BibliothequeDB");
            return new SqlConnection(cs);
        }
    }
}