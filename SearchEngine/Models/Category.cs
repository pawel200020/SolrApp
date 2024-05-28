﻿using System.ComponentModel.DataAnnotations;

namespace SearchEngine.Models
{
    internal class Category
    {
        [Key] 
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
