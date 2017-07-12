using System;
using System.Collections.Generic;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.WcfService.Service.Interface
{
    public interface IFormService
    {
        List<FormMain> GetFormList(string keyword, int page, int rows, string sort, string order, out int totalCount);

        FormMain GetFormBaseInfo(Guid formId);
        List<FormLayout> GetFormLayoutList();
        int DeleteFormBaseInfo(Guid formId, string modifyBy);
        Guid SaveFormBaseInfo(FormMain form);
        FromRender RenderForm(string dataId, string aliasName, string prop=null);
        int UpdateStructure(Guid formId, string userId, string prop, string javascript, string styles, List<FormControl> formControls);
        FormMain GetFormBaseInfoByAlias(string alias);
    }
}
