using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SqlData.Context;
using SqlData.Models;

namespace StudioWPF.DataAccess
{
    /// <summary>
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    public partial class CategoryPage : Page
    {
        private readonly AppDbContext _context;

        public CategoryPage(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            RefreshDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.Categories.Add(new Category()
            {
                CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                CreatedDate = DateTime.UtcNow,
                Name = txtCategoryName.Text,
                Description = txtCategoryDescription.Text
            });
            _context.SaveChanges();
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            dtCategoryGrid.ItemsSource = _context.Categories.ToList();
            dtCategoryGrid.IsReadOnly = false;
        }
    }
}
