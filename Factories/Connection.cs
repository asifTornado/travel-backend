
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backEnd.Factories.IFactories;

namespace backEnd.Factories;

    internal sealed class Connection : IConnection
    {
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=true;TrustServerCertificate=True";

      

        public SqlConnection GetConnection()
        {
            return new SqlConnection(
                connectionString);
        }
    }
