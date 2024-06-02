namespace SolrEngine
{
    public interface ISolrManager
    {
        public bool IndexElements(IEnumerable<SqlData.Models.Product> products);
        public bool IndexSingleElement(SqlData.Models.Product product);
        public bool UpdateSingleElementDescription(SqlData.Models.Product product);
        public bool EraseSingleElement(SqlData.Models.Product product);
        public void ContentSearch(string phrase, IEnumerable<string> fields, DateTime? dateFilter);
    }
}
