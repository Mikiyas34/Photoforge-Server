namespace Photoforge_Server;

public class Config
{
    public static string DbConnectionString() { return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PhotoforgeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; }
}
