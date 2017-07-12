using System;
using System.Collections.Generic;

namespace PwC.C4.Configuration.Messager.Model
{
    public class ConfigurationType
    {
        public Guid Id { get; set; }

        public int IdentityId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime CreateTime { get; set; }

        public string Creator { get; set; }

        public bool IsDeleted { get; set; }

        public int Status { get; set; }

        public PageModel<ConfigurationDetail> ConfigurationDetails { get; set; } 
    }
}