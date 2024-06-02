using AutoMapper;
using CommonServiceLocator;
using Flurl;
using SolrEngine.Helpers;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using SqlData;
using SqlData.Context;
using Product = SolrEngine.Models.Product;

namespace SolrEngine;

public class SolrManager : ISolrManager
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private bool _initalized = false;

    private static readonly Dictionary<string, string> SolrFields = new()
    {
        {nameof(Product.Id), "id"},
        {nameof(Product.Name), "name"},
        {nameof(Product.Category), "category"},
        {nameof(Product.CreatedBy),"created_by"},
        {nameof(Product.Price),"price"},
        {nameof(Product.Description),"description"},
        {nameof(Product.Quantity),"quantity"},

    };

    private readonly string [] _defaultResultFields = new[]
    {
        SolrFields[nameof(Product.Id)],
        SolrFields[nameof(Product.Name)],
        SolrFields[nameof(Product.Description)],
        SolrFields[nameof(Product.Category)],
        SolrFields[nameof(Product.Price)],

    };

    public SolrManager(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private void InitIfNeeded()
    {
        if (!_initalized)
        {
            var url = _context.AppParameters.First(x => x.Name == Enum.GetName(AppParams.SolrUrl)).Value;
            var login = _context.AppParameters.First(x => x.Name == Enum.GetName(AppParams.SolrLogin)).Value;
            var password = EncryptionManager.Decrypt(_context.AppParameters.First(x => x.Name == Enum.GetName(AppParams.SolrPassword)).Value!);
            var connection = new SolrConnection(Url.Combine(url, Enum.GetName(SolrCollectionsEnum.Products)))
            {
                HttpWebRequestFactory = new HttpWebAdapters.BasicAuthHttpWebRequestFactory(login, password)
            };
            Startup.Init<Product>(connection);
            _initalized = true;
        }
    }

    public bool IndexElements(IEnumerable<SqlData.Models.Product> products)
    {
        InitIfNeeded();
        var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
        var query = new SolrQuery("*:*");
        solr.Delete(query);

        foreach (var product in products)
            solr.Add(_mapper.Map<Product>(product));
        return true;
    }

    public bool IndexSingleElement(SqlData.Models.Product product)
    {
        InitIfNeeded();
        var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
        solr.Add(_mapper.Map<Product>(product));
        return true;    
    }

    public bool UpdateSingleElementDescription(SqlData.Models.Product product)
    {
        InitIfNeeded();
        var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
        solr.AtomicUpdate(product.Id.ToString(),
            new[]
            {
                new AtomicUpdateSpec("description", AtomicUpdateType.Set, product.Description)
            });
        return true;
    }

    public bool EraseSingleElement(SqlData.Models.Product product)
    {
        InitIfNeeded();
        var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();
        solr.Delete(_mapper.Map<Product>(product));
        return true;
    }



    public void ContentSearch(string phrase, IEnumerable<string> fields, DateTime? dateFilter)
    {
        InitIfNeeded();
        var solFields = fields.Select(x => SolrFields[x]);
        var opt = new QueryOptions();
        opt.Fields = _defaultResultFields;
        //TODO: Search
    }
}