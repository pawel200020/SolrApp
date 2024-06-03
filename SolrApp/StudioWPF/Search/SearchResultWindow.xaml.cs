using SolrEngine.Models;
using System.Windows;

namespace StudioWPF.Search
{
    /// <summary>
    /// Interaction logic for SearchResultWindow.xaml
    /// </summary>
    public partial class SearchResultWindow : Window
    {
        public SearchResultWindow(IEnumerable<ProductWithHighlight> data)
        {
            InitializeComponent();
            gdData.ItemsSource = data;
        }
    }
}
