namespace OAuth.Client.Android.Models.Results
{
    public class IPAdressResult
    {
        /// <summary>
        /// IP Adress.
        /// </summary>
        public string Adress { get; set; }
        /// <summary>
        /// Confiance in this IP.
        /// </summary>
        public int Confiance { get; set; }
        /// <summary>
        /// This IP already been banned.
        /// </summary>
        public bool AlreadyBeenBanned { get; set; }

        public IPAdressResult()
        {

        }
    }
}