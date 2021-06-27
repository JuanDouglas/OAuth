using OAuth.Api.Models.Enums;
using OAuth.Dal.Models;

namespace OAuth.Api.Models.Result
{
    public class File
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public FileType FileType { get; set; }

        public File()
        {
        }
        public File(Image profileImage)
        {
            ID = profileImage.Id;
            FileName = profileImage.FileName;
            FileType = (FileType)profileImage.FileType;
        }
    }
}
