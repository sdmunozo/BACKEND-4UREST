namespace NetShip.DTOs
{
    public class InitCatalogDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid BranchId { get; set; }
    }

    public class InitCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid CatalogId { get; set; }
    }

    public class InitProductDTO
    {
        public string? Alias { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }

    public class InitModifiersGroupDTO
    {
        public string? Alias { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
    }

    public class InitModifierDTO
    {
        public string? Alias { get; set; } = string.Empty;
        public string? Price { get; set; } = string.Empty;
        public Guid ModifiersGroupId { get; set; }
    }
}
