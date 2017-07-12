using System;

namespace PwC.C4.Configuration.Messager.Model
{
    public class ConfigSetting
    {

        public string AppCode { get; set; }

        public short Major { get; set; }

        public int Minor { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateBy { get; set; }
    }
}