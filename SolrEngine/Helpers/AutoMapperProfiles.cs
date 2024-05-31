
using AutoMapper;
using SolrEngine.Models;
namespace SolrEngine.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<SqlData.Models.Product, Product>().ForMember(x => x.Category, y => y.MapFrom(z => z.Category == null  ? "" : z.Category.Name ));
        }
    }
}