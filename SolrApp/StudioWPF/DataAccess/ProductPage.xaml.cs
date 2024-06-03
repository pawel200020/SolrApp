using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using SolrEngine;
using SqlData.Context;
using SqlData.Models;

namespace StudioWPF.DataAccess
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private readonly AppDbContext _context;
        private readonly ISolrManager _solrManager;
        private readonly Regex _numberRegex = new Regex("[^0-9.-]+");
        private readonly Regex _priceRegex = new Regex(" ^(-?)(0|([1-9][0-9]*))(\\.[0-9]+)?$");
        public ProductPage(AppDbContext context, ISolrManager solrManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _solrManager = solrManager ?? throw new ArgumentNullException(nameof(solrManager));
            InitializeComponent();
            RefreshDataGrid();
            cbCategory.ItemsSource = _context.Categories.ToArray();
        }
        private bool IsIntAllowed(string text) => !_numberRegex.IsMatch(text);
        private bool IsDoubleAllowed(string text) => !_priceRegex.IsMatch(text);

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!double.TryParse(txtPrice.Text, out var price) || !int.TryParse(txtQuantity.Text, out var quanitity))
                return;
            Product product;
            bool onlyDescription = true;
            if (int.TryParse(txtId.Text, out var id) && id < 0)
            {
                product = new Product()
                {
                    Name = txtName.Text,
                    Price = price,
                    Quantity = quanitity,
                    Description = txtDescription.Text,
                    CreatedBy = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    CreationDate = DateTime.UtcNow,
                    Category = cbCategory.SelectedItem as Category
                };
                onlyDescription = false;
                _context.Products.Add(product);

            }
            else
            {
                product = _context.Products.Find(id);
                
                if (product.Description != txtDescription.Text)
                    product.Description = txtDescription.Text;
                if (product.Price != price)
                {
                    product.Price = price;
                    onlyDescription = false;
                }

                if (product.Quantity != quanitity)
                {
                    product.Quantity = quanitity;
                    onlyDescription = false;

                }
                if (product.Name != txtName.Text)
                {
                    product.Name = txtName.Text;
                    onlyDescription = false;

                }
                if (product.Category is not null && product.Category != cbCategory.SelectedItem as Category)
                {
                    product.Category = cbCategory.SelectedItem as Category;
                    onlyDescription = false;
                }
                _context.Products.Update(product);
            }
            
            _context.SaveChanges();

            if (onlyDescription )
                _solrManager.UpdateSingleElementDescription(product);
            else
                _solrManager.IndexSingleElement(product);
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
            if (item is null) return;
            _solrManager.EraseSingleElement(item);
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
