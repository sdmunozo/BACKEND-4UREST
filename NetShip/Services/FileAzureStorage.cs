using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NetShip.Services
{
    public class FileAzureStorage : IFileStorage
    {
        private readonly string? connectionString;

        public FileAzureStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión de Azure Storage no está configurada.");
            }
        }


        public async Task Delete(string? path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var name = Path.GetFileName(path);
            var blob = client.GetBlobClient(name);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> Upload(string container, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName).ToLower();
            var name = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(name);

            // Se calcula el tamaño original del archivo para comparar después de la compresión.
            long originalSize = file.Length;

            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            using (var ms = new MemoryStream())
            {
                if (extension == ".jpg" || extension == ".jpeg")
                {
                    // Comprimir y guardar la imagen como JPEG
                    var encoder = new JpegEncoder
                    {
                        Quality = 75 // Ajusta la calidad aquí
                    };
                    image.SaveAsJpeg(ms, encoder);
                }
                else if (extension == ".png")
                {
                    // Para PNG, la compresión es sin pérdida y no modifica la calidad
                    image.SaveAsPng(ms);
                }
                // Aquí puedes añadir más condiciones para otros formatos si es necesario

                ms.Position = 0;
                var compressedSize = ms.Length;
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };
                await blob.UploadAsync(ms, blobHttpHeaders);

                // Calcular y mostrar el porcentaje de compresión
                double compressionPercentage = 100 * (1 - (double)compressedSize / originalSize);
                Console.WriteLine($"Porcentaje de compresión: {compressionPercentage:F2}%");
            }

            return blob.Uri.ToString();
        }
    }
}



/*
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace NetShip.Services
{
    public class FileAzureStorage : IFileStorage
    {
        private string? connectionString;

        public FileAzureStorage( IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task Delete(string? path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var name = Path.GetFileName(path);
            var blob = client.GetBlobClient(name);
            await blob.DeleteIfExistsAsync();


        }

        public async Task<string> Upload(string container, IFormFile file)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(file.Name);
            var name = $"{Guid.NewGuid}.{extension}";
            var blob = client.GetBlobClient(name);
            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = file.ContentType;
            await blob.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
            return blob.Uri.ToString();
        }
    }
}

*/