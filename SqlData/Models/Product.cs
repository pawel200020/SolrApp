using System.ComponentModel.DataAnnotations;

namespace SqlData.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public required string CreatedBy {get; set; }
        public virtual Category? Category { get; set; }
    }
}
