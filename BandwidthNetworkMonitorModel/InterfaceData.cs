// Author: Aleksandar Stojimirovic
// E-mail: stojimirovic@yahoo.com
// Mob:    +381 60 087 2516

namespace BandwidthNetworkMonitorModel
{
    public class InterfaceData
    {
        public string No { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public long BytesReceived { get; set; }
        public bool ChkBytesReceived { get; set; }
        public long BytesSent { get; set; }
        public bool ChkBytesSent { get; set; }

    }
}
