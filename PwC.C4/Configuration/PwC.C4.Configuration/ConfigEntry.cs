using System;

namespace PwC.C4.Configuration
{
    internal class ConfigEntry
    {
        public delegate object CreateObjectDelegate(string sectionName, Type type,
            out int majorVersion, out int minorVersion);

        public string Name;

        private int major;

        public int MajorVersion
        {
            get
            {
                int ret;                
                ret = major;                
                return ret;
            }
            set
            {                
                major = value;               
            }
        }

        private int minor;

        public int MinorVersion
        {
            get
            {
                int ret;
                ret = minor;
                return ret;
            }
            set
            {
                minor = value;
            }
        }

        public Type EntryType
        {
            get
            {
                return type;
            }
        }

        private bool isSet;

        public bool IsSet
        {
            get
            {
                bool ret;
                ret = isSet;
                return ret;
            }
        }

        private object locker;        
        private Type type;
        private CreateObjectDelegate OnCreate;

        public ConfigEntry(string sectionName, Type type, CreateObjectDelegate creater)
        {
            this.Name = sectionName;
            this.type = type;

            isSet = false;
            locker = new object();
            OnCreate = creater;
        }

        private object val;


        public object Value
        {
            get
            {
                object ret;                
                if (isSet)
                    ret = val;
                else
                {
                    lock(locker)
                    {
                        if (isSet) //double confirm, for performance
                            ret = val;
                        else
                        {                            
                                val = OnCreate(this.Name, this.type, out major, out minor);
                                isSet = true;
                        }                        
                    }
                }
                return val;
            }
        }
    }
   
}
