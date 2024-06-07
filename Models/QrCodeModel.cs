namespace VERAExample.Models
{
    public class QrCodeModel
    {
        public string url { get; set; }
        public long expiry { get; set; }
        public string qrCode { get; set; }
        public string status { get; set; }
        public string correlationId { get; set; }
    }
}
