﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
﻿using System.Linq;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Helpers;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.Infrastructure.BaseLogger;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
﻿using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.DataService.Persistance
{
    internal static class MailMasterDao
    {
        private static readonly LogWrapper Log = new LogWrapper();

        public static int InsertToMailQueue(MailQueueModel model)
        {
            try
            {
                Database db = Database.GetDatabase(DatabaseInstance.C4Base);
                return SafeProcedure.ExecuteNonQuery(db,
                    "dbo.insert_MAILQUEUE"
                    , parameters =>
                    {
                        parameters.AddWithValue("@mAPPCODE", model.AppCode);
                        parameters.AddWithValue("@mCC", model.MailCc);
                        parameters.AddWithValue("@mREPLYTO", model.ReplyTo);
                        parameters.AddWithValue("@mBCC", model.MailBcc);
                        parameters.AddWithValue("@mORGANISATION", model.Organisation);
                        parameters.AddWithValue("@mSUBJECT", model.Subject);
                        parameters.AddWithValue("@mSUBMITBY", model.SubmitBy);
                        parameters.AddWithValue("@mSUBMITDATE", DateTime.Now);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@mSENDDATE", model.SendDate);
                        parameters.AddWithValue("@mIMMEDIATEFLAG", model.ImmediateFlag);
                        parameters.AddWithValue("@mMTO", model.MailTo);
                        parameters.AddWithValue("@mMFROM", model.MailFrom);
                        parameters.AddWithValue("@mContent", model.Content);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@encode", "utf-8");
                    });
            }
            catch (Exception ee)
            {
                Log.Error("InsertToMailQueue error,data:" + JsonHelper.Serialize(model), ee);
                return -1;
            }
           
        }

        public static int InsertToMailQueueWithAttachment(MailQueueModel model, List<MailAttachment> attachments)
        {
            try
            {
                Database db = Database.GetDatabase(DatabaseInstance.C4Base);
                var data = SafeProcedure.ExecuteScalar(db,
                    "dbo.insert_MAILQUEUEWithAttachment"
                    , parameters =>
                    {
                        parameters.AddWithValue("@mAPPCODE", model.AppCode);
                        parameters.AddWithValue("@mCC", model.MailCc);
                        parameters.AddWithValue("@mREPLYTO", model.ReplyTo);
                        parameters.AddWithValue("@mBCC", model.MailBcc);
                        parameters.AddWithValue("@mORGANISATION", model.Organisation);
                        parameters.AddWithValue("@mSUBJECT", model.Subject);
                        parameters.AddWithValue("@mSUBMITBY", model.SubmitBy);
                        parameters.AddWithValue("@mSUBMITDATE", DateTime.Now);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@mSENDDATE", model.SendDate);
                        parameters.AddWithValue("@mIMMEDIATEFLAG", model.ImmediateFlag);
                        parameters.AddWithValue("@mMTO", model.MailTo);
                        parameters.AddWithValue("@mMFROM", model.MailFrom);
                        parameters.AddWithValue("@mContent", model.Content);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@encode", "utf-8");
                        parameters.AddWithValue("@attachments",attachments.ToTable());
                    });
                int r = -1;
                int.TryParse(data.ToString(), out r);
                return r;
            }
            catch (Exception ee)
            {
                Log.Error("InsertToMailQueue error,data:" + JsonHelper.Serialize(model), ee);
                return -1;
            }
        }

        public static int InsertToMailQueueWithAttachment(MailQueueModel model, List<int> attachments)
        {
            try
            {
                Database db = Database.GetDatabase(DatabaseInstance.C4Base);
                var data = SafeProcedure.ExecuteScalar(db,
                    "dbo.insert_MAILQUEUEWithAttachmentId"
                    , parameters =>
                    {
                        parameters.AddWithValue("@mAPPCODE", model.AppCode);
                        parameters.AddWithValue("@mCC", model.MailCc);
                        parameters.AddWithValue("@mREPLYTO", model.ReplyTo);
                        parameters.AddWithValue("@mBCC", model.MailBcc);
                        parameters.AddWithValue("@mORGANISATION", model.Organisation);
                        parameters.AddWithValue("@mSUBJECT", model.Subject);
                        parameters.AddWithValue("@mSUBMITBY", model.SubmitBy);
                        parameters.AddWithValue("@mSUBMITDATE", DateTime.Now);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@mSENDDATE", model.SendDate);
                        parameters.AddWithValue("@mIMMEDIATEFLAG", model.ImmediateFlag);
                        parameters.AddWithValue("@mMTO", model.MailTo);
                        parameters.AddWithValue("@mMFROM", model.MailFrom);
                        parameters.AddWithValue("@mContent", model.Content);
                        parameters.AddWithValue("@mPOSTEDFLAG", "N");
                        parameters.AddWithValue("@encode", "utf-8");
                        parameters.AddWithValue("@attachmentIds", attachments.ToTable());
                    });
                int r = -1;
                int.TryParse(data.ToString(), out r);
                return r;
            }
            catch (Exception ee)
            {
                Log.Error("InsertToMailQueue error,data:" + JsonHelper.Serialize(model), ee);
                return -1;
            }
        }

        public static int InsertToMailAttachment(MailAttachment attachment)
        {
            try
            {
                Database db = Database.GetDatabase(DatabaseInstance.C4Base);
                var data = SafeProcedure.ExecuteScalar(db,
                    "dbo.insert_MAILATTACHMENT"
                    , parameters =>
                    {
                        parameters.AddWithValue("@AttachmentName", attachment.Name);
                        parameters.AddWithValue("@AttachmentFileName", attachment.FileName);
                        parameters.AddWithValue("@AttachmentContent", attachment.Content);
                        parameters.AddWithValue("@AttachmentMIMEType", attachment.MimeType);
                        parameters.AddWithValue("@LinkedResourceFlag", attachment.LinkedResourceFlag);
                    });
                int r = -1;
                int.TryParse(data.ToString(), out r);
                return r;
            }
            catch (Exception ee)
            {
                Log.Error("InsertToMailQueue error,data:" + JsonHelper.Serialize(attachment), ee);
                return -1;
            }
        }

        public static int InsertToMailQueueBatch(List<MailQueueModel> model)
        {
            #warning 临时用一下循环。。。。
            return model.Any() ? model.Select(InsertToMailQueue).Count(data => data > 0) : 0;
        }
    }
}
