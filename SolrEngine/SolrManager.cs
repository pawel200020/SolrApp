using AutoMapper;
using CommonServiceLocator;
using SolrEngine.Models;
using SolrNet;

namespace SolrEngine
{
    public class SolrManager : ISolrManager
    {
        private readonly IMapper _mapper;
        private bool _initalized = false;
        private string _solrUrl = string.Empty;

        public SolrManager(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void InitIfNeeded(string solrUrl)
        {
            if (!_initalized || _solrUrl != solrUrl)
            {
                Startup.Init<Product>(solrUrl);
                _solrUrl = solrUrl;
            }
        }

        public bool IndexElements(IEnumerable<SqlData.Models.Product> products)
        {
            Startup.Init<Product>("http://localhost:8983/solr/products");
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
            foreach (var product in products)
            {
                var p2 = product as SqlData.Models.Product;
                var product2 = _mapper.Map<Product>(p2);
                solr.Add(_mapper.Map<Product>(product));
            }
            return false;
        }
    }
}
