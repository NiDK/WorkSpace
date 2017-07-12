using System;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Converter.Config;
using PwC.C4.Dfs.Converter.Persistance;
using PwC.C4.Dfs.Converter.Service;
using PwC.C4.Infrastructure.Logger;
using Quartz;

namespace PwC.C4.Dfs.Converter
{
    internal class ProcessConvert : IJob
    {
        private readonly ServiceSetting _serviceSetting;
        readonly LogWrapper _log = new LogWrapper();
        public ProcessConvert()
        {
            _serviceSetting = DfsConvertConfig.Instance.GetServiceSetting();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var enableAppcode = DfsConvertConfig.Instance.GetEnableApp();
                if (!BaseDao.IsNew(enableAppcode)) return;
                _log.Error("Start convert:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                var undo = BaseDao.GetUnconvetionFiles(enableAppcode, _serviceSetting.Buffer);
                undo.ForEach(record =>
                {
                    var dfsPath = DfsPath.Parse(record);
                    if (dfsPath.Keyspace.ToLower() == "image")
                    {
                        ImageResizeService.Resize(dfsPath);
                    }
                    else
                    {
                        VideoConvertService.Convert(dfsPath);
                    }
                });
                _log.Error("Convert done!"+DateTime.Now.ToString("yyyy-MM-dd HH: mm:ss"));
            }
            catch (Exception ee)
            {
                _log.Error("ProcessConvert Execute error",ee);
            }
            
        }
    }
}