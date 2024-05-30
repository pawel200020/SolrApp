using System.Windows;
using SqlData.Context;
using StudioWPF.DataAccess;

namespace StudioWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        

        public MainWindow(AppContextFactory factory)
        {
            _context = factory.CreateDbContext(Array.Empty<string>());
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Equals(TabControl.SelectedItem, CategoryTab))
            {
                CategoryFrame.Navigate(new CategoryPage(_context ));
            }
            else if (Equals(TabControl.SelectedItem, ProductTab))
            {
                ProductFrame.Navigate(new ProductPage());
            }
            else if (Equals(TabControl.SelectedItem, ConfigurationTab))
            {
                ConfigFrame.Navigate(new ConfigurationPage(_context));
            }
            else if (Equals(TabControl.SelectedItem, SearchTab))
            {
                SearchFrame.Navigate(new SolrSearch());
            }

        }
    }
}