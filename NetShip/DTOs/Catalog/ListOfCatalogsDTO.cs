

namespace NetShip.DTOs.CatalogDTOs
{
    public class ListOfCatalogsDTO
    {
        public int Total { get; set; }
        public List<CatalogDetailsDTO>? Catalogs { get; set; }
    }
}
