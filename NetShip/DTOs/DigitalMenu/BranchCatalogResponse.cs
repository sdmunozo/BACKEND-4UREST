using System;
using System.Collections.Generic;

namespace NetShip.DTOs.DigitalMenu
{
    public class BranchCatalogResponse
    {
        public string BrandId { get; set; } = string.Empty;
        public string BranchId { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string InstagramLink { get; set; } = string.Empty;
        public string FacebookLink { get; set; } = string.Empty;
        public string WebsiteLink { get; set; } = string.Empty;
        public string BrandLogo { get; set; } = string.Empty;
        public string BrandSlogan { get; set; } = string.Empty;
        public string MenuBackground { get; set; } = string.Empty;
        public List<Catalog> Catalogs { get; set; } = new List<Catalog>();
    }

    public class Catalog
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class Category
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Product> Products { get; set; } = new List<Product>();
    }

    public class Item
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
    }

    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public List<ModifiersGroup> ModifiersGroups { get; set; } = new List<ModifiersGroup>();
    }

    public class ModifiersGroup
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string isSelectable { get; set; } = string.Empty;
        public List<Modifier> Modifiers { get; set; } = new List<Modifier>();
    }

    public class Modifier
    {
        public string Id { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
    }
}
