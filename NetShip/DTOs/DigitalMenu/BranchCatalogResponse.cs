using System;
using System.Collections.Generic;

namespace NetShip.DTOs.DigitalMenu
{
    public class BranchCatalogResponse
    {
        public string BrandId { get; set; }
        public string BranchId { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string WebsiteLink { get; set; }
        public string BrandLogo { get; set; }
        public string BrandSlogan { get; set; }
        public string MenuBackground { get; set; }
        public List<Catalog> Catalogs { get; set; }
    }

    public class Catalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Item> Items { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Price { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<ModifiersGroup> ModifiersGroups { get; set; }
    }

    public class ModifiersGroup
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Modifier> Modifiers { get; set; }
    }

    public class Modifier
    {
        public string Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Price { get; set; }
    }
}
