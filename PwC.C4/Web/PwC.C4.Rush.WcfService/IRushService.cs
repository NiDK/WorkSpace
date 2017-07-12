using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRushService" in both code and config file together.
    [ServiceContract]
    public interface IRushService
    {
        [OperationContract]
        Tuple<string, string, MemoryStream> DownloadContentFile(string aliasName, string fileid);

        [OperationContract]
        string GetLinkTrackingUrl( string notesUrl);

        [OperationContract]
        Tuple<string, string, MemoryStream> DownloadFile(string aliasName, string fileid);

        [OperationContract]
        List<FormMain> GetFormList(string keyword, int page, int rows, string sort, string order, out int totalCount);

        [OperationContract]
        FormMain GetFormBaseInfo(Guid formId);

        [OperationContract]
        List<FormLayout> GetFormLayoutList();

        [OperationContract]
        int DeleteFormBaseInfo(Guid formId, string modifyBy);

        [OperationContract]
        Guid SaveFormBaseInfo(FormMain form);

        [OperationContract]
        FromRender RenderForm(string dataId, string aliasName, string prop = null);

        [OperationContract]
        int UpdateStructure(Guid formId, string userId, string prop, string javascript, string styles, List<FormControl> formControls);
    }
}
