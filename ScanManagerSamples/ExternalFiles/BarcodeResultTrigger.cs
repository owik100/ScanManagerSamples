using System;

namespace ScanManagerTriggerReferences
{
    public class BarcodeResultTrigger
    {
        public BarcodeTypes BarcodeType { get; set; }

        public BarcodeFormats BarcodeFormat { get; set; }

        public string DisplayValue { get; set; }

        public string RawValue { get; set; }

        public int ScanResultId { get; set; }
    }

    public enum BarcodeTypes
    {
        Unknown,
        ContactInfo,
        Email,
        Isbn,
        Phone,
        Product,
        Sms,
        Text,
        Url,
        WiFi,
        GeographicCoordinates,
        CalendarEvent,
        DriversLicense
    }

    [Flags]
    public enum BarcodeFormats
    {
        Code128 = 1,
        Code39 = 2,
        Code93 = 4,
        CodaBar = 8,
        DataMatrix = 0x10,
        Ean13 = 0x20,
        Ean8 = 0x40,
        Itf = 0x80,
        QRCode = 0x100,
        Upca = 0x200,
        Upce = 0x400,
        Pdf417 = 0x800,
        Aztec = 0x1000,
        All = 0xFFFF
    }
}
