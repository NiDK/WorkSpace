using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using ImageResizer;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Dfs.Converter.Config;
using PwC.C4.Dfs.Converter.Model;
using PwC.C4.Dfs.Converter.Persistance;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Converter.Service
{
    public static class ImageResizeService
    {
        static readonly LogWrapper Log = new LogWrapper();
        public static void Resize(DfsPath dfs)
        {
            try
            {
                var settings = DfsConvertConfig.Instance.GetImageSettings(dfs.AppCode);
                var inputFilePath = dfs.GetFilePhysicalPath();
                Parallel.ForEach(settings, (setting) => { Process(inputFilePath, dfs, setting); });
                BaseDao.CompleteConvert(dfs.FileId);
            }
            catch (Exception ee)
            {
                Log.Error("Resize error,dfspath:"+dfs.ToString()
                    ,ee);
            }
        }

        private static void Process(string inputFilePath, DfsPath dfs,
            KeyValuePair<ImageSize, string> setting)
        {
            var ind = setting.Value.Split(new string[] { "$" }, StringSplitOptions.RemoveEmptyEntries);
            var startTime = new DateTime();
            var mode = Const.ImageSizeByEnum[setting.Key];
            var set = ind[0] + "&format=" + dfs.FileExtension;
            if (ind[1].Contains(dfs.FileExtension.ToLower()))
            {
               
                var result = "Success";
                if (File.Exists(inputFilePath))
                {
                    var outputFileName = inputFilePath.Replace(".", $"-{mode}.");
                    var stream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                    
                    var i = new ImageJob(stream, outputFileName, new Instructions(set));
                    i.Build();
                }
                else
                {
                    result = "FileNotExist";
                }
                CommonService.UpdateConvertInfoToDb(dfs, startTime, mode, set, result);
            }
            else
            {
                CommonService.UpdateConvertInfoToDb(dfs, startTime, mode, set, "IncorrectFileType");
            }
            
        }
    }
}
