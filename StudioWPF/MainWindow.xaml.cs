using System.Windows;
using SearchEngine.Context;
using SqlData.Context;

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
    }
}