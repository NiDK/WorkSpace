using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NReco.VideoConverter;
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
    public static class VideoConvertService
    {
        static readonly LogWrapper Log = new LogWrapper();

        public static void Convert(DfsPath dfs)
        {
            try
            {
                var settings = DfsConvertConfig.Instance.GetVideoSettings(dfs.AppCode);
                var inputFilePath = dfs.GetFilePhysicalPath();
                Parallel.ForEach(settings, (setting) => { Process(inputFilePath, dfs, setting); });
                BaseDao.CompleteConvert(dfs.FileId);
            }
            catch (Exception ee)
            {
                Log.Error("Convert error", ee);
            }

        }

        private static void Process(string inputFilePath, DfsPath dfs, KeyValuePair<VideoSize, ConvertSettings> setting)
        {
            var startTime = DateTime.Now;
            var mode = Const.VideoSizeByEnum[setting.Key];
            if (setting.Value.CustomInputArgs.Contains(dfs.FileExtension.ToLower()))
            {
                setting.Value.CustomInputArgs = "";
                var result = "Success";
                if (File.Exists(inputFilePath))
                {
                    var outputFileName = inputFilePath.Replace(".", $"-{mode}.");
                    var ff = new FFMpegConverter();
                    ff.ConvertMedia(inputFilePath, dfs.FileExtension, outputFileName, dfs.FileExtension, setting.Value);
                }
                else
                {
                    result = "FileNotExist";
                }
                CommonService.UpdateConvertInfoToDb(dfs, startTime, mode, setting.Value, result);
            }
            else
            {
                CommonService.UpdateConvertInfoToDb(dfs, startTime, mode, setting.Value, "IncorrectExtName");
            }
            
        }
    }
}
