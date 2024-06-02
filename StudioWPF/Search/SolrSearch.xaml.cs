using System.Windows;
using System.Windows.Controls;
using SolrEngine;
using SolrEngine.Models;

namespace StudioWPF.Search
{
    /// <summary>
    /// Interaction logic for SolrSearch.xaml
    /// </summary>
    public partial class SolrSearch : Page
    {
        private readonly ISolrManager _manager;

        public SolrSearch(ISolrManager manager)
        {
            _manager = manager;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> fieldsToSearch = new();
            if(((bool) cheName.IsChecked!))
                fieldsToSearch.Add(nameof(Product.Name));
            if (((bool)cheCategory.IsChecked!))
                fieldsToSearch.Add(nameof(Product.Category));
            if (((bool)cheCreatedBy.IsChecked!))
                fieldsToSearch.Add(nameof(Product.CreatedBy));
            if (((bool)chePrice.IsChecked!))
                fieldsToSearch.Add(nameof(Product.Price));
            if (((bool)cheDescrpition.IsChecked!))
                fieldsToSearch.Add(nameof(Product.Description));
            if (!fieldsToSearch.Any())
            {
                MessageBox.Show("At least one field must selected to search!", "Select fields", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _manager.ContentSearch(txtSeachPhrase.Text,fieldsToSearch,pcrDate.SelectedDate);

        }
    }
}
