using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.WcfService.Service.Persistance
{
    internal class LinkTrackingDao
    {
        private static readonly LogWrapper Log = new LogWrapper();
        #region MongoDB Operation

        public static IMongoClient Client;
        public static IMongoDatabase Database;

        public static IMongoDatabase GetDatabase(string connection)
        {
            var conn = AppSettings.Instance.GetConntectStringV2(connection);
            var dbName = MongoUrl.Create(conn).DatabaseName;
            Client = new MongoClient(conn);

            Database = Client.GetDatabase(dbName);
            return Database;
        }
        #endregion


        public static string GetLinkTrackingUrl(string notesUrl)
        {
            try
            {
                var lastid = notesUrl.Split('/').LastOrDefault();

                var mongodb = GetDatabase(System.Configuration.ConfigurationManager.AppSettings["LinkTrackingConn"]);
                var linkcollection =
                    mongodb.GetCollection<LinkTracking>(
                        System.Configuration.ConfigurationManager.AppSettings["Linkcollection"]);
                var appservercolletion = mongodb.GetCollection<ApplicationCodeWebServer>(
                       System.Configuration.ConfigurationManager.AppSettings["Appwebservercollection"]);
                var applist = appservercolletion.AsQueryable().Select(m => m.ApplicationCode).Distinct().ToList();
                var filter = Builders<LinkTracking>.Filter.Eq("NotesDocID", lastid) & Builders<LinkTracking>.Filter.In("APPCode", applist);
            
                var link = linkcollection.Find(filter).ToListAsync().Result.FirstOrDefault();
                if (link?.APPCode != null)
                {
                    var serverfilter = Builders<ApplicationCodeWebServer>.Filter.Eq("ApplicationCode", link.APPCode) & Builders<ApplicationCodeWebServer>.Filter.Eq("FormName", link.FormName);
                    var webserver = appservercolletion.Find(serverfilter).ToListAsync().Result.FirstOrDefault();
                    if (webserver != null)
                    {
                        var url = webserver.WebServer + link.MongoDocID;
                        return url;
                    }
                    return "";
                }
                else
                {
                    return notesUrl;
                }
            }
            catch (Exception ex)
            {
                Log.Error("GetLinkTrackingUrl error,url:" + notesUrl, ex);
                return "";
            }
        }
    }
}
