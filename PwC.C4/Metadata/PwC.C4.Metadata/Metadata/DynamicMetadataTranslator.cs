using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Exceptions;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Service;

namespace PwC.C4.Metadata.Metadata
{
    public class DynamicMetadataTranslator
    {

        private static List<string> _columns;
        private static List<string> _stringFormatColumns;
        private static List<string> _stringFormatFields;
        private static Dictionary<string, string> _stringFormatColDic;
        private static Dictionary<string, string> _joinFormatColDic;
        private static List<string> _joinFormatColumns;
        private static Dictionary<string, List<string>> _columnsDic; 
        private static Dictionary<string, Dictionary<string, string>> _mapping;
        private static Dictionary<string, List<string>> _stringFormatColumnsDic;
        private static Dictionary<string, List<string>> _joinFormatColumnsDic; 

        public class TranslatorContext<TModel>
        {
            public string EntityName { get; set; }
            public HashSet<string> Fields { get; set; }
            public List<TModel> Models { get; set; }
            public Func<TModel, DynamicMetadata[]> DataResovler { get; set; }
            public Dictionary<string, Dictionary<object, object>> ThreadThempVariable { get; set; }
            public IList<string> SpecialExplanationField { get; set; }

            public Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object>
                SpecialExplanationFieldFunction { get; set; }

            public void DoTranslate()
            {
                foreach (var model in Models)
                {
                    DynamicMetadata[] datas = DataResovler(model);
                    TranslateModels(datas);
                }
            }

            private void TranslateModels(DynamicMetadata[] datas)
            {
                foreach (DynamicMetadata t in datas)
                {
                    if (t != null)
                    {
                        if (!t.IsTranslatored)
                        {
                            TranslateSingleModel(t, Fields);
                        }
                        t.IsTranslatored = true;
                    }
                }
            }

            private IEnumerable<string> GetTranslateFields(IEnumerable<string> fields)
            {
                var result = from i in fields
                             where (from o in _columns select o).Contains(i)
                             select i;
                return result.ToList();
            }

            private void TranslateSingleModel(DynamicMetadata expandoModel, HashSet<string> fields)
            {
                var translateFields = GetTranslateFields(fields);

                foreach (var field in translateFields)
                {
                    TranslateField(field, expandoModel);
                }
                if (_stringFormatFields != null)
                {
                    foreach (var field in _stringFormatFields)
                    {
                        if (expandoModel == null || expandoModel[field] == null || string.IsNullOrEmpty(expandoModel[field].ToString())) continue;
                        try
                        {
                            var format = "";
                            if (_stringFormatColDic.ContainsKey(field))
                            {
                                format = _stringFormatColDic[field];
                            }
                            else
                            {
                                var colSetting = MetadataSettings.Instance.GetColumn(EntityName, field);
                                if (colSetting.Properties.ContainsKey("DataFormat") &&
                                    !string.IsNullOrEmpty(colSetting.Properties["DataFormat"]))
                                {
                                    format = colSetting.Properties["DataFormat"];
                                }
                                _stringFormatColDic.Set(field, format);
                            }
                            //var convertData =
                            //    (DateTime) TypeHelper.TypeConverter("datetime", expandoModel[field].ToString());
                            var convertData = expandoModel.SafeGet<DateTime>(field);
                            expandoModel[field] = convertData.ToString(format);
                        }
                        catch (Exception e)
                        {
                            expandoModel[field] = expandoModel[field];
                        }
                    }
                }

                foreach (var field in fields)
                {
                    if (SpecialExplanationField != null && SpecialExplanationField.Count > 0 &&
                        SpecialExplanationField.Contains(field))
                    {

                        if (expandoModel != null)
                        {

                            try
                            {
                                expandoModel[field] = SpecialExplanationFieldFunction(field, expandoModel,
                                    ThreadThempVariable);
                            }
                            catch (Exception e)
                            {
                                expandoModel[field] = "";
                                
                            }
                        }

                    }
                   
                }
            }

