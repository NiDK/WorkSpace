using System;
using System.Collections.Generic;
using System.Web;

using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Service;
using PwC.C4.TemplateEngine.Model;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.TemplateEngine.Extensions
{
    public static class HtmlExtend
    {
        private static Dictionary<string, MetadataControl> _metadataControlDic;
        private static Dictionary<string, MetadataEntityControl> _metadataEntityControlDic;
        private static PwC.C4.Infrastructure.Logger.LogWrapper _log = new LogWrapper();
        public static MetadataControl BuildControl(string entityName, string columnName, string datasourceGroup = null,
            string className = null,
            object attrs = null)
        {
            var key = string.Format("{0}-{1}-{2}", entityName, columnName, datasourceGroup ?? "");
            if (_metadataControlDic == null)
            {
                _metadataControlDic = new Dictionary<string, MetadataControl>();
            }
            if (_metadataControlDic.ContainsKey(key) && !CacheHelper.GetCacheItem<bool>("RemoveCache-MetadataChanged-HtmlExtend_metadataControlDic"))
            {
                
                return _metadataControlDic[key];
            }
            else
            {
                var render = new MetadataRender();
                var col = render.BuildControl(entityName, columnName, datasourceGroup, className, attrs);
                if (_metadataControlDic.ContainsKey(key))
                {
                    _metadataControlDic[key] = col;
                }
                else
                {
                    _metadataControlDic.Add(key, col);
                }
                CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-HtmlExtend_metadataControlDic", false);
                return col;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="isDefaultControl">Model auto build control,does not need build control by GenerateHtml</param>
        /// <returns></returns>
        public static MetadataEntityControl GetMetadataEntityControl(string entityName, bool isDefaultControl = false)
        {
            if (_metadataEntityControlDic == null)
            {
                _metadataEntityControlDic = new Dictionary<string, MetadataEntityControl>();
            }
            if (_metadataEntityControlDic.ContainsKey(entityName) && !CacheHelper.GetCacheItem<bool>("RemoveCache-MetadataChanged-HtmlExtend_metadataEntityControlDic"))
            {
               
                return _metadataEntityControlDic[entityName];
            }
            else
            {
                var entity = MetadataSettings.Instance.GetEntity(entityName);
                var ent = new MetadataEntityControl
                {
                    EntityName = entityName,
                    EnttiyDesc = entity.Description,
                    Controls = new List<MetadataControl>()
                };
                if (isDefaultControl)
                    entity.Columns.ForEach(col => ent.Controls.Add(BuildControl(entityName, col.Name)));
                if (_metadataEntityControlDic.ContainsKey(entityName))
                {
                    _metadataEntityControlDic[entityName] = ent;
                }
                else
                {
                    _metadataEntityControlDic.Add(entityName, ent);
                }
                CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-HtmlExtend_metadataEntityControlDic", false);
                return ent;
            }

        }

        public static IHtmlString GetHtmlSnippet(string code)
        {
            var item= HtmlSnippetService.Instance().GetHtmlSnippet(code);
            if (item != null)
            {
                return new HtmlString(item.Html);
            }
            return new HtmlString("");
        }

        //public static MetadataEntityControl GetMetadataEntityControl<T>(PageMode mode, string dataId = null,
        //    bool isDefaultControl = false) where T : DynamicMetadata
        //{
        //    var entityName = MetadataHelper.GetEntityName<T>();

        //    Guid gid;
        //    var isData = Guid.TryParse(dataId, out gid);
        //    if (isData)
        //    {
        //        var entity = MetadataSettings.Instance.GetEntity(entityName);
        //        var ent = new MetadataEntityControl
        //        {
        //            EntityName = entityName,
        //            EnttiyDesc = entity.Description,
        //            Controls = new List<MetadataControl>(),
        //            DataId = dataId,
        //            Mode = mode
        //        };
        //        var iService = ProviderFactory.GetProvider<IEntityService>();
        //        switch (mode)
        //        {
        //            case PageMode.Edit:                    
        //                ent.Metadata = iService.GetEntity<T>(gid);
        //                break;
        //            case PageMode.Preview:
        //                ent.Metadata = iService.GetEntityTranslated<T>(gid);
        //                break;
        //        }
        //        return ent;
        //    }
        //    else
        //    {
        //        return GetMetadataEntityControl(entityName, isDefaultControl);
        //    }

        //}

        /// <summary>
        /// GetMetadataEntityControl
        /// </summary>
        /// <typeparam name="T">Model from DynamicMetadata</typeparam>
        /// <param name="mode">PageMode</param>
        /// <param name="dataId"></param>
        /// <param name="data">GetEntity<T>(gid)/GetEntityTranslated<T>(gid)</param>
        /// <param name="isDefaultControl"></param>
        /// <returns></returns>
        public static MetadataEntityControl GetMetadataEntityControl<T>(PageMode mode, string dataId = null, T data = null,
           bool isDefaultControl = false,string entityName=null) where T : DynamicMetadata
        {
            var _entityName = MetadataHelper.GetEntityName<T>(entityName);

            try
            {
                var entity = MetadataSettings.Instance.GetEntity(_entityName);
                var ent = new MetadataEntityControl
                {
                    EntityName = _entityName,
                    EnttiyDesc = entity.Description,
                    Controls = new List<MetadataControl>(),
                    DataId = dataId,
                    Mode = mode
                };

                if (mode != PageMode.Add && data != null)
                {
                    ent.Metadata = data;
                }

                return ent;
            }
            catch (Exception ee)
            {
                _log.Error("GetMetadataEntityControl error!", ee);
                return GetMetadataEntityControl(_entityName, isDefaultControl);
            }

        }

        public static void ClearCache()
        {
            _metadataControlDic = new Dictionary<string, MetadataControl>();
            _metadataEntityControlDic = new Dictionary<string, MetadataEntityControl>();
        }
    }
}
