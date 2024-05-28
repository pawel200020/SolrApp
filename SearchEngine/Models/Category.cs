using System.ComponentModel.DataAnnotations;

namespace SearchEngine.Models
{
    internal class Category
    {
        [Key] public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }



    }
}
