using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolrEngine
{
    public interface ISolrManager
    {
        public void InitIfNeeded(string solrUrl);
        public bool IndexElements(IEnumerable<SqlData.Models.Product> products);
    }
}
