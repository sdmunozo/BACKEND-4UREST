namespace NetShip.Services
{
    public interface IFileStorage
    {
        Task Delete(string? path, string container);
        Task<string> Upload(string container, IFormFile file);
        async Task<string> Edit (string? path, string container, IFormFile file) 
        {
            await Delete(path, container);
            return await Upload(container, file);
        }
    }
}
