namespace NetShip.DTOs.DigitalMenu
{
    public class DeviceTrackingDTO
    {
        public Guid BranchId { get; set; }
        public string SessionId { get; set; }
        public string UserAgent { get; set; }
        public string Platform { get; set; }
        public string Vendor { get; set; }
        public string Language { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string OS { get; set; }
        public double PixelRatio { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string Orientation { get; set; }
    }
}
