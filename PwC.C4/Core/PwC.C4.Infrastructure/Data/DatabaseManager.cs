using System;

namespace PwC.C4.Infrastructure.Data
{
    internal class DatabaseManager
    {
        private static DatabaseManager instance = new DatabaseManager();

        private DatabaseCollection databases;

        internal DatabaseManager()
        {
            databases = new DatabaseCollection();
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        internal static DatabaseManager Instance
        {
            get { return instance; }
        }

        internal Database GetDatabase(string instanceName,string conntectString =null)
        {
            if (!databases.Contains(instanceName))
            {
                lock (databases)
                {
                    Database database = new Database(instanceName, conntectString);

                    if (!databases.Contains(instanceName))
                    {
                        databases.Add(database);
                    }
                }
            }

            return databases[instanceName];
        }


        internal DatabaseCollection Databases
        {
            get { return databases; }
        }

    }
}