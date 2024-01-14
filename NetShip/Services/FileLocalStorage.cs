
namespace NetShip.Services
{
    public class FileLocalStorage : IFileStorage
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileLocalStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task Delete(string? path, string container)
        {
            if (string.IsNullOrEmpty(path))
                return Task.CompletedTask;

            var fileName = Path.GetFileName(path);
            var fileDirectory = Path.Combine(env.WebRootPath, container, fileName);

            if (File.Exists(fileDirectory))
                File.Delete(fileDirectory);

            return Task.CompletedTask;
        }

        public async Task<string> Upload(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, container);

            if(!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);

            using ( var ms = new MemoryStream()) 
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray(); 
                await File.WriteAllBytesAsync(path, content);

                var url = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}";
                var fileUrl = Path.Combine(url, container, fileName).Replace("\\", "/");

                return fileUrl;
            }
        }
    }
}
