namespace OAuth.Client.Android.Models.Results
{
    public class ApplicationResult
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Redirect_URL { get; set; }
        public string Site { get; set; }
        public FileModel Icon { get; set; }
    }
}