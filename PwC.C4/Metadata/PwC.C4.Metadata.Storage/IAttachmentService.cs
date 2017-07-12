using System;
using System.Collections.Generic;
using System.IO;
using PwC.C4.Metadata.Model;

namespace PwC.C4.Metadata.Storage
{
    public interface IAttachmentService
    {
        Guid SaveEntityAttachment<T>(string fileName, string fileExtName, string userId,Stream stream);

        List<Attachment> GetEntityAttachments<T>(List<Guid> fileIds, bool withStream = false, bool isCreateFileToLocal = false);

        List<Attachment> GetEntityAttachments<T>(List<string> fileIds, bool withStream = false, bool isCreateFileToLocal = false);

    }
}
