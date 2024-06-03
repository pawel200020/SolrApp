using System.Globalization;
using AutoMapper;
using CommonServiceLocator;
using Flurl;
using SolrEngine.Helpers;
using SolrEngine.Models;
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
    private ISolrOperations<Product> _solr;


    private static readonly Dictionary<string, string> SolrFields = new()
    {
        {nameof(Product.Id), "id"},
        {nameof(Product.Name), "name"},
        {nameof(Product.Category), "category"},
        {nameof(Product.CreatedBy),"created_by"},
        {nameof(Product.CreationDate),"creation_date"},
        {nameof(Product.Price),"price"},
        {nameof(Product.Description),"description"},
        {nameof(Product.Quantity),"quantity"},

    };

    private readonly string[] _defaultResultFields = new[]
    {
        SolrFields[nameof(Product.Id)],
        SolrFields[nameof(Product.Name)],
        SolrFields[nameof(Product.Description)],
        SolrFields[nameof(Product.Category)],
        SolrFields[nameof(Product.Price)],
        SolrFields[nameof(Product.Quantity)],
        "score"

    };

    public SolrManager(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        InitIfNeeded();
        _solr = ServiceLocator.Current.GetInstance<ISolrOperations<Product>>();

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
        var query = new SolrQuery("*:*");
        _solr.Delete(query);

        foreach (var product in products)
            IndexSingleElement(product);
        return true;
    }

    public bool IndexSingleElement(SqlData.Models.Product product)
    {
        _solr.Add(_mapper.Map<Product>(product));
        return true;
    }

    public bool UpdateSingleElementDescription(SqlData.Models.Product product)
    {
        _solr.AtomicUpdate(product.Id.ToString(),
            new[]
            {
                new AtomicUpdateSpec("description", AtomicUpdateType.Set, product.Description)
            });
        return true;
    }

    public bool EraseSingleElement(SqlData.Models.Product product)
    {
        _solr.Delete(_mapper.Map<Product>(product));
        return true;
    }

    private string ConvertEndDateToSolrFormat(DateTime date) => $"{date.Year}-{date.Month}-{date.Day}T23:59:59.999Z";
    private string ConvertStartDateToSolrFormat(DateTime date) => $"{date.Year}-{date.Month}-{date.Day}T00:00:00.000Z";

    private SolrQueryByRange<string>? GetDateFilter(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue && !endDate.HasValue) return null;
        if (startDate.HasValue && endDate.HasValue)
            return new SolrQueryByRange<string>(SolrFields[nameof(Product.CreationDate)],
                $"{startDate.Value.Year}-{startDate.Value.Month.ToString("00")}-{startDate.Value.Day.ToString("00")}T00:00:00.000Z", $"{endDate.Value.Year.ToString("00")}-{endDate.Value.Month.ToString("00")}-{endDate.Value.Day.ToString("00")}T23:59:59.999Z");
        if (startDate.HasValue)
            return new SolrQueryByRange<string>(SolrFields[nameof(Product.CreationDate)],
                $"{startDate.Value.Year}-{startDate.Value.Month.ToString("00")}-{startDate.Value.Day.ToString("00")}T00:00:00.000Z", "NOW");
        return new SolrQueryByRange<string>(SolrFields[nameof(Product.CreationDate)],
            $"1001-01-01T00:00:00.000Z", $"{endDate.Value.Year}-{endDate.Value.Month.ToString("00")}-{endDate.Value.Day.ToString("00")}T23:59:59.999Z");
    }
    public IEnumerable<ProductWithHighlight> ContentSearch(string phrase, IEnumerable<string> fields, DateTime? startDateFilter, DateTime? endDateFilter)
    {
        var solrFields = fields.Select(x => SolrFields[x]).ToArray();
        var opt = new QueryOptions();
        opt.Fields = _defaultResultFields;
        var extraParams = new Dictionary<string, string>();
        opt.Highlight = new HighlightingParameters()
        {
            Fields = solrFields,
            Fragsize = 500
        };
        extraParams.Add("defType", "edismax");
        extraParams.Add("wt", "xml");
        var dateRange = GetDateFilter(startDateFilter, endDateFilter);
        if(dateRange is not null)
            opt.AddFilterQueries(dateRange);
        var qf = string.Join(" ", solrFields);
        extraParams.Add("qf", qf);
        opt.ExtraParams = extraParams;
        var queryResult = _solr.Query(phrase, opt);

        return queryResult.Join(queryResult.Highlights, x => x.Id.ToString(), y => y.Key,
            (x, y) =>
            {
                var result = _mapper.Map<ProductWithHighlight>(x);
                result.Highlight = string.Join("\n", y.Value.Select(z => string.Join(" ", $"{z.Key} : {string.Join(", ", z.Value)}")));
                return result;
            });
    }
}