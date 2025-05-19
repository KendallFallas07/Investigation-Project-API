using Microsoft.Data.SqlClient;
using DotNetEnv;

class ConnectionSQL
{
    protected static SqlConnection GetConnect()
    {
        string host = Env.GetString("HOST");
        string port = Env.GetString("PORT");
        string user = Env.GetString("USER");
        string password = Env.GetString("PASSWORD");
        string database = Env.GetString("DATABASE");
        string connectionString = $"Server={host},{port};Database={database};User Id={user};Password={password};TrustServerCertificate=True;";

        return new SqlConnection(connectionString);
    }
}