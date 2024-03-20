using QRCoder;

namespace NetShip.Services
{
    public class QrCodeService
    {
        public string GenerateQrCode(string url, string outputPath)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);

            // Verifica si el directorio del outputPath existe, si no, créalo
            var directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(outputPath, qrCodeBytes);

            return outputPath;
        }
    }

}
