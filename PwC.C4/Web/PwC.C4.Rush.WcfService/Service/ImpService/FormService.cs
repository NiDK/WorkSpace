using System;
using System.Collections.Generic;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Storage;
using PwC.C4.Rush.WcfService.Models;
using PwC.C4.Rush.WcfService.Service.Interface;
using PwC.C4.Rush.WcfService.Service.Persistance;

namespace PwC.C4.Rush.WcfService.Service.ImpService
{
    public class FormService : IFormService
    {


        #region Singleton

        private static FormService _instance = null;
        private static readonly object LockHelper = new object();

        public FormService()
        {
        }

        public static IFormService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance != null) return _instance;
                    _instance = new FormService();

                }
            }
            return _instance;
        }

        #endregion

        public List<FormMain> GetFormList(string keyword, int page, int rows, string sort, string order,
            out int totalCount)
        {
            var queryString = string.IsNullOrEmpty(keyword) ? "1=1" : $" FormName like '%{keyword}%'";
            var orderString = (string.IsNullOrEmpty(sort) || string.IsNullOrEmpty(order))
                ? "ModifyTime desc"
                : $"{sort} {order}";
            if (page < 1) page = 1;
            if (rows < 1) rows = 10;
            var datas = FormDao.GetFormList(rows, page, orderString, queryString, out totalCount);
            return datas;
        }

        public FormMain GetFormBaseInfo(Guid formId)
        {
            return FormDao.GetFormBaseInfo(formId);
        }

        public List<FormLayout> GetFormLayoutList()
        {
            return FormDao.GetFormLayoutList();
        }

        public int DeleteFormBaseInfo(Guid formId, string modifyBy)
        {
            return FormDao.DeleteFormBaseInfo(formId, modifyBy);
        }

        public Guid SaveFormBaseInfo(FormMain form)
        {
            if (form.Id == Guid.Empty)
            {
                form.Id = Guid.NewGuid();
            }
            var c = FormDao.SaveFormBaseInfo(form);
            if (c > 0)
            {
                return form.Id;
            }
            return Guid.Empty;

        }

        public FromRender RenderForm(string dataId, string aliasName, string prop=null)
        {
            var dic = new Dictionary<string, object>();
            var formInfo = FormDao.GetFormBaseInfoByAlias(aliasName);
            var layout = FormDao.GetLayoutHtml(formInfo.Layout);
            if (prop != null)
            {
                formInfo.Props = prop;
            }
            if (dataId == "PreviewData")
            {
                foreach (var property in formInfo.Properties)
                {
                    dic.Add(property, "XXX XXX XXX XXX XXX");
                }
            }
            else
            {
                var p = PwC.C4.Metadata.Storage.ProviderFactory.GetProvider<IEntityService>(formInfo.ConnName,
                    formInfo.EntityName);
                dic = p.GetEntity<DynamicMetadata>(dataId, formInfo.Properties).Properties;
            }

            var form = new FromRender
            {
                Data = dic,
                FormName = formInfo.FormName,
                Layout = layout,
                Controls = formInfo.FormStructure,
                Javascripts = formInfo.JavaScript,
                Styles =  formInfo.Styles
            };

            return form;
        }

        public int UpdateStructure(Guid formId, string userId, string prop, string javascript, string styles, List<FormControl> formControls)
        {
            return FormDao.UpdateStructure(formId, userId, prop, javascript, styles, formControls);
        }

        public FormMain GetFormBaseInfoByAlias(string alias)
        {
            return FormDao.GetFormBaseInfoByAlias(alias);
        }
    }
}
