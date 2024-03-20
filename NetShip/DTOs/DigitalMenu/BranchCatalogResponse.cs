using System;
using System.Collections.Generic;

namespace NetShip.DTOs.DigitalMenu
{
    public class BranchCatalogResponse
    {
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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Item> Items { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Item
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }
    }

    public class Product
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<ModifiersGroup> ModifiersGroups { get; set; }
    }

    public class ModifiersGroup
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<Modifier> Modifiers { get; set; }
    }

    public class Modifier
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }
    }
}
