using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using SqlData.Context;
using SqlData.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace StudioWPF.DataAccess
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private readonly AppDbContext _context;
        private readonly Regex _numberRegex = new Regex("[^0-9.-]+");
        private readonly Regex _priceRegex = new Regex(" ^(-?)(0|([1-9][0-9]*))(\\.[0-9]+)?$");
        public ProductPage(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            RefreshDataGrid();
            cbCategory.ItemsSource = _context.Categories.ToArray();
        }
        private bool IsIntAllowed(string text) => !_numberRegex.IsMatch(text);
        private bool IsDoubleAllowed(string text) => !_priceRegex.IsMatch(text);

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(!double.TryParse(txtPrice.Text, out var price) ||  !int.TryParse(txtQuantity.Text, out var quanitity))
                return;
            if (int.TryParse(txtId.Text, out var id) && id < 0)
            {
                _context.Products.Add(new Product()
                {
                    Name = txtName.Text,
                    Price = price,
                    Quantity = quanitity,
                    Description = txtDescription.Text,
                    CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    CreationDate = DateTime.UtcNow,
                    Category = cbCategory.SelectedItem as Category
                });
            }
            else
            {
                var product = _context.Products.Find(id);
                product.Name = txtName.Text;
                product.Price = price;
                product.Quantity = quanitity;
                product.Description = txtDescription.Text;
                product.Category = cbCategory.SelectedItem as Category;
            }
            
            _context.SaveChanges();
            ClearForm();
            RefreshDataGrid();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
            => e.Handled = !IsIntAllowed(e.Text);

        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsIntAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private void PricePastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsDoubleAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
        private void RefreshDataGrid()
            => dtProductGrid.ItemsSource = _context.Products.ToList();

        private void txtPrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        => e.Handled = !IsDoubleAllowed(e.Text);

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = dtProductGrid.SelectedItem as Product;
            if(item is null) return;
            _context.Products.Remove(item);
            _context.SaveChanges();
            RefreshDataGrid();

        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            var item = dtProductGrid.SelectedItem as Product;
            if (item is null) return;
            txtName.Text = item.Name;
            txtPrice.Text = item.Price.ToString();
            txtDescription.Text = item.Description;
            txtQuantity.Text = item.Quantity.ToString();
            txtId.Text = item.Id.ToString();
            lbOperationInfo.Content = "Edit a product";
            btAdd.Content = "Save";
            if (item.Category != null) cbCategory.SelectedItem = item.Category;
        }

        private void btEdit_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btClear_Click(object sender, RoutedEventArgs e)
            => ClearForm();

        private void ClearForm()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtDescription.Text = "";
            txtQuantity.Text = "";
            txtId.Text = "-1";
            cbCategory.SelectedIndex = -1;

            lbOperationInfo.Content = "Create a product";
            btAdd.Content = "Add";
        }
    }
}
