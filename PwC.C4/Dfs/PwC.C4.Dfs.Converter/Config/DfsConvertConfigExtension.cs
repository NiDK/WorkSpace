using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NReco.VideoConverter;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Dfs.Converter.Model;

namespace PwC.C4.Dfs.Converter.Config
{
    public static class DfsConvertConfigExtension
    {
        private static Dictionary<string, Dictionary<ImageSize, string>> _imgDic;

        private static Dictionary<string, Dictionary<VideoSize, ConvertSettings>> _vidDic;

        private static Dictionary<string, List<string>> _vidTypeDic;

        public static List<string> GetEnableApp(this DfsConvertConfig config)
        {
            var data = config.EnableConvertApps.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return data.Length > 0 ? data.ToList() : new List<string>();
        }

        public static ConvertInfo GetAppConvertInfo(this DfsConvertConfig config,string appCode)
        {
            var settings = config.ConvertInfos.FirstOrDefault(c => c.AppCode == appCode) ??
                               config.ConvertInfos.FirstOrDefault(c => c.AppCode == "DefaultConvertSettings");
            return settings;
        }

        public static Dictionary<ImageSize, string> GetImageSettings(this DfsConvertConfig config,
            string appCode)
        {
            if (_imgDic != null)
            {
                if (_imgDic.ContainsKey(appCode))
                {
                    return _imgDic[appCode];
                }
            }
            else
            {
                _imgDic =
                    new Dictionary<string, Dictionary<ImageSize, string>>(
                        StringComparer.InvariantCultureIgnoreCase);
            }
            var datas = new Dictionary<ImageSize, string>();
            var app = GetAppConvertInfo(config, appCode);
            foreach (var imageSetting in app.ImageSettings)
            {
                var size = Const.ImageSizeDicByName[imageSetting.Size];
                datas.Add(size, imageSetting.ToConvert(app.EnableImageType));
            }
            if (_imgDic.ContainsKey(appCode))
            {
                _imgDic[appCode] = datas;
            }
            else
            {
                _imgDic.Add(appCode, datas);
            }
            return datas;
        }

        public static Dictionary<VideoSize, ConvertSettings> GetVideoSettings(this DfsConvertConfig config, string appCode)
        {
            if (_vidDic != null)
            {
                if (_vidDic.ContainsKey(appCode))
                {
                    return _vidDic[appCode];
                }
            }
            else
            {
                _vidDic =
                    new Dictionary<string, Dictionary<VideoSize, ConvertSettings>>(
                        StringComparer.InvariantCultureIgnoreCase);
            }
            var datas = new Dictionary<VideoSize, ConvertSettings>();
            var app = GetAppConvertInfo(config, appCode);
            foreach (var video in app.VideoSettings)
            {
                var size = Const.VideoSizeDicByName[video.Size];
                datas.Add(size, video.ToConvert(app.EnableVideoType));
            }
            if (_vidDic.ContainsKey(appCode))
            {
                _vidDic[appCode] = datas;
            }
            else
            {
                _vidDic.Add(appCode, datas);
            }
            return datas;
        }

        public static List<string> GetEnableVideoFile(this DfsConvertConfig config, string appCode)
        {
            if (_vidTypeDic != null)
            {
                if (_vidTypeDic.ContainsKey(appCode))
                {
                    return _vidTypeDic[appCode];
                }
            }
            else
            {
                _vidTypeDic =
                    new Dictionary<string, List<string>>(
                        StringComparer.InvariantCultureIgnoreCase);
            }
            var data = GetAppConvertInfo(config, appCode);
            var enableType = data.EnableVideoType.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (_vidTypeDic.ContainsKey(appCode))
            {
                _vidTypeDic[appCode] = enableType;
            }
            else
            {
                _vidTypeDic.Add(appCode, enableType);
            }
            return enableType;
        }

        public static ServiceSetting GetServiceSetting(this DfsConvertConfig config)
        {
            return config.ServiceSetting;
        }
    }
}
