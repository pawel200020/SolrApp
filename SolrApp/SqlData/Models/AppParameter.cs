﻿using System.ComponentModel.DataAnnotations;

namespace SqlData.Models
{
    public class AppParameter
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Value { get; set; }
    }
}
