using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    public static class PreferenceDao
    {

        internal static string Get(string appcode,string key)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            var data = SafeProcedure.ExecuteScalar(db, "dbo.Preference_Get", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@appcode", appcode);
                parameters.AddWithValue("@key", key);
            });
            return data == null ? "" : data.ToString();
        }

        internal static bool Set(string appcode, string key, string value)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            return SafeProcedure.ExecuteNonQuery(db, "dbo.Preference_Set", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@appcode", appcode);
                parameters.AddWithValue("@key", key);
                parameters.AddWithValue("@value", value);
            }) > 0;
        }

        internal static bool DeleteKey(string appcode, string key)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);
            return SafeProcedure.ExecuteNonQuery(db, "dbo.Preference_Delete", delegate(IParameterSet parameters)
            {
                parameters.AddWithValue("@appcode", appcode);
                parameters.AddWithValue("@key", key);
            }) > 0;
        }
    }
}
