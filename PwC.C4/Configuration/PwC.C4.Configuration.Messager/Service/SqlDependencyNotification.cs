using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Configuration.Data;
using PwC.C4.Configuration.Messager.Service.Interface;
using PwC.C4.Configuration.Messager.Service.ServiceImp;

namespace PwC.C4.Configuration.Messager.Service
{
    public class SqlDependencyNotification : IDisposable
    {
        private readonly string _sampleConnectionString;
        private readonly string _notificationStoredProcedure;
        private SqlDependency _sampleSqlDependency;
        private SqlCommand _sampleSqlCommand;
        private SqlConnection _sampleSqlConnection;
        private Guid _configDetailId;
        private IConfigurationService _iService = null;
        public SqlDependencyNotification()
        {
            this._sampleConnectionString = ConnectionStringProvider.GetConnectionString(DatabaseInstance.C4Base);
            this._notificationStoredProcedure = "dbo.ConfigurationDetail_DataChange";
            _iService = ConfigurationService.Instance();
        }

        public void Dispose()
        {
            if (null != this._sampleSqlDependency)
            {
                this._sampleSqlDependency.OnChange -= null;
            }

            this._sampleSqlCommand?.Dispose();
            this._sampleSqlConnection?.Dispose();
            this._sampleSqlDependency = null;
            this._sampleSqlCommand = null;
            this._sampleSqlConnection = null;

            SqlDependency.Stop(this._sampleConnectionString);
        }

        public void RegisterDependencyUsingDefaultQueue()
        {
            SqlDependency.Stop(this._sampleConnectionString);
            SqlDependency.Start(this._sampleConnectionString);
            this.ConfigureDependencyUsingStoreProcedureAndDefaultQueue();
        }

        private void ConfigureDependencyUsingStoreProcedureAndDefaultQueue()
        {
            if (null != this._sampleSqlDependency)
            {
                this._sampleSqlDependency.OnChange -= null;
            }
            this._sampleSqlCommand?.Dispose();
            this._sampleSqlConnection?.Dispose();
            this._sampleSqlDependency = null;
            this._sampleSqlCommand = null;
            this._sampleSqlConnection = null;
            this._sampleSqlConnection = new SqlConnection(this._sampleConnectionString);
            this._sampleSqlCommand = new SqlCommand
            {
                Connection = this._sampleSqlConnection,
                CommandType = CommandType.StoredProcedure,
                CommandText = this._notificationStoredProcedure,
                Notification = null
            };

            this._sampleSqlDependency = new SqlDependency(this._sampleSqlCommand);
            this._sampleSqlDependency.OnChange += this.SqlDependencyOnChange;
            this._sampleSqlCommand.Connection.Open();
            using (IDataReader reader = this._sampleSqlCommand.ExecuteReader(CommandBehavior.CloseConnection))
            {
                if (reader.Read())
                    Guid.TryParse(reader["id"].ToString(), out _configDetailId);
            }
            ;

            this._sampleSqlCommand?.Dispose();
            this._sampleSqlConnection?.Dispose();
        }

        private void SqlDependencyOnChange(object sender, SqlNotificationEventArgs eventArgs)
        {
            this.ConfigureDependencyUsingStoreProcedureAndDefaultQueue();
            if (eventArgs.Info == SqlNotificationInfo.Insert)
            {
                var detail = _iService.ConfigurationDetail_GetEntityById(_configDetailId, "download");
                _iService.SaveFileToServer(detail);
            }
        }
    }
}
