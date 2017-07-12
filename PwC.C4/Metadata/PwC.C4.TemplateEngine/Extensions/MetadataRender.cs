using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Model.Enum;
using PwC.C4.Metadata.Service;
using PwC.C4.TemplateEngine.Model;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.TemplateEngine.Extensions
{
    public class MetadataRender
    {
        
       private static Dictionary<string,MetadataControl>  _metadataControlDic;

        public MetadataControl BuildControl(string entityName, string columnName, string datasourceGroup,
            string className, object attrObjects, string value = null)
        {
            var dicKey = string.Format("{0}-{1}-{2}", entityName, columnName, datasourceGroup);
            var hasValue = false;
            var dic = TypeHelper.ObjectToDictionary(attrObjects);
            if (!dic.ContainsKey("value") && !string.IsNullOrEmpty(value))
            {
                hasValue = true;
                value = value.Replace("\"", "&quot;");
            }
                
            MetadataControl control;
            
            if (_metadataControlDic == null)
            {
                _metadataControlDic = new Dictionary<string, MetadataControl>(StringComparer.OrdinalIgnoreCase);
            }
            if (_metadataControlDic.ContainsKey(dicKey) && !CacheHelper.GetCacheItem<bool>("RemoveCache-MetadataChanged-MetadataRender_metadataControlDic"))
            {
                control = _metadataControlDic[dicKey];
            }
            else
            {
                var column = MetadataSettings.Instance.GetColumn(entityName, columnName);

                control = new MetadataControl
                {
                    Name = column.Name,
                    Group = datasourceGroup,
                    DataType = column.Type,
                    Label = column.Properties["Label"],
                    Description = column.Description,
                    BaseControlType = (ControlType) int.Parse(column.Properties["DataControlType"]),
                    DataSourceType = (DataSourceType) int.Parse(column.Properties["DataSourceType"]),
                    IsRequire = bool.Parse(column.Properties["Require"]),
                    InputRegular = column.Properties["InputRegular"],
                    InvalidMessage = column.Properties["InvalidMsg"],
                    DataSource = column.Properties["DataSource"],
                    DataFormat = column.Properties["DataFormat"],
                    Size = column.Size
                };
                var regInfo = control.IsRequire ? "required=\"required\"" : "";
                regInfo = regInfo + " " +
                          (string.IsNullOrEmpty(control.InputRegular) ? "" : "reg=\"" + control.InputRegular + "\"");
                regInfo = regInfo + " " +
                          (string.IsNullOrEmpty(control.InvalidMessage)
                              ? ""
                              : "invalid-msg=\"" + control.InvalidMessage + "\"");
                control.RequireInfo = regInfo;
                if (_metadataControlDic.ContainsKey(dicKey))
                {
                    _metadataControlDic[dicKey] = control;
                }
                else
                {
                    _metadataControlDic.Add(dicKey, control);
                }

                CacheHelper.SetCacheItem("RemoveCache-MetadataChanged-MetadataRender_metadataControlDic", false);
            }

            var maxlength = "";
            if (control.Size != 0)
            {
                maxlength = "maxlength=\"" + control.Size + "\"";
            }

            var regular = "";
            if (!string.IsNullOrEmpty(control.InputRegular))
            {
                regular = "regular=\"" + control.InputRegular + "\"";
            }

            var valueMode = "normal";

            if (dic.ContainsKey("C4ValueMode"))//该属性仅在radio，checkbox，select 起效
            {
                var keyModel = dic["C4ValueMode"];
                switch (keyModel.ToString().ToLower())
                {
                    case "key":
                        valueMode = "key";//当radio，checkbox，select 该属性设置为key时，则展示key而不展示value。例如，Key=1，value="wulala",在select中则为<option value="1">1</optiom>
                        break;
                    default:break;
                }
                dic.Remove("C4ValueModel");
            }


            var isPure = false;
            if (dic.ContainsKey("C4DisplayDesc"))
            {
                bool.TryParse(dic["C4DisplayDesc"].ToString(), out isPure);
                dic.Remove("C4DisplayDesc");
            }

            var attrs = dic.Aggregate("", (current, o) => current + " " + o.Key + "=\"" + o.Value + "\" ");
            switch (control.BaseControlType)
            {
                case ControlType.Hidden:
                    var hdSb = new StringBuilder();
                    hdSb.Append("<input type=\"hidden\" name=\"");
                    hdSb.Append(control.Name);
                    hdSb.Append("\" class=\"metadata metadataHidden " + className + " ");
                    hdSb.Append(control.Name);
                    hdSb.Append("\" id=\"Metadata-");
                    hdSb.Append(control.Name + "\" ");
                    if (hasValue)
                        hdSb.Append("value=\"" + value + "\" ");
                    hdSb.Append(attrs + " " + control.RequireInfo + "/>");
                    control.ControlHtml = new MvcHtmlString(hdSb.ToString());
                    control.Html = hdSb.ToString();
                    break;
                case ControlType.Password:
                    var pswdSb = new StringBuilder();
                    pswdSb.Append("<input type=\"password\" name=\"");
                    pswdSb.Append(control.Name);
                    pswdSb.Append("\" class=\"metadata metadataPassword " + className + " ");
                    pswdSb.Append(control.Name);
                    pswdSb.Append("\" id=\"Metadata-");
                    pswdSb.Append(control.Name + "\" ");
                    if (hasValue)
                        pswdSb.Append("value=\"" + value + "\" ");
                    pswdSb.Append(attrs + " " + control.RequireInfo + " " + maxlength + " " + regular +
                                  "/>");
                    control.ControlHtml = new MvcHtmlString(pswdSb.ToString());
                    control.Html = pswdSb.ToString();
                    break;
                case ControlType.File:
                    var flSb = new StringBuilder();
                    flSb.Append("<input type=\"file\" name=\"");
                    flSb.Append(control.Name);
                    flSb.Append("\" class=\"metadata metadataFile " + className + " ");
                    flSb.Append(control.Name);
                    flSb.Append("\" id=\"Metadata-");
                    flSb.Append(control.Name + "\" ");
                    if (hasValue)
                        flSb.Append("value=\"" + value + "\" ");
                    flSb.Append(attrs + " " + control.RequireInfo + "/>");
                    control.ControlHtml = new MvcHtmlString(flSb.ToString());
                    control.Html = flSb.ToString();
                    break;
                case ControlType.Textarea:
                    var taSb = new StringBuilder();
                    taSb.Append("<textarea name=\"");
                    taSb.Append(control.Name);
                    taSb.Append("\" class=\"metadata metadataTextarea " + className + " ");
                    taSb.Append(control.Name);
                    taSb.Append("\" id=\"Metadata-");
                    taSb.Append(control.Name);
                    taSb.Append("\" " + attrs + " " + control.RequireInfo + " " + maxlength + " " + regular +
                                ">");
                    taSb.Append((hasValue ? value : ""));
                    taSb.Append("</textarea>");
                    control.ControlHtml = new MvcHtmlString(taSb.ToString());
                    control.Html = taSb.ToString();
                    break;
                case ControlType.TextBox:
                    var tbSb = new StringBuilder();
                    tbSb.Append("<input type=\"text\" name=\"");
                    tbSb.Append(control.Name);
                    tbSb.Append("\" class=\"metadata metadataTextBox " + className + " ");
                    tbSb.Append(control.Name);
                    tbSb.Append("\" id=\"Metadata-");
                    tbSb.Append(control.Name + "\" ");
                   
                    if (hasValue)
                    {
                        if (!string.IsNullOrEmpty(control.DataFormat) &&
                            control.DataType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                        {
                            DateTime date;
                            var rel = DateTime.TryParse(value, out date);
                            if (rel)
                            {
                                value = date.ToString(control.DataFormat);
                            }
                        }
                        tbSb.Append("value=\"" + value + "\" ");
                    }
                    tbSb.Append(attrs + " " + control.RequireInfo + " " + maxlength + " " + regular +
                                "/>");
                    control.ControlHtml = new MvcHtmlString(tbSb.ToString());
                    control.Html = tbSb.ToString();
                    break;
                case ControlType.Select:
                    var stSb = new StringBuilder();
                    stSb.Append("<select name=\"");
                    stSb.Append(control.Name);
                    stSb.Append("\" class=\"metadata metadataSelect " + className + " ");
                    stSb.Append(control.Name);
                    stSb.Append("\" id=\"Metadata-");
                    stSb.Append(control.Name);
                    stSb.Append("\" " + attrs + " " + control.RequireInfo + ">");
                    var stDataSource = DataSourceService.Instance()
                        .GetDataSourceBy(control.DataSourceType, control.DataSource, datasourceGroup);
                    stDataSource.ForEach(source =>
                    {
                        stSb.Append("<option value=\""+ source.Key +"\"");
                        if (hasValue)
                        {
                            stSb.Append(string.Equals(source.Key, value, StringComparison.OrdinalIgnoreCase)
                                ? "selected=\"selected\""
                                : "");
                        }
                        stSb.Append(">");

                        if (value != "key" && isPure)
                        {
                            stSb.Append(source.Desc);
                        }
                        else
                        {
                            stSb.Append(valueMode == "key" ? source.Key : source.Value);
                        }
                        
                        stSb.Append("</option>");
                    });
                    stSb.Append("</select>");
                    control.ControlHtml = new MvcHtmlString(stSb.ToString());
                    control.DataSourceObjects = stDataSource;
                    control.Html = stSb.ToString();
                    break;
                case ControlType.Radio:
                    var roSb = new StringBuilder();
                    var roDataSource = DataSourceService.Instance()
                        .GetDataSourceBy(control.DataSourceType, control.DataSource, datasourceGroup);
                    roDataSource.ForEach(source =>
                    {
                        roSb.Append("<div class=\"metadata metadataRadio ");
                        roSb.Append(control.Name);
                        roSb.Append(" " + className + "\">");
                        roSb.Append("<label><input type=\"radio\" class=\"metadata metadataRadio radio-");
                        roSb.Append(control.Name);
                        roSb.Append("\" value=\"" + source.Key + "\"  name=\"" + control.Name + "\" ");
                        if (hasValue)
                        {
                            roSb.Append(string.Equals(source.Key, value, StringComparison.OrdinalIgnoreCase)
                                ? "checked=\"checked\" "
                                : " ");
                        }
                        roSb.Append(attrs + " " + control.RequireInfo + "><span>");
                        if (value != "key" && isPure)
                        {
                            roSb.Append(source.Desc);
                        }
                        else
                        {
                            roSb.Append(valueMode == "key" ? source.Key : source.Value);
                        }
                        roSb.Append("</span></label></div>");
                    });
                    control.ControlHtml = new MvcHtmlString(roSb.ToString());
                    control.DataSourceObjects = roDataSource;
                    control.Html = roSb.ToString();
                    break;
                case ControlType.CheckBox:
                    var cbSb = new StringBuilder();
                    var cbDataSource = DataSourceService.Instance()
                        .GetDataSourceBy(control.DataSourceType, control.DataSource, datasourceGroup);
                    var vals = new List<string>();
                    if (hasValue)
                    {
                       vals = value.Split(new string[] { "|C4|" }, StringSplitOptions.RemoveEmptyEntries).ToLowerList();
                    }
                    cbDataSource.ForEach(source =>
                    {
                        cbSb.Append("<div class=\"metadata metadataCheckBox ");
                        cbSb.Append(control.Name);
                        cbSb.Append("\">");
                        cbSb.Append("<label><input type=\"checkbox\"  class=\"metadata metadataCheckbpx " + className +
                                    " checkbox-");
                        cbSb.Append(control.Name);
                        cbSb.Append("\" value=\"" + source.Key + "\"  name=\"" + control.Name + "\" ");
                        if (hasValue)
                        {
                            cbSb.Append(vals.Contains(source.Key.ToLower())
                                ? "checked=\"checked\" "
                                : " ");
                        }
                        cbSb.Append(attrs + " " + control.RequireInfo + "><span>");
                        if (value != "key" && isPure)
                        {
                            cbSb.Append(source.Desc);
                        }
                        else
                        {
                            cbSb.Append(valueMode == "key" ? source.Key : source.Value);
                        }
                        cbSb.Append("</span></label></div>");
                    });
                    control.ControlHtml = new MvcHtmlString(cbSb.ToString());
                    control.DataSourceObjects = cbDataSource;
                    control.Html = cbSb.ToString();
                    break;
            }
            return control;
        }

        public IHtmlString Raw(string value)
        {
            return new HtmlString(value);
        }

        public MvcHtmlString SubmitBtn(string value, object attr, string formName)
        {
            var btn = new StringBuilder();
            btn.Append("<button type=\"button\" id=\"SubmitBtnForRender\" name=\"SubmitBtnForRender\" formName=\""+ formName +"\" value=\"" + value + "\"");
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                btn.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            btn.Append(">" + value + "</button>");
            return new MvcHtmlString(btn.ToString());
        }

    }
}
