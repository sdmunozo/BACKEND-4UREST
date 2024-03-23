using NetShip.DTOs.PricePerItemPerPlatformDTOs;
using NetShip.Entities;

namespace NetShip.DTOs.Items
{
    public class CrItemReqDTO
    {
        public Guid CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public IFormFile? Icon { get; set; }
        public double Price { get; set; }

        //public List<PricePerItemPerPlatformDTO> PricePerItem { get; set; }
    }
}
