using System.Windows;
using System.Windows.Controls;
using AutoMapper;
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
        private Dictionary<string, Page> _pages;


        public MainWindow(AppContextFactory factory, ISolrManager solrManager)
        {
            _solrManager = solrManager ?? throw new ArgumentNullException(nameof(solrManager));
            _context = factory.CreateDbContext(Array.Empty<string>());
            _pages = new Dictionary<string, Page>();
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is not System.Windows.Controls.TabControl)
                return;

            FrameChanged();
        }

        private void FrameChanged()
        {
            if (Equals(TabControl.SelectedItem, CategoryTab) && !_pages.TryGetValue(nameof(CategoryPage), out var page))
            {
                page = new CategoryPage(_context);
                _pages.Add(nameof(CategoryPage), page);
                CategoryFrame.Navigate(page);

            }
            else if (Equals(TabControl.SelectedItem, ProductTab) && !_pages.TryGetValue(nameof(ProductPage), out page))
            {
                page = new ProductPage(_context, _solrManager);
                _pages.Add(nameof(ProductPage), page);
                ProductFrame.Navigate(page);
            }
            else if (Equals(TabControl.SelectedItem, ConfigurationTab) && !_pages.TryGetValue(nameof(ConfigurationPage), out page))
            {
                page = new ConfigurationPage(_context, _solrManager);
                _pages.Add(nameof(ConfigurationPage), page);
                ConfigFrame.Navigate(page);
            }
            else if (Equals(TabControl.SelectedItem, SearchTab) && !_pages.TryGetValue(nameof(SolrSearch), out page))
            {
                page = new SolrSearch(_solrManager);
                _pages.Add(nameof(SolrSearch), page);
                SearchFrame.Navigate(page);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(this, "Refreshing studio erase all unsaved data.\nDo you wish to proceed?", "Warning",
                 MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;

            _pages = new Dictionary<string, Page>();
            FrameChanged();
        }
    }
}