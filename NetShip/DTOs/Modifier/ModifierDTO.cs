﻿namespace NetShip.DTOs.Modifier
{
    public class ModifierDTO
    {
        public Guid Id { get; set; }
        public Guid ModifiersGroupId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsScheduleActive { get; set; }
        public int Sort { get; set; }
        public string? Icon { get; set; }
        public int MinModifier { get; set; }
        public int MaxModifier { get; set; }
    }
}
