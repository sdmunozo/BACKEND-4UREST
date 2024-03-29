﻿using System.ComponentModel.DataAnnotations;

namespace NetShip.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public Guid CatalogId { get; set; }
        public string Icon { get; set; } = string.Empty;
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Product> Products { get; set; } = new List<Product>();
        public Catalog Catalog { get; set; }

    }
}