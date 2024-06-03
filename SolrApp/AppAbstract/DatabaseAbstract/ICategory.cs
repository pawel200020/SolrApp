namespace AppAbstract.DatabaseAbstract
{
    public interface ICategory
    {
        public int Id { get;}
        public  string Name { get; }
        public string? Description { get;}
        public  string CreatedBy { get;}
        public  DateTime CreatedDate { get;}
    }
}
