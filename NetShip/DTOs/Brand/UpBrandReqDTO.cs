using NetShip.Entities;

namespace NetShip.DTOs.Brand
{
    public class UpBrandReqDTO
    {
        public string? Name { get; set; }
        public IFormFile? Logo { get; set; }
        public string? Slogan { get; set; }
        public string? Instagram { get; set; }
        public string? Facebook { get; set; }
        public string? Website { get; set; }
        public IFormFile? CatalogsBackground { get; set; }
    }
}
