﻿namespace NetShip.Entities
{
    public class ModifiersGroup
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsScheduleActive { get; set; } = false;
        public int Sort { get; set; } = -1;
        public string Alias { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool isSelectable { get; set; } = false;
        public int MinModifiers { get; set; } = -1;
        public int MaxModifiers { get; set; } = -1;
        public List<Modifier> Modifiers { get; set; } = new List<Modifier>();
        public Product Product { get; set; }

    }
}