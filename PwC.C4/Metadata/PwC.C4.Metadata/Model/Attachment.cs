using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Metadata.Model
{
    public class Attachment
    {
        public string _id { get; set; }

        public Guid FileId
        {
            get { return new Guid(this._id); }
            set { this._id = value.ToString(); }
        }

        public string FileName { get; set; }

        public string FileExtName { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public Stream Stream { get; set; }
    }
}
