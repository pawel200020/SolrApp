using SolrNet.Attributes;

namespace SolrEngine.Models
{
    public class Product
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }
        [SolrField("name")]
        public string Name { get; set; }
        [SolrField("price")]
        public double Price { get; set; }
        [SolrField("description")]
        public string? Description { get; set; }
        [SolrField("quantity")]
        public int Quantity { get; set; }
        [SolrField("creation_date")]
        public DateTime CreationDate { get; set; }
        [SolrField("created_by")]
        public string CreatedBy { get; set; }
        [SolrField("category")]
        public string? Category { get; set; }
    }
}
