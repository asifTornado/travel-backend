using System;
using Microsoft.Data.SqlClient;
using Dapper;

namespace backEnd.Factories.IFactories
{
    public interface IConnection
    {
        SqlConnection GetConnection();
    }
}