            private void TranslateField(string field, DynamicMetadata model)
            {

                if (model.Properties.ContainsKey(field))
                {
                    var valDe = model.GetProperty(field);
                    string val = null;
                    if (valDe != null)
                    {
                         val = valDe.ToString();
                    }
                    
                    if (valDe!=null && !string.IsNullOrEmpty(val))
                    {
                        var dataSourceInfo = MetadataSettings.Instance.GetColumnDataSource(EntityName, field);
                        var group = "";
                        if (!string.IsNullOrEmpty(dataSourceInfo.Item3))
                        {
                            var oKey = "Original-" + dataSourceInfo.Item3;
                            if (model.HasProperty(oKey))
                            {
                                group = model[oKey].ToString();
                            }
                            else if (model.HasProperty(dataSourceInfo.Item3))
                            {
                                group = model[dataSourceInfo.Item3].ToString();
                            }
                            else
                            {
                                group = null;
                            }
                        }
                        var dic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                        var key = string.Format("{0}-{1}-{2}", EntityName, field, group).ToLower();
                        if (_mapping.ContainsKey(key) && _mapping[key].Count > 0)
                        {
                            dic = _mapping[key];
                        }
                        else
                        {
                            var datasouce = DataSourceService.Instance()
                                .GetDataSourceBy(dataSourceInfo.Item1, dataSourceInfo.Item2, group);
                            datasouce.ForEach(c =>
                            {
                                var keys = c.Key.ToLower();
                                dic.Set(keys, c.Desc ?? c.Value);
                            });
                            if (_mapping.ContainsKey(key))
                            {
                                _mapping[key] = dic;
                            }
                            else
                            {
                                _mapping.Add(key, dic);
                            }
                        }

                        if (_joinFormatColumns.Contains(field))
                        {
                            var chart = " | ";
                            if (_joinFormatColDic.ContainsKey(field))
                            {
                                chart = _joinFormatColDic[field];
                            }
                            else
                            {
                                var colSetting = MetadataSettings.Instance.GetColumn(EntityName, field);
                                if (colSetting.Properties.ContainsKey("JoinKeyword") &&
                                    !string.IsNullOrEmpty(colSetting.Properties["JoinKeyword"]))
                                {
                                    chart = colSetting.Properties["JoinKeyword"];
                                }
                                _joinFormatColDic.Set(field,chart);
                            }
                            var trValue = new List<string>();
                            var newVal =
                                val.Split(new[] {"|C4|"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                            newVal.ForEach(c =>
                            {
                                c = c.ToLower();
                                if (dic.ContainsKey(c))
                                    trValue.Add(dic[c]);
                            });
                            model.SetProperty("Original-" + field, model[field]);
                            model[field] = string.Join(chart, trValue);
                        }
                        else
                        {
                            if (dic.ContainsKey(val))
                            {
                                model.SetProperty("Original-" + field, model[field]);
                                model[field] = dic[val];
                            }

                        }
                    }
                }
            }
        }

        public static void Translate<TModel>(List<TModel> models,
            IList<string> fields,
            Func<TModel, DynamicMetadata[]> dataResovler,
            Dictionary<string, Dictionary<object, object>> threadThempVariable,
            IList<string> specialFileds,
            Func<string, DynamicMetadata, Dictionary<string, Dictionary<object, object>>, object> callback,
            string entityName = null, bool? isEntity = null)
        {
            try
            {
                var isEntityTr = isEntity ?? false;
                entityName = entityName ?? MetadataHelper.GetEntityName<TModel>();
                if (_columnsDic == null)
                    _columnsDic = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
                if (_mapping == null)
                    _mapping = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
                if (_stringFormatColumnsDic == null)
                    _stringFormatColumnsDic = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
                if (_joinFormatColumnsDic == null)
                    _joinFormatColumnsDic = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
                var comKey = entityName + "-" + (isEntityTr ? "entity" : "list");
                comKey = comKey.ToLower();
                if (_columnsDic.ContainsKey(comKey))
                {
                    _columns = _columnsDic[comKey];
                }
                else
                {
                    _columns = MetadataSettings.Instance.GetTranslatorColumns(entityName, isEntityTr);
                    if (_columnsDic.ContainsKey(comKey))
                    {
                        _columnsDic[comKey] = _columns;
                    }
                    else
                    {
                        _columnsDic.Add(comKey, _columns);
                    }
                }

                if (_stringFormatColumnsDic.ContainsKey(entityName))
                {
                    _stringFormatColumns = _stringFormatColumnsDic[entityName];
                }
                else
                {
                    _stringFormatColumns = MetadataSettings.Instance.GetStringFormatColumns(entityName);
                    if (_stringFormatColumnsDic.ContainsKey(entityName))
                    {
                        _stringFormatColumnsDic[entityName] = _stringFormatColumns;
                    }
                    else
                    {
                        _stringFormatColumnsDic.Add(entityName, _stringFormatColumns);
                    }
                }

                if (_joinFormatColumnsDic.ContainsKey(entityName))
                {
                    _joinFormatColumns = _joinFormatColumnsDic[entityName];
                }
                else
                {
                    _joinFormatColumns = MetadataSettings.Instance.GetJoinFormatColumns(entityName);
                    if (_joinFormatColumnsDic.ContainsKey(entityName))
                    {
                        _joinFormatColumnsDic[entityName] = _joinFormatColumns;
                    }
                    else
                    {
                        _joinFormatColumnsDic.Add(entityName, _joinFormatColumns);
                    }
                }

                if (_stringFormatFields == null)
                {
                    //_stringFormatFields = _stringFormatColumns.Where(c => fields.Contains(c)).ToList();
                    _stringFormatFields = _stringFormatColumns;
                }

                if (_stringFormatColDic == null)
                {
                    _stringFormatColDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                }

                if (_joinFormatColDic == null)
                {
                    _joinFormatColDic = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                }

                if (fields == null || !fields.Any())
                {
                    fields = _columns;
                }
                var translatorContext = new TranslatorContext<TModel>
                {
                    EntityName = entityName,
                    Models = models,
                    DataResovler = dataResovler,
                    Fields = new HashSet<string>(fields),
                    ThreadThempVariable = threadThempVariable,
                    SpecialExplanationField = specialFileds,
                    SpecialExplanationFieldFunction = callback
                };
                translatorContext.DoTranslate();
                models.ForEach(c =>
                {
                    dynamic m = c;
                    //m.RemoveOriginalProperty();
                    c = m;
                });
            }
            catch (Exception ee)
            {
                var log =new LogWrapper();
                log.Error(new TranslatorException("Translator error", ee));
            }
            
        }

        public static void ClearCache()
        {
            _columns = null;
            _stringFormatColumns = null;
            _columnsDic = null;
            _mapping = null;
            _stringFormatColumnsDic = null;
            _joinFormatColDic = null;
            _joinFormatColumns = null;
            _joinFormatColumnsDic = null;
            _stringFormatColDic = null;
            _stringFormatFields = null;
        }
    }
}
