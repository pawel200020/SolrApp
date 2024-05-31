using System.ComponentModel.DataAnnotations;

namespace SqlData.Models
{
    public class Category 
    {
        [Key] 
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
