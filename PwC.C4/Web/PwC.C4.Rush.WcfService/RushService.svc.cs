using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using PwC.C4.Configuration.Data;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Model;
using PwC.C4.Metadata.Storage;
using PwC.C4.Rush.WcfService.Models;
using PwC.C4.Rush.WcfService.Service.ImpService;
using PwC.C4.Rush.WcfService.Service.Persistance;

namespace PwC.C4.Rush.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RushService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RushService.svc or RushService.svc.cs at the Solution Explorer and start debugging.
    public class RushService : IRushService
    {

        public Tuple<string, string, MemoryStream> DownloadContentFile(string aliasName, string fileid)
        {
            try
            {
                var form = FormService.Instance().GetFormBaseInfoByAlias(aliasName);
                var conn = ConnectionStringProvider.GetConnectionString(form.ConnName);
                var dbName = MongoUrl.Create(conn).DatabaseName;
                var client = new MongoClient(conn);

                var database = client.GetDatabase(dbName);
                var id = new ObjectId(fileid);
                GridFSBucket bucket = new GridFSBucket(database);
                IGridFSBucket g = bucket;


                IAsyncCursor<GridFSFileInfo> gfsi = g.Find(new BsonDocument() {{"_id", id}});
                GridFSFileInfo fi = gfsi.FirstOrDefault();

                MemoryStream ms = new MemoryStream();
                g.DownloadToStream(fi.Id, ms);

                var fileconentType = MimeMapping.GetMimeMapping(fi.Filename);
                Tuple<string, string, MemoryStream> tuple = new Tuple<string, string, MemoryStream>(fi.Filename,
                    fileconentType, ms);

                return tuple;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetLinkTrackingUrl(string notesUrl)
        {
            return LinkTrackingDao.GetLinkTrackingUrl(notesUrl);
        }


        public Tuple<string, string, MemoryStream> DownloadFile(string aliasName, string fileId)
        {
            var form = FormService.Instance().GetFormBaseInfoByAlias(aliasName);
            var file =
                ProviderFactory.GetProvider<IAttachmentService>(form.ConnName, form.EntityName)
                    .GetEntityAttachments<DynamicMetadata>(new List<Guid>() {new Guid(fileId)}, true, false);
            var filename = "";
            var fileExtname = "";
            MemoryStream ms = new MemoryStream();
            if (file != null && file.Any())
            {
                Attachment f = file.First();
                filename = f.FileName;
                fileExtname = f.FileExtName;

                f.Stream.CopyTo(ms);
            }
            Tuple<string, string, MemoryStream> tuple = new Tuple<string, string, MemoryStream>(filename, fileExtname,
                ms);
            return tuple;
        }

        public List<FormMain> GetFormList(string keyword, int page, int rows, string sort, string order,
            out int totalCount)
        {
            return FormService.Instance().GetFormList(keyword, page, rows, sort, order, out totalCount);
        }

        public FormMain GetFormBaseInfo(Guid formId)
        {
            return FormService.Instance().GetFormBaseInfo(formId);
        }

        public List<FormLayout> GetFormLayoutList()
        {
            return FormService.Instance().GetFormLayoutList();
        }

        public int DeleteFormBaseInfo(Guid formId, string modifyBy)
        {
            return FormService.Instance().DeleteFormBaseInfo(formId, modifyBy);
        }

        public Guid SaveFormBaseInfo(FormMain form)
        {
            return FormService.Instance().SaveFormBaseInfo(form);
        }

        public FromRender RenderForm(string dataId, string aliasName, string prop = null)
        {
            return FormService.Instance().RenderForm(dataId, aliasName, prop);
        }

        public int UpdateStructure(Guid formId, string userId, string prop, string javascript, string styles,
            List<FormControl> formControls)
        {
            return FormService.Instance().UpdateStructure(formId, userId, prop, javascript, styles, formControls);
        }
    }
}
