namespace PwC.C4.Configuration.Data
{
    public class ConnectionStringProvider
    { 
        public static string GetConnectionString(string key)
        {
            return ConnectionStringCollection.Instances[key];
        }
    }
}
