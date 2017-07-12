using System;

namespace PwC.C4.Configuration
{
    public class VersionIncompatibleException : ApplicationException
    {
        public VersionIncompatibleException(string msg)
            : base(msg)
        {
        }

        public VersionIncompatibleException(string msg, int versionInClass, int versionInConfig)
            : base(msg)
        {
            VersionInClass = versionInClass;
            VersionInConfig = versionInConfig;
        }

        public int VersionInClass, VersionInConfig;
    }
}
