using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.DataService.Model;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.DataService.Helpers
{

    public static class MailHelper
    {
        public static DataTable ToTable(this MailAttachment attachment)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AttachmentName", typeof(string));
            dataTable.Columns.Add("AttachmentFileName", typeof(string));
            dataTable.Columns.Add("AttachmentContent", typeof(byte[]));
            dataTable.Columns.Add("AttachmentMIMEType", typeof(string));
            dataTable.Columns.Add("LinkedResourceFlag", typeof(bool));
            var dataRow = dataTable.NewRow();
            dataRow["AttachmentName"] = attachment.Name;
            dataRow["AttachmentFileName"] = attachment.FileName;
            dataRow["AttachmentContent"] = attachment.Content;
            dataRow["AttachmentMIMEType"] = attachment.MimeType;
            dataRow["LinkedResourceFlag"] = attachment.LinkedResourceFlag;
            dataTable.Rows.Add(dataRow);
            return dataTable;
        }

        public static DataTable ToTable(this List<MailAttachment> attachments)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AttachmentName", typeof(string));
            dataTable.Columns.Add("AttachmentFileName", typeof(string));
            dataTable.Columns.Add("AttachmentContent", typeof(byte[]));
            dataTable.Columns.Add("AttachmentMIMEType", typeof(string));
            dataTable.Columns.Add("LinkedResourceFlag", typeof(bool));
            attachments.ForEach(attachment =>
            {
                var dataRow = dataTable.NewRow();
                dataRow["AttachmentName"] = attachment.Name;
                dataRow["AttachmentFileName"] = attachment.FileName;
                dataRow["AttachmentContent"] = attachment.Content;
                dataRow["AttachmentMIMEType"] = attachment.MimeType;
                dataRow["LinkedResourceFlag"] = attachment.LinkedResourceFlag;
                dataTable.Rows.Add(dataRow);
            });

            return dataTable;
        }
        public static DataTable ToTable(this List<int> attachmentIds)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            attachmentIds.ForEach(id =>
            {
                var dataRow = dataTable.NewRow();
                dataRow["Id"] = id;
                dataTable.Rows.Add(dataRow);
            });

            return dataTable;
        }

    }
}
