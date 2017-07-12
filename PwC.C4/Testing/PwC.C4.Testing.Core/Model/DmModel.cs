using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Testing.Core.Model
{
    [MetaObject(Name = "Entity_AppCode")]
    public class DmModel:DynamicMetadata
    {
        [MetaColumn(IsPk = true, Name = "Id")]
        public string Id { get; set; } 
    }
}
