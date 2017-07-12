using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Rush.Service
{
    public class C4DataFormat : IFormattable
    {
        PwC.C4.Infrastructure.Logger.LogWrapper log = new LogWrapper();
        private readonly object _data;
        public C4DataFormat(object data)
        {
            this._data = data;
        }

        public override string ToString()
        {
            return this.ToString("", CultureInfo.CurrentCulture);
        }


        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            try
            {
                if (String.IsNullOrEmpty(format)) format = "";
                if (format.IndexOf("-", StringComparison.Ordinal) >= 0)
                {

                    var para = format.Split(new string[] {"-"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var orderedPara = new Dictionary<string, string>();
                    foreach (var p in para)
                    {
                        var index = p.IndexOf(" ", StringComparison.Ordinal);
                        var pt = p.Substring(0, index).ToLower();
                        orderedPara.Add(pt, p.Substring(index, p.Length - index));
                    }
                    if (orderedPara.Count > 0)
                    {
                        var type = "datetime";
                        if (orderedPara.ContainsKey("t"))
                        {
                            type = orderedPara["t"].ToLower();
                        }
                        var t = this._data.GetType();
                        if (t == TypeHelper.GetType(type))
                        {
                            var date = Convert.ToDateTime(this._data);
                            var formatString = "";

                            if (orderedPara.ContainsKey("l"))
                            {
                                orderedPara["l"] =
                                    orderedPara["l"].ToLower().Replace(" ", "").Replace("p", "+").Replace("m", "-");
                                if (string.IsNullOrEmpty(orderedPara["l"]))
                                {
                                    date = date.ToLocalTime();
                                }
                                else
                                {
                                    var timezone = 0;
                                    int.TryParse(orderedPara["l"], out timezone);
                                    date = date.AddHours(timezone);
                                }
                            }
                            if (orderedPara.ContainsKey("f"))
                            {
                                formatString = orderedPara["f"];
                            }
                            else
                            {
                                return date.ToString(CultureInfo.InvariantCulture);
                            }
                            return date.ToString(formatString);
                        }

                    }

                }
                return this._data.ToString();
            }
            catch (Exception ee)
            {
                log.Error("C4Data format error,format:" + format, ee);
                return this._data.ToString();
            }

        }
    }
}
