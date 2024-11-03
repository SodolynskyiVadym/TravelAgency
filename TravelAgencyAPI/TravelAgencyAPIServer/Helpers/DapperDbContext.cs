using System.Data;
using Microsoft.Data.SqlClient;

namespace TravelAgencyAPIServer.Helpers;

public class DapperDbContext
{ 
    public IDbConnection Connection { get; set; }
    
    public DapperDbContext(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }
}