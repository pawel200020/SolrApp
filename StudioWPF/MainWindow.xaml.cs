using System.Windows;
using SolrEngine;
using SqlData.Context;
using StudioWPF.Configuration;
using StudioWPF.DataAccess;
using StudioWPF.Search;

namespace StudioWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISolrManager _solrManager;
        private readonly AppDbContext _context;
        

        public MainWindow(AppContextFactory factory, ISolrManager solrManager)
        {
            _solrManager = solrManager ?? throw new ArgumentNullException(nameof(solrManager));
            _context = factory.CreateDbContext(Array.Empty<string>());
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is not System.Windows.Controls.TabControl)
                return;
            if (Equals(TabControl.SelectedItem, CategoryTab))
            {
                CategoryFrame.Navigate(new CategoryPage(_context ));
            }
            else if (Equals(TabControl.SelectedItem, ProductTab))
            {
                ProductFrame.Navigate(new ProductPage(_context,_solrManager));
            }
            else if (Equals(TabControl.SelectedItem, ConfigurationTab))
            {
                ConfigFrame.Navigate(new ConfigurationPage(_context,_solrManager));
            }
            else if (Equals(TabControl.SelectedItem, SearchTab))
            {
                SearchFrame.Navigate(new SolrSearch(_solrManager));
            }

        }
    }
}