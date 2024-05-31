namespace AppAbstract.DatabaseAbstract
{
    public interface IProduct
    {
        public int Id { get; }
        public string Name { get; }
        public double Price { get; }
        public string? Description { get; }
        public int Quantity { get; }
        public DateTime CreationDate { get; }
        public string CreatedBy { get; }
        public ICategory? Category { get; }
    }
}
