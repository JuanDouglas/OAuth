using OAuth.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Api.Models.Result
{
    public class File
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public int FileType { get; set; }

        public File() { 
        }
        public File(Image profileImage)
        {
            ID = profileImage.Id;
            FileName = profileImage.FileName;
            FileType = profileImage.FileType;
        }
    }
}
